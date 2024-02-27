using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public TextMeshProUGUI dialogHeadingUI;
    public GameObject confirmationDialog;
    public GameObject hudPanel;
    private GameManager gameManager;
    // 0 = restart. Anything else = quit
    private int action;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameManager.isActive = true;
        hudPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    // initiate confirmation dialog with customized text
    public void InitiateConfirmationDialog(int opt)
    {
        string text;
        action = opt;
        if (action == 0)
        {
            text = "restart";
        }
        else
        {
            text = "quit";
        }
        // show confirmation dialog
        dialogHeadingUI.text = text;
        confirmationDialog.SetActive(true);
    }

    public void YesButtonPress()
    {
        Time.timeScale = 1;
        if (action == 0)
        {
            gameManager.RestartGame();
        } else
        {
            gameManager.GoToMainMenu();
        }
    }

    public void NoButtonPress()
    {
        confirmationDialog.SetActive(false);
    }
}
