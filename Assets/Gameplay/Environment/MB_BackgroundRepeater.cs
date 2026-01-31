using UnityEngine;

public class MB_BackgroundRepeater : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject backgroundPrefab;         // root has SpriteRenderer

    [Header("Tiling")]
    [SerializeField, Range(2, 4)] int poolSize = 3;       // 2 works, 3 is safer
    [SerializeField] float extraPadding = 0.5f;           // helps avoid seams/pop

    Camera cam; // gameplay camera

    Transform[] chunks;
    float chunkWidth; // world units

    void Awake()
    {
        if (!cam) cam = Camera.main;

        // Get sprite width from prefab root SpriteRenderer
        var sr = backgroundPrefab.GetComponent<SpriteRenderer>();
        if (!sr || !sr.sprite)
        {
            Debug.LogError("backgroundPrefab root must have a SpriteRenderer with a sprite.");
            enabled = false;
            return;
        }

        // bounds.size.x already accounts for scale in the scene instance,
        // but prefab bounds won't include instance scale. We'll compute after instantiating.
        chunks = new Transform[poolSize];

        // Create chunks
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(backgroundPrefab, transform);
            chunks[i] = go.transform;
        }

        // Now compute chunk width using the first instance (includes its scale)
        var firstSR = chunks[0].GetComponent<SpriteRenderer>();
        chunkWidth = firstSR.bounds.size.x;

        // Lay them out side-by-side
        Vector3 startPos = transform.position;
        for (int i = 0; i < poolSize; i++)
        {
            chunks[i].position = startPos + Vector3.right * (chunkWidth * i);
        }
    }

    void LateUpdate()
    {
        // Camera left edge in world units
        float camHalfWidth = cam.orthographicSize * cam.aspect;
        float camLeft = cam.transform.position.x - camHalfWidth;

        // Find rightmost chunk X (center position)
        float rightmostX = chunks[0].position.x;
        for (int i = 1; i < chunks.Length; i++)
            if (chunks[i].position.x > rightmostX) rightmostX = chunks[i].position.x;

        // Recycle any chunk that is fully offscreen to the left
        for (int i = 0; i < chunks.Length; i++)
        {
            Transform t = chunks[i];
            float chunkRightEdge = t.position.x + chunkWidth * 0.5f;

            if (chunkRightEdge < camLeft - extraPadding)
            {
                // Move it to the immediate right of the current rightmost chunk
                t.position = new Vector3(rightmostX + chunkWidth, t.position.y, t.position.z);
                rightmostX = t.position.x; // update, in case multiple recycle in one frame
            }
        }
    }
}
