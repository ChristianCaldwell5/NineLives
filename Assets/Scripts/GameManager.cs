using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    // game state
    public bool isActive = true;
    private int lives;
    private int fruitsCollected;

    // UI
    public TextMeshProUGUI livesCountUI;
    public TextMeshProUGUI fruitCountUI;
    public TextMeshProUGUI gameOverFruitTotalUI;
    public TextMeshProUGUI pausedLivesCounterUI;
    public TextMeshProUGUI pausedFruitCounterUI;
    public TextMeshProUGUI levelSummaryLivesCounterUI;
    public TextMeshProUGUI levelSummaryFruitCounterUI;
    public GameObject chestHint;
    public GameObject abilityHint;
    public GameObject hud;
    public GameObject gameOverOverlay;
    public GameObject pauseMenuOverlay;
    public GameObject levelSummaryOverlay;

    // Start is called before the first frame update
    void Start()
    {
        //lives = MainManager.Instance.LivesCarriedOver > 0 ? MainManager.Instance.LivesCarriedOver : 9;
        //fruitsCollected = MainManager.Instance.FruitCarriedOver > 0 ? MainManager.Instance.FruitCarriedOver : 0;
        lives = 9;
        fruitsCollected = 0;
        livesCountUI.text = lives.ToString();
        fruitCountUI.text = fruitsCollected.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public int GetLivesCount()
    {
        return lives;
    }

    public int GetFruitCollectedCOunt()
    {
        return fruitsCollected;
    }

    public void IncrementFruitCount()
    {
        fruitsCollected++;
        fruitCountUI.text = fruitsCollected.ToString();
        // check for extra health
        CheckForOneUp();
    }

    public void UpdateLivesCount(int amount)
    {
        lives += amount;
        livesCountUI.text = lives.ToString();
    }

    public void InitiateGameOver()
    {
        isActive = false;
        gameOverFruitTotalUI.text = fruitsCollected.ToString();
        hud.SetActive(false);
        gameOverOverlay.SetActive(true);
    }

    public void PauseGame()
    {
        hud.SetActive(false);
        pausedLivesCounterUI.text = lives.ToString();
        pausedFruitCounterUI.text = fruitsCollected.ToString();
        pauseMenuOverlay.SetActive(true);
        isActive = false;
        Time.timeScale = 0;
    }

    public void ShowLevelSummary()
    {
        isActive = false;
        Time.timeScale = 0;
        levelSummaryLivesCounterUI.text = lives.ToString();
        levelSummaryFruitCounterUI.text = fruitsCollected.ToString();
        levelSummaryOverlay.SetActive(true);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadNextLevel() 
    {
        isActive = true;
        Time.timeScale = 1;
        MainManager.Instance.LivesCarriedOver = GetLivesCount();
        MainManager.Instance.FruitCarriedOver = GetFruitCollectedCOunt();
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex+1);
    }

    public void ToggleChestHint()
    {
        chestHint.SetActive(false);
        abilityHint.SetActive(true);
    }

    private void CheckForOneUp()
    {
        if (fruitsCollected % 5 == 0)
        {
            UpdateLivesCount(1);
        }
    }
}
