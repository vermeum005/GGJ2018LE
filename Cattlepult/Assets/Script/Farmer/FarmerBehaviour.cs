using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerBehaviour : MonoBehaviour {

    private float speed;
    private GameObject pen;
    private float radius;

    private GameObject[] cowList;

    private Vector3 direction;
    private GameObject pickedUpCow;


    public float maxSpeed = 1;        //Speed at which the farmer moves
    public float pickUpDist = 10;
    //private bool isInRightPen = false;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void FixedUpdate () {
        movement();
        movePickedUpCow();
        handleInput();
	}

    void movement(){
        /* Downscale speed for easy of use */
        speed = maxSpeed / 100;

        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");
        if (new Vector3(moveHorizontal, moveVertical, 0).magnitude != 0)
            direction = new Vector3(moveHorizontal, moveVertical, 0).normalized;

        Vector3 tmpPos = transform.position;
        tmpPos += new Vector3(moveHorizontal, moveVertical, 0) * speed;

        transform.position = tmpPos;
    }

    void ringBell() {
        cowList = GameObject.FindGameObjectsWithTag("Cow");
        foreach (GameObject cow in cowList) {
            cow.GetComponent<CowBehaviour>().setRunning(transform.position);
        }
    }

    void handleInput() {
        if (Input.GetButtonDown("Fire1")) {
            if (pickedUpCow != null) ejectCow();
            else pickUpCow();
        } if (Input.GetButtonDown("Fire2")) {
            ringBell();
        }
    }

    public void ejectCow() {
        Debug.Log(direction.normalized);
        pickedUpCow.GetComponent<CowBehaviour>().droppedByFarmer(direction.normalized, 3);
        pickedUpCow = null;
    }

    public void movePickedUpCow() {
        if (pickedUpCow != null) {
            pickedUpCow.transform.position = transform.position;
        }
    }

    public void pickUpCow() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, pickUpDist);
        if (hit.collider != null) {
            if (hit.collider.gameObject.CompareTag("Cow")) {
                hit.collider.gameObject.GetComponent<CowBehaviour>().pickUpByFarmer();
                pickedUpCow = hit.collider.gameObject;
            }
        } 
    }
}
