using System.Collections;
using UnityEngine;

public class MB_Health : MonoBehaviour
{
    [SerializeField] float health = 50f;
    [SerializeField] float pushSpeed = 50f;
    [SerializeField] float totalDamagedTime = 0.5f;

    [Header ("Player Specific")]
    [SerializeField] float invincibilityTime = 0.5f;
    [SerializeField] MB_DeathScreen deathScreen;
    [SerializeField] MB_UIInGame healthBar;
    bool invincibilityFrame = false;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator playerAnimator;

    bool hit = false;
    Vector2 moveDirection = Vector3.zero;
    Coroutine currentRoutine;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        if(gameObject.tag == "Player")
        {
            playerAnimator = GetComponent<Animator>();
        }
    }

    private void FixedUpdate()
    {
        if (hit)
        {
            MoveOnHit();
        }
    }

    public float GetCurrentHealth()
    {
        return health;
    }

    public void DealDamage(float damage)
    {
        if(invincibilityFrame){ return; }

        health -= damage;

        if(gameObject.tag == "Player")
        {
            healthBar.UpdateHealthBar(health);
        }

        
        moveDirection = new Vector2(-transform.localScale.x, 0f);

        if(gameObject.tag != "Player")
        {
            if (currentRoutine != null) { StopCoroutine(currentRoutine); }
            currentRoutine = StartCoroutine(TimeToMove());
        }


            LeanTween.value(gameObject, UpdateColor, Color.white, Color.red, 0.2f)
                .setEaseOutBack().setLoopPingPong(1);

        if (health <= 0f)
        {
            if (gameObject.tag == "Player")
            {
                deathScreen.ShowLooseScreen();
                playerAnimator.Play("Death");
                Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
                foreach(Collider2D collider in colliders)
                {
                    collider.enabled = false;
                }
                return;
            }

            if (gameObject.tag == "Enemy" && TryGetComponent<EnemyBehaviour>(out var enemy))
            {
                Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
                foreach(Collider2D collider in colliders)
                {
                    collider.enabled = false;
                }
                enemy.SetDead();
                return;
            }

            Kill();
            return;
        }
        if(gameObject.tag == "Player")
        {
            StartCoroutine(InvincibilityTime());
        }
    }

    void Kill()
    {
        if(gameObject.tag == "Player")
        {
            deathScreen.ShowLooseScreen();
        }
        GameObject.Destroy(gameObject);
    }

    IEnumerator TimeToMove()
    {
        hit = true;
        yield return new WaitForSecondsRealtime(totalDamagedTime);
        hit = false;
    }
    IEnumerator InvincibilityTime()
    {
        yield return new WaitForSecondsRealtime(2f);
        invincibilityFrame = false;
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
