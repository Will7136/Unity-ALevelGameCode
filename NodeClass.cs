using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodeclass  {
	public int x;	// This is the x coordinate for the node on the grid
	public int y;	// This is the y coordinate for the node on the grid
	public int contents; // This is a integer that is used to tell what is on this tile
	public int gValue; //gValue is the real diatance from the start node to this node
	public int hValue; //hValue is a heuristic value that guesses the distance from this node to the end node
	public int fValue; //fValue is the sum of the g and h values 
	public Nodeclass Parent; // is the parent of the node (ie the node before it on the path to the end node)
	public bool walkable; // This shows whether a character is able to move onto or through this space


	public int CalcF(){				//This returns the F value of this node and
		int value = gValue + hValue;//can be used to either get the F value or when setting it
		return(value);
	}

	public int calcH(Nodeclass startNode, Nodeclass endNode){	//This takes two nodes that are entered and find the h value heuristic
		int value;
		int xDifference;
		int yDifference;

		if ((endNode.x - startNode.x) < 0){
			xDifference = (endNode.x - startNode.x) * -1;	//This will find the absolute difference between the x values  
		}
		else{
			xDifference = (endNode.x - startNode.x);
		}
		if ((endNode.y - startNode.y) < 0){
			yDifference = (endNode.y - startNode.y) * -1; //This will find the absolute difference between the y values 
		}
		else{
			yDifference = (endNode.y - startNode.y);
		}
		value = xDifference + yDifference;
		return(value);
	}

	public Nodeclass(int Xpos, int Ypos){
		x = Xpos;
		y = Ypos;
		contents = 0;
		gValue = 0;
		hValue = 0;
		fValue = 0;
		Parent = null;
		walkable = true;
	}

}
