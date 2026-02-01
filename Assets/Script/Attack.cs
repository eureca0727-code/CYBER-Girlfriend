using UnityEngine;

public class Attack : MonoBehaviour
{
    public float riseSpeed = 10f;
    public float heightOffset = 0f; // 목표보다 얼마나 덜 올라올지

    private float targetY;
    private bool rising = true;

    public void Initialize(float cameraMiddleY)
    {
        targetY = cameraMiddleY - heightOffset; // offset 적용
    }

    void Update()
    {
        if (rising)
        {
            transform.Translate(Vector2.up * riseSpeed * Time.deltaTime);

            float boxHalfHeight = GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2f;
            if (transform.position.y + boxHalfHeight >= targetY)
            {
                rising = false;
            }
        }
        else
        {
            transform.Translate(Vector2.down * riseSpeed * Time.deltaTime);

            if (transform.position.y < targetY - 10f)
            {
                Destroy(gameObject);
            }
        }
    }
}