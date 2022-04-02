using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

	public bool playerTurn = true;
	private int score = 0;
	private Enemy enemy;
	private playerInputSystem playerInp;
	private int playerScore = 0;
	private GameManager GM;
	private CombatScript comSript;
	public bool needToChangeTurn = false;
	private ui UIManager;




	public void changeTurn(){
		playerTurn = !playerTurn;	//changes the turn
	}


	private void Start() {
		enemy = GameObject.FindObjectOfType<Enemy>();
		playerInp = GameObject.FindObjectOfType<playerInputSystem>();
		GM = GameObject.FindObjectOfType<GameManager>();
		comSript = GameObject.FindObjectOfType<CombatScript>();
		UIManager = GameObject.FindObjectOfType<ui>();
	}

	private void Update() {
		if (playerTurn == true){
			playerInp.playerTurn();	//so long as its the players turn this will run
		}
		else if (playerTurn == false){
			List<Nodeclass> path = testPathfind();	//gets the path from pathfinding
			if (!CanEnemyAttack()){	//checks if the enemy can attack 
				enemy.moveEnemyOnce(path);	//if not then the enemy moves to the next position in path
			}
			changeTurn();	//calls change turn
		}
	}

	
	private List<Nodeclass> testPathfind(){
		List<Nodeclass> path = new List<Nodeclass>();
		path = enemy.pathfind();	//gets the path
		string pathString = "";
		for (int x = 0; x < path.Count; x++){
			string nextNode = "(" + (path[x].x).ToString() + ", " + (path[x].y).ToString() + ")";
			pathString = pathString + nextNode + "   ";	//simply used for debugging
		}

		Debug.Log(pathString);
		path.RemoveAt(0);	//removes the current position
		return(path);
	}
	public void getCoin(){
		playerScore = playerScore + 5;	//increases the score by 5
		Debug.Log("NEW SCORE IS : " + playerScore);
		UIManager.UpdateScore(5);	//increases score in the ui

	}

	private bool CanEnemyAttack(){
		List<Nodeclass> neighbours = enemy.getNeighbours(GM.getEnemyNode(), GM, GM.getXDimension(), GM.getYDimension());
		Nodeclass playerNode = GM.getPlayerNode();//gets enemuy neighbours and player node

		foreach(Nodeclass node in neighbours){	//checks through all of those neighbours
			if(node == playerNode){	//if any of the neighbours are the player
				comSript.playerHit(5);	//attacks the player
				return(true);//returns true to prevent the enemy from moving
			}
		}
		return(false);
	}

}
