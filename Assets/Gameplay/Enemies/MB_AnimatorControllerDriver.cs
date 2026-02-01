using UnityEngine;

public class MB_AnimatorControllerDriver : MonoBehaviour
{
    [SerializeField] float attackRepeatDelay = 0.2f; // tune

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

        anim.SetBool("Charge", currentState == EnemyBehaviour.EnemyState.Charging);

        if (currentState == EnemyBehaviour.EnemyState.Attacking)
        {
            // Only fire Attack when we're not already playing Attack (or transitioning into it)
            if (!IsInAttackAnimOrTransition())
            {
                // anim.ResetTrigger("Attack"); // optional safety
                anim.SetTrigger("Attack");
                // anim.SetBool("Charge", false);
            }
        }
    }

    bool IsInAttackAnimOrTransition()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }
}
