using UnityEngine;
using UnityEngine.UI;

public class MB_UIInGame : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] MB_Health playerHealth;
    [SerializeField] Image nextArrow;
    public float duration = 0.5f; // time for one fade (0 → 1 or 1 → 0)
    public float maxAlpha = 1f;   // maximum opacity

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar.maxValue = playerHealth.GetCurrentHealth();
        healthBar.value = playerHealth.GetCurrentHealth();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayNextArrow()
    {
        // Make sure starting alpha is 0
        Color c = nextArrow.color;
        c.a = 0f;
        nextArrow.color = c;

        // Tween alpha
        LeanTween.value(gameObject, 0f, maxAlpha, duration)
            .setLoopPingPong(3) // ping-pong 3 times
            .setOnUpdate((float val) => {
                Color col = nextArrow.color;
                col.a = val;
                nextArrow.color = col;
            });
    }

    public void UpdateHealthBar(float health)
    {
        print(health);
        healthBar.value = health;
    }
}