using UnityEngine;

public class MB_TriggerLevel : MonoBehaviour
{
    [SerializeField] MB_EnemySpawner spawner;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            spawner.InitiateLevelSequence();
            gameObject.SetActive(false);
        }
    }
}
