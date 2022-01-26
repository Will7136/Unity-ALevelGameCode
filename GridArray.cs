using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridArray : MonoBehaviour {




	public int xDimension;		//This is the width of the grid
	public int yDimension;		//This is the height of the grid

	private int enemyCount;		//This will be the number of enemys in the map
	private int itemCount;		//This will be the number of items (ie coins) 
	private int barrierCount;	//This will be the number of barriers
	//private int mapSize;		//This is the total number of cells in the grid
	private int xPos;			//This is used to hold an x coordinate
	private int yPos;			//This is used to hold a y coordinate
	private int[,] mapArray;

	//private Random rand = new Random();		//This is used to get values for some of the variables above


	private void Place(int Count, int type){	//takes the niumber of things to place (count), and an integer representing the type of thing it is placing in the grid
		for (int i = 1;i<=Count;i++){	//loops for 'Count' number of times
			xPos = Random.Range(0, xDimension-1);	//chooses a random x coordinate in the array
			yPos = Random.Range(0, yDimension-1);	//chooses a random y coordinate in the array

			while (mapArray[yPos, xPos] != 0){
			xPos = Random.Range(0, xDimension-1);	//chooses new coordinates if the current coordinate already contains something
			yPos = Random.Range(0, yDimension-1);
			}

			mapArray[yPos,xPos] = type;		//changes the value in the chosen coordinates corresponding position in grid array to the value of type.
		}

		//0 is empty tile
		//1 is player on tile
		//2 is enemy on tile
		//3 is item on tile
		//4 is barrier on tile
	}


	private void Update() {
		if(Input.GetMouseButtonDown(0)){
			this.generate();
			string row = "";
			for(int x = 0;x< xDimension;x++){
				for(int y = 1;y< yDimension;y++){
					int entry = mapArray[y,x];
					row = row + "(" + entry.ToString() + ")";
				}
				row = row + "\n";
			}
			Debug.Log(row);
		}
	}

	private void generate() {
		xDimension = Random.Range(4, 7);	//generates a random number between 4 and 6 for the horizontal size of the grid
		yDimension = Random.Range(4, 7);	//generates a random number between 4 and 6 for the vertical size of the grid
	//	mapSize = xDimension*yDimension;	//holds the overall size of the map for later calculations
		mapArray = new int[yDimension,xDimension]; //creates an empty array that has a element for every cell in the grid

		this.Place(1,1);	//This will place the player on the grid

		enemyCount = Random.Range(1,4);		//generates a random number between 1 and 3 for the number of enemies in the level
		this.Place(enemyCount, 2);			//Calls place fucntion to fill in item placement on the grid

		barrierCount = Random.Range(1,4);	//generates a random number between 1 and 3 for the number of barriers in the level
		this.Place(barrierCount, 4);		//This will place any enemies on the grid

		itemCount = Random.Range(1,3);		//generates how many items will be in the level (between 1 and 2)
		this.Place(itemCount, 3);			//Calls place fucntion to fill in item placement on the grid
	}
}
