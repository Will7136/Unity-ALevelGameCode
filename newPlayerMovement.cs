using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newPlayerMovement : MonoBehaviour {

		GameObject replacePlayer;
		GameObject floorTiles;
		GameManager Gm;
		NodeArrayCreator nodeArrayCreator;
		[SerializeField] GameObject newPlayer;

		void Start(){
			Gm = GameObject.FindObjectOfType<GameManager>();
			nodeArrayCreator = GameObject.FindObjectOfType<NodeArrayCreator>();
		}

	
		public void movePlayerOnce(Nodeclass nextNode){
			replacePlayer = GameObject.FindGameObjectWithTag("Player");	//find the enemy currently on the board
			Vector3 originalCoordinatePlayer = new Vector3();	// this is the vector3 position of the original player
			originalCoordinatePlayer = replacePlayer.transform.position;
			Destroy(replacePlayer);	//destroys the original enemy
			floorTiles = GameObject.Find("FloorTiles");	//finds the floorTiles gameObject
			int xDiff = nextNode.x - Gm.getPlayerNode().x;	//finds x and y difference that from the current node to the next node
			int yDiff = (nextNode.y - Gm.getPlayerNode().y) * -1;
			float currentX = originalCoordinatePlayer.x;	//x and y coordinates of the original enemy
			float currentY = originalCoordinatePlayer.y;

			GameObject PInstance = Instantiate(newPlayer, new Vector3((currentX + xDiff),(currentY + yDiff), 0), Quaternion.identity) as GameObject;	//instantiates the new enemy in their new position
			PInstance.transform.SetParent(floorTiles.transform);
			nodeArrayCreator.changeCharacterLocation(nextNode.x, nextNode.y, 1);	//updates the position for the enemy
			Gm.newNodeArray();	//updates nodeArray for the player
		}
}
