using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class MB_EnemySpawner : MonoBehaviour
{
    [SerializeField] SOS_EnemyWave[] Waves;
    [SerializeField] CinemachineCamera playerCamera;
    [SerializeField] GameObject player;

    [Header("Level Bounds")]
    [SerializeField] GameObject LeftBound;
    [SerializeField] GameObject RightBound;

    [Header("SpawnLocations")]
    [SerializeField] BoxCollider2D leftSpawnLocations;
    [SerializeField] BoxCollider2D rightSpawnLocations;

    bool startLevel = false;
    int maxWave;
    int currentWave;
    Vector3 cameraDamping;
    GameObject[] currentWaveLeft;
    GameObject[] currentWaveRight;
    List<GameObject> enemiesInWave = new List<GameObject>();

    private void Awake()
    {
        maxWave = Waves.Length-1;
        currentWave = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (startLevel)
        {
            enemiesInWave.RemoveAll(item => item == null);
            if (enemiesInWave.Count <= 0)
            {
                if (currentWave >= maxWave)
                {
                    startLevel = false;
                    EnableCamera();
                    return;
                }
                currentWave += 1;
                SpawnWave();
            }
        }
    }

    private void SpawnWave()
    {
        currentWaveLeft = Waves[currentWave].GetLeftSideEnemies();
        currentWaveRight = Waves[currentWave].GetRightSideEnemies();
        SpawnSide(currentWaveLeft, leftSpawnLocations);
        SpawnSide(currentWaveRight, rightSpawnLocations);
    }
    void SpawnSide(GameObject[] enemies, BoxCollider2D location)
    {
        foreach (GameObject enemy in enemies)
        {
            enemiesInWave.Add(Instantiate(enemy, RandomSpawnPoint(location), Quaternion.identity));
        }
    }

    Vector2 RandomSpawnPoint(BoxCollider2D box)
    {
        Vector2 localPoint = new Vector2(
            Random.Range(-box.size.x * 0.5f, box.size.x * 0.5f),
            Random.Range(-box.size.y * 0.5f, box.size.y * 0.5f)
        );

        return box.transform.TransformPoint(box.offset + localPoint);
    }

 

    void DisableCamera()
    {
        playerCamera.Follow = null;
    }
    void EnableCamera()
    {
        playerCamera.Follow = player.transform;
        LeftBound.SetActive(false);
        RightBound.SetActive(false);
    }

    public void InitiateLevelSequence()
    {
        playerCamera.CancelDamping(true);
        SpawnWave();
        startLevel = true;
        DisableCamera();
        LeftBound.SetActive(true);
        RightBound.SetActive(true);

    }
}
