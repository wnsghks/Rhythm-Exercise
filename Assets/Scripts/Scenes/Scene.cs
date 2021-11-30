using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    protected enum SceneType { Lobby, FreeStyle, InGame };

    [Serializable]
    public struct SceneClips
    {
        public AudioClip move;
        public AudioClip click;
        public AudioClip escape;
    }

    public SceneClips clips;
    private AudioSource sfxAudio;

    protected void SfxPlay( AudioClip _clip )
    {
        if ( _clip == null )
        {
            Debug.LogError( "clip is null." );
            return;
        }

        sfxAudio.PlayOneShot( _clip );
    }

    protected virtual void Awake()
    {
        SoundManager.OnRelease += SoundRelease;
        sfxAudio = gameObject.AddComponent<AudioSource>();

        Camera.main.orthographicSize = ( Screen.height / ( GlobalSetting.PPU * 2f ) ) * GlobalSetting.PPU;
    }

    protected virtual void SoundRelease() { }

    protected void Change( SceneType _type )
    {
        SceneManager.LoadScene( _type.ToString() );
        SoundManager.Inst.Stop();
    }
}
