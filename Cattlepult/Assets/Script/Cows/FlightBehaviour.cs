using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightBehaviour : MonoBehaviour {

    private Vector3[] pFlight = new Vector3[3];
    private Vector3[] pScale = new Vector3[3];
    private Vector3[] pShadow = new Vector3[3];
    private float t;
    private float flightTime;
    private float flightScale = 0.01f;
    private Vector3 stdScale;
    private float maxFlightScale;
    private Vector3 target;
    private Renderer rend;
    private Vector3 shadowLocPos;
    private Vector3 shadowLocScale;
    private GameObject shadow;
    public bool cattlepulted = false;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void throwCow(Vector3 origin, Vector3 target, float height = 0, float flightTime = 1, float maxFlightScale = 0)
    {
        this.shadow = this.transform.Find("Shadow").gameObject;
        GetComponent<Collider2D>().isTrigger = true;
        this.flightTime = flightTime;
        Vector3 dVec = target - origin;
        Vector3 ortVec = new Vector3(dVec.y, -dVec.x, 0) * height;

        if (ortVec.y < 0) ortVec *= -1;

        pFlight[0] = origin;
        pFlight[1] = origin + dVec / 2 + ortVec;
        pFlight[2] = target;

        pScale[0] = new Vector3(0, 1, 0);
        pScale[1] = new Vector3(dVec.magnitude / 2, maxFlightScale + 1, 0);
        pScale[2] = new Vector3(dVec.magnitude, 1, 0);

        pShadow[0] = origin;
        pShadow[1] = origin + dVec / 2 + new Vector3(dVec.y, -dVec.x, 0);
        pShadow[2] = target;

        stdScale = transform.localScale;
        this.target = target;
        this.maxFlightScale = maxFlightScale;
        shadowLocPos = shadow.transform.localPosition;
        shadowLocScale = shadow.transform.localScale;
        t = 0;

        GetComponent<CowBehaviour>().setFlying();
    }

    public void landCow()
    {
        GetComponent<Collider2D>().isTrigger = false;
        if (cattlepulted)
        {
            foreach (GameObject cow in GameObject.FindGameObjectsWithTag("Cow"))
            {
                if (GetComponent<Collider2D>().IsTouching(cow.GetComponent<Collider2D>()))
                {
                    if (GetComponent<CowBehaviour>().getSize() + 1 >= cow.GetComponent<CowBehaviour>().getSize())
                    {
                        cow.GetComponent<CowBehaviour>().destroyCow();
                        Destroy(this.gameObject);
                    }
                }
            }
            foreach (GameObject farmer in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (GetComponent<Collider2D>().IsTouching(farmer.GetComponent<CircleCollider2D>()))
                {
                    farmer.GetComponent<FarmerBehaviour>().stunFarmer(this.gameObject.GetComponent<CowBehaviour>().getSize());
                }
            }
        }

        foreach (GameObject Barn in GameObject.FindGameObjectsWithTag("Barn"))
        {
            if (GetComponent<Collider2D>().IsTouching(Barn.GetComponent<Collider2D>()))
            {
                Barn.GetComponent<House1>().takeDamage(this.gameObject.GetComponent<CowBehaviour>().damage);
                Destroy(this.gameObject);
            }
        }
        
            
        transform.position = target;
        GetComponent<CowBehaviour>().setIdle();
        GetComponent<CowBehaviour>().setAnimationBool(false);
        rend.sortingOrder = 2;

    }

    public void flyLikeABird()
    {
        float dt = 1 / flightTime;
        t += dt * Time.deltaTime;

        Vector3 pos = (1 - t)*(1 - t) * pFlight[0] + 2 * (1 - t) * t * pFlight[1] + t * t * pFlight[2];
        transform.position = pos;

        Vector3 scale = stdScale * ((1 - t)*(1 - t) * pScale[0] + 2 * (1 - t) * t * pScale[1] + t * t * pScale[2]).y;
        transform.localScale = scale;

        Vector3 posNoHeight = (1 - t) * (1 - t) * pShadow[0] + 2 * (1 - t) * t * pShadow[1] + t * t * pShadow[2];
        Vector3 posDif = posNoHeight - pos;

        shadow.transform.localPosition = shadowLocPos + posDif;

        if (t + dt * Time.deltaTime > 1)
        {
            landCow();
        }
    }

}
