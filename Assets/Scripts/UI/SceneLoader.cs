using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public bool GameScene;

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(LevelsManager.Instance.Levels[LevelsManager.Instance.CurrentLevelIndex]);
        LevelsManager.Instance.CurrentLevelIndex++;
    }

    public void LoadStory()
    {
        StartCoroutine(LoadStoryScene());
    }

    IEnumerator LoadStoryScene()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("Cutscene");
    }

    public void LoadVictory()
    {
        StartCoroutine(LoadVictoryScene());
    }

    IEnumerator LoadVictoryScene()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("FinalScene");
    }

    public void LoadMainMenu()
    {
        StartCoroutine(ExitToMainMenu());
    }

    IEnumerator ExitToMainMenu()
    {
        transition.SetTrigger("Start");
        AudioManager.Instance.StopSound(SoundName.THEME);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(LevelsManager.Instance.MainMenuScene);
        GameManager.Instance.CurrentGameState = GameState.READY;
        LevelsManager.Instance.ExitGame();
    }

    public void ReloadLevel()
    {
        StartCoroutine(ReloadCurrentLevel());
    }

    IEnumerator ReloadCurrentLevel()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(LevelsManager.Instance.Levels[LevelsManager.Instance.CurrentLevelIndex - 1]);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameScene)
        {
            PoolingManager.Instance.PooledObjectsParent = GameObject.FindWithTag("PooledObjectParent");
            GameManager.Instance.SceneLoader = this;
            GameManager.Instance.LoadNewLevel();
            AudioManager.Instance.PlayNormalSound();
        }
    }
}
