using UnityEngine;

public class MB_LightEnemy : EnemyBehaviour
{
    protected new void Awake() 
    {
        base.Awake();
    }

    public override void Chasing()
    {
        // Pick/refresh a lane target occasionally (prevents constant perfect tracking)
        laneRepathTimer -= Time.fixedDeltaTime;
        if (laneRepathTimer <= 0f)
        {
            // offset makes enemies not stack exactly on player Y
            laneTargetY = player.transform.position.y + Random.Range(-0.35f, 0.35f);
            laneRepathTimer = Random.Range(0.6f, 1.4f);
        }

        Vector2 pos = rb.position;
        float dx = player.transform.position.x - pos.x;
        float dyToLane = laneTargetY - pos.y;

        bool inLane = Mathf.Abs(dyToLane) <= laneTolerance;

        float dy = player.transform.position.y - pos.y;
        if (Mathf.Abs(dx) <= attackRangeX && Mathf.Abs(dy) <= attackRangeY)
        {
            currentState = EnemyState.Attacking;
            return;
        }
        // Movement:
        // - Always advance in X.
        // - Apply limited Y correction, stronger when not in lane.
        float yFactor = inLane ? 0.15f : ySteerWeight;  // small drift once in lane
        Vector2 step = new Vector2(Mathf.Sign(dx), Mathf.Clamp(dyToLane, -1f, 1f) * yFactor).normalized;

        rb.MovePosition(pos + step * followSpeed * Time.fixedDeltaTime);
    }

	public override void Attacking() 
    {
        Vector2 pos = rb.position;

        float dx = player.transform.position.x - pos.x;
        float dy = player.transform.position.y - pos.y;

        bool inAttackRange =
            Mathf.Abs(dx) <= attackRangeX &&
            Mathf.Abs(dy) <= attackRangeY;

        bool shouldDisengage =
            Mathf.Abs(dx) > attackRangeX + disengageExtraX ||
            Mathf.Abs(dy) > attackRangeY + disengageExtraY;

        // If player moved away enough, stop attacking so the animation driver stops re-triggering.
        if (!inAttackRange && shouldDisengage)
        {
            currentState = EnemyState.Idle;
            return;
        }

        // Optional: keep facing player while attacking
        LookAtPlayer();
    }

	public override void Dead() {}


    protected override void LookAtPlayer()
    {
        if(transform.position.x <= player.transform.position.x)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            return;
        }
        transform.localScale = new Vector3(-1f, 1f, 1f);
    }
}
