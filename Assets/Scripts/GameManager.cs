using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool gameOver = false;


    // Update is called once per frame
    void Update()
    {
        if(Stats.Lives <= 0 && !gameOver)
        {
            gameOver = true;
            Debug.Log("Game Over");
        }
    }
}
