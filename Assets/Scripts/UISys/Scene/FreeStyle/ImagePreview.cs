using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using DG.Tweening;

public class ImagePreview : MonoBehaviour
{
    public FreeStyleMainScroll scroller;
    public Sprite defaultSprite;
    public RectTransform previewObject;
    private RawImage previewImage;
    private Texture2D prevTexture;
    
    private Coroutine coroutine;

    private void Awake()
    {
        scroller.OnSelectSong += ChangeImage;

        if ( !previewObject.TryGetComponent( out previewImage ) )
             Debug.LogError( "Preview BGA object is not found." );
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        ClearPreviewTexture();
    }

    private void ClearPreviewTexture()
    {
        if ( prevTexture )
        {
            if ( ReferenceEquals( prevTexture, defaultSprite.texture ) )
                 return;

            DestroyImmediate( prevTexture );
        }
    }

    private void ChangeImage( Song _song )
    {
        if ( !ReferenceEquals( coroutine, null ) )
        {
            StopCoroutine( coroutine );
            coroutine = null;
        }

        if ( !_song.hasVideo && !_song.hasSprite )
        {
            ClearPreviewTexture();
            coroutine = StartCoroutine( LoadPreviewImage( _song.imagePath ) );
        }
    }

    private IEnumerator LoadPreviewImage( string _path )
    {
        bool isExist = System.IO.File.Exists( _path );
        if ( isExist )
        {
            var ext = System.IO.Path.GetExtension( _path );
            if ( ext.Contains( ".bmp" ) )
            {
                BMPLoader loader = new BMPLoader();
                BMPImage img = loader.LoadBMP( _path );
                prevTexture = img.ToTexture2D();
            }
            else
            {
                using ( UnityWebRequest www = UnityWebRequestTexture.GetTexture( _path ) )
                {
                    www.method = UnityWebRequest.kHttpVerbGET;
                    using ( DownloadHandlerTexture handler = new DownloadHandlerTexture() )
                    {
                        www.downloadHandler = handler;
                        yield return www.SendWebRequest();

                        if ( www.result == UnityWebRequest.Result.ConnectionError ||
                             www.result == UnityWebRequest.Result.ProtocolError )
                        {
                            Debug.LogError( $"UnityWebRequest Error : {www.error}" );
                            throw new System.Exception( $"UnityWebRequest Error : {www.error}" );
                        }

                        prevTexture = handler.texture;
                    }
                }
            }
        }
        else
            prevTexture = defaultSprite.texture;

        var texSize = Global.Math.GetScreenRatio( prevTexture, new Vector2( 752f, 423f ) );
        previewObject.sizeDelta = texSize;

        previewImage.texture = prevTexture;
        previewObject.localScale = new Vector3( 0f, 1f, 1f );
        previewImage.enabled = true;
        previewObject.DOScaleX( 1f, .25f );
    }
}
