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

    bool startLevel = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
        print("NotWorking");
        startLevel = true;
        DisableCamera();
        LeftBound.SetActive(true);
        RightBound.SetActive(true);

        Invoke("EnableCamera", 3f);
    }
}
