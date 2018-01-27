using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowBehaviour : MonoBehaviour
{
    enum State { Idle, BreedingIdle, Running, PickUp, CattlePult, Breeding };
    // Use this for initialization
    private bool isInRightPen;
    private State state;
    private Vector3 position;
    private Vector3 runningVector;
    private int walkingTimer;
    private int randomWalkingTime;
    private int standingTimer;
    private int randomStandingTime;
    private float randomX, randomY;
    private Vector3 farmerPosition;
    private int randomRunningTime;
    private int runningTimer;
    private float runningSpeed = 0.03f;
    void Start()
    {
        state = State.Idle;
        randomWalkingTime = Random.Range(60, 180);
        randomRunningTime = Random.Range(60, 180);
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case State.Idle:
                Idle();
                break;

            case State.BreedingIdle:
                break;

            case State.Running:
                Running(farmerPosition);
                break;

            case State.PickUp:
                break;

            case State.CattlePult:
                break;

            case State.Breeding:
                break;
        }
    }
    void Idle()
    {
        walkingTimer++;

        if (walkingTimer < randomWalkingTime)
        {
            movement(new Vector3(randomX, randomY, 0));
            randomStandingTime = Random.Range(60, 180);
            standingTimer = 0;
        }
        else
        {
            standingTimer++;
            if (standingTimer > randomStandingTime)
            {
                walkingTimer = 0;
                randomWalkingTime = Random.Range(180, 480);
                randomX = Random.Range(-1f, 1f);
                randomY = Random.Range(-1f, 1f);
            }
        }

    }

    void Running(Vector3 farmerPosition)
    {
        position = transform.position;
        runningTimer++;
     
        
        
        position += runningVector * runningSpeed;
        transform.position = position;
        if (runningTimer > randomRunningTime) {
            runningTimer = 0;
            state = State.Idle;
            
        }
    }

    private void movement(Vector3 move)
    {
        move /= 100;
        position = transform.position;
        position += move;
        transform.position = position;
    }

    public void setRunning(Vector3 farmerPosition)
    {       
        state = State.Running;
        this.farmerPosition = farmerPosition;
        runningVector = position - farmerPosition;
        runningVector.Normalize();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (state == State.BreedingIdle && other.gameObject.tag == "Cow")
        {
            if (other.gameObject.GetComponent<CowBehaviour>().state == State.BreedingIdle)
            {

            }
        }
    }
}