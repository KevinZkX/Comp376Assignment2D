using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Color { YELLOW, BLUE, RED, BLACK, GRAY };

public class Ball : MonoBehaviour {
    SpriteRenderer mSpriteRenderer;
    Rigidbody2D mRigidbody2D;
    CircleCollider2D mCollider2D;
    Sprite[] sprites;
    float speed;
    float radius = 0.32f;
    public Vector2 direction;
    public Color mColor;
    public bool staticBall = false;
    public List<Ball> neighbours;
    public bool added = false;

    int row;
    int coloum;

	// Use this for initialization
	void Start () {
        init();
        speed = 10;
	}

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update () {
	}

    public void getNeighbours () {
        Collider2D[] nearCollider = Physics2D.OverlapCircleAll(transform.position, 0.32f);
        foreach (Collider2D temp in nearCollider) {
            if (temp.gameObject.CompareTag("Ball"))
                neighbours.Add(temp.gameObject.GetComponent<Ball>());
        }
    }

    public void setRow (int row) {
        this.row = row;
    }

    public void setColoum(int coloum) {
        this.coloum = coloum;
    }

    public int getRow() {
        return row;
    }

    public int getColoum() {
        return coloum;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Border")) {
            direction = Vector2.Reflect(direction, Vector2.right);
            move(direction);
        }

        //else if (collider.collider.gameObject.CompareTag("Ball")) {
        //    gameObject.
        //}
    }

    public void move (Vector2 newDirection) {
        SetDirection(newDirection);
        mRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        mRigidbody2D.velocity = direction * speed;
    }

    void SetDirection (Vector2 newDirection) {
        direction = newDirection;
        //Debug.Log(direction);
    }

    int ColorRandomer () {
        return (int)Random.Range(0, 100);
    }

    void BallColorRandmoer () {
        if (!staticBall)
        {
            //render ball
            //small than 50 is a yellow ball
            if (ColorRandomer() < 20)
            {
                mColor = Color.YELLOW;
            }
            //greater or equal than 50 is a red ball    
            else if (ColorRandomer() >= 20 && ColorRandomer() < 40)
            {
                mColor = Color.RED;
            }

            //greater or equal than 50 is a blue ball    
            else if (ColorRandomer() >= 40 && ColorRandomer() < 60)
            {
                mColor = Color.BLUE;
            }

            //greater or equal than 50 is a black ball    
            else if (ColorRandomer() >= 60 && ColorRandomer() < 80)
            {
                mColor = Color.BLACK;
            }

            //greater or equal than 50 is a gray ball    
            else if (ColorRandomer() >= 80)
            {
                mColor = Color.BLUE;
            }
        }

        switch (mColor)
        {
            case Color.YELLOW:
                mSpriteRenderer.sprite = sprites[248];
                break;
            case Color.RED:
                mSpriteRenderer.sprite = sprites[236];
                break;
            case Color.BLUE:
                mSpriteRenderer.sprite = sprites[180];
                break;
            case Color.BLACK:
                mSpriteRenderer.sprite = sprites[222];
                break;
            case Color.GRAY:
                mSpriteRenderer.sprite = sprites[208];
                break;
        }
        
    }
    //initialize the ball
    void init()
    {
        //get sprite renderer
        mSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("OoO_教授飞妈ver");

        BallColorRandmoer();
        //mSpriteRenderer.sortingOrder = 4;
        //create rigidbody2D
        mRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        //Set it to static
        mRigidbody2D.bodyType = RigidbodyType2D.Static;
        //create collider
        mCollider2D = gameObject.GetComponent<CircleCollider2D>();
        //set direction to (0, 0)
        direction = new Vector2(0, 0);
        mRigidbody2D.freezeRotation = true;
    }
}
