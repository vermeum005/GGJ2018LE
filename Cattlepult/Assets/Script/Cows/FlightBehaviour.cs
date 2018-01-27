using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightBehaviour : MonoBehaviour {

    private Vector3[] pFlight = new Vector3[3];
    private Vector3[] pScale = new Vector3[3];
    private float t;
    private float flightTime;
    private float flightScale = 0.01f;
    private Vector3 stdScale;
    private float maxFlightScale;
    private Vector3 target;
    private Renderer rend;
    private void Start()
    {
        rend = GetComponent<Renderer>();
    }
    public void throwCow(Vector3 origin, Vector3 target, float height = 0, float flightTime = 1, float maxFlightScale = 0)
    {
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

        stdScale = transform.localScale;
        this.target = target;
        this.maxFlightScale = maxFlightScale;
        t = 0;

        GetComponent<CowBehaviour>().setFlying();
    }

    public void landCow()
    {
        GetComponent<Collider2D>().isTrigger = false;
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

        if (t + dt * Time.deltaTime > 1)
        {
            landCow();
        }
    }
}
