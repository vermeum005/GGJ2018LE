﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {

    // Use this for initialization
    public GameObject cattlePult;
    private bool active = true;
    private float speed;
    private Vector3 direction;
    public float maxSpeed = 2;
    private Renderer rend;
    private Vector3 startPos;
    void Start () {
        startPos = cattlePult.transform.position;
        this.transform.position = startPos;
        rend = GetComponent<Renderer>();
        rend.enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (active)
        {
            movement();
            inputs();
        }
	}
    public void activateCrosshair()
    {
        active = true;
        rend.enabled = true;
    }
    public Vector3 getPosition()
    {
        return transform.position;
    }
    void movement()
    {
        /* Downscale speed for easy of use */
        speed = maxSpeed / 100;

        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        direction = new Vector3(moveHorizontal, moveVertical, 0);

        Vector3 tmpPos = transform.position;
        tmpPos += new Vector3(moveHorizontal, moveVertical, 0) * speed;

        transform.position = tmpPos;
    }
    void inputs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //give location information to Jelle's code
            active = false;
            rend.enabled = false;
            cattlePult.GetComponent<CattlePult>().firePult(transform.position);
        }
    }
}
