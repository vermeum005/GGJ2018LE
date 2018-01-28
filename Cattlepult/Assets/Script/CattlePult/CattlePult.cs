using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CattlePult : MonoBehaviour {
    public enum PultState { Loaded, Shooting, Empty };
    private PultState state;
    public GameObject crosshair;
    private GameObject farmer;
    private GameObject loadedCow;
    public float throwHeight = 1f;
    public float airTime = 2f;
    public float maxScale = 1f;
    private Animator anim;

    public int playerId;

    // Use this for initialization
    void Start() {
        state = PultState.Empty;
        anim = GetComponent<Animator>();
        if (playerId == 2)
            transform.rotation = new Quaternion(0, 180, 0, 0);
    }

    // Update is called once per frame
    void Update() {
        switch (state)
        {
            case PultState.Empty:
                break;
            case PultState.Loaded:
                //aimingPult();
                break;
            case PultState.Shooting:
                break;
        }
    }

    public void loadCattlePult(GameObject farmer, GameObject cow)
    {
        this.farmer = farmer;
        anim.SetBool("Loaded", true);
        loadedCow = cow;
        state = PultState.Loaded;
        crosshair.GetComponent<Crosshair>().activateCrosshair();
    }

    public PultState getState()
    {
        return state;
    }

    public void firePult(Vector3 crosspos)
    {
        anim.SetBool("Loaded", false);
        farmer.GetComponent<FarmerBehaviour>().stopAiming();
        loadedCow.GetComponent<FlightBehaviour>().cattlepulted = true;
        loadedCow.GetComponent<FlightBehaviour>().throwCow(transform.position, crosspos, throwHeight, airTime, maxScale);
        loadedCow.transform.Find("Shadow").GetComponent<SpriteRenderer>().enabled = true;
    }
}
