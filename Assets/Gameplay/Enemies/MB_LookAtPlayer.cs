using UnityEngine;

public class MB_LookAtPlayer : MonoBehaviour
{

    GameObject player;

    private void Awake()
    {
        player = FindFirstObjectByType<MB_PlayerController>().gameObject;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {}

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();
    }

    void LookAtPlayer()
    {
        if(transform.position.x <= player.transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            return;
        }
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
