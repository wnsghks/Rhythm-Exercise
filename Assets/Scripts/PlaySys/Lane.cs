using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lane : MonoBehaviour
{
    public int Key { get; private set; }
    public NoteSystem  NoteSys  { get; private set; }
    public InputSystem InputSys { get; private set; }

    public event Action<int/*Lane Key*/> OnLaneInitialize;

    public SpriteRenderer keyImage;
    public Sprite keyDefaultSprite, keyPressSprite;
    private SpriteRenderer rdr;
    private Color color;

    private void Awake()
    {
        NoteSys  = GetComponent<NoteSystem>();
        InputSys = GetComponent<InputSystem>();
        rdr      = GetComponent<SpriteRenderer>();

        if ( ( GameSetting.CurrentVisualFlag & GameVisualFlag.LaneEffect ) != 0 )
             InputSys.OnInputEvent += LaneEffect;

        if ( ( GameSetting.CurrentVisualFlag & GameVisualFlag.ShowGearKey ) != 0 )
             InputSys.OnInputEvent += KeyEffect;

        color = rdr.color;
        color.a = .75f;
        rdr.color = Color.clear;
    }

    private void LaneEffect( bool _isEnable ) => rdr.color = _isEnable ? color : Color.clear;

    private void KeyEffect( bool _isEnable )=> keyImage.sprite = _isEnable ? keyPressSprite : keyDefaultSprite;

    public void SetLane( int _key )
    {
        Key = _key;
        UpdatePosition( _key );
        OnLaneInitialize?.Invoke( Key );
    }

    public void UpdatePosition( int _key )
    {
        transform.position = new Vector3( GameSetting.NoteStartPos + ( GameSetting.NoteWidth * _key ) + ( GameSetting.NoteBlank * _key ) + GameSetting.NoteBlank, GameSetting.JudgePos, 90f );
        transform.localScale = new Vector3( GameSetting.NoteWidth, ( Screen.height * .13f ), 1f );

        keyImage.transform.position   = new Vector3( transform.position.x, keyImage.transform.position.y, keyImage.transform.position.z );
        keyImage.transform.localScale = new Vector3( transform.localScale.x + GameSetting.NoteBlank, keyImage.transform.localScale.y );
    }
}
