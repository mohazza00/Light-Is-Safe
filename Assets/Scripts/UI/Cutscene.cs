using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CutsceneType
{
    STORY,
    THANKS,
}
public class Cutscene : MonoBehaviour
{
    public CutsceneType CutsceneType;
    public GameObject[] Panels;
    public int CurrentPanel;
    public bool CanSkip;
    public SceneLoader SceneLoader;

    private void Start()
    {
        CurrentPanel = 0;
        CanSkip = true;
        Panels[CurrentPanel].SetActive(true);

    }
    private void Update()
    {
        if (CurrentPanel >= Panels.Length)
            return;

        if (!CanSkip)
            return;


        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(ShowNextPanel());
        }
    }

    IEnumerator ShowNextPanel()
    {
        Panels[CurrentPanel].GetComponent<Animator>().SetBool("Hide", true);
        yield return new WaitForSeconds(0.5f);
        CurrentPanel++;
        if(CurrentPanel >= Panels.Length)
        {

            if (CutsceneType == CutsceneType.STORY)
            {
                LevelsManager.Instance.StoryShown = true;
                SceneLoader.LoadNextLevel();
            }
            else if (CutsceneType == CutsceneType.THANKS)
            {
                LevelsManager.Instance.StoryShown = false;
                SceneLoader.LoadMainMenu();
            }
               
        }
        else
        {
            Panels[CurrentPanel].SetActive(true);
            yield return new WaitForSeconds(0.5f);
            CanSkip = true;
        }
    
    }
}
