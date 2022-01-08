using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteRenderer : MonoBehaviour
{
    private InGame scene;
    public float Time { get; private set; }
    public float CalcTime { get; private set; }
    public float SliderTime { get; private set; }
    public float CalcSliderTime { get; private set; }
    public bool IsSlider { get; private set; }

    public bool isHolding;
    public float column;
    private SpriteRenderer rdr;
    private float newTime;

    public void SetInfo( Note _data )
    {
        newTime = _data.calcTime;
        Time           = _data.time;
        CalcTime       = _data.calcTime;
        SliderTime     = _data.sliderTime;
        CalcSliderTime = _data.calcSliderTime;
        IsSlider       = _data.isSlider;

        isHolding = false;

        column = GlobalSetting.NoteStartPos + ( _data.line * GlobalSetting.NoteWidth ) + ( ( _data.line + 1 ) * GlobalSetting.NoteBlank );

        ScaleUpdate();
        if ( _data.line == 1 || _data.line == 4 ) SetColor( new Color( 0.2078432f, 0.7843138f, 1f, 1f ) );
        else                                      SetColor( Color.white );
    }

    public void SetColor( Color _color ) => rdr.color = _color;

    private void ScaleUpdate()
    {
        if ( IsSlider ) transform.localScale = new Vector3( GlobalSetting.NoteWidth, Mathf.Abs( ( CalcSliderTime - CalcTime ) * InGame.Weight ), 1f );
        else            transform.localScale = new Vector3( GlobalSetting.NoteWidth, GlobalSetting.NoteHeight, 1f );
    }

    private void Awake()
    {
        rdr = GetComponent<SpriteRenderer>();
        scene = GameObject.FindGameObjectWithTag( "Scene" ).GetComponent<InGame>();
    }

    private void OnEnable() => scene.OnScrollChanged += ScaleUpdate;

    private void OnDisable() => scene.OnScrollChanged -= ScaleUpdate;

    private void LateUpdate()
    {
        if ( isHolding )
        {
            float startPos     = ( ( CalcTime       - InGame.PlaybackChanged ) * InGame.Weight );
            float endPos       = ( ( CalcSliderTime - InGame.PlaybackChanged ) * InGame.Weight );
            float currentScale = Mathf.Abs( endPos - startPos ) - Mathf.Abs( startPos );
            transform.localScale = new Vector3( GlobalSetting.NoteWidth, currentScale, 1f );

            transform.position = new Vector3( column, GlobalSetting.JudgeLine, 2f );
            newTime = InGame.PlaybackChanged;
        }
        else
        {
            transform.position = new Vector3( column, GlobalSetting.JudgeLine + ( ( newTime - InGame.PlaybackChanged ) * InGame.Weight ), 2f );
        }
    }

}
