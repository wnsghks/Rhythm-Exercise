using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

public class NowPlaying : SingletonUnity<NowPlaying>
{
    public ReadOnlyCollection<Song> Songs { get; private set; }

    public  Song CurrentSong => curSong;
    private Song curSong;

    public  Chart CurrentChart => curChart;
    private Chart curChart;

    public  int CurrentSongIndex 
    {
        get => curSongIndex;
        set
        {
            if ( value >= Songs.Count )
                throw new System.Exception( "Out of Range. " );

            curSongIndex = value;
            curSong      = Songs[value];
        }
    }
    private int curSongIndex;

    public static double Playback;        // 노래 재생 시간
    public static double PlaybackChanged; // BPM 변화에 따른 노래 재생 시간

    public bool IsPlaying { get; private set; }
    private readonly double waitTime = -2d;
    private double startTime;
    private double savedTime;

    private double totalTime;

    private WaitUntil waitPlayback;

    private void Awake()
    {
        waitPlayback = new WaitUntil( () => Playback >= GameSetting.SoundOffset * .001d );

        //using ( FileConverter converter = new FileConverter() )
        //    converter.ReLoad();

        using ( FileParser parser = new FileParser() )
        {
            ReadOnlyCollection<Song> songs;
            parser.ParseFilesInDirectories( out songs );
            Songs = songs;
        }

        CurrentSongIndex = 0;
    }

    private void Update()
    {
        if ( !IsPlaying ) return;

        Playback = savedTime + ( System.DateTime.Now.TimeOfDay.TotalSeconds - startTime );
        PlaybackChanged = GetChangedTime( Playback );

        //if ( Playback >= totalTime + 1.5d )
        //     Stop();
    }

    public void Initialize()
    {
        IsPlaying = false;
        Playback = waitTime;
        PlaybackChanged = 0d;

        totalTime = curSong.totalTime * .001d;
        using ( FileParser parser = new FileParser() )
        {
            parser.TryParse( curSong.filePath, out curChart );
        }
    }

    public void Stop()
    {
        StopAllCoroutines();
        IsPlaying = false;
        Playback = waitTime;
        PlaybackChanged = 0d;
        SceneChanger.Inst.LoadScene( SceneType.FreeStyle );
    }

    public void Play() => StartCoroutine( MusicStart() );

    /// <summary>
    /// False : Playback is higher than the Last Note Time.
    /// </summary>
    /// <param name="_isPause"></param>
    /// <returns></returns>
    public bool Pause( bool _isPause )
    {
        if ( Playback >= totalTime )
             return false;


        if ( _isPause )
        {
            IsPlaying = false;
            SoundManager.Inst.Pause = true;

            double prevPos = waitTime + ( SoundManager.Inst.Position * .001d );
            if ( prevPos > 0d )
            {
                savedTime = prevPos;
                SoundManager.Inst.Position = ( uint )( savedTime * 1000d );
            }
            else
            {
                SoundManager.Inst.Position = 0;
                savedTime = waitTime;
            }
        }
        else
        {
            StartCoroutine( PauseStart() );
        }

        return true;
    }

    private IEnumerator PauseStart()
    {
        SceneChanger.CurrentScene.InputLock( true );
        while ( Playback >= savedTime )
        {
            Playback -= Time.deltaTime * 3f;
            PlaybackChanged = GetChangedTime( Playback );
            yield return null;
        }

        startTime = System.DateTime.Now.TimeOfDay.TotalSeconds;
        IsPlaying = true;

        yield return waitPlayback;
        SoundManager.Inst.Pause = false;

        yield return YieldCache.WaitForSeconds( 3f );
        SceneChanger.CurrentScene.InputLock( false );
    }

    private IEnumerator MusicStart()
    {
        SoundManager.Inst.LoadBgm( CurrentSong.audioPath, false, false, false );
        SoundManager.Inst.PlayBgm( true );
        SoundManager.Inst.Position = 0;
        startTime = System.DateTime.Now.TimeOfDay.TotalSeconds;
        IsPlaying = true;
        savedTime = waitTime;

        yield return waitPlayback;
        SoundManager.Inst.Pause = false;
    }

    public double GetChangedTime( double _time ) // BPM 변화에 따른 시간 계산
    {
        var timings = CurrentChart.timings;
        double newTime = _time;
        double prevBpm = 0d;
        for ( int i = 0; i < timings.Count; i++ )
        {
            double time = timings[i].time;
            double bpm  = timings[i].bpm;

            if ( time > _time ) break;
            newTime += ( bpm - prevBpm ) * ( _time - time );
            prevBpm = bpm;
        }
        return newTime;
    }
}
