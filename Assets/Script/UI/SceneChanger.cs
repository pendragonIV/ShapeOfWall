using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private const string MENU = "MainMenu";
    private const string GAME = "GameScene";
    private const string LEVEL_CHOOSE = "LevelScene";

    [SerializeField]
    private Transform sceneTransition;

    private void Start()
    {
        PlayChangeStataeAnimation();
    }

    public void PlayChangeStataeAnimation()
    {
        sceneTransition.GetComponent<Animator>().Play("SceneTransition");
    }

    public void ChangeToWallHomeScreen()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeToAnotherScene(MENU));
    }

    public void ChangeToWallChallengeScene()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeToAnotherScene(GAME));
    }

    private IEnumerator ChangeToAnotherScene(string sceneName)
    {

        //Optional: Add animation here
        sceneTransition.GetComponent<Animator>().Play("SceneTransitionReverse");
        yield return new WaitForSecondsRealtime(1f);

        SceneManager.LoadSceneAsync(sceneName);

    }
}
