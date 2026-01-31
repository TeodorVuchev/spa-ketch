using UnityEngine;

public class MB_FollowPlayer : MonoBehaviour
{
    [SerializeField] float followSpeed = 20f;

    GameObject player;
    Rigidbody2D rb;

    private void Awake()
    {
        player = FindFirstObjectByType<MB_PlayerController>().gameObject;
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if (transform.position != player.transform.position)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * Time.fixedDeltaTime * followSpeed);
        }
    }
}
