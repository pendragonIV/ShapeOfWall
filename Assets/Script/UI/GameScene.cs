using DG.Tweening;
using System.Xml;
using TMPro;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Transform _capturedImage;
    [SerializeField] private GameObject _ingameMenuOfWall;
    [SerializeField] private GameObject _countDownPanel;
    [SerializeField] private TMP_Text _score;


    private string[] _gameLoseTexts = new string[]
    {
        "You are almost there!",
        "Try carefully next time!",
        "Try again?",
        "You can do it!",   
        "You are so close!",
    };

    public void UpdateSOWScore(int score)
    {
        string newScore = "" + score.ToString("00000");
        _score.text = newScore; 
    }

    private void Start()
    {
        _gameOverPanel.SetActive(false);
        _capturedImage.gameObject.SetActive(false);
        _ingameMenuOfWall.SetActive(false);
        _countDownPanel.SetActive(true);
    }


    public void UpdateCountDownText(int countDown)
    {
        TMP_Text text = _countDownPanel.GetComponentInChildren<TMP_Text>();
        text.text = countDown.ToString();
    }

    public void ShowIngameMenuOfSOW()
    {
        _ingameMenuOfWall.SetActive(true);
        CanvasGroup canvasGroup = _ingameMenuOfWall.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, .3f).SetUpdate(true).SetEase(Ease.Linear);
        RectTransform rectTransform = _ingameMenuOfWall.transform.GetChild(0).GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 500);
        rectTransform.DOAnchorPosY(0, .3f).SetUpdate(true).SetEase(Ease.OutBack);
        Time.timeScale = 0;
    }

    public void HideIngameMenuOfSOW()
    {
        CanvasGroup canvasGroup = _ingameMenuOfWall.GetComponent<CanvasGroup>();
        canvasGroup.DOFade(0, .3f).SetUpdate(true).SetEase(Ease.Linear);
        RectTransform rectTransform = _ingameMenuOfWall.transform.GetChild(0).GetComponent<RectTransform>();
        rectTransform.DOAnchorPosY(500, .3f).SetUpdate(true).SetEase(Ease.InBack).OnComplete(() =>
        {
            _ingameMenuOfWall.SetActive(false);
            Time.timeScale = 1;
        });
    }

    public void ShowCapturedImage()
    {
        _gameOverPanel.SetActive(true);
        _capturedImage.gameObject.SetActive(true);
        _capturedImage.localScale = Vector3.one * 1.5f;
        _capturedImage.DOScale(Vector3.one, .3f).SetUpdate(true).SetEase(Ease.OutBack);
        _capturedImage.DORotate(new Vector3(0, 0, 12), .3f).SetUpdate(true).SetEase(Ease.Linear);
        TMP_Text text = _capturedImage.GetChild(1).GetComponent<TMP_Text>();
        text.text = _gameLoseTexts[Random.Range(0, _gameLoseTexts.Length)];
        TMP_Text scoreText = _capturedImage.GetChild(2).GetComponent<TMP_Text>();
        scoreText.text = "Score: " + GameManager.instance.GetScore().ToString("00000");
    }

    public void HideCapturedImage()
    {
        _capturedImage.gameObject.SetActive(false);
        _gameOverPanel.SetActive(false);
    }

    public void HideCountDownPanel()
    {
        _countDownPanel.SetActive(false);
    }
}
