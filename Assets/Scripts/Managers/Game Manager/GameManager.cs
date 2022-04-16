using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    READY,
    SHOWING_THE_LEVEL,
    PLAYING,
    PAUSED,
    GAMEOVER,
    LEVEL_CLEARED,
}

public class GameManager : Singleton<GameManager>
{
    [Header("Debugging")]
    public bool DebugMode;

    [Header("Settings")]
    public SceneLoader SceneLoader;
    public CameraController CameraController;
    public PlayerController Player;
    public SoundName[] ScarySounds;
    public GameState CurrentGameState;

    [Header("Managers")]
    public UIManager UIManager;
    public LightsManager LightsManager;
   
    public override void Awake()
    {
        base.Awake();
        LightsManager = GetComponent<LightsManager>();
    }

    private void Start()
    {
        LightsManager.SetupLights();
        Player = FindObjectOfType<PlayerController>();
        StopAllCoroutines();
        if(DebugMode)
            CurrentGameState = GameState.PLAYING;
        else
            CurrentGameState = GameState.READY;
    }

    private void Update()
    {
        if(CurrentGameState == GameState.LEVEL_CLEARED)
            return;

        switch (CurrentGameState)
        {
            case GameState.READY:

                if (Input.GetKeyDown(KeyCode.J))
                {
                    AudioManager.Instance.PlaySound(SoundName.SKIP);
                    StartCoroutine(StartGameRoutine());
                    CurrentGameState = GameState.SHOWING_THE_LEVEL;
                }
                break;

            case GameState.PLAYING:
                if (DebugMode) return;
                if (LightsManager.LightsOn)
                {
                    LightsManager.LightsCountDown();
                }
                break;

            case GameState.PAUSED:

                if (Input.GetKeyDown(KeyCode.S)) UIManager.PauseMoveCursorDown();
                if (Input.GetKeyDown(KeyCode.W)) UIManager.PauseMoveCursorUp();
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.J)) UIManager.PauseSelectItem();
                break;

            case GameState.GAMEOVER:

                if (Input.GetKeyDown(KeyCode.S)) UIManager.GameoverMoveCursorDown();
                if (Input.GetKeyDown(KeyCode.W)) UIManager.GameoverMoveCursorUp();
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.J)) UIManager.GameoverSelectItem();
                break;
        }
    }


    public void GameOver()
    {
        UIManager.ShowGameoverPanel();
        LightsManager.CurrentLightsState = LightsState.ON;
    }

    public void ClearLevel()
    {
        StopAllCoroutines();
        AudioManager.Instance.StopSound(SoundName.THEME);
        AudioManager.Instance.PlaySound(SoundName.MISSION_CLEAR);
        AudioManager.Instance.PlayNormalSound();

        UIManager.HideHUD();
        UIManager.ShowLevelClearedPanel();
        StartCoroutine(ClearLevelRoutine());

        LightsManager.ResetGlobalLight();
        CurrentGameState = GameState.LEVEL_CLEARED;

    }

    IEnumerator ClearLevelRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        if (LevelsManager.Instance.CurrentLevelIndex < LevelsManager.Instance.Levels.Count)
        {
            SceneLoader.LoadNextLevel();
        }
        else
        {
            SceneLoader.LoadVictory();
        }   
    }

    public void PauseGame()
    {
        UIManager.ShowPauseMenu();
        Time.timeScale = 0;
        CurrentGameState = GameState.PAUSED;
    }

    public void ResumeGame()
    {
        UIManager.HidePauseMenu();
        Time.timeScale = 1;
        CurrentGameState = GameState.PLAYING;
    }

    IEnumerator StartGameRoutine()
    {
        UIManager.StepsPanel.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        CameraController.IsShowingLevel = true;
        while (CameraController.IsShowingLevel)
        {
            yield return null;
        }
        UIManager.ShowHUD();
        yield return new WaitForSeconds(0.5f);
        UIManager.Ready.SetActive(true);
        yield return new WaitForSeconds(1f);
        UIManager.Ready.SetActive(false);
        UIManager.Go.SetActive(true);
        AudioManager.Instance.PlaySound(SoundName.START);
        yield return new WaitForSeconds(1f);
        UIManager.Go.SetActive(false);
        CurrentGameState = GameState.PLAYING;
    }

    public void LoadNewLevel()
    {
        AudioManager.Instance.PlaySound(SoundName.THEME);

        CameraController = Camera.main.GetComponent<CameraController>();
        UIManager = FindObjectOfType<UIManager>();
 
        Player = FindObjectOfType<PlayerController>();
        CurrentGameState = GameState.READY;
        
        UIManager.GetUIElements();
        LightsManager.SetupLights();
        StopAllCoroutines();
    }
}
