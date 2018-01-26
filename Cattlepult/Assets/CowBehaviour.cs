using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowBehaviour : MonoBehaviour {
    enum State { Idle, BreedingIdle, Running, PickUp, CattlePult, Breeding };
    // Use this for initialization
    private bool isInRightPen;
    private State state;
    private Vector3 position;
    private int walkingTimer;
    private int randomWalkingTime;
    private int standingTimer;
    private int randomStandingTime;
    private float randomX, randomY;
    void Start()
    {
        state = State.Idle;
        randomWalkingTime = Random.Range(60, 180);
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
            
            Debug.Log(randomX + " " + randomY); 
            movement(new Vector3(randomX, randomY, 0));
            randomStandingTime = Random.Range(60, 180);
            standingTimer = 0;
        }
        else {
            standingTimer++;
            if (standingTimer > randomStandingTime) {
                walkingTimer = 0;
                randomWalkingTime = Random.Range(180, 480);
                randomX = Random.Range(-1f, 2f);
                randomY = Random.Range(-1f, 2f);
            }
        }
        
    }

    private void movement(Vector3 move)
    {
        move /= 100;
        position = transform.position;
        position += move;
        transform.position = position;
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
