using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action OnPlatformSpawned = delegate { };
    public static event Action OnGameEnded = delegate { };
    public static List<GameObject> platforms = new List<GameObject>();
    public static PlatformSpawner[] platformSpawners;

    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject bestScore;

    private PlatformSpawner currentSpawner;
    private int previousSpawnerIndex;
    private int spawnerIndex;


    private void Start()
    {
        platformSpawners = FindObjectsOfType<PlatformSpawner>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (MovingPlatform.currentPlatform != null)
            {
                MovingPlatform.currentPlatform.Stop();
                //Taptick.CreateOneShot(200);
            }

            do
            {
                spawnerIndex = UnityEngine.Random.Range(0, platformSpawners.Length);
            }
            while (spawnerIndex == previousSpawnerIndex);
            previousSpawnerIndex = spawnerIndex;

            currentSpawner = platformSpawners[spawnerIndex];

            if (MovingPlatform.currentPlatform != null)
            {
                currentSpawner.SpawnPlatform();

                foreach (GameObject item in platforms)
                {
                    if (platforms.Count > 2)
                    {
                        item.transform.position -= new Vector3(0, MovingPlatform.currentPlatform.transform.localScale.y, 0);
                    }
                }
                OnPlatformSpawned();
            }
            else
            {
                MovingPlatform.lastPlatform.gameObject.AddComponent<Rigidbody>();
                OnGameEnded();
                gameOver.SetActive(true);
            }
            if (bestScore.activeSelf)
            {
                bestScore.SetActive(false);
            }
        }

    }
    public void ReloadLevel()
    {
        platforms.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
