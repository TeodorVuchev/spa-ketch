using UnityEngine;
using UnityEngine.UI;

public class MB_UIInGame : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] MB_Health playerHealth;

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

    public void UpdateHealthBar(float health)
    {
        print(health);
        healthBar.value = health;
    }
}