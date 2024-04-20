using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;

public class MenuHandler : MonoBehaviour
{

    public List<GameObject> catDisplays;
    public GameObject guideDialog;
    private int currentCatIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        catDisplays[currentCatIndex].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // start the game at level 1
    public void PlayGame()
    {
        // save selected cat
        MainManager.Instance.SelectedCat = currentCatIndex;
        SceneManager.LoadScene(1);
    }

    // close the game
    public void Exit()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit(); // original code to quit Unity player
        #endif
    }

    /**
     * Change cat display for cat selection
     * @param [int] dir the direction to scroll
     */
    public void ScrollCat(int dir)
    {
        catDisplays[currentCatIndex].SetActive(false);

        currentCatIndex += dir;
        if (currentCatIndex >= catDisplays.Count)
        {
            currentCatIndex = 0;
        } else if (currentCatIndex < 0)
        {
            currentCatIndex = catDisplays.Count - 1;
        }
        catDisplays[currentCatIndex].SetActive(true);
    }

    /**
     * Toggle how to play guide
     */
    public void ToggleHowToDialog(bool toggle)
    {
        guideDialog.SetActive(toggle);
    }

}
