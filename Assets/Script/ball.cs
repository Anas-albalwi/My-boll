using UnityEngine;
using TMPro;

public class Ball : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    float ystart;
    float xstart;
    float originalSpeed;

    public TextMeshProUGUI blueScoreText;
    public TextMeshProUGUI redScoreText;

    private int blueScore = 0;
    private int redScore = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = speed;
        LaunchBall();

        InvokeRepeating("ApplySmartWind", 7f, 7f);
        InvokeRepeating("SlowDownTemporarily", 11f, 11f);
    }

    void LaunchBall()
    {
        xstart = Random.value < 0.5f ? 1f : -1f;
        ystart = Random.Range(-0.5f, 0.5f);
        Vector2 direction = new Vector2(xstart, ystart).normalized;
        rb.linearVelocity = direction * speed;
    }

    void ApplySmartWind()
    {
        Vector2 currentVelocity = rb.linearVelocity.normalized;

        float newX = currentVelocity.x;
        float newY = currentVelocity.y;

        if (currentVelocity.x > 0.1f || currentVelocity.x < -0.1f)
        {
            newY += Random.Range(-2f, 2f);
        }
        else if (currentVelocity.y > 0.1f || currentVelocity.y < -0.1f)
        {
            newX += Random.Range(-2f, 2f);
        }

        Vector2 newDirection = new Vector2(newX, newY).normalized;
        rb.linearVelocity = newDirection * speed;
        Debug.Log("Windy");
    }

    void SlowDownTemporarily()
    {
        speed = originalSpeed * 0.8f;
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
        Debug.Log("Slow");

        Invoke("RestoreSpeed", 2f);
    }

    void RestoreSpeed()
    {
        speed = originalSpeed;
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
        Debug.Log("Normal");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);

        if (collision.gameObject.name == "Blue gole")
        {
            blueScore++;
            blueScoreText.text = blueScore.ToString();
            Debug.Log("Blue Score: " + blueScore);
            ResetBall();

        }
        else if (collision.gameObject.name == "Red gole")
        {
            redScore++;
            redScoreText.text = redScore.ToString();
            Debug.Log("Red Score: " + redScore);
            ResetBall();

        }
    }
    void ResetBall()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;
        Invoke("LaunchBall", 1f); 
    }
}
