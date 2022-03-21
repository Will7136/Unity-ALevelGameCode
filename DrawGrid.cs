using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGrid : MonoBehaviour {

    private GridArray gridObject;
	public int[,] gridArray;
	private bool OddY;
	private int midY;
	private int midX;
	private Transform FloorTiles;


	
    public GameObject FloorTile;
	public GameObject PlayerTile;
	public GameObject EnemyTile;
	public GameObject ItemTile;
	public GameObject BarrierTile;
	public List<GameObject> tileList = new List<GameObject>();
	public GameObject[,] tileArray;
	

    private void Start() {

		FloorTiles = new GameObject ("FloorTiles").transform;	//Creates a floorTiles game object that acts as the parent to the rest of the grid tiles
        gridObject = GameObject.FindObjectOfType<GridArray>();	//gets the grid array
		gridObject.generate();									// generates a grid array
		gridArray = gridObject.getArray();	//assigns the actual array made by grid object to an array
		for (int y = 0;y<gridObject.yDimension;y++){		//loops through every entry in the array from
			for(int x = 0;x<gridObject.xDimension;x++){		//top to bottom, left to right
				GameObject Instance = Instantiate(FloorTile, new Vector3(x,-y,0f), Quaternion.identity) as GameObject;
				Instance.transform.SetParent(FloorTiles);	//Creates an instance of a floor tile to create the grid and childs it to an empty gameobject
				tileList.Add(Instance);	//gameobject is added to a list for later
				if (gridArray[y,x] == 1){	// find if the player is on this tile
					GameObject PInstance = Instantiate(PlayerTile, new Vector3(x,-y,0f), Quaternion.identity) as GameObject;
					PInstance.transform.SetParent(FloorTiles);	//creates an instance of a player object on this tile
				}
				else if (gridArray[y,x] == 2){	// find if an enemy is on this tile
					GameObject EInstance = Instantiate(EnemyTile, new Vector3(x,-y,0f), Quaternion.identity) as GameObject;
					EInstance.transform.SetParent(FloorTiles);	//creates an instance of an enemy object on this tile
				}
				else if (gridArray[y,x] == 3){	// find if there is an item on this tile
					GameObject IInstance = Instantiate(ItemTile, new Vector3(x,-y,0f), Quaternion.identity) as GameObject;
					IInstance.transform.SetParent(FloorTiles);	//creates an instance of an item object on this tile
				}
				else if (gridArray[y,x] == 4){	// find if there is a barrier on this tile
					GameObject BInstance = Instantiate(BarrierTile, new Vector3(x,-y,0f), Quaternion.identity) as GameObject;
					BInstance.transform.SetParent(FloorTiles);	//creates an instance of a barrier object on this tile
				}
				//Tiles are placed at location x, -y, 0f as the x position on the grid will always be the same as the position in the table 
			}	//eg when x is 4, the actual coordinate in game will be (x, y, 0f)
		}		// -y is used since, as y increases in the loop, you are actually moving down the table. 
				// without using -y the grid would be built upside down (ie the coordiante (2,1) in a 5x5 table would be (2,5) in game)
		FloorTiles.transform.position = new Vector3(-(((gridObject.xDimension) -1.0f)/2.0f), ((gridObject.yDimension -1.0f)/2.0f),0);
				// This then moves the position of the parent containing all tiles to center the whole grid. 
				//The position itself is dependant on the width and height of the grid.
    }
	
}
