using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yellow_Ball : MonoBehaviour
{
    
    public string ball_colour = "yellow";
    private Vector2 direction;
    private Stack<GameObject> connected_yellow_ball_counter;
    private CircleCollider2D circleCollider;

    Rigidbody2D yellowBall;

    float ballSpeed = 0;

    private void Start()
    {
        connected_yellow_ball_counter = new Stack<GameObject>();
    }

    private void Awake()
    {
        yellowBall = GetComponent<Rigidbody2D>();

        //int colour_random_number = (int)Random.Range(0.0f, 100.0f);
        yellowBall.bodyType = RigidbodyType2D.Static;

        yellowBall.freezeRotation = true;

        //yellowBall.velocity = Vector2.up * ballSpeed;

    }

    public void setDirection(Vector2 direction)
    {
        //Debug.Log("Yellow ball");
        yellowBall.bodyType = RigidbodyType2D.Dynamic;
        ballSpeed = 5;
        yellowBall.velocity = direction * ballSpeed;
        this.direction = direction;
        //transform.Translate(new Vector3(direction.x, direction.y, 0) * Time.deltaTime, Space.World);
    }

    public void SetupCollider() {
        circleCollider = gameObject.AddComponent<CircleCollider2D>();
    }

    public void setZeroVelocity()
    {
        yellowBall.velocity = new Vector2(0, 0);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Border")
        {
            //Debug.Log("in it");
            direction = Vector2.Reflect(direction, new Vector2(1, 0));
            setDirection(direction);
        }

        else if (collision.collider.tag == "YellowBall")
        {

            if (collision.collider.transform.position != new Vector3(0, -3.6f, 0))
            {
                yellowBall.bodyType = RigidbodyType2D.Static;
                connected_yellow_ball_counter.Push(collision.gameObject);
                Debug.Log(connected_yellow_ball_counter.Count);
                if (connected_yellow_ball_counter.Count >= 2)
                {
                    foreach (GameObject yB in connected_yellow_ball_counter)
                    {
                        Destroy(yB);
                    }
                    Destroy(gameObject);
                }

            }
        }

        else if (collision.collider.tag == "RedBall")
        {
            if (collision.collider.transform.position != new Vector3(0, -3.6f, 0))
                yellowBall.bodyType = RigidbodyType2D.Static;
        }
    }
}
