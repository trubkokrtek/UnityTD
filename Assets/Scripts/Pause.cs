using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject Camera;
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.Escape) || Input.GetKey("p")) && !isPaused)
        {
            PauseGame();
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKey("p")) && isPaused)
        {
            Unpause();
            
        }
    }
    void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        this.GetComponent<Stats>().pauseGame();
        Camera.GetComponent<CameraController>().doMovement = false;
    }
    public void Unpause()
    {
        Time.timeScale = 1f;
        isPaused = false;
        this.GetComponent<Stats>().Continue();
        Camera.GetComponent<CameraController>().doMovement = true;
    }
}
