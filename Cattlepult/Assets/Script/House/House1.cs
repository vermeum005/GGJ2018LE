using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class House1 : MonoBehaviour {

    public float hp;
    private int winner;
    public bool win;
    public Slider health;

    // Use this for initialization
    void Start () {
        hp = 100;
        health.value = hp / 100;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (hp <= 0 || win) {
            gameOver();
            winner = 1;
        }
	}
    public int getWinner() {
        return winner;
    }

    public void takeDamage(int damage) {
        hp -= damage;
        health.value = hp / 100;
    }

    void gameOver() {
        if (this.gameObject.name == "BarnRight")
        {
            SceneManager.LoadScene("EndScene2");
        }
        else
            SceneManager.LoadScene("EndScene1");
    }


}
