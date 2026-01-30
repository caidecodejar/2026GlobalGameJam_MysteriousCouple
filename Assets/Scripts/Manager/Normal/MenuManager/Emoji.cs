using UnityEngine;

public class EmojiAutoMove : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 2f;
    public float changeDirectionTime = 2f;

    private Vector2 moveDirection;
    private float timer;
    private Transform emojiTrans;
    private Camera mainCamera;

    void Awake()
    {
        emojiTrans = GetComponent<Transform>();
        mainCamera = Camera.main;
    }

    void Start()
    {
        RandomizeDirection();
        timer = changeDirectionTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            RandomizeDirection();
            timer = changeDirectionTime;
        }

        emojiTrans.Translate(moveDirection * moveSpeed * Time.deltaTime);
        CheckAndRebound();
    }

    void RandomizeDirection()
    {
        float angle = Random.Range(0f, 360f);
        moveDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    void CheckAndRebound()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(emojiTrans.position);
        Vector2 emojiSize = GetEmojiSize();

        if (screenPos.x < 0 + emojiSize.x / 2 || screenPos.x > Screen.width - emojiSize.x / 2)
        {
            moveDirection.x = -moveDirection.x;
        }

        if (screenPos.y < 0 + emojiSize.y / 2 || screenPos.y > Screen.height - emojiSize.y / 2)
        {
            moveDirection.y = -moveDirection.y;
        }
    }

    Vector2 GetEmojiSize()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
        {
            return new Vector2(sr.sprite.bounds.size.x * emojiTrans.localScale.x,
                               sr.sprite.bounds.size.y * emojiTrans.localScale.y);
        }
        return new Vector2(1f, 1f);
    }
}