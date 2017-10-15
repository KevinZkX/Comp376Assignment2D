using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the really game controller
public class BallMatrix {

    //private int row = 2;
    //private int coloum = 8;
    public List<List<GameObject>> balls;
    public List<GameObject> visted;
    public  List<GameObject> destory;
    public List<GameObject> fallDown;
    public List<GameObject> destoryNeighbour;

    private BallMatrix() {
        balls = new List<List<GameObject>>();
        visted = new List<GameObject>();
        destory = new List<GameObject>();
        fallDown = new List<GameObject>();
        destoryNeighbour = new List<GameObject>();
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

    public void addNewBallToTheTop (GameObject ball_clone)
    {
        float x = ball_clone.transform.position.x;
        int i = Mathf.RoundToInt((x + 2.24f) / 0.64f);
        float fix_x = -2.24f + 0.64f * i;
        ball_clone.GetComponent<Ball>().setRow(0);
        ball_clone.GetComponent<Ball>().setColoum(i);
        balls[0].Add(ball_clone);
        ball_clone.transform.position = new Vector3(fix_x, balls[0][0].transform.position.y);
    }

    //add new ball to the ball list and update its position
    public void addNewBall (GameObject ball_clone, Ball collision) {
        int row = collision.getRow();
        int coloum = collision.getColoum();

        float clone_x = ball_clone.transform.position.x;
        float clone_y = ball_clone.transform.position.y;

        float collision_x = ball_clone.transform.position.x;
        float collision_y = ball_clone.transform.position.y;

        int ball_list_length = balls.Count;

        bool empty_position = true;

        //Debug.Log(row + " " + coloum);

        //check if the collision is on an even row
        if (row % 2 == 0)
        {
            //first check if the new ball is on the top or buttom
            //if clone_y > clone_x, it means ball_clone is on the top of collision
            if (clone_y > collision_y)
            {
                //the check clone_x and collision_x
                //if clone_x > collision_x, it means ball_clone is on the right of collision
                if (clone_x > collision_x)
                {
                    //check if the new position is empty or not 
                    foreach (GameObject go in balls[row - 1])
                    {
                        if (go.GetComponent<Ball>().getColoum() == coloum)
                        {
                            empty_position = false;
                            break;
                        }
                    }

                    if (!empty_position)
                    {
                        ball_clone.transform.position = new Vector3(collision.transform.position.x - 0.32f, collision.transform.position.y + 0.55f);
                        ball_clone.GetComponent<Ball>().setRow(row - 1);
                        ball_clone.GetComponent<Ball>().setColoum(coloum - 1);
                    }

                    else
                    {
                        ball_clone.transform.position = new Vector3(collision.transform.position.x + 0.32f, collision.transform.position.y + 0.55f);
                        ball_clone.GetComponent<Ball>().setRow(row - 1);
                        ball_clone.GetComponent<Ball>().setColoum(coloum);
                    }

                }
                //if clone_x < collision_x , it means ball_clone is on the left of collison
                else if (clone_x <= collision_x)
                {
                    //check if the new position is empty or not 
                    foreach (GameObject go in balls[row - 1])
                    {
                        if (go.GetComponent<Ball>().getColoum() == coloum - 1)
                        {
                            empty_position = false;
                            break;
                        }
                    }

                    if (!empty_position)
                    {
                        ball_clone.transform.position = new Vector3(collision.transform.position.x + 0.32f, collision.transform.position.y + 0.55f);
                        ball_clone.GetComponent<Ball>().setRow(row - 1);
                        ball_clone.GetComponent<Ball>().setColoum(coloum);
                    }
                    else
                    {
                        ball_clone.transform.position = new Vector3(collision.transform.position.x - 0.32f, collision.transform.position.y + 0.55f);
                        ball_clone.GetComponent<Ball>().setRow(row - 1);
                        ball_clone.GetComponent<Ball>().setColoum(coloum - 1);
                    }

                }
            }

            //if clone_y < clone_x, it means ball_clone is on the buttom of collision
            else if (clone_y <= collision_y)
            {
                //the check clone_x and collision_x
                //if clone_x > collision_x, it means ball_clone is on the right of collision
                if (clone_x > collision_x)
                {
                    //check if the new position is empty or not 
                    if (row != ball_list_length - 1)
                    {
                        foreach (GameObject go in balls[row + 1])
                        {
                            if (go.GetComponent<Ball>().getColoum() == coloum)
                            {
                                empty_position = false;
                                break;
                            }
                        }
                    }
                    if (!empty_position || coloum == 7)
                    {
                        ball_clone.transform.position = new Vector3(collision.transform.position.x - 0.32f, collision.transform.position.y - 0.55f);
                        ball_clone.GetComponent<Ball>().setRow(row + 1);
                        ball_clone.GetComponent<Ball>().setColoum(coloum - 1);
                    }
                    else
                    {
                        ball_clone.transform.position = new Vector3(collision.transform.position.x + 0.32f, collision.transform.position.y - 0.55f);
                        ball_clone.GetComponent<Ball>().setRow(row + 1);
                        ball_clone.GetComponent<Ball>().setColoum(coloum);
                    }
                    
                    
                }
                //if clone_x < collision_x , it means ball_clone is on the left of collison
                else if (clone_x <= collision_x)
                {

                    //check if the new position is empty or not 
                    if (row != ball_list_length - 1)
                    {
                        foreach (GameObject go in balls[row + 1])
                        {
                            if (go.GetComponent<Ball>().getColoum() == coloum - 1)
                            {
                                empty_position = false;
                                break;
                            }
                        }
                    }
                    if (!empty_position || coloum == 0)
                    {
                        ball_clone.transform.position = new Vector3(collision.transform.position.x + 0.32f, collision.transform.position.y - 0.55f);
                        ball_clone.GetComponent<Ball>().setRow(row + 1);
                        ball_clone.GetComponent<Ball>().setColoum(coloum);
                    }
                    else
                    {
                        ball_clone.transform.position = new Vector3(collision.transform.position.x - 0.32f, collision.transform.position.y - 0.55f);
                        ball_clone.GetComponent<Ball>().setRow(row + 1);
                        ball_clone.GetComponent<Ball>().setColoum(coloum - 1);
                    }
                    
                       
                }
            }
        }

        //collision is on an odd row
        else
        {
            //first check if the new ball is on the top or buttom
            //if clone_y > clone_x, it means ball_clone is on the top of collision
            if (clone_y > collision_y)
            {
                //the check clone_x and collision_x
                //if clone_x > collision_x, it means ball_clone is on the right of collision
                if (clone_x >= collision_x)
                {
                    //check if the new position is empty or not 
                    foreach (GameObject go in balls[row - 1])
                    {
                        if (go.GetComponent<Ball>().getColoum() == coloum + 1)
                        {
                            empty_position = false;
                            break;
                        }
                    }

                    if (!empty_position)
                    {
                        ball_clone.transform.position = new Vector3(collision.transform.position.x - 0.32f, collision.transform.position.y + 0.55f);
                        ball_clone.GetComponent<Ball>().setRow(row - 1);
                        ball_clone.GetComponent<Ball>().setColoum(coloum);
                    }
                    else
                    {
                        ball_clone.transform.position = new Vector3(collision.transform.position.x + 0.32f, collision.transform.position.y + 0.55f);
                        ball_clone.GetComponent<Ball>().setRow(row - 1);
                        ball_clone.GetComponent<Ball>().setColoum(coloum + 1);
                    }
                    
                }
                //if clone_x < collision_x , it means ball_clone is on the left of collison
                else if (clone_x < collision_x)
                {
                    //check if the new position is empty or not 
                    foreach (GameObject go in balls[row - 1])
                    {
                        if (go.GetComponent<Ball>().getColoum() == coloum)
                        {
                            empty_position = false;
                            break;
                        }
                    }

                    if (!empty_position)
                    {
                        ball_clone.transform.position = new Vector3(collision.transform.position.x + 0.32f, collision.transform.position.y + 0.55f);
                        ball_clone.GetComponent<Ball>().setRow(row - 1);
                        ball_clone.GetComponent<Ball>().setColoum(coloum + 1);
                    }
                    else
                    {
                        ball_clone.transform.position = new Vector3(collision.transform.position.x - 0.32f, collision.transform.position.y + 0.55f);
                        ball_clone.GetComponent<Ball>().setRow(row - 1);
                        ball_clone.GetComponent<Ball>().setColoum(coloum);
                    }

                    
                }
            }

            //if clone_y < clone_x, it means ball_clone is on the buttom of collision
            else if (clone_y <= collision_y)
            {
                //the check clone_x and collision_x
                //if clone_x > collision_x, it means ball_clone is on the right of collision
                if (clone_x >= collision_x)
                {
                    //check if the new position is empty or not 
                    if (row != ball_list_length - 1)
                    {
                        foreach (GameObject go in balls[row + 1])
                        {
                            if (go.GetComponent<Ball>().getColoum() == coloum + 1)
                            {
                                empty_position = false;
                                break;
                            }
                        }
                    }
                    if (!empty_position)
                    {
                        ball_clone.transform.position = new Vector3(collision.transform.position.x - 0.32f, collision.transform.position.y - 0.55f);
                        ball_clone.GetComponent<Ball>().setRow(row + 1);
                        ball_clone.GetComponent<Ball>().setColoum(coloum);
                    }
                    else
                    {
                        ball_clone.transform.position = new Vector3(collision.transform.position.x + 0.32f, collision.transform.position.y - 0.55f);
                        ball_clone.GetComponent<Ball>().setRow(row + 1);
                        ball_clone.GetComponent<Ball>().setColoum(coloum + 1);
                    }
                    
                }
                //if clone_x < collision_x , it means ball_clone is on the left of collison
                else if (clone_x < collision_x)
                {
                    //check if the new position is empty or not 
                    if (row != ball_list_length - 1)
                    {
                        foreach (GameObject go in balls[row + 1])
                        {
                            if (go.GetComponent<Ball>().getColoum() == coloum)
                            {
                                empty_position = false;
                                break;
                            }
                        }
                    }
                    if (!empty_position)
                    {
                        ball_clone.transform.position = new Vector3(collision.transform.position.x + 0.32f, collision.transform.position.y - 0.55f);
                        ball_clone.GetComponent<Ball>().setRow(row + 1);
                        ball_clone.GetComponent<Ball>().setColoum(coloum+1);
                    }
                    else
                    {
                        ball_clone.transform.position = new Vector3(collision.transform.position.x - 0.32f, collision.transform.position.y - 0.55f);
                        ball_clone.GetComponent<Ball>().setRow(row + 1);
                        ball_clone.GetComponent<Ball>().setColoum(coloum);
                    }

                    
                }
            }
        }

        //add the new ball to the balls list
        int ball_clone_row = ball_clone.GetComponent<Ball>().getRow();
        //creat a new row
        if (ball_clone_row >= ball_list_length)
        {
            List<GameObject> temp = new List<GameObject>();
            temp.Add(ball_clone);
            balls.Add(temp);
        }

        //no need to create a new row
        else
        {
            balls[ball_clone_row].Add(ball_clone);
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
                foreach (GameObject go in balls[row-1])
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
            }
            //odd row
            else {
                //upper
                foreach (GameObject go in balls[row-1])
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
            }
        }
        //other 
        else {
            //even
            if (row % 2 == 0) {
                //upper row
                foreach (GameObject go in balls[row-1])
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
                foreach (GameObject go in balls[row - 1])
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

    public void DestoryList (GameObject go, Color color) {
        if (!visted.Contains(go))
        {
            visted.Add(go);
            if (go.GetComponent<Ball>().mColor == color) {
                destory.Add(go);
                foreach (GameObject neighbour in go.GetComponent<Ball>().neighbours) {
                    DestoryList(neighbour, color);
                }
            }
        }
    }

    //TODO: 
    public void DestoryNeighbour ()
    {
        visted.RemoveRange(0, visted.Count);
        foreach (GameObject go in destory)
        {

        }
    }
    private int FallDownCount (GameObject go) {
        int count = 0;
        if (go.GetComponent<Ball>().neighbours.Count < 4 && !visted.Contains(go)) {
            count++;
            visted.Add(go);
            foreach (GameObject down in balls[go.GetComponent<Ball>().getRow()]) {
                count += FallDownCount(down);
            }
        }
        return count;
    }

    private void FallDown (GameObject go) {
        if (!visted.Contains(go))
        {
            fallDown.Add(go);
            visted.Add(go);
            foreach (GameObject neighbour in go.GetComponent<Ball>().neighbours)
            {
                fallDown.Add(neighbour);
            }
        }
    }

    public void FallDownList (GameObject go)
    {
        visted.RemoveRange(0, visted.Count);
        int count = FallDownCount(go);
        visted.RemoveRange(0, visted.Count);
        if (count == balls[go.GetComponent<Ball>().getRow()].Count)
        {
            if (visted.Count != 0)
                visted.RemoveRange(0, visted.Count);
            FallDown(go);
            visted.RemoveRange(0, visted.Count);
        }
    }
}


