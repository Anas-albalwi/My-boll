using UnityEngine;
using TMPro;

public class Ball : MonoBehaviour
{
    public ParticleSystem wainParticl;
    public float speed = 5f;
    private Rigidbody2D rb;
    float ystart;
    float xstart;
    float originalSpeed;

    public TextMeshProUGUI blueScoreText;
    public TextMeshProUGUI redScoreText;
    public TextMeshProUGUI winnerText;

    private int blueScore = 0;
    private int redScore = 0;

    private int winningScore = 3;
    private bool gameOver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = speed;
        LaunchBall();

        InvokeRepeating("ApplySmartWind", 7f, 7f);
        InvokeRepeating("SlowDownTemporarily", 11f, 11f);

        winnerText.gameObject.SetActive(false);
    }

    void LaunchBall()
    {
        if (gameOver) return;

        xstart = Random.value < 0.5f ? 1f : -1f;
        ystart = Random.Range(-0.5f, 0.5f);
        Vector2 direction = new Vector2(xstart, ystart).normalized;
        rb.linearVelocity = direction * speed;
    }

    void ApplySmartWind()
    {
        if (gameOver) return;

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
        if (gameOver) return;

        speed = originalSpeed * 0.8f;
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
        Debug.Log("Slow");

        Invoke("RestoreSpeed", 2f);
    }

    void RestoreSpeed()
    {
        if (gameOver) return;

        speed = originalSpeed;
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
        Debug.Log("Normal");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameOver) return;

        Debug.Log("Collided with: " + collision.gameObject.name);

        if (collision.gameObject.name == "Blue gole")
        {
            blueScore++;
            blueScoreText.text = blueScore.ToString();
            Debug.Log("Blue Score: " + blueScore);
            CheckForWinner();
            ResetBall();
        }
        else if (collision.gameObject.name == "Red gole")
        {
            redScore++;
            redScoreText.text = redScore.ToString();
            Debug.Log("Red Score: " + redScore);
            CheckForWinner();
            ResetBall();
        }
        if (collision.gameObject.CompareTag("paddel"))
        {
            originalSpeed += 0.1f;
        }
    }

    void ResetBall()
    {
        if (gameOver) return;

        rb.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;
        Invoke("LaunchBall", 1f);
    }

    void CheckForWinner()
    {
        if (blueScore >= winningScore)
        {
            ShowWinner("Red");
        }
        else if (redScore >= winningScore)
        {
            ShowWinner("Blue");
        }
    }

    void ShowWinner(string winner)
    {
        winnerText.gameObject.SetActive(true);
        winnerText.text = winner + " Wins!";
        Debug.Log(winner + " Wins!");
        
        blueScore = 0;
        redScore = 0;
        blueScoreText.text = "0";
        redScoreText.text = "0";

        gameOver = true;

        rb.linearVelocity = Vector2.zero;

        CancelInvoke("ApplySmartWind");
        CancelInvoke("SlowDownTemporarily");
        wainParticl.gameObject.SetActive(true);
        wainParticl.Play();
    }
}
