using UnityEngine;

public class MB_AttackCollision : MonoBehaviour
{
    [SerializeField] Collider2D attackHitBox;
    [SerializeField] Collider2D heavyAttackHitBox;

    void Awake()
    {
        attackHitBox.enabled = false;
        heavyAttackHitBox.enabled = false;
    }

    // Called by animation event
    public void EnableAttackHitbox()
    {
        attackHitBox.enabled = true;
    }

    // Called by animation event
    public void DisableAttackHitbox()
    {
        attackHitBox.enabled = false;
    }
    public void EnableHeavyAttackHitbox()
    {
        heavyAttackHitBox.enabled = true;
    }

    // Called by animation event
    public void DisableHeavyAttackHitbox()
    {
        heavyAttackHitBox.enabled = false;
    }
}
