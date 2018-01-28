using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breedingHeart : MonoBehaviour {
    private Vector3 position;
    private float lifeTime = 3;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        lifeTime -= Time.deltaTime;
        position = transform.position;
        position.y += 0.005f;
        transform.position = position;
        if (lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }

	}
}
