using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTileColor : MonoBehaviour {

	SpriteRenderer spriteRend;
	public int SelctVal;
	private DrawGrid drawGrid;
	private GameManager GM;
	private Enemy enemy;
	private bool neighbourIsEnemy = false;
	private int enemylistLoaction = 0;



	private void Start () {
		spriteRend = GetComponent<SpriteRenderer>();	//finds the sprite renderer
		GM = GameObject.FindObjectOfType<GameManager>();	//finds the game Manager
		drawGrid = GameObject.FindObjectOfType<DrawGrid>();	//gets draw grid
		enemy = GameObject.FindObjectOfType<Enemy>();// gets Enemy
	}

	public void SelectTile(Color chosenColor){	// takes a color as a parameter
		spriteRend.color = chosenColor;	//changes the sprite color to the color from the parameter
	}
	public void DeselectTile(){
		spriteRend.color = Color.white;//sets the the color of the tile to white
	}

	public void SelectPlayerTile(int X, int Y){
		List<GameObject> suroundingTiles = GetSurroundingTiles(X, Y);	// gets all surrounding tiles
		int counter = 0;	//counter for how far through the list we are
		foreach(GameObject tile in suroundingTiles){
			if (neighbourIsEnemy){	//if one of the gameobjects contains the enemy
				if (counter == enemylistLoaction){// if the current item in the list is the same one that contains the enemy
					tile.GetComponent<ChangeTileColor>().SelectTile(Color.red);	//changes the color to red 
				}
				else{
					tile.GetComponent<ChangeTileColor>().SelectTile(Color.blue);	// changes the color to blue
				}
			}
			else{
				tile.GetComponent<ChangeTileColor>().SelectTile(Color.blue);	// changes the color to blue
			}
			counter = counter + 1;	//increments the counter
		}
		SelectTile(Color.blue);	//colors the current tile blue
	}

	public void DeselectPlayerTile(int X , int Y){
		List<GameObject> suroundingTiles = GetSurroundingTiles(X, Y);	// gets all surrounding tiles
		foreach(GameObject tile in suroundingTiles){
			tile.GetComponent<ChangeTileColor>().DeselectTile();	//deselects each one
		}
		DeselectTile();	//deselects the current tile
	}

	private List<GameObject> GetSurroundingTiles(int X, int Y){
		List<Nodeclass> NodeList = new List<Nodeclass>();	//list for the Neighbour Nodes
		List<GameObject> TilesList = new List<GameObject>();	// list for the corresponding Gameobjects

		NodeList = enemy.getNeighbours(GM.getNode(X, Y), GM, GM.getXDimension(), GM.getYDimension());	//calls getNeighbours from the enemy script 
		int c = 0;	//c is used as a counter of how many loops there have been
		foreach(Nodeclass node in NodeList){
			if (node.contents == 2){	//if any of these tiles contain the enemy 
				neighbourIsEnemy = true;	//bool shows one of the tiles conatains an enemy
				enemylistLoaction = c;	//loaction in the list is c 
			}
			GameObject tile = drawGrid.tileList[(node.x + node.y*(GM.getXDimension()))];
			TilesList.Add(tile);	//adds the tile to the list
			c =c+1;// increments c

		}

		return (TilesList);
	}
}
