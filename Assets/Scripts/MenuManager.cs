using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button soundBtn;
    public Button pauseBtn;
    [Header("Viewer Panels")]
    public GameObject cartridgeGo;
    public GameObject levelPanel;
    public GameObject levels;
    public GameObject backBtn;
    public GameObject playBtn;
    public GameObject pauseBtn0;
    public GameObject headingText;
    public List<GameObject> screenblocks;
    private bool isGamePaused = false;

    private void Awake()
    {
        if(SceneManager.GetActiveScene().name == "0_Home")
        {
            FindObjectOfType<AudioManager>().soundEffectAudio.loop = true;
            FindObjectOfType<AudioManager>().soundEffectAudio.clip = FindObjectOfType<AudioManager>().jingle;
            FindObjectOfType<AudioManager>().soundEffectAudio.Play();
        }
        else
        {
            FindObjectOfType<AudioManager>().soundEffectAudio.loop = false;
            FindObjectOfType<AudioManager>().soundEffectAudio.clip = null;
        }

        if (PlayerPrefs.GetInt("Mute") == 1)
        {
            FindObjectOfType<AudioManager>().soundEffectAudio.mute = true;
            //soundBtn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "SOUND: OFF";
        }
        else
        {
            FindObjectOfType<AudioManager>().soundEffectAudio.mute = false;
            //soundBtn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "SOUND: ON";
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SoundToggleButton()
    {
        AudioManager.Instance.soundEffectAudio.mute = !AudioManager.Instance.soundEffectAudio.mute;
        if (AudioManager.Instance.soundEffectAudio.mute)
        {
            PlayerPrefs.SetInt("Mute", 1);
            //soundBtn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "SOUND: OFF";
        }
        else
        {
            PlayerPrefs.SetInt("Mute", 0);
            //soundBtn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "SOUND: ON";
        }
    }

    public void SoundOffButton()
    {
        AudioManager.Instance.soundEffectAudio.mute = true;
        PlayerPrefs.SetInt("Mute", 1);
    }

    public void SoundOnButton()
    {
        AudioManager.Instance.soundEffectAudio.mute = false;
        PlayerPrefs.SetInt("Mute", 0);
    }

    public void PlayButton()
    {
        playBtn.SetActive(false);
        cartridgeGo.SetActive(false);
        backBtn.SetActive(true);
        levels.SetActive(true);
        levelPanel.SetActive(true);
        headingText.SetActive(false);
        foreach(GameObject a in screenblocks)
        {
            a.SetActive(true);
        }
    }

    public void BackButton()
    {
        playBtn.SetActive(true);
        cartridgeGo.SetActive(true);
        backBtn.SetActive(false);
        levels.SetActive(false);
        levelPanel.SetActive(false);
        headingText.SetActive(true);
        foreach (GameObject a in screenblocks)
        {
            a.SetActive(false);
        }
    }

    public void HomeButton()
    {
        SceneManager.LoadScene("0_Home");
    }

    public void PauseButton()
    {
        isGamePaused = !isGamePaused;
        if(isGamePaused)
        {
            Time.timeScale = 0;
            // Disable scripts that still work while timescale is set to 0
            FindObjectOfType<GameHandler>().enabled = false;
            if (FindObjectOfType<GameHandler>().snake != null)
                FindObjectOfType<GameHandler>().snake.enabled = false;

            //pauseBtn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "RESUME";
            // pauseBtn0.SetActive(false);
            // playBtn.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            // Disable scripts that still work while timescale is set to 0
            FindObjectOfType<GameHandler>().enabled = true;
            if (FindObjectOfType<GameHandler>().snake != null)
                FindObjectOfType<GameHandler>().snake.enabled = true;

            //pauseBtn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "PAUSE";
            // pauseBtn0.SetActive(true);
            // playBtn.SetActive(false);
        }
        
    }

    // Load Level
    public void LoadLevel1()
    {
        SceneManager.LoadScene("1_Snake");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("2_Space_In");
    }

    public void LoadLevel3()
    {

    }
}
