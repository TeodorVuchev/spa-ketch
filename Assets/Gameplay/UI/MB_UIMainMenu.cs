using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class MB_UIManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] RectTransform startButtonRect;
    [SerializeField] RectTransform controlsButtonRect;
    [SerializeField] RectTransform logo;

    [Header("Logo")]
    [SerializeField] float logoTargetY;
    [SerializeField] float logoDuration;
    [SerializeField] float scaleSize;
    [SerializeField] float scaleDuration;

    [Header("Delays")]
    [SerializeField] float buttonApppearence;

    [Header("Start")]
    [SerializeField] float buttTargetY;
    [SerializeField] float controlButtTargetY;
    [SerializeField] float buttDuration;

    [Header("Controls")]
    [SerializeField] GameObject controlScreen;

    [Header("Cinematics")]
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip intro;
    [SerializeField] VideoClip loop;

    void Awake()
    {
        LeanTween.reset();
        videoPlayer.playOnAwake = false;
    }

    void Start()
    {
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        videoPlayer.clip = intro;
        videoPlayer.isLooping = false;

        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        videoPlayer.frame = 0;
        videoPlayer.loopPointReached += OnIntroFinished;
        videoPlayer.Play();
    }

    void OnIntroFinished(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= OnIntroFinished;
        StartCoroutine(PlayLoop());

        LeanTween.moveY(logo, logoTargetY, logoDuration)
                 .setEaseOutQuad();

        Invoke(nameof(JumpLogo), logoDuration);
        Invoke(nameof(MoveInStart), buttonApppearence);
    }

    IEnumerator PlayLoop()
    {
        videoPlayer.clip = loop;
        videoPlayer.isLooping = true;

        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        videoPlayer.frame = 0;
        videoPlayer.Play();
    }

    void MoveInStart()
    {
        LeanTween.moveY(startButtonRect, buttTargetY, buttDuration)
                 .setEaseOutQuad();
        LeanTween.moveY(controlsButtonRect, controlButtTargetY, buttDuration)
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
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OpenControls()
    {
        controlScreen.SetActive(true);
    }
    public void CloseControls()
    {
        controlScreen.SetActive(false);
    }
}