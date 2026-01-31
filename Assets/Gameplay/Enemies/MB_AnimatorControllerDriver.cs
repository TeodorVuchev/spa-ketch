using UnityEngine;

public class MB_AnimatorControllerDriver : MonoBehaviour
{
    Animator anim;
    EnemyBehaviour enemyBehaviour;

    void Awake()
    {
        anim = GetComponent<Animator>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    void FixedUpdate()
    {
        anim.SetBool("Moving", false);
        anim.SetBool("Attack", false);
        anim.SetBool("Dead", false);
        
        switch (enemyBehaviour.GetCurrentState()) {
		case EnemyBehaviour.EnemyState.Idle:
            anim.SetBool("Moving", false);
			break;
		case EnemyBehaviour.EnemyState.Chasing:
			anim.SetBool("Moving", true);
			break;
		case EnemyBehaviour.EnemyState.Attacking:
			anim.SetTrigger("Attack");
			break;
        case EnemyBehaviour.EnemyState.Dead:
            anim.SetBool("Dead", true);
            break;
		default:
			break;
		}   
    }
}
