using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class House1 : MonoBehaviour {

    private int hp;
    private int winner;

	// Use this for initialization
	void Start () {
        hp = 100;
	}
	
	// Update is called once per frame
	void Update () {
        if (hp <= 0) {
            gameOver();
            winner = 1;
        }
	}
    public int getWinner() {
        return winner;
    }

    public void takeDamage(int damage) {
        hp -= damage;
    }

    void gameOver() {

        SceneManager.LoadScene("EndScene2");
    }
}
