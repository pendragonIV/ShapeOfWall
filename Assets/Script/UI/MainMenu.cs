using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Transform gameLogo;
    [SerializeField]
    private Transform tutorPanel;
    [SerializeField]
    private Transform guideLine;
    [SerializeField]
    private Transform playBtn;
    [SerializeField]
    private Transform sceneComponents;
    [SerializeField]
    private Color aimColor;


    private void Start()
    {
        tutorPanel.gameObject.SetActive(false);

        gameLogo.GetComponent<CanvasGroup>().alpha = 0f;
        gameLogo.GetComponent<CanvasGroup>().DOFade(1, 2f).SetUpdate(true);
        RectTransform rectTransform = gameLogo.GetComponent<RectTransform>();
        gameLogo.localScale = Vector3.one * 3;
        gameLogo.DOScale(Vector3.one, 2f).SetEase(Ease.OutElastic).SetUpdate(true); 
    }

    public void PopupTutorialForPlayer()
    {
        tutorPanel.gameObject.SetActive(true);
        guideLine.gameObject.SetActive(true);
        SlideAndFadeUIIn(tutorPanel.GetComponent<CanvasGroup>(), guideLine.GetComponent<RectTransform>());
        sceneComponents.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(false);
    }

    public void HideTutorFromPlayer()
    {
        StartCoroutine(SLideUIOutAndFade(tutorPanel.GetComponent<CanvasGroup>(), guideLine.GetComponent<RectTransform>()));
        sceneComponents.gameObject.SetActive(true);
        playBtn.gameObject.SetActive(true);
    }

    private void SlideAndFadeUIIn(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 700, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 0), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }

    private IEnumerator SLideUIOutAndFade(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 700), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);

        yield return new WaitForSecondsRealtime(.3f);
        guideLine.gameObject.SetActive(true);
        tutorPanel.gameObject.SetActive(false);

    }

    private void OnApplicationQuit()
    {
        DOTween.KillAll();
    }

}
