using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	
	// Update is called once per frame
	private GridArray gridObject;
	private NodeArrayCreator nodeArrayCreator;
	public Nodeclass[,] nodeArray;
	private Enemy enemy;
	private List<Nodeclass> pathQueue;
	

	// void Update () {
	// 	if (Input.GetKeyDown("space")){	//gets the pathfinding list when space is pressed
	// 		pathQueue = testPathfind();
	// 		return;
	// 	}
	// 	if (Input.GetKeyDown("p")){	//moves the enemy when p is pressed
	// 		enemy.moveEnemyOnce(pathQueue);
	// 	}
	// }

	void Start(){
		gridObject = GameObject.FindObjectOfType<GridArray>();
		nodeArrayCreator = GameObject.FindObjectOfType<NodeArrayCreator>();
		enemy = GameObject.FindObjectOfType<Enemy>();
		nodeArrayCreator.beginNodeArr();
		nodeArray = nodeArrayCreator.getNodeArray();
	}

	public Nodeclass getNode(int x, int y){

		Nodeclass node = nodeArray[y,x];	//returns the corresponding node for
		return(node);						//the given x and y
	}

	public Nodeclass getEnemyNode(){
		for (int y = 0;y<gridObject.yDimension;y++){		
			for(int x = 0;x<gridObject.xDimension;x++){	//loops through node in the grid
				if (nodeArray[y,x].contents == 2){		//checks if the node contains an enemy
					Nodeclass eNode = nodeArray[y,x];	//gets the node that it is inspecting
					return(eNode);			//returns the node
				}
			}
		}
		return(null); //returns null if it doesnt find an enemy
	}
	public Nodeclass getPlayerNode(){
		for (int y = 0;y<gridObject.yDimension;y++){		
			for(int x = 0;x<gridObject.xDimension;x++){	//loops through node in the grid
				if (nodeArray[y,x].contents == 1){		//checks if the node contains the player
					Nodeclass pNode = nodeArray[y,x];	//gets the node that it is inspecting
					return(pNode);		//returns the node
				}
			}
		}
		return(null);	//returns null if it doesnt find the player
	}

	public int getXDimension(){
		return(gridObject.xDimension);	//returns the x dimension
	}
	public int getYDimension(){
		return(gridObject.yDimension);	// returns the y dimension
	}

	// private List<Nodeclass> testPathfind(){
	// 	List<Nodeclass> path = new List<Nodeclass>();
	// 	path = enemy.pathfind();
	// 	string pathString = "";
	// 	for (int x = 0; x < path.Count; x++){
	// 		string nextNode = "(" + (path[x].x).ToString() + ", " + (path[x].y).ToString() + ")";
	// 		pathString = pathString + nextNode + "   ";
	// 	}

	// 	Debug.Log(pathString);
	// 	path.RemoveAt(0);
	// 	return(path);
	// }


	public void newNodeArray(){
		nodeArray = nodeArrayCreator.getNodeArray();//updates the nodeArray in case it has changed
	}


}
