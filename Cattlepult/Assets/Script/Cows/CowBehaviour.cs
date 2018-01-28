using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowBehaviour : MonoBehaviour
{
    enum State { Idle, BreedingIdle, Running, PickUp, CattlePult, inAir, Breeding, PBreeding, Dying};
    public static int maxCowsInPen = 20;

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

    //breedingHeart stuff
    public GameObject breedingHeart;
    private float heartTimer = 0;
    private float heartOffset;

    public int damage;
    // Breeding Timer
    private float breedingTimer = 0;
    public float deathSpeed = 0.08f;

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
        
        partSystem.GetComponent<ParticleSystem>().Stop();
        randomX = Random.Range(-1f, 1f);
        randomY = Random.Range(-1f, 1f);
        rend = GetComponent<Renderer>();
        anim = GetComponent<Animator>();
    }

    public bool canBreed()
    {
        int num = 0;
        foreach (GameObject cow in GameObject.FindGameObjectsWithTag("Cow"))
        {
            if (cow.GetComponent<Collider2D>().isTrigger) continue;
            if (transform.position.x * cow.transform.position.x <= 0) continue;
            num += 1;
        }

        return num < maxCowsInPen;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                Idle();
                breedingTimer -= Time.deltaTime;
                if (breedingTimer <= 0 && canBreed())
                {
                    state = State.BreedingIdle;
                    partSystem.GetComponent<ParticleSystem>().Play();
                }
                break;

            case State.BreedingIdle:
                if (!canBreed()) {
                    state = State.BreedingIdle;
                    partSystem.GetComponent<ParticleSystem>().Stop();
                    breedingTimer = 0;
                }
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

            case State.Dying:
                runAway();
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

    public void runAway()
    {
        Vector3 dir = new Vector3(1, 0, 0);
        if (transform.position.x <= 0) {
            dir *= -1;
            transform.rotation = new Quaternion(0, 0, 0, 0);
        } else {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }

        Vector3 pos = transform.position;
        transform.position = pos + dir * deathSpeed;
        /* when out of screen */
        if (Mathf.Abs(transform.position.x) > 50)
            Destroy(this.gameObject);
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
        transform.localScale = new Vector3(0.15f, 0.15f, 0);
        switch (size)
        {
            case 1:
                sizeScale = 1;
                damage = 1;
                break;
            case 2:
                sizeScale = 1.2f;
                damage = 2;
                break;
            case 3:
                sizeScale = 1.5f;
                damage = 8;
                break;
            case 4:
                sizeScale = 1.75f;
                damage = 20;
                break;
            case 5:
                sizeScale = 2f;
                damage = 50;
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
        transform.Find("Shadow").GetComponent<SpriteRenderer>().enabled = false;
        anim.SetBool("pickUp", true);
    }

    public void droppedByFarmer(Vector3 dir, float dist)
    {
        transform.Find("Shadow").GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<FlightBehaviour>().cattlepulted = false;
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
            breedingTimer = 20;
        }
        if (heartTimer <= 0)
        {
            heartOffset = Random.Range(-0.5f, 0.5f);
            Instantiate(breedingHeart, new Vector3(transform.position.x + heartOffset, transform.position.y, transform.position.z), Quaternion.identity);
            heartTimer = 0.5f;
        }
    }

    private void realBreeding()
    {
        timer -= Time.deltaTime;
        heartTimer -= Time.deltaTime;
        if (timer <= 0)
        {
            partSystem.GetComponent<ParticleSystem>().Stop();
            state = State.Idle;
            timer = 2;
            breedThemCows(size, otherSize);
            breedingTimer = 20;
        }
        if (heartTimer <= 0)
        {
            heartOffset = Random.Range(-0.5f, 0.5f);
            Instantiate(breedingHeart, new Vector3(transform.position.x + heartOffset, transform.position.y, transform.position.z), Quaternion.identity);
            heartTimer = 0.5f;
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

    public int getSize()
    {
        return size;
    }

    public void destroyCow()
    {
        anim.SetBool("pickUp", false);
        anim.SetFloat("speed", 1);
        GetComponent<Collider2D>().isTrigger = true;
        state = State.Dying;
    }
}