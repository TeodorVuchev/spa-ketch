using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MB_UIManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] RectTransform startButtonRect;
    [SerializeField] RectTransform logo;

    [Header ("Logo")]
    [SerializeField] float logoTargetY;
    [SerializeField] float logoDuration;
    [SerializeField] float scaleSize;
    [SerializeField] float scaleDuration;

    [Header("Delays")]
    [SerializeField] float buttonApppearence;

    [Header ("Start")]
    [SerializeField] float buttTargetY;
    [SerializeField] float buttDuration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeanTween.reset();
        LeanTween.moveY(logo, logoTargetY, logoDuration)
                 .setEaseOutQuad();
        Invoke("JumpLogo", logoDuration);
        Invoke("MoveInStart", buttonApppearence);
    }

    void MoveInStart()
    {
        LeanTween.moveY(startButtonRect, buttTargetY, buttDuration)
                 .setEaseOutQuad();
    }
    void JumpLogo()
    {
        LeanTween.scale(logo, Vector3.one * scaleSize, scaleDuration)
                 .setEaseInOutCubic()
                 .setLoopPingPong();
    }
    
    public void LoadGame()
    {
        SceneManager.LoadScene("Start");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
