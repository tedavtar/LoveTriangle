using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

	GameState gs;
	// Use this for initialization
	void Start () {
		gs = GameObject.Find ("GameState").GetComponent<GameState> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (gs.isMyTurn) {
			switch(gs.myStage){
			case Stage.first:
				//GameObject.Find ("DrawButton").GetComponent<Button>().interactable = true;
				break;
			case Stage.second:
				break;
			default:
				break;
			}
		}
	}
}
