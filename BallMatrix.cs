using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the really game controller
public class BallMatrix {

    //private int row = 2;
    //private int coloum = 8;
    public List<List<GameObject>> balls;

    private BallMatrix() {
        balls = new List<List<GameObject>>();
    }

    private class Nested {
        static Nested () {
            
        }

        internal static readonly BallMatrix ballMatrix = new BallMatrix();
    }

    public static BallMatrix CreateBallMatrix  {
        get { return Nested.ballMatrix; }
    }

    private void BubbleSort (List<GameObject> resource, int size) {
        GameObject temp;

        //first sort y
        for (int i = 0; i < size - 1; i++) {
            for (int j = 0; i < size - 1 - i; j++) {
                if (resource[j+1].transform.position.y < resource[j].transform.position.y) {
                    temp = resource[j];
                    resource[j] = resource[j + 1];
                    resource[j + 1] = temp;
                }
            }
        }

        //second sort x
        for (int i = 0; i < size - 1; i++)
        {
            for (int j = 0; i < size - 1 - i; j++)
            {
                if (resource[j + 1].transform.position.x < resource[j].transform.position.x & Mathf.Approximately(resource[j + 1].transform.position.y, resource[j].transform.position.y))
                {
                    temp = resource[j];
                    resource[j] = resource[j + 1];
                    resource[j + 1] = temp;
                }
            }
        }
    }

    //store the initial ball into the list
    public void addCurrentBalls() {
        GameObject[] allBalls = GameObject.FindGameObjectsWithTag("Ball");
        //Debug.Log(allBalls[8].transform.position);
        int coloum_number;
        coloum_number = (int)((allBalls[0].GetComponent<Ball>().transform.position.y - allBalls[allBalls.Length - 1].GetComponent<Ball>().transform.position.y)/0.55)+2;

        for (int i = 0; i < coloum_number; i++) {
            List<GameObject> temp = new List<GameObject>();
            int c = 0;
            foreach (GameObject ball in allBalls) {
                if (Mathf.Approximately((2.8f - 0.55f * i), ball.GetComponent<Ball>().transform.position.y) && !ball.GetComponent<Ball>().added) {
                    
                    if (ball != null) {
                        ball.GetComponent<Ball>().setRow(i);
                        ball.GetComponent<Ball>().setColoum(c);
                        temp.Add(ball);
                        ball.GetComponent<Ball>().added = true;
                        c++;
                    }
                }
            }
            if (temp.Count != 0) {
                balls.Add(temp);
                //Debug.Log("Temp: " + temp[0].GetComponent<Ball>().getRow());
            }
                
        }
        //Debug.Log("Balls: " + balls.Count);
    }

    public void addNewBall (GameObject ball_clone, Ball collision) {
        int row = collision.getRow();
        int coloum = collision.getColoum();
        Debug.Log(row + " " + coloum);

        //even row on the very left
        if (coloum == 0 && row % 2 == 0) {
            Debug.Log("even left");
            ball_clone.transform.position = new Vector3(collision.transform.position.x + 0.32f, collision.transform.position.y - 0.55f);
            ball_clone.GetComponent<Ball>().setRow(row + 1);
            ball_clone.GetComponent<Ball>().setColoum(coloum);
        }

        //even row on the very right
        else if (coloum == 7 && row % 2 == 0) {
            Debug.Log("even right");

            ball_clone.transform.position = new Vector3(collision.transform.position.x - 0.32f, collision.transform.position.y - 0.55f);
            ball_clone.GetComponent<Ball>().setRow(row + 1);
            ball_clone.GetComponent<Ball>().setColoum(coloum - 1);
        }

        //odd row on the very left
        else if (coloum == 0 && row % 2 != 0)
        {
            ball_clone.transform.position = new Vector3(collision.transform.position.x - 0.32f, collision.transform.position.y - 0.55f);
            ball_clone.GetComponent<Ball>().setRow(row + 1);
            ball_clone.GetComponent<Ball>().setColoum(coloum);
        }
        //odd row on the very right
        else if (coloum == 6 && row % 2 != 0)
        {
            ball_clone.transform.position = new Vector3(collision.transform.position.x + 0.32f, collision.transform.position.y - 0.55f);
            ball_clone.GetComponent<Ball>().setRow(row + 1);
            ball_clone.GetComponent<Ball>().setColoum(coloum + 1);
        }
        //other ball
        else  {
           
            if (ball_clone.transform.position.x >= collision.transform.position.x)
            {
                ball_clone.transform.position = new Vector3(collision.transform.position.x + 0.32f, collision.transform.position.y - 0.55f);
                //even row
                if (row % 2 == 0) {
                    ball_clone.GetComponent<Ball>().setRow(row + 1);
                    ball_clone.GetComponent<Ball>().setColoum(coloum);
                } 
                //odd row
                else {
                    ball_clone.GetComponent<Ball>().setRow(row + 1);
                    ball_clone.GetComponent<Ball>().setColoum(coloum + 1);
                }

            }
            else if (ball_clone.transform.position.x < collision.transform.position.x)
            {
                ball_clone.transform.position = new Vector3(collision.transform.position.x - 0.32f, collision.transform.position.y - 0.55f);
                //even row
                if (row % 2 == 0) {
                    ball_clone.GetComponent<Ball>().setRow(row + 1);
                    ball_clone.GetComponent<Ball>().setColoum(coloum - 1);
                } 
                //odd row
                else {
                    ball_clone.GetComponent<Ball>().setRow(row + 1);
                    ball_clone.GetComponent<Ball>().setColoum(coloum);
                }

            }
        }

        Debug.Log("keep going");
        if (balls.Count == row + 2) {
            Debug.Log("check");
            balls[row + 1].Add(ball_clone);
        }
        else {
            Debug.Log(balls[balls.Count - 1].Count);

            List<GameObject> temp = new List<GameObject>();
            temp.Add(ball_clone);
            balls.Add(temp);
            Debug.Log(balls[balls.Count - 1].Count);

        }
    }

    /*
     * update neighbour list of a ball
     */
    public List<GameObject> getNeighbours (GameObject ball) {
        List<GameObject> temp = new List<GameObject>();
        int row = ball.GetComponent<Ball>().getRow();
        int coloum = ball.GetComponent<Ball>().getColoum();
        //if it is the first row
        if (row == 0) {
            //same row
            foreach (GameObject go in balls[row]) {
                //left
                if (go.GetComponent<Ball>().getColoum() - coloum == -1) {
                    temp.Add(go);
                }
                //right
                else if (go.GetComponent<Ball>().getColoum() - coloum == 1) {
                    temp.Add(go);
                }
            }
            //buttom
            foreach (GameObject go in balls[row+1]) {
                //left
                if (go.GetComponent<Ball>().getColoum() - coloum == -1)
                {
                    temp.Add(go);
                }
                //right
                else if (go.GetComponent<Ball>().getColoum() - coloum == 0)
                {
                    temp.Add(go);
                }
            }
        }
        //if it is the last row
        else if (row == balls.Count -1) {
            //even row
            if (row % 2 == 0) {
                //upper
                foreach (GameObject go in balls[row])
                {
                    //left
                    if (go.GetComponent<Ball>().getColoum() - coloum == -1)
                    {
                        temp.Add(go);
                    }
                    //right
                    else if (go.GetComponent<Ball>().getColoum() - coloum == 0)
                    {
                        temp.Add(go);
                    }
                }
                //same row
                foreach (GameObject go in balls[row + 1])
                {
                    //left
                    if (go.GetComponent<Ball>().getColoum() - coloum == -1)
                    {
                        temp.Add(go);
                    }
                    //right
                    else if (go.GetComponent<Ball>().getColoum() - coloum == 1)
                    {
                        temp.Add(go);
                    }
                }
            }
            //odd row
            else {
                //upper
                foreach (GameObject go in balls[row])
                {
                    //left
                    if (go.GetComponent<Ball>().getColoum() - coloum == 0)
                    {
                        temp.Add(go);
                    }
                    //right
                    else if (go.GetComponent<Ball>().getColoum() - coloum == 1)
                    {
                        temp.Add(go);
                    }
                }
                //same row
                foreach (GameObject go in balls[row + 1])
                {
                    //left
                    if (go.GetComponent<Ball>().getColoum() - coloum == -1)
                    {
                        temp.Add(go);
                    }
                    //right
                    else if (go.GetComponent<Ball>().getColoum() - coloum == 1)
                    {
                        temp.Add(go);
                    }
                }                
            }
        }
        //other 
        else {
            //even
            if (row % 2 == 0) {
                //upper row
                foreach (GameObject go in balls[row])
                {
                    //left
                    if (go.GetComponent<Ball>().getColoum() - coloum == -1)
                    {
                        temp.Add(go);
                    }
                    //right
                    else if (go.GetComponent<Ball>().getColoum() - coloum == 0)
                    {
                        temp.Add(go);
                    }
                }
                //same row
                foreach (GameObject go in balls[row])
                {
                    //left
                    if (go.GetComponent<Ball>().getColoum() - coloum == -1)
                    {
                        temp.Add(go);
                    }
                    //right
                    else if (go.GetComponent<Ball>().getColoum() - coloum == 1)
                    {
                        temp.Add(go);
                    }
                }
                //buttom
                foreach (GameObject go in balls[row + 1])
                {
                    //left
                    if (go.GetComponent<Ball>().getColoum() - coloum == -1)
                    {
                        temp.Add(go);
                    }
                    //right
                    else if (go.GetComponent<Ball>().getColoum() - coloum == 0)
                    {
                        temp.Add(go);
                    }
                }
            }
            //odd
            else {
                //upper
                foreach (GameObject go in balls[row])
                {
                    //left
                    if (go.GetComponent<Ball>().getColoum() - coloum == 0)
                    {
                        temp.Add(go);
                    }
                    //right
                    else if (go.GetComponent<Ball>().getColoum() - coloum == 1)
                    {
                        temp.Add(go);
                    }
                }
                //same row
                foreach (GameObject go in balls[row + 1])
                {
                    //left
                    if (go.GetComponent<Ball>().getColoum() - coloum == -1)
                    {
                        temp.Add(go);
                    }
                    //right
                    else if (go.GetComponent<Ball>().getColoum() - coloum == 1)
                    {
                        temp.Add(go);
                    }
                }
                //buttom
                foreach (GameObject go in balls[row])
                {
                    //left
                    if (go.GetComponent<Ball>().getColoum() - coloum == 0)
                    {
                        temp.Add(go);
                    }
                    //right
                    else if (go.GetComponent<Ball>().getColoum() - coloum == 1)
                    {
                        temp.Add(go);
                    }
                }
            }
        }
        return temp;
    }

}


