using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class MB_UIInGame : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] MB_Health playerHealth;
    [SerializeField] UnityEngine.UI.Image nextArrow;
    [SerializeField] RectTransform comboScale;
    [SerializeField] TextMeshProUGUI combonumberText;
    [SerializeField] TextMeshProUGUI comboText;

    Coroutine stopCombo;
    public float duration = 0.5f; // time for one fade (0 → 1 or 1 → 0)
    public float maxAlpha = 1f;   // maximum opacity

    int combocount = 0;

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

    public void ComboIncrease()
    {
        if(stopCombo != null)
        {
            StopCoroutine(stopCombo);
        }
        combocount += 1;
        combonumberText.text = combocount.ToString();
        FadeIn(combonumberText);
        FadeIn(comboText);
        LeanTween.scale(comboScale, new Vector3(1.2f, 1.2f, 1.2f), 0.1f).setEaseOutQuint().setLoopPingPong(1);
        stopCombo = StartCoroutine(HoldingCombo());
    }

    IEnumerator HoldingCombo()
    {
        yield return new WaitForSeconds(3f);
        stopCombo = null;
        combocount = 0;
        Color c = comboText.color;
        Color a = combonumberText.color;
        c.a = 0f;
        a.a = 0f;
        comboText.color = c;
        combonumberText.color = a;
    }

    public void FadeIn(TextMeshProUGUI text)
    {
        // Ensure starting alpha is 0
        Color c = text.color;
        c.a = 0f;
        text.color = c;

        LeanTween.value(gameObject, 0f, 1f, duration)
                 .setEaseOutQuint()
                 .setOnUpdate((float value) =>
                 {
                     Color col = text.color;
                     col.a = value;
                     text.color = col;
                 });
    }
    public void FadeOut(TextMeshProUGUI text)
    {
        // Ensure starting alpha is 0
        Color c = text.color;
        text.color = c;

        LeanTween.value(gameObject, 0f, 1f, duration)
                 .setEaseOutQuint()
                 .setOnUpdate((float value) =>
                 {
                     Color col = text.color;
                     col.a = value;
                     text.color = col;
                 });
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
    public void LoadGame()
    {
        SceneManager.LoadScene("Start");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}