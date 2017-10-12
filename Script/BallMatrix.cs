using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the really game controller
public class BallMatrix : MonoBehaviour{

    //private int row = 2;
    //private int coloum = 8;
    public List<List<GameObject>> balls = new List<List<GameObject>>();

    private void Start()
    {
        addCurrentBalls();
    }


    private void BubbleSort (GameObject[] resource, int size) {
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
                if (resource[j + 1].transform.position.x < resource[j].transform.position.x & resource[j + 1].transform.position.y.Equals(resource[j].transform.position.y))
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
            foreach (GameObject ball in allBalls) {
                if (Mathf.Approximately((2.8f - 0.55f * i), ball.GetComponent<Ball>().transform.position.y) && !ball.GetComponent<Ball>().added) {
                    if (ball != null) {
                        temp.Add(ball);
                        ball.GetComponent<Ball>().added = true;
                    }
                }
            }
            if (temp.Count != 0)
                balls.Add(temp);
            Debug.Log("Temp: " + temp.Count);
        }
        Debug.Log("Balls: " + balls.Count);
    }

    public void addNewBall () {
        
    }

}
