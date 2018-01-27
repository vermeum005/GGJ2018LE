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
    public float maxFlightScale = 2;
    
    public void throwCow(Vector3 origin, Vector3 target, float height = 10, float flightTime = 4)
    {
        this.flightTime = flightTime;
        Vector3 dVec = target - origin;

        pFlight[0] = origin;
        pFlight[1] = dVec / 2 + new Vector3(dVec.y, -dVec.x, 0) * height;
        pFlight[2] = target;

        pScale[0] = new Vector3(0, 1, 0);
        pScale[1] = new Vector3(dVec.magnitude / 2, maxFlightScale + 1, 0);
        pScale[2] = new Vector3(dVec.magnitude, 1, 0);

        stdScale = transform.localScale;
        t = 0;

        GetComponent<CowBehaviour>().setFlying();
    }

    public void landCow()
    {
        GetComponent<CowBehaviour>().setIdle();
    }

    public void flyLikeABird()
    {
        float dt = 1 / flightTime;
        t += dt * Time.deltaTime;

        Vector3 pos = Mathf.Pow((1 - t), 2) * pFlight[0] + 2 * (1 - t) * t * pFlight[1] + t * t * pFlight[2];
        transform.position = pos;

        Vector3 scale = stdScale * (Mathf.Pow((1 - t), 2) * pScale[0] + 2 * (1 - t) * t * pScale[1] + t * t * pScale[2]).y;
        transform.localScale = scale;

        if (t + dt * Time.deltaTime > 1)
        {
            landCow();
        }

    }
}
