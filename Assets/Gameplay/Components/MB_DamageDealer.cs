using UnityEngine;

public class MB_DamageDealer : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    [SerializeField] MB_UIInGame gameUI;
    [SerializeField] Animator HitVFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MB_Health hitHealth = collision.GetComponent<MB_Health>();
        if (hitHealth == null) { return; }
        hitHealth.DealDamage(damage);
        if(gameObject.tag == "Player")
        {
            gameUI.ComboIncrease();
        }
        HitVFX.SetTrigger("PlayVFX");
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
