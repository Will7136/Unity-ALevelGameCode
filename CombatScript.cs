using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatScript : MonoBehaviour {


	[SerializeField] public int playerHealth = 20;
	[SerializeField] public int enemyHealth = 20;
	private Enemy enemy;
	private playerInputSystem pInputS;
	private TurnManager TM;
	private ui UIManager; 


	public void playerHit(int Damage){
		Debug.Log("Player health was " + playerHealth);
		playerHealth = playerHealth - Damage;	//changes the players actual health in the code
		Debug.Log("Player health is " + playerHealth);

			// if (!TM.needToChangeTurn){
			// 	TM.needToChangeTurn = true;
			// }
			// else{
			// 	TM.needToChangeTurn = false;
			// }
			UIManager.UpdatePHealth(Damage);//updates the health in the ui 
	}
	public void enemyHit(int Damage){
		Debug.Log("Enemy health was " + enemyHealth);
		enemyHealth = enemyHealth - Damage;	//changes the enemies actual health in the code
		Debug.Log("Enemy health is " + enemyHealth);
		UIManager.UpdateEHealth(Damage);	//updates the health in the ui 

		if (!TM.needToChangeTurn){
			TM.needToChangeTurn = true;
		}
		else{	//used to only change the turn every two player turns
			TM.changeTurn();
			TM.needToChangeTurn = false;
		}
	}

	private void Update() {
		if (enemyHealth <= 0){	//checks if the player has won
			Destroy(GameObject.FindGameObjectWithTag("Enemy"));
			Debug.Log("You Win");
			pInputS.GetComponent("playerInputSystem");
			UIManager.UpdateScore(20);	//adds the score for defeating the enemy 
			SceneManager.LoadScene("Win");	//loads the win screen scene
		}
		else if(playerHealth <= 0){//checks if the enemy has won
			Destroy(GameObject.FindGameObjectWithTag("Player"));
			Destroy(enemy.GetComponent("Enemy"));
			SceneManager.LoadScene("You Lose");	//loads the lose screen scene
		}
	}

	private void Start() {
		pInputS = GameObject.FindObjectOfType<playerInputSystem>();
		enemy = GameObject.FindObjectOfType<Enemy>();
		TM = GameObject.FindObjectOfType<TurnManager>();
		UIManager = GameObject.FindObjectOfType<ui>();
	}

}
