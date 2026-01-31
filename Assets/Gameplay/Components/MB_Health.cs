using System.Collections;
using UnityEngine;

public class MB_Health : MonoBehaviour
{
    [SerializeField] float health = 50f;
    [SerializeField] float pushSpeed = 50f;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    bool hit = false;
    Vector2 moveDirection = Vector3.zero;
    Coroutine currentRoutine;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (hit)
        {
            MoveOnHit();
        }
    }
    public void DealDamage(float damage)
    {
        health -= damage;
        
        moveDirection = new Vector2(transform.localScale.x, 0f);

        if (currentRoutine != null) { StopCoroutine(currentRoutine); }
        currentRoutine = StartCoroutine(TimeToMove());

        LeanTween.value(gameObject, UpdateColor, Color.white, Color.red, 0.2f)
            .setEaseLinear().setLoopPingPong(1);

        print("Test");
        if (health < 0)
        {
            GameObject.Destroy(gameObject);
        }
    }

    IEnumerator TimeToMove()
    {
        hit = true;
        yield return new WaitForSecondsRealtime(0.5f);
        hit = false;
    }

    void MoveOnHit()
    {
        rb.MovePosition(rb.position + moveDirection * Time.fixedDeltaTime * pushSpeed);
    }

    private void UpdateColor(Color col)
    {
        spriteRenderer.color = col;
    }
}
