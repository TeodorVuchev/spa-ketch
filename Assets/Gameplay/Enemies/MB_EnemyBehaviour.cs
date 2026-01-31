using UnityEngine;
using System.Collections;

public abstract class EnemyBehaviour : MonoBehaviour 
{
	public enum EnemyState 
	{
		Initializing,
		Idle,
		SawPlayer,
		Chasing,
		Attacking,
		Fleeing
	}

	public EnemyState currentState;

	public GameObject player;
    protected Rigidbody2D rb;

	protected void Awake() 
	{
		player = FindFirstObjectByType<MB_PlayerController>().gameObject;
		rb = GetComponent<Rigidbody2D>();
		currentState = EnemyState.Initializing;
	}

	public virtual void FixedUpdate () {
		// TODO: temporary, check with team if we're spawning enemies by hand or we'll rely on spawning logic
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
		case EnemyState.SawPlayer:
			SawPlayer();
			break;
		case EnemyState.Chasing:
			Chasing();
			break;
		case EnemyState.Attacking:
			Attacking();
			break;
		case EnemyState.Fleeing:
			Fleeing();
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


	public virtual void Idle() {}

	public virtual void SawPlayer() {}

	public virtual void Chasing() {}

	public virtual void Attacking() {}

	public virtual void Fleeing() {}
}