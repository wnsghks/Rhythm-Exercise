using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BGAPlayOption : OptionText
{
    protected override void Awake()
    {
        base.Awake();

        curIndex = GameSetting.IsBGAPlay ? 1 : 0;
        ChangeText( texts[curIndex] );
    }

    protected override void CreateObject()
    {
        for ( int i = 0; i < ( int )OPTION_BOOL.Count; i++ )
        {
            texts.Add( ( ( OPTION_BOOL )i ).ToString() );
        }
    }
    public override void Process()
    {
        GameSetting.IsBGAPlay = curIndex == 0 ? false : true;
        Debug.Log( ( OPTION_BOOL )curIndex );
    }
}