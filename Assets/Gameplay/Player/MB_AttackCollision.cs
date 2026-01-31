using UnityEngine;

public class MB_AttackCollision : MonoBehaviour
{
    [SerializeField] Collider2D attackHitBox;
    [SerializeField] Collider2D heavyAttackHitBox;

    void Awake()
    {
        if (attackHitBox != null)
            attackHitBox.enabled = false;
        if (heavyAttackHitBox != null)
            heavyAttackHitBox.enabled = false;
    }

    // Called by animation event
    public void EnableAttackHitbox()
    {
        if (attackHitBox != null)
            attackHitBox.enabled = true;
    }

    // Called by animation event
    public void DisableAttackHitbox()
    {
        if (attackHitBox != null)
            attackHitBox.enabled = false;
    }

    public void EnableHeavyAttackHitbox()
    {
        if (heavyAttackHitBox != null)
            heavyAttackHitBox.enabled = true;
    }

    // Called by animation event
    public void DisableHeavyAttackHitbox()
    {
        if (heavyAttackHitBox != null)
            heavyAttackHitBox.enabled = false;
    }
}
