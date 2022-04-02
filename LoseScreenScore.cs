using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreenScore : MonoBehaviour {

	void Start () {
		GameObject uiManager = GameObject.FindGameObjectWithTag("oldUImanager");
		int score = uiManager.GetComponent<ui>().getScore();
		Text scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
		scoreText.text = ("Score was " + score.ToString() + " points");	//shows the score they accumulated across the game
		uiManager.GetComponent<ui>().resetScore();	//resets the players score when they lose
	}
}
