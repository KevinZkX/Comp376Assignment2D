using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Color { YELLOW, BLUE, RED, BLACK, GRAY };

public class Ball : MonoBehaviour {
    SpriteRenderer mSpriteRenderer;
    Rigidbody2D mRigidbody2D;
    CircleCollider2D mCollider2D;
    Sprite[] sprites;
    BallMatrix ballMatrix;
    float speed;
    float radius = 0.32f;
    public Vector2 direction;
    public Color mColor;
    public bool staticBall = false;
    public List<GameObject> neighbours;
    public bool added = false;
    public Ball collideBall;
    public bool moving = false;
    public bool visited = false;
    public bool sucessDestory = false;

    public int row;
    public int coloum;

	// Use this for initialization
	void Start () {
	}

    private void Awake()
    {
        init();
        speed = 10;
        ballMatrix = BallMatrix.CreateBallMatrix;
    }

    // Update is called once per frame
    void FixedUpdate () {
        
    }

    private void Update()
    {
        checkDestory();
        if (gameObject.name != "Ready")
            getNeighbours();
        if (sucessDestory)
            checkFallDown();
        
    }

    private void LateUpdate()
    {
        if (gameObject.name != "Ready")
            getNeighbours();
    }



    public void checkFallDown () {
        Debug.Log("Check Fall Down");
        foreach (List<GameObject> goList in ballMatrix.balls)
        {
            foreach(GameObject go in goList)
            {
                ballMatrix.FallDownList(gameObject);
            }
        }
        
        Debug.Log("Fall Down: " + ballMatrix.fallDown.Count);
        ballMatrix.visted.RemoveRange(0, ballMatrix.visted.Count);
        ballMatrix.fallDown.RemoveRange(0, ballMatrix.fallDown.Count);
        //foreach (List<GameObject> goList in ballMatrix.balls)
        //{
        //    Excusive(goList, ballMatrix.fallDown);
        //}
        //foreach (GameObject go in ballMatrix.fallDown) {
        //    Destroy(go);
        //}
    }

    public void checkDestory () {
        if (gameObject.name == "NeedCheck")
        {
            ballMatrix.DestoryList(gameObject, mColor);
            Debug.Log("Destory: " + ballMatrix.destory.Count);
            if (ballMatrix.destory.Count > 2)
            {
                sucessDestory = true;
                Debug.Log("Ready to destory");
                foreach (List<GameObject> goList in ballMatrix.balls)
                {
                    Excusive(goList, ballMatrix.destory);
                }
                foreach (GameObject go in ballMatrix.destory)
                {
                    Destroy(go);
                }
            }
            ballMatrix.destory.RemoveRange(0, ballMatrix.destory.Count);
            Debug.Log("After destory: " + ballMatrix.destory.Count);
            ballMatrix.visted.RemoveRange(0, ballMatrix.visted.Count);
            Debug.Log("After visit: " + ballMatrix.visted.Count);
            gameObject.name = "Ball(Clone)";
        }
    }

    public void getNeighbours () {
        //if (neighbours != null) {
        //    neighbours.RemoveRange(0, neighbours.Count);
        //}
        //neighbours.AddRange(ballMatrix.getNeighbours(gameObject));

        neighbours = ballMatrix.getNeighbours(gameObject);

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
        //Debug.Log("get in");
        if (collision.collider.gameObject.CompareTag("Border")) {
            direction = Vector2.Reflect(direction, Vector2.right);
            move(direction);
        }

        else if ((collision.collider.gameObject.name  == "Moving") && (gameObject.name != "Ready" || gameObject.name == "Ball(Clone)")){
            collision.collider.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            ballMatrix.addNewBall(collision.collider.gameObject, gameObject.GetComponent<Ball>());
            Debug.Log("Add new ball to balls list");
            collision.collider.gameObject.GetComponent<Ball>().collideBall = GetComponent<Ball>();
            collision.collider.gameObject.name = "NeedCheck";
            collision.collider.gameObject.GetComponent<Ball>().getNeighbours();
            Debug.Log("Add new neighbours to the new ball" + " : New balls lsit size: " + ballMatrix.balls.Count);
            foreach (GameObject go in collision.collider.gameObject.GetComponent<Ball>().neighbours)
            {
                go.GetComponent<Ball>().getNeighbours();
            }
            Debug.Log("Update neighbours' neighbour, neighbour number: " + neighbours.Count);
            
        }
    }

    public void move (Vector2 newDirection) {
        SetDirection(newDirection);
        mRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        mRigidbody2D.freezeRotation = true;
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
    }

    //remove all the gameObject that is visited
    public static void Excusive(List<GameObject> list, List<GameObject> removeList)
    {
        List<GameObject> excusive = new List<GameObject>();
        foreach (GameObject go in list)
        {
            if (!removeList.Contains(go))
            {
                excusive.Add(go);
            }
        }
        list.RemoveRange(0, list.Count);
        list.AddRange(excusive);

    }
}
