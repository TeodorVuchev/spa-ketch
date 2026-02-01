using UnityEngine;

public class DestroyOnAnimEvent : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}