using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MB_PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 20f;
    [SerializeField] float heavyAttackChargeTime = 5f;

    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;


    bool comboWindowOpen = false;
    bool comboWindowUsed = false;
    bool charging = false;
    bool charged = false;
    int comboIndex = 0;
    int tweenId = 0;

    Vector2 movementInput;
    AnimatorStateInfo currentAnimationState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Fix for LeanTween not starting on subsequent play sessions
        LeanTween.reset();            // reset LeanTween internals globally
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Ensures Attack Input takes Priority
        currentAnimationState = animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimationState.IsName("Attack")) { return; }
        if (currentAnimationState.IsName("Charging")) { return; }

        ProcessMove();
    }

    void OnMove(InputValue Input)
    {
        movementInput = Input.Get<Vector2>();
        
        if(movementInput == Vector2.zero)
        {
            animator.SetBool("Walking", false);
            return;
        }
        if(movementInput.x != 0f)
        {
            transform.localScale = new Vector3(Mathf.Sign(movementInput.x), 1f, 1f);
        }
        animator.SetBool("Walking", true);

    }

    void ProcessMove()
    {
        rb.MovePosition(rb.position + movementInput * Time.fixedDeltaTime * movementSpeed);
    }

    void OnAttack(InputValue Input)
    {

            if (comboIndex == 0 && !comboWindowOpen)
            {
                animator.SetTrigger("Attacking");
                return;
            }

            if (comboWindowOpen)
            {
                comboWindowUsed = true;
            }
    }

    void OnHeavyAttack(InputValue Input)
    {
        if(Input.Get<float>() > Mathf.Epsilon)
        {
            charging = true;
            animator.SetBool("Charging", true);
            StartCoroutine(Charging());
            return;
        }
        if (charged == true)
        {
            animator.SetTrigger("HeavyAttack");
            charged = false;
            LeanTween.cancel(tweenId);
            tweenId = 0;
            spriteRenderer.color = Color.white;
        }
        else
        {
            animator.SetBool("Charging", false);
        }
            charging = false;
    }
    
    IEnumerator Charging()
    {
        yield return new WaitForSecondsRealtime(heavyAttackChargeTime);
        if (charging)
        {
            charged = true;
            StartColorLoop();
            
        }

    }

    // Call this to start the color loop
    public void StartColorLoop()
    {
        print("tweeenit!");

        print(spriteRenderer);
        LeanTween.reset();
        if(tweenId != 0) { LeanTween.cancel(gameObject); }
        // LeanTween.color uses a linear interpolation by default
        tweenId = LeanTween.value(gameObject, UpdateColor, Color.white, Color.yellow, 0.2f)
            .setEaseLinear()
            .setLoopPingPong().id; // goes back and forth endlessly
    }

    // Update the sprite color each frame
    private void UpdateColor(Color col)
    {
        spriteRenderer.color = col;
    }

    // Animation Events
    void OpenComboWindow()
    {
        comboWindowOpen = true;
    }

    void CloseComboWindow()
    {
        comboWindowOpen = false;
        if (comboWindowUsed && comboIndex < 2)
        {
            comboIndex++;
            animator.SetInteger("ComboIndex", comboIndex);
            comboWindowUsed = false;
            return;
        }
        comboIndex = 0;
        animator.SetInteger("ComboIndex", 0);
    }

    void FinishedHeavyAttack()
    {
        animator.SetBool("Charging", false);
    }
}



