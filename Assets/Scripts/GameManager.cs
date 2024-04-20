using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // game state
    public bool isActive = true;
    private int lives;
    private int fruitsCollected;
    private bool playerHasFruitBoost = false;

    // UI
    public TextMeshProUGUI livesCountUI;
    public TextMeshProUGUI fruitCountUI;
    public TextMeshProUGUI gameOverFruitTotalUI;
    public TextMeshProUGUI pausedLivesCounterUI;
    public TextMeshProUGUI pausedFruitCounterUI;
    public TextMeshProUGUI levelSummaryLivesCounterUI;
    public TextMeshProUGUI levelSummaryFruitCounterUI;
    public GameObject musicMuteToggleUI;
    public GameObject sfxMuteToggleUI;
    public GameObject chestHint;
    public GameObject abilityHint;
    public GameObject hud;
    public GameObject gameOverOverlay;
    public GameObject pauseMenuOverlay;
    public GameObject levelSummaryOverlay;
    public AudioSource mainCameraAudio;

    private Toggle musicMuteToggleComponent;
    private Toggle sfxMuteToggleComponent;
    private bool isMusicMuted = false;
    private bool isSfxMuted = false;

    // Start is called before the first frame update
    void Start()
    {
        musicMuteToggleComponent = musicMuteToggleUI.GetComponent<Toggle>();
        sfxMuteToggleComponent = sfxMuteToggleUI.GetComponent<Toggle>();
        if (MainManager.Instance)
        {
            lives = MainManager.Instance.LivesCarriedOver > 0 ? MainManager.Instance.LivesCarriedOver : 9;
            fruitsCollected = MainManager.Instance.FruitCarriedOver > 0 ? MainManager.Instance.FruitCarriedOver : 0;
            isMusicMuted = MainManager.Instance.IsMusicMute;
            isSfxMuted = MainManager.Instance.IsSfxMute;
        } else
        {
            lives = 9;
            fruitsCollected = 0;
        }
        mainCameraAudio.mute = isMusicMuted;
        musicMuteToggleComponent.SetIsOnWithoutNotify(isMusicMuted);
        sfxMuteToggleComponent.SetIsOnWithoutNotify(isSfxMuted);
        if (livesCountUI && fruitCountUI)
        {
            livesCountUI.text = lives.ToString();
            fruitCountUI.text = fruitsCollected.ToString();
        }
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

    public void SetFruitBoost()
    {
        playerHasFruitBoost = true;
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
        //playerAs.mute = true;
        isActive = false;
        Time.timeScale = 0;
        levelSummaryLivesCounterUI.text = lives.ToString();
        levelSummaryFruitCounterUI.text = fruitsCollected.ToString();
        levelSummaryOverlay.SetActive(true);
    }

    public void ResumeGame()
    {
        isActive = true;
        hud.SetActive(true);
        pauseMenuOverlay.SetActive(false);
    }

    public void GoToMainMenu()
    {
        CleanupGame();
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        CleanupGame();
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


    public void ToggleMusicMute()
    {
        mainCameraAudio.mute = !mainCameraAudio.mute;
        MainManager.Instance.IsMusicMute = mainCameraAudio.mute;
        Debug.Log("Toggled: " + MainManager.Instance.IsMusicMute);
    }

    public void ToggleSfxMute()
    {
        MainManager.Instance.IsSfxMute = !MainManager.Instance.IsSfxMute;
    }

    private void CheckForOneUp()
    {
        int factor = playerHasFruitBoost ? 4 : 5;
        if (fruitsCollected % factor == 0)
        {
            UpdateLivesCount(1);
        }
    }

    private void CleanupGame()
    {
        lives = 9;
        fruitsCollected = 0;
        livesCountUI.text = "9";
        fruitCountUI.text = "0";
        MainManager.Instance.LivesCarriedOver = 9;
        MainManager.Instance.FruitCarriedOver = 0;
        MainManager.Instance.SpeedBoostUnlocked = false;
        MainManager.Instance.RecoveryBoostUnlocked = false;
        MainManager.Instance.FruitBoostUnlocked = false;
    }
}
