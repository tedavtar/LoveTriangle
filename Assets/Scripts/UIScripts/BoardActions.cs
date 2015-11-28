using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardActions : MonoBehaviour {

	GameState gs;

	void Start(){
		gs = GameObject.Find ("GameState").GetComponent<GameState> ();
		//addPieceToTile ("M");
		//removePieceFromTile("M");
		//selectTile ("M");
		//selectReachablePieces ("P1", 3);
	
	}

	//places piece on tile with ID--gives the tile the piece, adds it to GameState
	public void addPieceToTile(string id){
		GameObject targetTile = GameObject.Find (id);
		targetTile.GetComponent<Tile> ().addPiece ();

		gs.TileWithPiece = id;
	}

	public void removePieceFromTile(string id){
		GameObject targetTile = GameObject.Find (id);
		targetTile.GetComponent<Tile> ().reset ();
	}

	public void selectTile(string id){
		GameObject targetTile = GameObject.Find (id);
		targetTile.GetComponent<Tile> ().select();
	}

	[RPC]
	public void selectReachablePiecesRemote(string id, int moves){
		selectReachablePieces1(id, moves);
		//replace this with selectReachablePieces1(id,moves);
	}

	public void selectReachablePieces(string id, int moves){
		List<string> reachablePieces = new List<string>();
		Tile targetTileScript = GameObject.Find (id).GetComponent<Tile> ();
		reachableHelper (reachablePieces, targetTileScript, moves); //should fill reachablePieces with IDs of them
		foreach(string reachableID in reachablePieces){
			if (!reachableID.Equals(id)){ //so only allow to move to DIFFERENT spot
				selectTile(reachableID);
			}
		}
	}

	public void reachableHelper(List<string> toAddTo, Tile currentTile, int moves){
		if (moves == 0) {
			toAddTo.Add(currentTile.ID);
		} else {
			if (currentTile.tile1 != null){
				reachableHelper(toAddTo,currentTile.tile1.GetComponent<Tile>(),moves - 1);
			}
			if (currentTile.tile2 != null){
				reachableHelper(toAddTo,currentTile.tile2.GetComponent<Tile>(),moves - 1);
			}
			if (currentTile.tile3 != null){
				reachableHelper(toAddTo,currentTile.tile3.GetComponent<Tile>(),moves - 1);
			}
		}
	}


	//now for the improved reachable!
	public void selectReachablePieces1(string id, int movesToDo){
		List<object[]> visitedNodes = new List<object[]>();
		Queue<object[]> fringe = new Queue<object[]> ();
		int moves = movesToDo;
		Tile t = GameObject.Find (id).GetComponent<Tile> ();
		object[] rootNode = makeNode (t, moves);
		visitedNodes.Add (rootNode);
		fringe.Enqueue (rootNode);
		while (true) {
			object[] testNode = fringe.Dequeue();
			int testMoves = (int)testNode[1];
			if (testMoves < 0){break;}
			Tile testTile = (Tile)testNode[0];
			
			moves = testMoves - 1;
			
			List<Tile> neighborTiles = new List<Tile>();
			if (testTile.tile1 != null){
				neighborTiles.Add(testTile.tile1.GetComponent<Tile>());
			}
			if (testTile.tile2 != null){
				neighborTiles.Add(testTile.tile2.GetComponent<Tile>());
			}
			if (testTile.tile3 != null){
				neighborTiles.Add(testTile.tile3.GetComponent<Tile>());
			}
			foreach(Tile neighborTile in neighborTiles){
				string testID = neighborTile.ID;
				bool shouldIVisit = visitNode(testID,moves,visitedNodes);
				if (shouldIVisit){
					object[] childNode = makeNode(neighborTile,moves);
					visitedNodes.Add (childNode);
					fringe.Enqueue (childNode);
				}
			}
			
		}
		
		foreach (object[] node in visitedNodes) {
			Tile nodeTile = (Tile)node[0];
			string nodeID = nodeTile.ID;
			int nodeMoves = (int)node[1];
			//print ("Node: Tile: " + nodeID + " moves: " + nodeMoves.ToString());
			
			
			
			if (!nodeID.Equals(id) && nodeMoves == 0){ //so only allow to move to DIFFERENT spot
				this.selectTile(nodeID);
			}
			
		}
		
	}
	
	public bool visitNode(string testID, int moves, List<object[]> visitedNodes){
		bool rtn = true;
		foreach(object[] node in visitedNodes){
			Tile nodeTile = (Tile)node[0];
			string nodeID = nodeTile.ID;
			int nodeMoves = (int)node[1];
			
			if(testID.Equals(nodeID)){
				/*
				int difference = nodeMoves - moves;
				if ((difference % 2) == 0){
					rtn = false;
				}*/
				if(nodeMoves == moves){
					return false;
				}
				
			}
		}
		return rtn;
	}
	
	public object[] makeNode(Tile t, int moves){
		object[] rtnNode = new object[2];
		rtnNode [0] = t;
		rtnNode [1] = moves;
		return rtnNode;
	}



}
