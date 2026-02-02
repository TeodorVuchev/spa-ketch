using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MB_PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 20f;
    [SerializeField] float heavyAttackChargeTime = 5f;
    [SerializeField] SpriteRenderer spriteRenderer;

    Animator animator;
    Rigidbody2D rb;


    bool comboWindowOpen = false;
    bool comboWindowUsed = false;
    bool charging = false;
    bool charged = false;
    int comboIndex = 0;


    Vector2 movementInput;
    AnimatorStateInfo currentAnimationState;
    AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        LeanTween.reset();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        print(spriteRenderer.color);
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
        if (!audioSource.isPlaying)
        {
            audioSource.Play();

        }
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
            audioSource.Play();
            animator.SetTrigger("HeavyAttack");
            charged = false;
           //eanTween.cancel(tweenId);
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
        // LeanTween.color uses a linear interpolation by default
       LeanTween.value(gameObject, UpdateColor, Color.white, Color.yellow, 0.2f)
            .setEaseLinear()
            .setLoopPingPong(); // goes back and forth endlessly
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



