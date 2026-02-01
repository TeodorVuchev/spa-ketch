using UnityEngine;
using UnityEngine.InputSystem;

public class MB_VIctoryReached : MonoBehaviour
{
    [SerializeField] GameObject victoryScreen;
    [SerializeField] GameObject victoryScreen1;
    [SerializeField] GameObject victoryScreen2;
    [SerializeField] PlayerInput playerInput;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        victoryScreen.SetActive(true);
        victoryScreen1.SetActive(true);
        victoryScreen2.SetActive(true);
        playerInput.actions.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
