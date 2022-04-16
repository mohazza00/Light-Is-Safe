using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelType
{
    MAIN_MENU,
    CREDITS,
    OPTIONS,
}

public class MainMenu : MonoBehaviour
{
    public SceneLoader SceneLoader;

    [Header("Main Menu")]
    public GameObject ManiMenuPanel;
    public MenuItem[] MenuItems;
    public int CurrentMenuItem;

    [Header("Credits")]
    public GameObject CreditsPanel;

    // [Header("Options")]
    

    private PanelType _currentPanel;

    private void Start()
    {
        CurrentMenuItem = 0;
        MenuItems[CurrentMenuItem].SelectItem();
        _currentPanel = PanelType.MAIN_MENU;
    }

    private void Update()
    {
        switch (_currentPanel)
        {
            case PanelType.MAIN_MENU:
                if (Input.GetKeyDown(KeyCode.S)) MoveCursorDown();             
                if (Input.GetKeyDown(KeyCode.W)) MoveCursorUp();               
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.J)) SelectItem();        
                break;

            case PanelType.CREDITS:
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.J)) GoBackToMenu();
                break;
        }
        
       
    }

    private void MoveCursorUp()
    {
        CurrentMenuItem--;

        if (CurrentMenuItem < 0)
        {
            CurrentMenuItem = MenuItems.Length - 1;
        }

        foreach (MenuItem item in MenuItems)
        {
            item.DeselectItem();
        }

        MenuItems[CurrentMenuItem].SelectItem();
    }

    private void MoveCursorDown()
    {
        CurrentMenuItem = (CurrentMenuItem + 1) % MenuItems.Length;

        foreach (MenuItem item in MenuItems)
        {
            item.DeselectItem();
        }

        MenuItems[CurrentMenuItem].SelectItem();
    }

    private void SelectItem()
    {
        AudioManager.Instance.PlaySound(SoundName.SWITCH);

        if (CurrentMenuItem == 0)
        {
            if(LevelsManager.Instance.StoryShown)
                SceneLoader.LoadNextLevel();
            else
                SceneLoader.LoadStory();
        }

        else if(CurrentMenuItem == 1)
        {
            CreditsPanel.SetActive(true);
            _currentPanel = PanelType.CREDITS;
        }

        else if(CurrentMenuItem == 2)
        {
            Application.Quit();
        }
    }

    private void GoBackToMenu()
    {
        AudioManager.Instance.PlaySound(SoundName.SWITCH);

        CreditsPanel.SetActive(false);
        _currentPanel = PanelType.MAIN_MENU;
    }
}
