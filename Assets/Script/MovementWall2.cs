using UnityEngine;

public class MovementWall2 : MonoBehaviour
{
    public float speed = 1000f;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        float moveY = 0;

        if (Input.GetKey(KeyCode.W))
            moveY = 20;
        else if (Input.GetKey(KeyCode.S))
            moveY = -20;

        rb.MovePosition(rb.position + new Vector2(0, moveY * speed * Time.deltaTime));
    }

}
