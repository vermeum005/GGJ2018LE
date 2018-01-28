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

    public GameObject cattlePult;
    private bool aiming = false;

    private int bellRingTimer;
    private int bellRingTime = 120;

    public float maxSpeed = 1;        //Speed at which the farmer moves
    public float pickUpDist = 10;
    //private bool isInRightPen = false;
    [SerializeField]
    private bool dontThrow = false;
    private Animator anim;
    private float offsety = 0.90f;
    private float offsetx = 0.10f;
    // stun variables
    public bool stunned = false;
    public float stunTimer = 0;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!aiming && !stunned)
        {
            movement();
            movePickedUpCow();
            handleInput();
        }
        else if (stunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                stunned = false;
                anim.SetBool("stunned", false);
            }
        }
	}

    void movement(){
        /* Downscale speed for easy of use */
        speed = maxSpeed / 100;

        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            anim.SetFloat("speed", 1);
        }
        else
        {
            anim.SetFloat("speed", 0);
        }
        if (new Vector3(moveHorizontal, moveVertical, 0).magnitude != 0)
            direction = new Vector3(moveHorizontal, moveVertical, 0).normalized;
        if (direction.x > 0)
        {
            transform.rotation = new Quaternion(0,180,0, 0);
        }
        else
            transform.rotation = new Quaternion(0, 0, 0, 0);
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
        bellRingTimer++;

        if (Input.GetButtonDown("Fire1")) {
            if (pickedUpCow != null && !dontThrow) ejectCow();
            else pickUpCow();
        } if (Input.GetButtonDown("Fire2") && bellRingTimer > bellRingTime) {
            ringBell();
            bellRingTimer = 0;
        }
    }

    public void ejectCow() {
        pickedUpCow.GetComponent<CowBehaviour>().droppedByFarmer(direction.normalized, 3);
        pickedUpCow = null;
        anim.SetBool("pickup", false);
    }

    public void movePickedUpCow() {
        if (pickedUpCow != null) {
            Vector3 tmpPos = transform.position;
            tmpPos.y += offsety;
            tmpPos.x += offsetx;
            pickedUpCow.transform.position = tmpPos;
        }
    }

    public void pickUpCow() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, pickUpDist);
        if (hit.collider != null) {
            if (hit.collider.gameObject.CompareTag("Cow")) {
                hit.collider.gameObject.GetComponent<CowBehaviour>().pickUpByFarmer();
                pickedUpCow = hit.collider.gameObject;
                anim.SetBool("pickup", true);
            }
        } 
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (pickedUpCow != null && other.tag == "CattlePult" && Input.GetButtonDown("Fire1"))
        {
            cattlePult.GetComponent<CattlePult>().loadCattlePult(this.gameObject, pickedUpCow);
            pickedUpCow.GetComponent<CowBehaviour>().loadCattlePult();
            aiming = true;
            pickedUpCow = null;
            anim.SetBool("pickup", false);
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "CattlePult")
            dontThrow = true;
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "CattlePult")
            dontThrow = false;
    }

    public void stopAiming()
    {
        aiming = false;
    }
    public void stunFarmer(int size)
    {
        anim.SetBool("stunned", true);
        switch (size)
        {
            case 1:
                stunTimer = 0.3f;
                stunned = true;
                break;
            case 2:
                stunTimer = 0.5f;
                stunned = true;
                break;
            case 3:
                stunTimer = 1f;
                stunned = true;
                break;
            case 4:
                stunTimer = 1.5f;
                stunned = true;
                break;
            case 5:
                stunTimer = 2f;
                stunned = true;
                break;
        }
    }
}
