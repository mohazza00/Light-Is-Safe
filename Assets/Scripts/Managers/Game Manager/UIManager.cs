using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Canvas;
    public GameObject GameoverPanel;
    public GameObject LevelClearedPanel;
    public GameObject StepsPanel;
    public GameObject HUD;
    public GameObject Ready;
    public GameObject Go;
    public GameObject PausePanel;

    [Header("Gameover")]
    public MenuItem[] GameoverMenuItems;
    public int CurrentGameoverMenuItem;

    [Header("Pause Game")]
    public MenuItem[] PauseMenuItems;
    public int CurrentPauseMenuItem;

    public void GetUIElements()
    {
        Canvas = GameObject.FindWithTag("Canvas");
        GameoverPanel = GameObject.FindWithTag("Gameover");
        LevelClearedPanel = GameObject.FindWithTag("MissionCleared");
        StepsPanel = GameObject.FindWithTag("HowToPlay");
        HUD = GameObject.FindWithTag("HUD");
        Ready = GameObject.FindWithTag("Ready");
        Go = GameObject.FindWithTag("Go");
        PausePanel = GameObject.FindWithTag("Pause");

        GameoverPanel.SetActive(false);
        LevelClearedPanel.SetActive(false);
        Ready.SetActive(false);
        Go.SetActive(false);
        PausePanel.SetActive(false);
        if (GameManager.Instance.DebugMode)
            Canvas.SetActive(false);
    }

    #region Show / Hide Functions
    public void ShowPauseMenu()
    {
        PausePanel.SetActive(true);
        CurrentPauseMenuItem = 0;
        PauseMenuItems[0].SelectItem();
    }

    public void HidePauseMenu()
    {
        PausePanel.SetActive(false);
    }

    public void ShowGameoverPanel()
    {
        GameoverPanel.SetActive(true);
        CurrentGameoverMenuItem = 0;
        GameoverMenuItems[0].SelectItem();
    }

    public void ShowLevelClearedPanel()
    {
        LevelClearedPanel.SetActive(true);
    }

    public void ShowHUD()
    {
        HUD.GetComponent<Animator>().SetBool("Show", true);
    }

    public void HideHUD()
    {
        HUD.GetComponent<Animator>().SetBool("Show", false);
    }
    #endregion

    #region Pause Menu
    public void PauseMoveCursorUp()
    {
        CurrentPauseMenuItem--;

        if (CurrentPauseMenuItem < 0)
        {
            CurrentPauseMenuItem = PauseMenuItems.Length - 1;
        }

        foreach (MenuItem item in PauseMenuItems)
        {
            item.DeselectItem();
        }

        PauseMenuItems[CurrentPauseMenuItem].SelectItem();
    }

    public void PauseMoveCursorDown()
    {
        CurrentPauseMenuItem = (CurrentPauseMenuItem + 1) % PauseMenuItems.Length;

        foreach (MenuItem item in PauseMenuItems)
        {
            item.DeselectItem();
        }

        PauseMenuItems[CurrentPauseMenuItem].SelectItem();
    }

    public void PauseSelectItem()
    {
        if (CurrentPauseMenuItem == 0)
        {
            GameManager.Instance.ResumeGame();
        }

        if (CurrentPauseMenuItem == 1)
        {
            Time.timeScale = 1f;
            GameManager.Instance.SceneLoader.LoadMainMenu();
        }

        if (CurrentPauseMenuItem == 2)
        {
            Application.Quit();
        }   
    }
    #endregion

    #region Game Over Menu
    public void GameoverMoveCursorUp()
    {
        CurrentGameoverMenuItem--;

        if (CurrentGameoverMenuItem < 0)
        {
            CurrentGameoverMenuItem = GameoverMenuItems.Length - 1;
        }

        foreach (MenuItem item in GameoverMenuItems)
        {
            item.DeselectItem();
        }

        GameoverMenuItems[CurrentGameoverMenuItem].SelectItem();
    }

    public void GameoverMoveCursorDown()
    {
        CurrentGameoverMenuItem = (CurrentGameoverMenuItem + 1) % GameoverMenuItems.Length;

        foreach (MenuItem item in GameoverMenuItems)
        {
            item.DeselectItem();
        }

        GameoverMenuItems[CurrentGameoverMenuItem].SelectItem();
    }

    public void GameoverSelectItem()
    {
        if (CurrentGameoverMenuItem == 0)
        {
            GameManager.Instance.SceneLoader.ReloadLevel();
        }

        if (CurrentGameoverMenuItem == 1)
        {
            Time.timeScale = 1f;
            GameManager.Instance.SceneLoader.LoadMainMenu();
        }

        if (CurrentGameoverMenuItem == 2)
        {
            Application.Quit();
        }
    }
    #endregion
}
