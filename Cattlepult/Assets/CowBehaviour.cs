using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowBehaviour : MonoBehaviour {
    enum State { Idle, BreedingIdle, Running, PickUp, CattlePult, Breeding };
    // Use this for initialization
    private bool isInRightPen;
    private State state;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

            case State.Breeding:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (state == State.BreedingIdle && other.gameObject.tag == "Cow")
        {
            if (other.gameObject.GetComponent<CowBehaviour>().state == State.BreedingIdle)
            {

            }
        }
    }
}
