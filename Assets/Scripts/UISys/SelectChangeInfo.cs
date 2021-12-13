using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectChangeInfo : MonoBehaviour
{
    private FreeStyle scene;

    public TextMeshProUGUI soundName;
    public TextMeshProUGUI time;
    public TextMeshProUGUI noteCount;
    public TextMeshProUGUI longNoteCount;
    public TextMeshProUGUI Bpm;


    private void Awake()
    {
        scene = GameObject.FindGameObjectWithTag( "Scene" ).GetComponent<FreeStyle>();
        scene.OnSelectSound += SelectChangedSoundInfo;
    }

    private void SelectChangedSoundInfo( Song _song )
    {
        soundName.text     = _song.version;
        noteCount.text     = _song.noteCount.ToString();
        longNoteCount.text = _song.longNoteCount.ToString();

        int second = ( int )( _song.totalTime * .001f );
        int minute = second / 60;
        second     = second % 60;
        time.text  = $"{minute:00}:{second:00}";

        if ( _song.minBpm == _song.maxBpm ) Bpm.text = _song.minBpm.ToString();
        else                                Bpm.text = $"{_song.minBpm} ~ {_song.maxBpm}";
    }
}
