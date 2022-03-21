using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private GameManager GM;
	private Nodeclass startNode;
	private Nodeclass endNode;
	private Nodeclass[,] nodeArray;
	private int xDim;
	
	private int yDim;
	private GameObject replaceEnemy;
	public GameObject newEnemy;
	private GameObject floorTiles;
	private NodeArrayCreator nodeArrayCreator;


	
			//	public List<int[]> pathfind(){		
			//		GM = GameObject.FindObjectOfType<GameManager>();
			//		private Nodeclass currentNode;
			//		private Nodeclass playerNode;
			//		private List<Nodeclass> openList = null;
			//		private List<Nodeclass> closedList = null;
			//		return(null);

	public List<Nodeclass> pathfind(){
		GM = GameObject.FindObjectOfType<GameManager>();	//This find the GameManager gameobject so that I am able to use its public functions
		nodeArray = GM.nodeArray;	//this gets the nodeArray from gameManager and copies it into another array for easy access
		startNode = GM.getEnemyNode();	//This finds the node that conatins the enemy and sets the value of startNode to it
		endNode = GM.getPlayerNode();	//This finds the node that conatins the player and sets the value of endNode to it
		List<Nodeclass> openList = new List<Nodeclass>();	//This creates an empty list that will contain nodes that need to be inspected 
		List<Nodeclass> closedList = new List<Nodeclass>();	//This creates an empty list that conatins nodes that have been visited and dont need to be visited again
		xDim = GM.getXDimension();
		yDim = GM.getYDimension();	//This gets the height and width of the grid 
	//	openList = null;
	//	closedList = null;


		openList.Add(startNode);	//To begin with startNode is added to the openlist

		for (int y = 0;y<yDim;y++){		//Every node in nodeArray gets some of its attributes set to default values 
			for(int x = 0;x<xDim;x++){
				nodeArray[y,x].gValue = 9999;	//This is the real distance from startNode so this is set to a large number to begin with
				nodeArray[y,x].hValue = 0;	//h hasnt yet been calculated so it is set to 0
				nodeArray[y,x].fValue = nodeArray[y,x].CalcF();	//F value is claculated based on the h and g values 
				nodeArray[y,x].Parent = null;	//Parent is set to null to begin with
			}
		}

		startNode.gValue = 0;	//this is the startNode so its g value is 0
		startNode.hValue = startNode.calcH(startNode, endNode);	//this works out its h value using manhattan heuristic
		startNode.fValue = startNode.CalcF();	// this calculates f based on the g and h



		while(openList.Count > 0){	//loops until opnelist is empty
			Nodeclass currentNode = LowestFNode(openList);	//calls lowestfNode to find the next best node

			if(currentNode == endNode){	//if this is the endnode then we have reached the end
				return(path());	//path is called to end the pathfinding and assemble a path for the enemy to follow
			}

			openList.Remove(currentNode);	//the currentnode is removed from openlist and added to closed list
			closedList.Add(currentNode);

			List<Nodeclass> neighbours = getNeighbours(currentNode, GM, xDim, yDim);

			for(int x = 0;x <neighbours.Count;x++){	//loops though all of the neighbours
				Nodeclass currentNeighbour = neighbours[x]; //selects the current neighbour being inspected

				if(closedList.Contains(currentNeighbour)==true){	//if the neighbour is in closed list then it should be ignored
					continue;
				}
				else if (currentNeighbour.walkable == false){	// if the neighbour isnt walkable it should be added to closed list and ignored
					closedList.Add(currentNeighbour);
					continue;
				}

				int newGCost = currentNode.gValue + 1;	// increases the gCost of current node by 1 to show that the neighbour is 1 step further away from startNode

				if(newGCost < currentNeighbour.gValue){	//if the new g cost is lower than the current neighbours g cost then 
					currentNeighbour.gValue = newGCost;	// the neighbours gcost is set to the new one
					currentNeighbour.hValue = currentNeighbour.calcH(currentNeighbour, endNode);
					currentNeighbour.fValue = currentNeighbour.CalcF();	// h and f are then set based on this new g value
					currentNeighbour.Parent = currentNode;	//The parent is then set to the previous node

					if (openList.Contains(currentNeighbour) == false){
						openList.Add(currentNeighbour);	//Adds the current neighbour to openlist if necessary
					}
				}

			}

		}
		Debug.Log("no Path");	//prints no path if something goes wrong and returns null
		return(null);
	}


	private Nodeclass LowestFNode(List<Nodeclass> list){
		Nodeclass currentLowest = list[0];	//this will hold the node with the current lowest 
		for (int x = 0; x < (list.Count - 1);x++){	//loops through the list 
			if(currentLowest.fValue > list[x].fValue){
				currentLowest = list[x];	//if this node has f value less than the f val of current lowest then it becomes current lowest
			}
		}
		return(currentLowest);	//the current lowest is returned
	}

	private List<Nodeclass> path(){	//this assembles the path
		List<Nodeclass> pathList = new List<Nodeclass>();	//creates a new list that will contain the path that was found
		pathList.Add(endNode);	//adds the end node to the list
		Nodeclass currentNode = endNode;
		while(currentNode.Parent != null){	//loops until it reaches the start node
			pathList.Add(currentNode.Parent);	//adds the parent to the list
			currentNode = currentNode.Parent;	//sets the currentnode to the parent of currentnode
		}
		pathList.Reverse();	//reverses the list so that it begins with startnode and ends with endnode
		return(pathList);	//returns the list
	}


	public List<Nodeclass> getNeighbours(Nodeclass node, GameManager GM, int xDim, int yDim){	//takes extra parameters so that it can also be used later outside of pathfinding
		List<Nodeclass> neighbourList = new List<Nodeclass>();	//creates a list to hold all of the neighbours

		if ((node.x -1) >=0){
			neighbourList.Add(GM.getNode((node.x -1), (node.y)));
		}		
		if ((node.x +1) <xDim){
			neighbourList.Add(GM.getNode((node.x +1), (node.y)));
		}		
		if ((node.y -1) >=0){
			neighbourList.Add(GM.getNode((node.x), (node.y - 1)));
		}		
		if ((node.y +1) <yDim){
			neighbourList.Add(GM.getNode((node.x), (node.y + 1)));
		}	//finds and adds the nodes to the left, right, up and down of current node and adds them to the list
		return(neighbourList);	//returns the list
	}


	public void moveEnemyOnce(List<Nodeclass> path){
		if(path.Count > 1){	//only runs if there is more than one space to move to. 
			replaceEnemy = GameObject.FindGameObjectWithTag("Enemy");	//find the enemy currently on the board
			Vector3 originalCoordinateEnemy = new Vector3();	// this is the vector3 position of the original enemy
			originalCoordinateEnemy = replaceEnemy.transform.position;
			Destroy(replaceEnemy);	//destroys the original enemy
			floorTiles = GameObject.Find("FloorTiles");	//finds the floorTiles gameObject
			Nodeclass nextNode = path[0];	//selects next node in the path
			int xDiff = nextNode.x - startNode.x;	//finds x and y difference that from the current node to the next node
			int yDiff = (nextNode.y - startNode.y) * -1;
			float currentX = originalCoordinateEnemy.x;	//x and y coordinates of the original enemy
			float currentY = originalCoordinateEnemy.y;

			GameObject EInstance = Instantiate(newEnemy, new Vector3((currentX + xDiff),(currentY + yDiff), 0), Quaternion.identity) as GameObject;	//instantiates the new enemy in their new position
			EInstance.transform.SetParent(floorTiles.transform);
			nodeArrayCreator.changeCharacterLocation(nextNode.x, nextNode.y, 2);	//updates the position for the enemy
			GM.newNodeArray();	//updates nodeArray for the player
			startNode = GM.getEnemyNode();	//Gets the new startNode
			path.RemoveAt(0);	//Removes the first item in te path
			//EInstance.transform.SetParent(floorTiles);
		}
	}

	void Start () {
		nodeArrayCreator = GameObject.FindObjectOfType<NodeArrayCreator>();	//finds the nodeArrayCreator which is used in moveEnemyOnce
	}
}
