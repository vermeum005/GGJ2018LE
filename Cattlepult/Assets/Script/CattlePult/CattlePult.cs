using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CattlePult : MonoBehaviour {
    public enum PultState { Loaded, Shooting, Empty };
    private PultState state;
    public GameObject crosshair;
    public GameObject farmer;
    private Animator anim;
    // Use this for initialization
    void Start() {
        state = PultState.Empty;
        anim = GetComponent<Animator>();
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

    public void loadCattlePult()
    {
        Debug.Log("loaded");
        anim.SetBool("Loaded", true);
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
    }
}
