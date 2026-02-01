using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MB_DeathScreen : MonoBehaviour
{
    [SerializeField] RectTransform deathScreen;
    [SerializeField] float animationDuration;
    [SerializeField] float waitforDeathScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void ShowLooseScreen()
    {
        StartCoroutine(ShowDeathScreen());
    }

    IEnumerator ShowDeathScreen()
    {
        yield return new WaitForSecondsRealtime(waitforDeathScreen);
        StartDeathScreen();
    }

    void StartDeathScreen()
    {
        LeanTween.moveY(deathScreen, 0f, animationDuration).setEaseOutBounce();
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
