using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red_Ball : MonoBehaviour
{

    public string ball_colour = "red";
    private Vector2 direction;

    Rigidbody2D redBall;

    float ballSpeed = 0;

    private void Awake()
    {
        redBall = GetComponent<Rigidbody2D>();

        //int colour_random_number = (int)Random.Range(0.0f, 100.0f);
        redBall.bodyType = RigidbodyType2D.Static;

        redBall.freezeRotation = true;

        //redBall.velocity = Vector2.up * ballSpeed;

    }

    public void setDirection(Vector2 direction)
    {
        //Debug.Log("Red ball");
        redBall.bodyType = RigidbodyType2D.Dynamic;
        ballSpeed = 5;
        redBall.velocity = direction * ballSpeed;
        this.direction = direction;
        //transform.Translate(new Vector3(direction.x, direction.y, 0) * Time.deltaTime, Space.World);
    }

    public void setZeroVelocity()
    {
        redBall.velocity = new Vector2(0, 0);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Border")
        {
            //Debug.Log("in it");
            direction = Vector2.Reflect(direction, new Vector2(1, 0));
            setDirection(direction);
        }

        else if (collision.collider.tag == "YellowBall" | collision.collider.tag == "RedBall")
        {

            if (collision.collider.transform.position != new Vector3(0, -3.6f, 0))
                redBall.bodyType = RigidbodyType2D.Static;
        }
    }
}
