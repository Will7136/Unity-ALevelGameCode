using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeArrayCreator : MonoBehaviour {

	private GridArray gridObject;
	private int [,] gridArray;
	private int[,] originalGridArray;
	private bool copyCheck = false;
	private Nodeclass [,] nodeArray;// array made of nodeclass that gives more information about each individual tile
	private GameManager GM;
	private TurnManager TM;



	void Start () {
		gridObject = GameObject.FindObjectOfType<GridArray>();	//finds the gridArray object
		GM = GameObject.FindObjectOfType<GameManager>();
		TM = GameObject.FindObjectOfType<TurnManager>();
	}

	public void beginNodeArr(){
		gridArray = gridObject.getArray();		// sets the array from gridObject to its own variable
		if (!copyCheck){
			originalGridArray = gridArray;	//will always hold the original grid array
			copyCheck = true;
		}
		nodeArray = new Nodeclass[(gridObject.yDimension),(gridObject.xDimension)]; //sets the dimensions of nodeArray to the same dimensions that grid array has
		nodeArray = this.generateNodeArr(gridArray, gridObject);//sets nodeArray to the nodeclass array returned from generateNodeArr
	}

	private Nodeclass[,] generateNodeArr(int[,] gridArr, GridArray gObjcet){
		Nodeclass[,] NodeArr = null;	//creates an empty nodeclass array
		NodeArr = new Nodeclass[(gObjcet.yDimension), (gObjcet.xDimension)];	//sets the dimensions of nodeArray to the same dimensions that gridarr has
		for (int y = 0;y<gridObject.yDimension;y++){		
			for(int x = 0;x<gridObject.xDimension;x++){	//loops through all values in grid array
				Nodeclass node = new Nodeclass(x, y);//creates new instance of nodeclass using constructor
				node.contents = gridArr[y,x]; //sets the contents of node to the same as the corresponding value from gridarr
				if (gridArr[y,x] == 4){
					node.walkable = false;	//if there is a barrier on theis tile, walkable is set to false
				}

				NodeArr[y,x] = node;	//sets the current entry in nodearr to node 
			}
		}
		return(NodeArr); //returns nodearr
	}

	public Nodeclass[,] getNodeArray(){
		return(nodeArray);	//returns the nodeArray
	}

	public void changeCharacterLocation(int newX, int newY, int charVal){
		Nodeclass originalPlayerNode = GM.getPlayerNode();
		for (int y = 0;y<gridObject.yDimension;y++){	//loops through all values in the arrays	
			for(int x = 0;x<gridObject.xDimension;x++){
				if (nodeArray[y,x].contents == charVal){	//If there is an enemy on this tile
					if ((gridArray[y,x] == 3) & (charVal == 1)){
						nodeArray[y,x].contents = 0;
						gridArray[y,x] = 0;
					}
					else if ((gridArray[y,x] == 2) & (gridArray[y,x] != charVal)){	//if there wasn't originally an empty tile or the enemy
						nodeArray[y,x].contents = gridArray[y,x];	//assigns contents to its original value
					}
					else{
						nodeArray[y,x].contents = 0;	// sets the contents to 0
					}
				}
				else if ((x == newX) & (y == newY)){	// if this is the new position of the enemy
					nodeArray[y,x].contents =charVal;	//assigns contents to charval
					if ((gridArray[y,x] == 3) & (charVal == 1)){
						TM.getCoin();
						GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");	//gets all objects with the coin tag
						GameObject Player = GameObject.FindGameObjectWithTag("Player");	//gets the player
						
						foreach(GameObject coin in coins){	//in case there is more than one coin in the level
							Vector3 coinPos = coin.transform.localPosition;	//gets the coins local position
							Vector3 playerPos = Player.transform.localPosition;	//gets the players local position
							float playerXPos = 9999;	//sets arbitrary values for x and y pos 
							float playerYPos = 9999;
							if((originalPlayerNode.x - newX) == -1){	//if the player has moved down
								playerXPos = Player.transform.localPosition.x + 1;
								playerYPos = Player.transform.localPosition.y;
							}
							else if((originalPlayerNode.x - newX) == 1){	//if the player has moved up
								playerXPos = Player.transform.localPosition.x - 1;
								playerYPos = Player.transform.localPosition.y;
							}
							else if((originalPlayerNode.y - newY) == -1){	//if the player has moved to the left
								playerXPos = Player.transform.localPosition.x;
								playerYPos = Player.transform.localPosition.y - 1;
							}
							else if((originalPlayerNode.y - newY) == 1){	//if the player has moved dto the right
								playerXPos = Player.transform.localPosition.x;
								playerYPos = Player.transform.localPosition.y + 1;
							}
							//sets xPos and yPos depending on what direction the player will move in 

							if((coinPos.x == playerXPos) & (coinPos.y == playerYPos)){
								Destroy(coin);	//if the player's new position is the same as this coin's position destroy the coin
							}	//this finds and destroys the correct coin
						}
					}
				}
			}
		}
	}
}
