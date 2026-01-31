using UnityEngine;
using System.Collections;

public abstract class EnemyBehaviour : MonoBehaviour 
{
	public enum EnemyState 
	{
		Initializing,
		Idle,
		Chasing,
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
	}

	public virtual void FixedUpdate () {
		if (!IsVisibleFrom(Camera.main, 0.1f))
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

	public virtual void Idle() {}


	public virtual void Chasing() {}

	public virtual void Attacking() {}

	public virtual void Dead() {}
}