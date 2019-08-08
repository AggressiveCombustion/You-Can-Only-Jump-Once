using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject pauseMenu;
    public bool paused = false;
    bool can_pause = true;

    public float speed = 1.0f;
    float savedSpeed = 1;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            PressPause();
        }
    }

    public void GoToNextLevel()
    {
        GameObject.Find("Fade").GetComponent<Animator>().SetTrigger("solid");
        TimerManager.instance.AddTimer(LoadNextScene, 1);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        GameObject.Find("Fade").GetComponent<Animator>().SetTrigger("solid");
        TimerManager.instance.AddTimer(ReloadScene, 1);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void PauseGame()
    {
        paused = true;
        pauseMenu.SetActive(true);
        savedSpeed = instance.speed;
        instance.speed = 0;

        can_pause = false;

        FindObjectOfType<PlayerController>().Pause();

        TimerManager.instance.AddTimer(EnablePauseButton, 0.2f, "enablePause", false);
    }

    public void UnpauseGame()
    {
        paused = false;
        pauseMenu.SetActive(false);
        instance.speed = savedSpeed;

        can_pause = false;

        FindObjectOfType<PlayerController>().Unpause();

        TimerManager.instance.AddTimer(EnablePauseButton, 0.2f, "enablePause", false);
    }

    void PressPause()
    {
        if(can_pause)
        {
            if (!paused)
            {
                PauseGame();
            }
            else
                UnpauseGame();
        }
    }

    void EnablePauseButton()
    {
        can_pause = true;
    }
}
