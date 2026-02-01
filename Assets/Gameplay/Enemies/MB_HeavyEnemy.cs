using UnityEngine;

public class MB_HeavyEnemy : EnemyBehaviour
{
    [SerializeField]
    public float idleWaitTimeMin = 0.0f;

    [SerializeField]
    public float idleWaitTimeMax = 10.0f;

    // Setting the default high for easier debugging (in theory)
    private float idleWaitTimeDefault = 40.0f;
    private float idleWaitTime = 40.0f;

    [SerializeField] float followSpeed = 10.0f;
    [SerializeField] float laneTolerance = 0.35f;     // tune this
    [SerializeField] float ySteerWeight = 0.35f;      // 0..1 (how much Y matters vs X)
    [SerializeField] float attackRangeX = 0.5f;
    [SerializeField] float attackRangeY = 0.45f;

    [SerializeField] float disengageExtraX = 0.35f;  // hysteresis (tune)
    [SerializeField] float disengageExtraY = 0.20f;
    float laneTargetY;        // enemy’s chosen lane near the player
    float laneRepathTimer;    // when to pick a new lane target

    protected new void Awake() 
    {
        base.Awake();
        idleWaitTimeDefault = Random.Range(idleWaitTimeMin, idleWaitTimeMax);
        idleWaitTime = idleWaitTimeDefault;
    }

    public override void Idle() 
    {
        idleWaitTime -= Time.deltaTime;
        if (idleWaitTime <= 0.0f) {
            LookAtPlayer();
            currentState = EnemyState.Chasing;
            idleWaitTime = idleWaitTimeDefault;
        }
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

        // If close enough, attack (needs both X & Y near-ish, not exact)
/*         if (Mathf.Abs(dx) <= attackRangeX && Mathf.Abs(player.transform.position.y - pos.y) <= attackRangeY)
        {
            currentState = EnemyState.Attacking;
            return;
        } */
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
            currentState = EnemyState.Idle;   // usually better than Idle
            return;
        }

        // Optional: keep facing player while attacking
        LookAtPlayer();

        // Optional: if you want them to "micro-step" into lane before continuing attacks:
        // (only do this if it looks good in your game)
        if (Mathf.Abs(dy) > attackRangeY * 0.9f) currentState = EnemyState.Chasing;
    }

	public override void Dead() {}

    // TODO: Abstract away maybe
    void FollowPlayer() 
    {
        if (transform.position != player.transform.position)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * Time.fixedDeltaTime * followSpeed);
        }
    }

    // TODO: Abstract away maybe
    void LookAtPlayer()
    {
        if (transform.position.x <= player.transform.position.x)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            return;
        }
        transform.localScale = new Vector3(-1f, 1f, 1f);
    }
}
