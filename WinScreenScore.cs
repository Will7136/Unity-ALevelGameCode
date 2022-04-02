using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreenScore : MonoBehaviour {

	void Start () {
		int score = GameObject.FindGameObjectWithTag("oldUImanager").GetComponent<ui>().getScore();
		Text scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
		scoreText.text = ("Score is " + score.ToString() + " points");	//changes value of score in the win screen text
	}
}
