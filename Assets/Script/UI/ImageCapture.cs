using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageCapture : MonoBehaviour
{
    public static ImageCapture instance;

    private Texture2D _screenShot;
    [SerializeField] private Image _imageToShow;
    private bool _isCaptured = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        _screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    public void CaptureWallImageNow()
    {
        if (_isCaptured)
        {
            return;
        }
        _isCaptured = true;
        StartCoroutine(Capture());
    }

    private IEnumerator Capture()
    {
        yield return new WaitForSecondsRealtime(.2f);
        yield return new WaitForEndOfFrame();
        _screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        _screenShot.Apply();
        ShowCapturedImage();
    }

    private void ShowCapturedImage()
    {
        Sprite sprite = Sprite.Create(_screenShot, new Rect(0, 0, _screenShot.width, _screenShot.height), Vector2.zero);
        _imageToShow.sprite = sprite;
        _imageToShow.SetNativeSize();
        _imageToShow.transform.localScale = Vector3.one * .8f;
        _imageToShow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 150);
        GameManager.instance.gameScene.ShowCapturedImage();
    }
}
