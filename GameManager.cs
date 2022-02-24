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
	

	void Update () {
		if (Input.GetKeyDown("space")){
			nodeArrayCreator.beginNodeArr();
			nodeArray = nodeArrayCreator.getNodeArray();
			pathQueue = testPathfind();
			return;
		}
		if (Input.GetKeyDown("p")){
			enemy.moveEnemyOnce(pathQueue);
		}
	}

	void Start(){
		gridObject = GameObject.FindObjectOfType<GridArray>();
		nodeArrayCreator = GameObject.FindObjectOfType<NodeArrayCreator>();
		enemy = GameObject.FindObjectOfType<Enemy>();
	}

	public Nodeclass getNode(int x, int y){

		Nodeclass node = nodeArray[y,x];
		return(node);
	}

	public Nodeclass getEnemyNode(){
		for (int y = 0;y<gridObject.yDimension;y++){		
			for(int x = 0;x<gridObject.xDimension;x++){
				if (nodeArray[y,x].contents == 2){
					Nodeclass eNode = nodeArray[y,x];
					return(eNode);
				}
			}
		}
		return(null);
	}
	public Nodeclass getPlayerNode(){
		for (int y = 0;y<gridObject.yDimension;y++){		
			for(int x = 0;x<gridObject.xDimension;x++){
				if (nodeArray[y,x].contents == 1){
					Nodeclass pNode = nodeArray[y,x];
					return(pNode);
				}
			}
		}
		return(null);
	}

	public int getXDimension(){
		return(gridObject.xDimension);
	}
	public int getYDimension(){
		return(gridObject.yDimension);
	}

	private List<Nodeclass> testPathfind(){
		List<Nodeclass> path = new List<Nodeclass>();
		path = enemy.pathfind();
		string pathString = "";
		for (int x = 0; x < path.Count; x++){
			string nextNode = "(" + (path[x].x).ToString() + ", " + (path[x].y).ToString() + ")";
			pathString = pathString + nextNode + "   ";
		}

		Debug.Log(pathString);
		path.RemoveAt(0);
		return(path);
	}


	public void newNodeArray(){
		nodeArray = nodeArrayCreator.getNodeArray();
	}

}
