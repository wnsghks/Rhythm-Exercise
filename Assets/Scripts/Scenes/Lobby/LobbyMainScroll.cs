using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LobbyMainScroll : ScrollOption, IKeyBind
{
    public GameObject optionCanvas, exitCanvas;
    public RectTransform leftImage, rightImage;
    private CanvasGroup optionGroup;

    private Scene scene;
    private RectTransform rt;

    protected override void Awake()
    {
        base.Awake();

        scene = GameObject.FindGameObjectWithTag( "Scene" ).GetComponent<Scene>();
        if ( !optionCanvas.TryGetComponent<CanvasGroup>( out optionGroup ) )
             Debug.LogError( $"Lobby Option CanvasGroup is null" );

        rt = transform as RectTransform;
        
        // ScrollOption
        var childRT = CurrentOption.transform as RectTransform;
        rt.anchoredPosition = -childRT.anchoredPosition;

        // Key
        KeyBind();
    }

    public override void PrevMove()
    {
        base.PrevMove();
        if ( !IsLoop && IsDuplicate )
            return;

        SoundManager.Inst.Play( SoundSfxType.MainSelect );

        var childRT = CurrentOption.transform as RectTransform;
        rt.DOAnchorPosX( -childRT.anchoredPosition.x, .25f );

        leftImage.localScale = new Vector3( .65f, .65f, 1f );
        leftImage.DOScale( 1f, .25f );
    }

    public override void NextMove()
    {
        base.NextMove();
        if ( !IsLoop && IsDuplicate )
            return;
        
        SoundManager.Inst.Play( SoundSfxType.MainSelect );

        var childRT = CurrentOption.transform as RectTransform;
        rt.DOAnchorPosX( -childRT.anchoredPosition.x, .25f );

        rightImage.localScale = new Vector3( .65f, .65f, 1f );
        rightImage.DOScale( 1f, .25f );
    }

    public void GotoFreeStyle()
    {
        if ( NowPlaying.Inst.IsParseSongs )
        {
            scene.LoadScene( SceneType.FreeStyle );
            SoundManager.Inst.Play( SoundSfxType.MainClick );
        }
    }

    public void ShowOptionCanvas()
    {
        optionGroup.alpha = 0f;
        optionCanvas.SetActive( true );
        DOTween.To( () => 0f, x => optionGroup.alpha = x, 1f, GlobalConst.OptionFadeDuration );
        scene.ChangeAction( SceneAction.Option );
        SoundManager.Inst.Play( SoundSfxType.MenuClick );
    }

    public void ShowExitCanvas()
    {
        exitCanvas.SetActive( true );
        scene.ChangeAction( SceneAction.Exit );
        SoundManager.Inst.Play( SoundSfxType.MenuClick );
    }

    public void KeyBind()
    {
        scene.Bind( SceneAction.Main, KeyCode.Return, () => CurrentOption.Process() );

        scene.Bind( SceneAction.Main, KeyCode.LeftArrow, () => PrevMove() );

        scene.Bind( SceneAction.Main, KeyCode.RightArrow, () => NextMove() );
    }
}
