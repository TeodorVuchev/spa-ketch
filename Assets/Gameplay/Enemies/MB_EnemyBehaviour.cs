using UnityEngine;
using System.Collections;

public abstract class EnemyBehaviour : MonoBehaviour 
{
	[SerializeField] float spawnPadding = 0.1f;
    [SerializeField] protected float idleWaitTimeMin = 0.0f;

    [SerializeField] protected float idleWaitTimeMax = 10.0f;

    // Setting the default high for easier debugging (in theory)
    protected float idleWaitTimeDefault = 40.0f;
    protected float idleWaitTime = 40.0f;

    [SerializeField] protected float followSpeed = 10.0f;
    [SerializeField] protected float laneTolerance = 0.35f;     // tune this
    [SerializeField] protected float ySteerWeight = 0.35f;      // 0..1 (how much Y matters vs X)
    [SerializeField] protected float attackRangeX = 0.5f;
    [SerializeField] protected float attackRangeY = 0.45f;

    [SerializeField] protected float disengageExtraX = 0.35f;  // hysteresis (tune)
    [SerializeField] protected float disengageExtraY = 0.20f;

    [SerializeField] protected float chargeDuration = 2.0f;

    protected float laneTargetY;        // enemy’s chosen lane near the player
    protected float laneRepathTimer;    // when to pick a new lane target

	public enum EnemyState 
	{
		Initializing,
		Idle,
		Chasing,
		Charging,
		Attacking,
		Dead
	}

	protected EnemyState currentState;

	public GameObject player;
    protected Rigidbody2D rb;

	protected void Awake() 
	{
		player = FindFirstObjectByType<MB_PlayerController>().gameObject;
		rb = GetComponent<Rigidbody2D>();
		currentState = EnemyState.Initializing;
        idleWaitTimeDefault = Random.Range(idleWaitTimeMin, idleWaitTimeMax);
        idleWaitTime = idleWaitTimeDefault;
	}

	public virtual void FixedUpdate () {
		if (!IsVisibleFrom(Camera.main, spawnPadding))
		{
			currentState = EnemyState.Idle;
		}

		switch (currentState) {
		case EnemyState.Initializing:
			currentState = EnemyState.Idle;
			break;
		case EnemyState.Idle:
			Idle();
			break;
		case EnemyState.Chasing:
			Chasing();
			break;
		case EnemyState.Charging:
			Charging();
			break;	
		case EnemyState.Attacking:
			Attacking();
			break;
		case EnemyState.Dead:
			Dead();
			break;
		default:
			break;
		}
	}

	// TODO: Abstract away maybe
    bool IsVisibleFrom(Camera cam, float screenPadding)
    {
        Vector3 v = cam.WorldToViewportPoint(transform.position);
        return v.z > 0f &&
            v.x > -screenPadding && v.x < 1f + screenPadding &&
            v.y > -screenPadding && v.y < 1f + screenPadding; 
    }

	public EnemyState GetCurrentState() { return currentState; }
	public void SetDead() { currentState = EnemyState.Dead; }

	public virtual void Idle()
	{
		idleWaitTime -= Time.deltaTime;
        if (idleWaitTime <= 0.0f) {
            LookAtPlayer();
            currentState = EnemyState.Chasing;
            idleWaitTime = idleWaitTimeDefault;
        }
	}

	public virtual void Chasing() {}

	public virtual void Charging() {}

	public virtual void Attacking() {}

	public virtual void Dead() {}

	protected virtual void LookAtPlayer() {}
}