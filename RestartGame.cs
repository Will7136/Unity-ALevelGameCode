using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour {



	void Start () {
		
	}
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return)){	//if the user presses enter restart the game
			SceneManager.LoadScene("SampleScene");
		}
	}
}
