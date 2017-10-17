using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixController : MonoBehaviour {

    BallMatrix ballMatrix;

	// Use this for initialization
	void Start () {
        ballMatrix = BallMatrix.CreateBallMatrix;
	}
	
	// Update is called once per frame
	//void LateUpdate () {
 //       ballMatrix.modifyFallList();
 //       Debug.Log("Fall list modified. actualFallDown count: " + ballMatrix.actualFallDown.Count);
	//}
}
