using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public GameScene gameScene;
    public bool _isGameOver = false;
    public bool _isGameStarted = false;
    private float _timeToReady = 4f;
    private int _score = 0;

    private void Start()
    {
        Time.timeScale = 0;
    }

    private void Update()
    {
        if(!_isGameStarted)
        {
            _timeToReady -= Time.unscaledDeltaTime;
            gameScene.UpdateCountDownText((int)_timeToReady);   
            if(_timeToReady <= 0)
            {
                _isGameStarted = true;
                gameScene.HideCountDownPanel(); 
                Time.timeScale = 1;
            }
        }
    }

    public void UpdateScore(int score)
    {
        _score += score;
        gameScene.UpdateSOWScore(_score);
    }

    public int GetScore()
    {
        return _score;
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }

    public void GameOfWallOver()
    {
        _isGameOver = true;
        ImageCapture.instance.CaptureWallImageNow();
    }
}
