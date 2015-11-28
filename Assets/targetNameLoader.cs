using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class targetNameLoader : MonoBehaviour {
	GameState gs;
	bool doneInitializing = false;

	public List<string> otherPlayers;

	// Use this for initialization
	void Start () {
		gs = GameObject.Find ("GameState").GetComponent<GameState> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (doneInitializing) {
			return;
		}

		if ((gs.players[0] == "") || (gs.players[1] == "") || (gs.players[2] == "")) {  
			return;
		}


		//at this point know all players are in gs.players so need to load stuff. first the boxes then targets wait NVM just the targets boxes before on join room







		otherPlayers = new List<string>();

		foreach (string pName in gs.players) {
			if(!gs.myName.Equals(pName)){
				otherPlayers.Add(pName);
			}
		}
		//so now otherPlayers should have length 2 and be names of the other 2 players--assuming no 2 players have same username
		print (otherPlayers.Count);
		//string[] otherplayers1 = otherPlayers.ToArray ();
		string name1 = otherPlayers [0];
		string name2 = otherPlayers[1];
		gameObject.GetComponentInChildren<Text> ().text = " " + name1;
		GameObject.Find ("Target2").GetComponentInChildren<Text> ().text = " " + name2;
		doneInitializing = true;
		//gameObject.GetComponentInChildren<Text>().text = " " + otherPlayers.


		/*
		//so at this point have the names of all the players (or else would have terminated earlier)
		for(int i=0; i<3;i++) {
			if(!gs.myName.Equals(gs.players[i])){//so not me
				/*
				if (gameObject.GetComponentInChildren<Text>().text == "Player2"){
					gameObject.GetComponentInChildren<Text>().text = " " + gs.players[i];
				} else {
					GameObject.Find ("Target2").GetComponentInChildren<Text>().text = " " + gs.players[i];
					doneInitializing = true; //
				}
				*/ /*
				if (indexA == 9000){
					indexA = i;
					break;
				}
				if (indexB == 9000){
					indexB = i;
					break;
				}
			}
		}
		gameObject.GetComponentInChildren<Text>().text = " " + gs.players[indexA];
		GameObject.Find ("Target2").GetComponentInChildren<Text>().text = " " + gs.players[indexB];
		doneInitializing = true; */
	}
}
