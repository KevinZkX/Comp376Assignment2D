﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGun : MonoBehaviour {

    private Vector2 direction = new Vector2(0,1);
    private float angle;
    private Vector3 axis;
    private bool ready_to_shoot = false;

    private Yellow_Ball yellow_clone;
    private Red_Ball red_clone;
    private Ball ball_clone;

    private bool yellow = false;
    private bool red = false;

    [SerializeField]
    GameObject gamePlay;
    [SerializeField]
    GameObject yellowBallPerfab;
    [SerializeField]
    GameObject redBallPerfab;
    [SerializeField]
    GameObject ballPerfab;

	// Use this for initialization
	void Start () {

        for (int i = 0; i <= 6; i++)
        {
            float y = 2.8f - 0.55f * i;
            if (i % 2 == 0)
            {
                for (int j = 0; j < 8; j++)
                {
                    float x = -2.24f + 0.64f * j;

                    ball_clone = Instantiate(ballPerfab, new Vector3(x, y), Quaternion.identity).GetComponent<Ball>();

                }

            }
            else
            {
                for (int j = 0; j < 7; j++)
                {
                    float x = -2.24f + 0.32f + 0.64f * j;
                    ball_clone = Instantiate(ballPerfab, new Vector3(x, y), Quaternion.identity).GetComponent<Ball>();
                }
            }
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        
        //rotate towards left
        /*It will first check if the left button is pushed,
         * Then, it will check the if the angle is not greater than 90 and not samller than 265 (265 here is to make sure the arrow can rotate
         * back because if the angle is smaller than 270 exctally, the condition will fale.
         */
        if (Input.GetButton("Left") && !(transform.rotation.eulerAngles.z > 90.0f && transform.rotation.eulerAngles.z < 265.0f)) {
            
            transform.RotateAround(transform.position, -Vector3.forward, -100f * Time.deltaTime);
			this.transform.rotation.ToAngleAxis(out angle, out axis);
            angle = angle * Mathf.PI / 180f;
		}

		//rotate towards right
		/*It will first check if the right button is pushed,
         * Then, it will check the if the angle is not smaller than 270 and not greater than 95 (95 here is to make sure the arrow can rotate
         * back because if the angle is greater than 270 exctally, the condition will fale.
         */
		else if (Input.GetButton("Right") && !(transform.rotation.eulerAngles.z > 95.0f && transform.rotation.eulerAngles.z < 270.0f)) {
           
            transform.RotateAround(transform.position, -Vector3.forward, 100f * Time.deltaTime);
			this.transform.rotation.ToAngleAxis(out angle, out axis);
            angle = (360 - angle) * Mathf.PI / 180f;
        }

        //shoot the ball
        if (Input.GetButtonDown("Shoot") && ready_to_shoot) {
            //if (yellow) 
            //{
            //    yellow_clone.setDirection(gameObject.transform.up);
            //    yellow_clone.SetupCollider();
            //    ready_to_shoot = false;
            //    yellow = false;
            //}
            //else if (red) 
            //{
            //    red_clone.setDirection(gameObject.transform.up);
            //    ready_to_shoot = false;
            //    red = false;
            //}

            ball_clone.move(gameObject.transform.up);
            ready_to_shoot = false;

        }

        else if (!ready_to_shoot)
		{


            ball_clone = Instantiate(ballPerfab, this.transform.position, Quaternion.identity).GetComponent<Ball>();
            ready_to_shoot = true;

            //if (colour_range < 50) 
            //{
            //    yellow_clone = Instantiate(yellowBallPerfab, this.transform.position, Quaternion.identity).GetComponent<Yellow_Ball>();
            //    //gamePlay.AddComponent<Yellow_Ball>();
            //    yellow = true;
            //    ready_to_shoot = true;
            //    yellow_clone.setZeroVelocity();
            //}
            //else if (colour_range > 50)
            //{
            //    red_clone = Instantiate(redBallPerfab, this.transform.position, Quaternion.identity).GetComponent<Red_Ball>();
            //    red = true;
            //    ready_to_shoot = true;
            //    red_clone.setZeroVelocity();
            //}
		}
	}
}
