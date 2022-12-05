using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteRenderer : MonoBehaviour
{
    private InGame game;
    private NoteSystem system;

    public SpriteRenderer head, body, tail;
    private Transform headTf, bodyTf, tailTf;

    private static readonly float BodyScaleOffset    = 128f / 32f; // PixelPerUnit  / TextureHeight
    private static readonly float BodyPositionOffset = 64f / 128f; // TextureHeight / PixelPerUnit
    private Note note;

    public double Time => note.time;
    public double CalcTime => note.calcTime;
    public double SliderTime => note.sliderTime;
    public double CalcSliderTime => note.calcSliderTime;
    public bool IsSlider => note.isSlider;
    public bool IsPressed { get; set; }
    public KeySound Sound => note.keySound;

    private float column;
    private static readonly Color NoteFailColor = new Color( .25f, .25f, .25f, 1f );
    private double newTime;

    private void Awake()
    {
        game = GameObject.FindGameObjectWithTag( "Scene" ).GetComponent<InGame>();
        game.OnScrollChange += ScrollUpdate;

        headTf = head.transform;
        headTf.localScale = new Vector2( GameSetting.NoteWidth, GameSetting.NoteHeight );

        bodyTf = body.transform;

        tailTf = tail.transform;
        tailTf.localScale = new Vector2( GameSetting.NoteWidth, GameSetting.NoteHeight );
        tail.enabled = false;
    }

    private void OnDestroy()
    {
        game.OnScrollChange -= ScrollUpdate;
    }

    public void SetInfo( int _lane, NoteSystem _system, in Note _note )
    {
        system  = _system;
        note    = _note;

        column = GameSetting.NoteStartPos + ( _lane * GameSetting.NoteWidth ) + ( ( _lane + 1 ) * GameSetting.NoteBlank );
        newTime = note.calcTime;

        ScrollUpdate();
        head.color = body.color = tail.color = Color.white;
        //head.enabled = !IsSlider;
        body.enabled = IsSlider;
        head.color = body.color = tail.color = Color.white;
    }

    public void SetBodyFail() => head.color = body.color = tail.color = NoteFailColor;

    private void ScrollUpdate()
    {
        if ( !IsSlider ) return;

        bodyTf.localPosition = Vector2.zero;
        SliderUpdate( CalcTime, CalcSliderTime );
    }

    public void Despawn()
    {
        IsPressed = false;
        system.Despawn( this );
    }

    private void SliderUpdate( double _time, double _sliderTime )
    {
        double bodyLength = ( _sliderTime - _time ) * GameSetting.Weight;
        var bodyScale = ( float )( bodyLength * BodyScaleOffset );
        bodyTf.localScale = bodyScale < 0 ? new Vector2( GameSetting.NoteWidth, 0f ) :
                                            new Vector2( GameSetting.NoteWidth, bodyScale );

        var tailPos = ( float )bodyLength - ( GameSetting.NoteHeight * BodyPositionOffset );
        tailTf.localPosition = tailPos < GameSetting.NoteHeight * BodyPositionOffset ? new Vector2( 0f, GameSetting.NoteHeight * BodyPositionOffset ) :
                                                                                       new Vector2( 0f, tailPos );
    }

    private void LateUpdate()
    {
       // 롱노트 판정선에 붙기
        Vector2 headPos;
        if ( IsPressed )
        {
            if ( transform.position.y <= GameSetting.JudgePos )
                 newTime = NowPlaying.PlaybackChanged;

            headPos = new Vector2( column, GameSetting.JudgePos + ( float )( ( newTime - NowPlaying.PlaybackChanged ) * GameSetting.Weight ) );
            SliderUpdate( newTime, CalcSliderTime );
        }
        else
        {
            headPos = new Vector2( column, GameSetting.JudgePos + ( float )( ( ( newTime - NowPlaying.PlaybackChanged ) * GameSetting.Weight ) ) );
        }

        transform.localPosition = headPos;
    }
}
