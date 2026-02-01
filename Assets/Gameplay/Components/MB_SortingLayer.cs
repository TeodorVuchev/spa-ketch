using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MB_SortingLayer : MonoBehaviour
{
    public UnityEngine.Transform feet;
    public int precision = 1000;
    public int offset = 0;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void LateUpdate()
    {
        float y = feet ? feet.position.y : transform.position.y;
        sr.sortingOrder = Mathf.RoundToInt(-y * precision) + offset;
    }

}
