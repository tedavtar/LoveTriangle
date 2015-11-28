using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tile : MonoBehaviour {

	public string ID;

	public bool hasPiece = false;
	public bool isSelected = false;

	public GameObject tile1;
	public GameObject tile2;
	public GameObject tile3;

	public BoardActions ba;
	//public GameObject[] tiles;

	GameState gs;
	GameObject sendBtn;

	void Start () {

		gs = GameObject.Find ("GameState").GetComponent<GameState> ();
		sendBtn = GameObject.Find ("SendButton");
		ba = GameObject.Find ("UIScripts").GetComponent<BoardActions> ();
		//tiles = GameObject.Find ("UIScripts").GetComponent<LoadBoard> ().allTiles;
	}


	public void clickResponse(){
		print (ID);
		if (!gs.canMovePieceOnBoard) {
			sendBtn.GetComponent<Buttons>().AddLine("Sorry, you currently cannot make any board actions");
			return;
		}
		//so now -reach here execution- player indeed can move pieces
		if (hasPiece) {
			//ba.selectReachablePieces(ID,gs.spacesICanMove); //later RPC this to show all players where can move
			GameObject.Find("UIScripts").GetComponent<PhotonView>().RPC("selectReachablePiecesRemote", PhotonTargets.All, ID, gs.spacesICanMove);
			return;
		}

		if (isSelected) { //so the piece is selected. On click need to remove piece from where it was, reset pieces selected + reset piece with tile then add tile to the piece that was clicked
			//I think I'll go about this super inefficient (on so many levels) right now as in add tag to tile prefab then findobjectswith that tag and reset them all whic would effectively reset what we need.  OK still going to do that but first store this array of tiles rathr than keep retrieving
			/*foreach(GameObject tile in tiles){
				//ba.removePieceFromTile
				tile.GetComponent<Tile>().reset();
				ba.addPieceToTile(ID); //definitely gotta RPC this since need to update new position of piece for everyone + this global reset
			}*/
			gs.canMovePieceOnBoard = false; //just added set since you're just about to move so afterwards no longer--a reset
			GameObject.Find("UIScripts").GetComponent<PhotonView>().RPC("ClearAllTilesAndAdd", PhotonTargets.All, ID);
			if (gs.myHome.Equals(ID)){
				print ("I win!");
				string endMessage = gs.myName + " wins!";
				sendBtn.GetComponent<PhotonView>().RPC("AddLineRemote", PhotonTargets.All, endMessage);
				return; //so won't get to line below to start next players turn and bam! game over
			}
			//let next player start turn
			GameObject.Find("NetworkManager").GetComponent<PhotonView>().RPC("StartTurnRemote", PhotonTargets.All,gs.playerWhoseTurnItIsAfterAttack);
		}

	}



	public void setID(string i){
		ID = i;
	}

	public void select(){
		float r = 46 / 255.0f;
		float g = 48 / 255.0f;
		float b = 146 / 255.0f;
		Color selectedColor = new Color (r,g,b,1);
		isSelected = true;
		gameObject.GetComponent<Image> ().color = selectedColor;
	}

	public void addPiece(){
		hasPiece = true;
		gameObject.GetComponent<Image> ().color = Color.red;
	}

	public void reset(){
		float r = 0 / 255.0f;
		float g = 113 / 255.0f;
		float b = 188 / 255.0f;
		Color unSelectedColor = new Color (r,g,b,1);
		hasPiece = false;
		isSelected = false;
		gameObject.GetComponent<Image> ().color = unSelectedColor;
	}


	//connects two tiles bi-directionally  also make static since it's a method particular to the Tile class, not a given tile
	public static void connectTiles(GameObject tile1, GameObject tile2) {
		connectTileHelper(tile1, tile2);
		connectTileHelper(tile2, tile1);
	}
	
	//connects two tiles uni-directionally
	public static void connectTileHelper(GameObject tile1, GameObject tile2) {
		Tile tile1Script = tile1.GetComponent<Tile>();
		
		if (!tile1Script.tile1) {
			tile1Script.tile1 = tile2;
		} else {
			if (!tile1Script.tile2) {
				tile1Script.tile2 = tile2;
			} else {
				if (!tile1Script.tile3) {
					tile1Script.tile3 = tile2;
				}
			}
		}
	}


}
