using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ui : MonoBehaviour {

    public Text pText;
    public Text eText;
    public Text sText;

    private int score = 0;
    private int pHealth = 20;
    private int eHealth = 20;



    public void UpdateScore(int changeVal){
        score = score + changeVal;  //increases score by changeval 
        sText.text = ("Score : " + score.ToString());   //sets the ui text to the new score
    }
    public void UpdatePHealth(int changeVal){
        if((pHealth - changeVal) < 0){  //makes sure it doesn't go into negatives
            pHealth = 0;
        }
        else{
            pHealth = pHealth - changeVal;  //otherwise decreases player health by changeval
        }

        pText.text = ("Player Health : " + pHealth.ToString()); //sets the ui text to the new health
    }
    public void UpdateEHealth(int changeVal){
        if((eHealth - changeVal) < 0){  //makes sure it doesn't go into negatives
            eHealth = 0;
        }
        else{
            eHealth = eHealth - changeVal;  //otherwise decreases enemy health by changeval
        }

        eText.text = ("Enemy Health : " + eHealth.ToString()); //sets the ui text to the new health
    }

    void Start(){
        int numOfGameObjects = GameObject.FindObjectsOfType<ui>().Length;//finds how many gameobjects there are with the ui script
        eText = GameObject.FindGameObjectWithTag("EnemyHealthText").GetComponent<Text>();
        pText = GameObject.FindGameObjectWithTag("PlayerHealthText").GetComponent<Text>();
        sText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
        //gets all of the text elements
        if (numOfGameObjects == 1){ //if there is only 1
            GameObject originalUIManager = GameObject.FindGameObjectWithTag("uiTag");
            originalUIManager.tag = "oldUImanager"; //sets this gameobjects tag to olduimanager
            DontDestroyOnLoad(transform.gameObject);//uses dontdestroyonload
        }
        else if (numOfGameObjects == 2){   //if there are 2
            GameObject oldUIManager = GameObject.FindGameObjectWithTag("oldUImanager");
            score = oldUIManager.GetComponent<ui>().getScore();//gets score from the olduimanager
            Destroy(oldUIManager);//destroys olduimanager
            GameObject originalUIManager = GameObject.FindGameObjectWithTag("uiTag");//finds new ui manager gameobject
            originalUIManager.tag = "oldUImanager"; //sets this gameobjects tag to olduimanager
            DontDestroyOnLoad(transform.gameObject);//uses dontdestroyonload
        }

        sText.text = ("Score : " + score);  //ensures that this value is passed into the score ui so that it can carry over between levels
    }


    public int getScore(){
        return(score);  //returns score
    }

    public void resetScore(){
        score = 0;  //resets the score if necessary
    }
}
