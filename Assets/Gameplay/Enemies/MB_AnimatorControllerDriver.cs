using UnityEngine;

public class MB_AnimatorControllerDriver : MonoBehaviour
{
    [SerializeField] float attackRepeatDelay = 0.2f; // tune
    float attackTimer;

    Animator anim;
    EnemyBehaviour enemyBehaviour;
    EnemyBehaviour.EnemyState currentState;
    EnemyBehaviour.EnemyState prevState;

    void Awake()
    {
        anim = GetComponent<Animator>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    void Update()
    {
        currentState = enemyBehaviour.GetCurrentState();

        anim.SetBool("Dead", currentState == EnemyBehaviour.EnemyState.Dead);

        // Prefer movement intent/velocity over raw state if you can (prevents flicker)
        anim.SetBool("Moving", currentState == EnemyBehaviour.EnemyState.Chasing);

        if (currentState == EnemyBehaviour.EnemyState.Attacking)
        {
            attackTimer -= Time.deltaTime;

            // Only fire Attack when we're not already playing Attack (or transitioning into it)
            if (attackTimer <= 0f && !IsInAttackAnimOrTransition())
            {
                anim.ResetTrigger("Attack"); // optional safety
                anim.SetTrigger("Attack");
                attackTimer = attackRepeatDelay; // small delay before next attempt
            }
        }
        else
        {
            attackTimer = 0f; // reset when leaving attack mode
        }
    }

    bool IsInAttackAnimOrTransition()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
    }
}
