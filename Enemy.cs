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
		GM = GameObject.FindObjectOfType<GameManager>();
		nodeArray = GM.nodeArray;
		startNode = GM.getEnemyNode();
		endNode = GM.getPlayerNode();
		xDim = GM.getXDimension();
		List<Nodeclass> openList = new List<Nodeclass>();
		List<Nodeclass> closedList = new List<Nodeclass>();
		yDim = GM.getYDimension();
	//	openList = null;
	//	closedList = null;


		openList.Add(startNode);

		for (int y = 0;y<yDim;y++){		
			for(int x = 0;x<xDim;x++){
				nodeArray[y,x].gValue = 9999;
				nodeArray[y,x].hValue = 0;
				nodeArray[y,x].fValue = nodeArray[y,x].CalcF();
				nodeArray[y,x].Parent = null;
			}
		}

		startNode.gValue = 0;
		startNode.hValue = startNode.calcH(startNode, endNode);
		startNode.fValue = startNode.CalcF();

		while(openList.Count > 0){
			Nodeclass currentNode = LowestFNode(openList);

			if(currentNode == endNode){
				return(path());
			}

			openList.Remove(currentNode);
			closedList.Add(currentNode);

			List<Nodeclass> neighbours = getNeighbours(currentNode);

			for(int x = 0;x <neighbours.Count;x++){
				Nodeclass currentNeighbour = neighbours[x];

				if(closedList.Contains(currentNeighbour)==true){
					continue;
				}
				else if (currentNeighbour.walkable == false){
					closedList.Add(currentNeighbour);
					continue;
				}

				int newGCost = currentNode.gValue + 1;

				if(newGCost < currentNeighbour.gValue){
					currentNeighbour.gValue = newGCost;
					currentNeighbour.hValue = currentNeighbour.calcH(currentNeighbour, endNode);
					currentNeighbour.fValue = currentNeighbour.CalcF();
					currentNeighbour.Parent = currentNode;

					if (openList.Contains(currentNeighbour) == false){
						openList.Add(currentNeighbour);
					}
				}

			}

		}
		Debug.Log("no Path");
		return(null);
	}


	private Nodeclass LowestFNode(List<Nodeclass> list){
		Nodeclass currentLowest = list[0];
		for (int x = 0; x < (list.Count - 1);x++){
			if(currentLowest.fValue > list[x].fValue){
				currentLowest = list[x];
			}
		}
		return(currentLowest);
	}

	private List<Nodeclass> path(){
		List<Nodeclass> pathList = new List<Nodeclass>();
		pathList.Add(endNode);
		Nodeclass currentNode = endNode;
		while(currentNode.Parent != null){
			pathList.Add(currentNode.Parent);
			currentNode = currentNode.Parent;
		}
		pathList.Reverse();
		return(pathList);
	}

	private List<Nodeclass> getNeighbours(Nodeclass node){
		List<Nodeclass> neighbourList = new List<Nodeclass>();

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
		}
		return(neighbourList);
	}


	public void moveEnemyOnce(List<Nodeclass> path){
		if(path.Count > 1){
			replaceEnemy = GameObject.FindGameObjectWithTag("Enemy");
			Vector3 originalCoordinateEnemy = new Vector3();
			originalCoordinateEnemy = replaceEnemy.transform.position;
			Destroy(replaceEnemy);
			floorTiles = GameObject.Find("FloorTiles");
			Nodeclass nextNode = path[0];
			int xDiff = nextNode.x - startNode.x;
			int yDiff = (nextNode.y - startNode.y) * -1;
			float currentX = originalCoordinateEnemy.x;
			float currentY = originalCoordinateEnemy.y;

			GameObject EInstance = Instantiate(newEnemy, new Vector3((currentX + xDiff),(currentY + yDiff), 0), Quaternion.identity) as GameObject;
			nodeArrayCreator.changeEnemyLocation(nextNode.x, nextNode.y);
			GM.newNodeArray();
			startNode = GM.getEnemyNode();
			path.RemoveAt(0);
			//EInstance.transform.SetParent(floorTiles);
		}
	}

	void Start () {
		nodeArrayCreator = GameObject.FindObjectOfType<NodeArrayCreator>();
	}
}
