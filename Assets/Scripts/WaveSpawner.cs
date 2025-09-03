using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.UIElements;

public class WaveSpawner : MonoBehaviour
{

    public List<Array> waves;
    public int enemiesAlive = 0;
    public Transform Spawner;
    public float WaitTime = 60f;
    public int WaveNumber = 0;
    public Stats stats;

    [Header("UI Elements")]
    public TextMeshProUGUI waveCountdownText;
    public TextMeshProUGUI waveNumberText;
    public TextMeshProUGUI enemiesAliveText;
    public TextMeshProUGUI waveInfoText;

    [Header("Enemies prefabs")]
    public Transform normalEnemyPrefab; //normal enemy - n
    public Transform fastEnemyPrefab; //fast but weak - f
    public Transform strongEnemyPrefab; //strong but slow - s
    public Transform armorEnemyPrefab; //gives resistance to other enemies - a
    public Transform healEnemyPrefab; //heals other enemies - h
    public Transform fighterEnemyPrefab; //splits into two upon death - d
    public Transform bossEnemyPrefab; //project B-055 - b

    private bool start;
    private float countdown = 2f;
    void Start()
    {
        start = false;
        waves = new List<Array>();
        waves.Add(new string[] { "5,n", "1" }); //wave 1
        waves.Add(new string[] { "5,n", "1" }); //wave 2
        waves.Add(new string[] { "5,n", "1" }); //wave 3
        waves.Add(new string[] { "5,n", "1" }); //wave 4
        waves.Add(new string[] { "7,n", "1" }); //wave 5
        waves.Add(new string[] { "7,n", "1" }); //wave 6
        waves.Add(new string[] { "7,n", "1" }); //wave 7
        waves.Add(new string[] { "10,n", "1" }); //wave 8
        waves.Add(new string[] { "10,n", "1" }); //wave 9
        waves.Add(new string[] { "1,b", "1" }); //wave 10
        waves.Add(new string[] { "5,f", "1,25" }); //wave 11
        waves.Add(new string[] { "5,s", "1,25" }); //wave 12
        waves.Add(new string[] { "3,n", "5,f", "1,25" }); //wave 13
        waves.Add(new string[] { "3,n", "5,s", "1,25" }); //wave 14
        waves.Add(new string[] { "5,f","5,s", "1,25" }); //wave 15
        waves.Add(new string[] { "5,n", "5,f", "5,s", "1,25" }); //wave 16
        waves.Add(new string[] { "7,f", "7,s", "1,25" }); //wave 17
        waves.Add(new string[] { "7,n", "7,f", "7,s", "1,25" }); //wave 18
        waves.Add(new string[] { "5,n", "10,f", "10,s", "1,25" }); //wave 19
        waves.Add(new string[] { "1,b", "1,25" }); //wave 20
        waves.Add(new string[] { "5,a", "5,n", "1,5" }); //wave 21
        waves.Add(new string[] { "5,h", "5,n", "1,5" }); //wave 22
        waves.Add(new string[] { "5,a", "5,s", "1,5" }); //wave 23
        waves.Add(new string[] { "5,h", "5,s", "1,5" }); //wave 24
        waves.Add(new string[] { "5,a", "5,f", "5,n", "1,5" }); //wave 25
        waves.Add(new string[] { "5,h", "5,f", "5,n", "1,5" }); //wave 26
        waves.Add(new string[] { "5,a", "5,f", "10,s", "1,5" }); //wave 27
        waves.Add(new string[] { "5,h", "5,f", "10,s", "1,5" }); //wave 28
        waves.Add(new string[] { "7,a", "7,f", "5,s", "5,f", "5,n", "1,5" }); //wave 29
        waves.Add(new string[] { "1,b", "1,5" }); //wave 30
        waves.Add(new string[] { "5,d", "5,n", "1,75" }); //wave 31
        waves.Add(new string[] { "5,d", "5,s", "1,75" }); //wave 32
        waves.Add(new string[] { "5,d", "5,f", "1,75" }); //wave 33
        waves.Add(new string[] { "5,d", "5,a", "1,75" }); //wave 34
        waves.Add(new string[] { "5,d", "5,h", "1,75" }); //wave 35
        waves.Add(new string[] { "5,d", "5,n", "5,f", "1,75" }); //wave 36
        waves.Add(new string[] { "5,d", "5,s", "5,f", "1,75" }); //wave 37
        waves.Add(new string[] { "5,d", "5,s", "5,a", "1,75" }); //wave 38
        waves.Add(new string[] { "5,d", "5,a", "5,h", "5,s", "1,75" }); //wave 39
        waves.Add(new string[] { "1,b", "1,75" }); //wave 40
        waves.Add(new string[] { "5,d", "5,n", "5,f", "5,s", "2" }); //wave 41
        waves.Add(new string[] { "5,d", "5,n", "5,f", "5,s", "5,a", "2" }); //wave 42
        waves.Add(new string[] { "5,d", "5,n", "5,f", "5,s", "5,h", "2" }); //wave 43
        waves.Add(new string[] { "5,d", "5,n", "5,f", "5,s", "5,a", "5,h", "2" }); //wave 44
        waves.Add(new string[] { "7,d", "7,n", "7,f", "7,s", "7,a", "7,h", "2" }); //wave 45
        waves.Add(new string[] { "7,d", "7,n", "7,f", "7,s", "7,a", "7,h", "2" }); //wave 46
        waves.Add(new string[] { "10,d", "10,n", "10,f", "10,s", "7,a", "7,h", "2" }); //wave 47
        waves.Add(new string[] { "10,d", "10,n", "10,f", "10,s", "10,a", "10,h", "2" }); //wave 48
        waves.Add(new string[] { "10,d", "10,n", "10,f", "10,s", "10,a", "10,h", "2" }); //wave 49
        waves.Add(new string[] { "1,b", "2" }); //wave 50
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !start)
        {
            start = true;
            countdown = 10f;
        }
        else if (Input.GetKeyDown(KeyCode.F) && start)
        {
            countdown = 0;
        }
        if (!start)
        {
            waveCountdownText.text = "Press F to start the game";
            waveNumberText.text = "Wave: 0";
            enemiesAliveText.text = "Enemies remaining:\n0";
            return;
        }
        if (countdown <= 0f || enemiesAlive == 0)
        {
            WaveNumber++;
            StartCoroutine(SpawnWave());
            countdown = WaitTime;

        }
        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        waveCountdownText.text = string.Format("{0:00.00}", countdown);
        if (countdown <= 5f)
        {
            waveCountdownText.color = Color.red;
        }
        else
        {
            waveCountdownText.color = Color.white;
            
        }
        enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        waveNumberText.text = WaveNumber.ToString();
        enemiesAliveText.text = $"Enemies remaining:\n{enemiesAlive}";
        waveInfoText.text = $"Wave {WaveNumber}";
        if (WaveNumber > waves.Count && enemiesAlive == 0)
        {
            stats.Win();
            start = false;
            return;
        }

    }
    IEnumerator SpawnWave()
    {
        string[] wave = null;
        float HpModifier = 0f;
        Transform enemyPrefab;
        Transform enemy;
        try
        {
            Debug.Log($"Wave {WaveNumber} Incoming");
            wave = (string[])waves[WaveNumber - 1];
            HpModifier = float.Parse(wave[wave.Length - 1]);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error spawning wave {WaveNumber}: {e.Message}");
        }
        finally
        {
            if (WaveNumber > waves.Count)
            {
                Debug.Log("No more waves to spawn.");
            }
        }
            if(waveCountdownText == null)
            {
            yield return null;
            }
            for (int i = 0; i < wave.Length - 1; i++)
            {

                string[] enemyInfo = wave[i].Split(",");
                int enemyCount = int.Parse(enemyInfo[0]);
                switch (enemyInfo[1])
                {
                    case "n":
                        enemyPrefab = normalEnemyPrefab;
                        break;
                    case "f":
                        enemyPrefab = fastEnemyPrefab;
                        break;
                    case "s":
                        enemyPrefab = strongEnemyPrefab;
                        break;
                    case "a":
                        enemyPrefab = armorEnemyPrefab;
                        break;
                    case "h":
                        enemyPrefab = healEnemyPrefab;
                        break;
                    case "d":
                        enemyPrefab = fighterEnemyPrefab;
                        break;
                    case "b":
                        enemyPrefab = bossEnemyPrefab;
                        break;
                    default:
                        enemyPrefab = normalEnemyPrefab;
                        break;
                }
                for (int j = 0; j < enemyCount; j++)
                {
                    enemy = Instantiate(enemyPrefab, Spawner.position, Spawner.rotation);
                    enemy.GetComponent<Enemy>().baseHealth *= HpModifier;
                    enemy.GetComponent<Enemy>().Health *= HpModifier;
                    yield return new WaitForSeconds(0.5f);
                }
            }
    }
}
