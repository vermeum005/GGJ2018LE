using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowBehaviour : MonoBehaviour
{
    enum State { Idle, BreedingIdle, Running, PickUp, CattlePult, inAir, Breeding, PBreeding };

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
    private float runningTime;
    private int runningTimer;
    private float runningSpeed = 0.03f;
    public float bellRingRange = 2;
    private Animator anim;

    public GameObject partSystem;
    // Breeding Timer
    private float breedingTimer = 5; 

    // Breeding information
    public int size;
    [SerializeField]
    private GameObject cowPrefab;
    private float sizeScale;
    private Renderer rend;

    // breeding variables
    private float timer = 2;
    private int otherSize;

    public float height = 10;
    public float timeInAir = 10;

    void Start()
    {
        state = State.Idle;
        randomWalkingTime = Random.Range(60, 180);
        
        randomX = Random.Range(-1f, 1f);
        randomY = Random.Range(-1f, 1f);
        rend = GetComponent<Renderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                Idle();
                breedingTimer -= Time.deltaTime;
                if (breedingTimer <= 0)
                {                    
                    state = State.BreedingIdle;
                    partSystem.GetComponent<ParticleSystem>().Play();
                }
                break;

            case State.BreedingIdle:
                Idle();
                break;

            case State.Running:
                Running();
                break;

            case State.PickUp:
                // insert struggle animation
                break;

            case State.CattlePult:
                break;

            case State.inAir:
                GetComponent<FlightBehaviour>().flyLikeABird();
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
            randomStandingTime = Random.Range(60, 180);
        }
        else
        {
            standingTimer++;
            anim.SetFloat("speed", 0);
            if (standingTimer > randomStandingTime)
            {
                walkingTimer = 0;
                randomWalkingTime = Random.Range(180, 480);
                randomX = Random.Range(-1f, 1f);
                randomY = Random.Range(-1f, 1f);
            }
        }

    }

    public void setIdle()
    {
        state = State.Idle;
    }
    public void setAnimationBool(bool watmotje)
    {
        anim.SetBool("pickUp", watmotje);
    }

    void Running()
    {
        position = transform.position;
        runningTimer++;

        position += runningVector * runningSpeed;
        transform.position = position;
        if (runningTimer > runningTime)
        {
            runningTimer = 0;
            setIdle();
        }
    }

    private void movement(Vector3 move)
    {
        move /= 100;
        position = transform.position;
        position += move;
        transform.position = position;
        if (move.x > 0)
            transform.rotation = new Quaternion(0, 180, 0, 0);
        else
            transform.rotation = new Quaternion(0, 0, 0, 0);
        anim.SetFloat("speed", 1);
    }

    private void cattlepult()
    {

    }

    public void setRunning(Vector3 farmerPosition)
    {
        
        this.farmerPosition = farmerPosition;
        runningVector = position - farmerPosition;
        if (runningVector.magnitude < bellRingRange) {
            runningTime = (bellRingRange - runningVector.magnitude) * 40;
            runningVector.Normalize();
            state = State.Running;            
        }      
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
        GetComponent<Collider2D>().isTrigger = true;
        anim.SetBool("pickUp", true);
    }

    public void droppedByFarmer(Vector3 dir, float dist)
    {
        GetComponent<FlightBehaviour>().throwCow(this.transform.position, this.transform.position + (dir * dist), 0, 1f, 1);
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
            partSystem.GetComponent<ParticleSystem>().Stop();
            state = State.Idle;
            timer = 2;
        }
    }

    private void realBreeding()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            partSystem.GetComponent<ParticleSystem>().Stop();
            state = State.Idle;
            timer = 2;
            breedThemCows(size, otherSize);
        }
    }

    public void setFlying()
    {
        rend.enabled = true;
        state = State.inAir;
        rend.sortingOrder = 6;
    }

    public void loadCattlePult()
    {
        state = State.CattlePult;
        rend.enabled = false;
    }
}