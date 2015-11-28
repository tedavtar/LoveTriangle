using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//the possible identities of the player
public enum Player{first,second,third};
public enum Stage{first,second,third};

public class GameState : MonoBehaviour {

	//public Dictionary<Player, string> players = new Dictionary<Player, string> ();
	public string[] players = new string[3];
	//the identity of the player
	public Player myIdentity;

	//the name of the player
	public string myName = "";
	
	//the stage the player is in
	public Stage myStage;
	
	//whether it's the player's turn or not
	public bool isMyTurn;

	public int Cf = 7;
	public int Ht = 7;
	public int It = 7;

	public bool amCounterAttacking = false;

	public int playerWhoseTurnItIsAfterAttack;
	public int attackingMeWith;
	public string myattacker;

	public bool canMovePieceOnBoard = false;
	public int spacesICanMove = 0;

	public string TileWithPiece = "uninitialized";


	public string myPortal; //ID of your portal
	public string myHome; //prob loading these 2 when join room
	public string wherePieceIs = "M";

	void Awake(){
		players[0] = "";
		players[1] = "";
		players[2] = "";
	}


	public CardInfo informationAboutSelectedCard = null;

	public string playerBeingTargeted = "";







	//bool notCalled = true;

	[RPC]
	public void registerPlayer(int player, string name){
		/*
		if ((int)player != (int)myIdentity) {
			if (notCalled){
				GameObject.Find ("Target1").GetComponent<Text>().text = name;
				notCalled = false;
			} else {
				GameObject.Find ("Target2").GetComponent<Text>().text = name;
			}
		}*/
		switch (player) {
		case (int)Player.first:
			players[0] = name;
			break;
		case (int)Player.second:
			players[1] = name;
			break;
		default:
			players[2] = name;
			break;
		}
	}
}
