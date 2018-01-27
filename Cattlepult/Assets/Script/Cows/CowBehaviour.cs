using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowBehaviour : MonoBehaviour
{
    enum State { Idle, BreedingIdle, Running, PickUp, CattlePult, Breeding, PBreeding };
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


    // Breeding information
    public int size;
    [SerializeField]
    private GameObject cowPrefab;
    private float sizeScale;

    // breeding variables
    private float timer = 2;
    private int otherSize;

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
                Idle();
                break;

            case State.Running:
                Running(farmerPosition);
                break;

            case State.PickUp:
                break;

            case State.CattlePult:
                break;

            case State.Breeding:
                realBreeding();
                break;

            case State.PBreeding:
                psuedoBreeding();
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

    public void setSize(int newSize)
    {
        this.size = newSize;
        switch (size)
        {
            case 1:
                sizeScale = 0;
                break;
            case 2:
                sizeScale = 1.2f;
                break;
            case 3:
                sizeScale = 1.5f;
                break;
            case 4:
                sizeScale = 1.75f;
                break;
            case 5:
                sizeScale = 2f;
                break;
        }
        float scalemod = transform.localScale.x;
        scalemod *= sizeScale;
        transform.localScale = new Vector3(scalemod, scalemod, 0);


    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (state == State.BreedingIdle && other.gameObject.tag == "Cow")
        {
            CowBehaviour otherCow = other.gameObject.GetComponent<CowBehaviour>();
            if (otherCow.state == State.BreedingIdle)
            {
                otherSize = otherCow.size;
                state = State.Breeding;
                otherCow.state = State.PBreeding;
            }
        }
    }

    public void pickUpByFarmer()
    {
        state = State.PickUp;
    }

    public void droppedByFarmer()
    {
        state = State.Idle;
    }

    private void breedThemCows(int parent1, int parent2)
    {
        GameObject babyCow = Instantiate(cowPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        if (parent1 == parent2)
        {
            babyCow.GetComponent<CowBehaviour>().setSize(parent1 + 1);
        }
        else
        {
            babyCow.GetComponent<CowBehaviour>().setSize(Mathf.FloorToInt(parent1 + parent2) / 2);
        }
    }

    private void psuedoBreeding()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            state = State.Idle;
            timer = 2;
        }
    }

    private void realBreeding()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            state = State.Idle;
            timer = 2;
            breedThemCows(size, otherSize);
        }
    }
}