using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGrid : MonoBehaviour {

    private GridArray gridObject;
	private int[,] gridArray;
	private bool OddY;
	private int midY;
	private int midX;
	private Transform FloorTiles;


	
    public GameObject FloorTile;
	public GameObject PlayerTile;
	public GameObject EnemyTile;
	public GameObject ItemTile;
	public GameObject BarrierTile;
	

    private void Start() {

		FloorTiles = new GameObject ("FloorTiles").transform;	//Creates a floorTiles game object that acts as the parent to the rest of the grid tiles
        gridObject = GameObject.FindObjectOfType<GridArray>();	//gets the grid array
		gridObject.generate();									// generates a grid array
		gridArray = gridObject.getArray();

		for (int y = 0;y<gridObject.yDimension;y++){
			for(int x = 0;x<gridObject.xDimension;x++){
				GameObject Instance = Instantiate(FloorTile, new Vector3(x,-y,0f), Quaternion.identity) as GameObject;
				Instance.transform.SetParent(FloorTiles);
				if (gridArray[y,x] == 1){
					GameObject PInstance = Instantiate(PlayerTile, new Vector3(x,-y,0f), Quaternion.identity) as GameObject;
					PInstance.transform.SetParent(FloorTiles);
				}
				else if (gridArray[y,x] == 2){
					GameObject EInstance = Instantiate(EnemyTile, new Vector3(x,-y,0f), Quaternion.identity) as GameObject;
					EInstance.transform.SetParent(FloorTiles);
				}
				else if (gridArray[y,x] == 3){
					GameObject IInstance = Instantiate(ItemTile, new Vector3(x,-y,0f), Quaternion.identity) as GameObject;
					IInstance.transform.SetParent(FloorTiles);
				}
				else if (gridArray[y,x] == 4){
					GameObject BInstance = Instantiate(BarrierTile, new Vector3(x,-y,0f), Quaternion.identity) as GameObject;
					BInstance.transform.SetParent(FloorTiles);
				}

			}
		}

		FloorTiles.transform.position = new Vector3(-(((gridObject.xDimension) -1.0f)/2.0f), ((gridObject.yDimension -1.0f)/2.0f),0);

    }
	
}
