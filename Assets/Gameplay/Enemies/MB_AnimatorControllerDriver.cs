using UnityEngine;

public class MB_AnimatorControllerDriver : MonoBehaviour
{
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
        anim.SetBool("Moving", currentState == EnemyBehaviour.EnemyState.Chasing);
        anim.SetBool("Dead", currentState == EnemyBehaviour.EnemyState.Dead);

        if (currentState != prevState)
        {
            if (currentState == EnemyBehaviour.EnemyState.Attacking)
                anim.SetTrigger("Attack");

            prevState = currentState;
        }
    }
}
