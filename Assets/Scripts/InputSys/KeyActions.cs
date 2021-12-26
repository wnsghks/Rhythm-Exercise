using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneAction
{
    Lobby, LobbyOption,
    Resolution, FrameRate, ScreenMode, SoundDriver, KeySetting,
    FreeStyle, FreeStyleOption,
    InGame, InGamePause,
    Result,
}

public class KeyActions 
{
    private Dictionary<SceneAction, StaticSceneKeyAction> keyActions = new Dictionary<SceneAction, StaticSceneKeyAction>();
    public SceneAction curAction { get; private set; }
    public bool IsLock { get; set; } = false;

    public void ActionCheck()
    {
        if ( IsLock ) return;

        if ( !keyActions.ContainsKey( curAction ) )
             Debug.Log( "The bound key does not exist." );

        keyActions[curAction].ActionCheck();
    }

    public void Bind( SceneAction _type, StaticSceneKeyAction _action )
    {
        if ( _action == null || keyActions.ContainsKey( _type ) )
            Debug.Log( "Duplicate binding." );

        keyActions.Add( _type, _action );
    }

    public void ChangeAction( SceneAction _type )
    {
        if ( !keyActions.ContainsKey( _type ) )
        {
            Debug.Log( "The bound key does not exist." );
            return;
        }

        curAction = _type;
    }

}
