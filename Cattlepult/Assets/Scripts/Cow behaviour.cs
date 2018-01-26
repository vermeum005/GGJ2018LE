using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cowbehaviour : MonoBehaviour {
    enum State { Idle, BreedingIdle, Running, PickUp, CattlePult};
    // Use this for initialization
    private State state;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
		switch (state)
        {
            case State.Idle:
                break;

            case State.BreedingIdle:
                break;

            case State.Running:
                break;

            case State.PickUp:
                break;

            case State.CattlePult:
                break;
        }
	}
}
