using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerBehaviour : MonoBehaviour {

    private float speed;
    private GameObject pen;
    private float radius;

    [SerializeField]
    private float maxSpeed = 1;        //Speed at which the farmer moves
    //private bool isInRightPen = false;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void FixedUpdate () {
        movement();
	}

    void movement(){

        /* Downscale speed for easy of use */
        speed = maxSpeed / 100;

        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");

        Vector3 tmpPos = transform.position;
        tmpPos += new Vector3(moveHorizontal, moveVertical, 0) * speed;

        transform.position = tmpPos;
    }
}
