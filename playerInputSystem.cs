using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInputSystem : MonoBehaviour {

	[SerializeField] private Camera InpCamera;
	private GameManager GM;
	private Vector3 mousePos;
	private Nodeclass[,] nodeArray;
	private int mousePosX;
	private int mousePosY;
	private DrawGrid drawGrid;
	private bool selectedTile = false;
	private bool reselectedTileAgain = false;
	private bool PlayerStillSelected = false;
	int preX = 9999;
	int preY = 9999;
	private newPlayerMovement movePlayer;
	private Enemy enemy;
	private bool playerHasMoved = false;
	private CombatScript comScript;


	void Start() {
		GM = GameObject.FindObjectOfType<GameManager>();	//finds the game Manager
		drawGrid = GameObject.FindObjectOfType<DrawGrid>();
		movePlayer = GameObject.FindObjectOfType<newPlayerMovement>();
		enemy = GameObject.FindObjectOfType<Enemy>();
		nodeArray = GM.nodeArray;
		InpCamera.transform.position = new Vector3(((GM.getXDimension() -1.0f)/2.0f), -(((GM.getYDimension() -1.0f)/2.0f)),0);
		comScript = GameObject.FindObjectOfType<CombatScript>();
		//Moves the camera by the exact opposite transformation that was done to each tile when instantaiting them
	}

	public void playerTurn(){
		if (Input.GetMouseButtonDown(0)){
			//Debug.Log(MainCamera.ScreenToWorldPoint(Input.mousePosition));
			WhenClickTile();
		}
	}


	private void WhenClickTile(){
			mousePos = InpCamera.ScreenToWorldPoint(Input.mousePosition);	//find the world position according to the attatched camera

			mousePosX = (int)Mathf.Round(mousePos.x);	// rounds the mouse x coordinate to the nearest integer
			mousePosY = -(int)Mathf.Round(mousePos.y);	// rounds the mouse y coordinate to the nearest integer
			Debug.Log("(" + mousePosX + " ," + mousePosY + ")");	//logs the coordinate selected for testing purposes
			CheckTile(nodeArray, mousePosX, mousePosY); 
	}

	private void CheckTile(Nodeclass[,] array, int X, int Y){
		Debug.Log(array[Y, X].contents);	//logs what is on that position
		GameObject tile = drawGrid.tileList[(X + Y*(GM.getXDimension()))];	//finds the corresponding gameobject for that node
		bool sameTileSelected = false;	//bool to show if the same tile has been selected twice in a row
		if ((preX == X) & (preY == Y)){
			sameTileSelected = true;	//sets the bool to true if the tile is the same one as last time
		}

		if (nodeArray[Y, X].contents == 1){	//if this tile contains the player

			if (!selectedTile){	// if this is the first tile that has been selected 
				tile.GetComponent<ChangeTileColor>().SelectPlayerTile(X, Y);//select the tile
				selectedTile = true;	//there has now been a tile selected
				preX = X;	//previous coordinates are now set to the current ones
				preY = Y;
				PlayerStillSelected = true;	//shows that the player is the tile slected for later
			}
			else{
				if((sameTileSelected) & (!reselectedTileAgain)){	//If the user selects a tile that is already selected
					tile.GetComponent<ChangeTileColor>().DeselectPlayerTile(X, Y);	//Deselets the player tile
					reselectedTileAgain = true;	//shows that the tile selected twice is now deselected

				}
				else if((sameTileSelected) & (reselectedTileAgain)){	//if the same tile has been selected another time but it is deslected currently
					tile.GetComponent<ChangeTileColor>().SelectPlayerTile(X, Y);	// selects the player tile
					reselectedTileAgain = false;	//the tile is now selected again
					PlayerStillSelected = true;	//the player tile is still selected
				}
				else{//if the same tile hasnt been selected
					tile = drawGrid.tileList[(preX + preY*(GM.getXDimension()))];
					tile.GetComponent<ChangeTileColor>().DeselectTile();	//previous tile is deselected
					tile = drawGrid.tileList[(X + Y*(GM.getXDimension()))];
					tile.GetComponent<ChangeTileColor>().SelectPlayerTile(X, Y);	//playertile is selected
					reselectedTileAgain = false;	//the same tile hasn't been selected
					PlayerStillSelected = true;	//player tile is selected 
				}
				preX = X;	//previous coordinates are now set to the current ones
				preY = Y;
			}
		}
		else{	//if the tile isnt the player
			if (!selectedTile){	// if this is the first tile that has been selected 
				tile.GetComponent<ChangeTileColor>().SelectTile(Color.magenta);	//tiles color is set to magneta
				selectedTile = true;	//there has now been a tile selected
				preX = X;	//previous coordinates are now set to the current ones
				preY = Y;
			}
			else{
				if((sameTileSelected) & (!reselectedTileAgain)){	//If the user selects a tile that is already selected
					tile.GetComponent<ChangeTileColor>().DeselectTile();	//deselects the current tile
					reselectedTileAgain = true;	//the tile is now deselected
				}
				else if((sameTileSelected) & (reselectedTileAgain)){	//if the same tile has been selected another time but it is deslected currently
					tile.GetComponent<ChangeTileColor>().SelectTile(Color.magenta);	//tiles color is set to magneta
					reselectedTileAgain = false;	//the same tile hasn't been selected
				}
				else if (PlayerStillSelected == true){	//if the same tile hasnt been selected but the player tile was the previous tile selected
					tile = drawGrid.tileList[(preX + preY*(GM.getXDimension()))];
					tile.GetComponent<ChangeTileColor>().DeselectPlayerTile(preX, preY);	//deselects playertile and surrounding tiles
					tile = drawGrid.tileList[(X + Y*(GM.getXDimension()))];// new tile is selected
					tile.GetComponent<ChangeTileColor>().SelectTile(Color.magenta);	//tiles color is set to magneta
					reselectedTileAgain = false;	//the same tile hasn't been selected
					PlayerStillSelected = false;	//player is no longer selected
					CheckIfCanMove(X, Y, tile);
					CheckIfCanAttack(X, Y, tile);
				}
				
				else{
					tile.GetComponent<ChangeTileColor>().SelectTile(Color.magenta);	//new tile is selected
					tile = drawGrid.tileList[(preX + preY*(GM.getXDimension()))];
					tile.GetComponent<ChangeTileColor>().DeselectTile();//previous tile is deselected
					reselectedTileAgain = false;
				}
				
				if (playerHasMoved == false){
					preX = X;	//previous coordinates are now set to the current ones
					preY = Y;
				}
				else{
					playerHasMoved =false;
				}

			}
		}
		
	}

	private void CheckIfCanMove(int X, int Y, GameObject tile){
		List<Nodeclass> surroundingNodes = enemy.getNeighbours(GM.getPlayerNode(), GM, GM.getXDimension(), GM.getYDimension());	
		//gets the tiles surrounding the player
		foreach(Nodeclass node in surroundingNodes){//loops through all of the neighbours
			if ((nodeArray[X, Y] == node) & (nodeArray[Y,X].contents != 4) & (nodeArray[Y,X].contents != 2)){
				movePlayer.movePlayerOnce(node);	//Checks if the player can move to the chosen position
				tile.GetComponent<ChangeTileColor>().DeselectTile();
				preX = 9999;	//changes colour and resets the required variables.
				preY = 9999;
				selectedTile = false;
			}
		}
	}

	private void CheckIfCanAttack(int X, int Y, GameObject tile){
		List<Nodeclass> surroundingNodes = enemy.getNeighbours(GM.getPlayerNode(), GM, GM.getXDimension(), GM.getYDimension());
		//gets the tiles surrounding the player
		foreach(Nodeclass node in surroundingNodes){
			if ((nodeArray[Y, X] == node) & (node.contents == 2)){	//if the is the selected node and it contains an enemy
				comScript.enemyHit(4);	//attacks enemy if possible
				tile.GetComponent<ChangeTileColor>().DeselectTile();
				preX = 9999;	//changes colour and resets the required variables.
				preY = 9999;
				selectedTile = false;
			}
		}

	}

}
