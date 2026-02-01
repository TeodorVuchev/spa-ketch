using UnityEngine;

public class MB_EnemyAudio : MonoBehaviour
{
    AudioSource enemyAudio;
    float volumeRemember;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        enemyAudio = GetComponent<AudioSource>();
        volumeRemember = enemyAudio.volume;
        enemyAudio.volume = 0f;
        Invoke("EnableAudio", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnableAudio()
    {
        enemyAudio.volume = volumeRemember;
    }

    public void Punching()
    {
        enemyAudio.Play();
    }
}
