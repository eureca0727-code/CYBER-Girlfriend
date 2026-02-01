using UnityEngine;
using TMPro;

public class AttackTrigger : MonoBehaviour
{
    [Header("References")]
    public GameObject attackPrefab;
    public TextMeshProUGUI codeText;

    [Header("Settings")]
    public string attackCode = "M*cr%23";
    public float warningTime = 5f;
    public float attackXOffset = 0f;

    private bool isTriggered = false;
    private float timer = 0f;
    private bool isWarning = false;
    private GameObject player; // Transform 대신 GameObject로 변경

    void Start()
    {
        if (codeText != null)
        {
            codeText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isWarning)
        {
            timer += Time.deltaTime;

            if (codeText != null)
            {
                codeText.text = attackCode + "\n" + (warningTime - timer).ToString("F1") + "s";
            }

            if (timer >= warningTime)
            {
                SpawnAttack(); // 여기서 현재 플레이어 위치 사용!
                isWarning = false;

                if (codeText != null)
                {
                    codeText.gameObject.SetActive(false);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("뭔가 들어옴: " + other.tag); // 추가

        if (other.CompareTag("Player") && !isTriggered)
        {
            Debug.Log("플레이어 감지!"); // 추가
            isTriggered = true;
            player = other.gameObject;
            StartWarning();
            GetComponent<Collider2D>().enabled = false;
        }
    }
    void StartWarning()
    {
        isWarning = true;
        timer = 0f;

        if (codeText != null)
        {
            codeText.gameObject.SetActive(true);
            codeText.text = attackCode;
        }
    }

    void SpawnAttack()
    {
        // 자식의 SpriteRenderer에서 크기 가져오기
        SpriteRenderer spriteRenderer = attackPrefab.GetComponentInChildren<SpriteRenderer>();
        float boxHeight = spriteRenderer != null ? spriteRenderer.bounds.size.y : 5f; // 없으면 기본값 5
        Debug.Log(boxHeight);
        Vector3 pos = new Vector3(
            player.transform.position.x + attackXOffset,
            player.transform.position.y - (boxHeight / 2f)-5,
            0f
        );

        GameObject attack = Instantiate(attackPrefab, pos, Quaternion.identity);

        Attack script = attack.GetComponent<Attack>();
        if (script != null)
        {
            script.Initialize(Camera.main.transform.position.y);
        }
    }
}