using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : Singleton<LevelsManager>
{
    public string MainMenuScene = "MainMenu";
    public List<string> Levels = new List<string>();
    public int CurrentLevelIndex;

    public bool StoryShown;


    private void Start()
    {
        CurrentLevelIndex = 0;
    }

    public void ExitGame()
    {
        CurrentLevelIndex = 0;
    }

}
