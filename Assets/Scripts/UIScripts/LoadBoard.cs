using UnityEngine;
using System.Collections;

public class LoadBoard : MonoBehaviour {

	public GameObject tilePrefab;

	public BoardActions ba;
	public GameObject[] allTiles;

	GameObject flagella1 = null;
	GameObject flagella2 = null;
	GameObject flagella3 = null;
	GameObject portal1 = null;
	GameObject portal2 = null;
	GameObject portal3 = null;
	GameObject matrix1 = null;
	GameObject matrix2 = null;
	GameObject matrix3 = null;
	GameObject membraneRight1 = null;
	GameObject membraneRight2 = null;
	GameObject membraneRight3 = null;
	GameObject membraneLeft1 = null;
	GameObject membraneLeft2 = null;
	GameObject membraneLeft3 = null;
	GameObject middleTile = null;

	GameObject MTile = null;

	GameObject P1Tile = null;
	GameObject P2Tile = null;
	GameObject P3Tile = null;

	GameObject Middle1Tile = null; //refers to tile with ID M11-like a semi portal sorta that is tri junction bw 2 membrane segments and a matrix segment
	GameObject Middle2Tile = null;
	GameObject Middle3Tile = null;

	// Use this for initialization
	void Awake () {
		ba = GameObject.Find ("UIScripts").GetComponent<BoardActions> ();

		flagella1 = GameObject.Find ("Flagella1");
		flagella2 = GameObject.Find ("Flagella2");
		flagella3 = GameObject.Find ("Flagella3");
		portal1 = GameObject.Find ("Portal1");
		portal2 = GameObject.Find ("Portal2");
		portal3 = GameObject.Find ("Portal3");
		matrix1 = GameObject.Find ("Matrix1");
		matrix2 = GameObject.Find ("Matrix2");
		matrix3 = GameObject.Find ("Matrix3");
		membraneRight1 = GameObject.Find ("MembraneRight1");
		membraneRight2 = GameObject.Find ("MembraneRight2");
		membraneRight3 = GameObject.Find ("MembraneRight3");
		membraneLeft1 = GameObject.Find ("MembraneLeft1");
		membraneLeft2 = GameObject.Find ("MembraneLeft2");
		membraneLeft3 = GameObject.Find ("MembraneLeft3");
		middleTile = GameObject.Find ("MiddleTile");
		//setUpBoard (3,11,4);
	}



	[RPC]
	void ClearAllTilesAndAdd(string id){
		foreach (GameObject tile in allTiles) {
			tile.GetComponent<Tile>().reset();	
		}
		ba.addPieceToTile (id);
	}





	/* nah, since all players entering room have room info before clicking, just let Room1 handle that since has access to info
	[RPC]
	void setUpBoardRemote (int matrixSize, int membraneSize, int flagellaSize) {
		setUpBoard (matrixSize,membraneSize,flagellaSize);
	}*/

	public void setUpBoard (int matrixSize, int membraneSize, int flagellaSize) {

		//load Middle tile
		MTile = Instantiate (tilePrefab) as GameObject;
		MTile.transform.SetParent (middleTile.transform);
		Tile mt = MTile.GetComponent<Tile>();
		mt.setID("M");
		mt.name = mt.ID;

		//load Portal1 tile
		P1Tile = Instantiate (tilePrefab) as GameObject;
		P1Tile.transform.SetParent (portal1.transform);
		Tile p1t = P1Tile.GetComponent<Tile>();
		p1t.setID("P1");
		p1t.name = p1t.ID;

		//load Portal2 tile
		P2Tile = Instantiate (tilePrefab) as GameObject;
		P2Tile.transform.SetParent (portal2.transform);
		Tile p2t = P2Tile.GetComponent<Tile>();
		p2t.setID("P2");
		p2t.name = p2t.ID;

		//load Portal3 tile
		P3Tile = Instantiate (tilePrefab) as GameObject;
		P3Tile.transform.SetParent (portal3.transform);
		Tile p3t = P3Tile.GetComponent<Tile>();
		p3t.setID("P3");
		p3t.name = p3t.ID;






		//load Flagella1 tiles
		GameObject prevFlagella1Tile = null;
		for (int i=0; i<flagellaSize; i++){
			GameObject tile = Instantiate (tilePrefab) as GameObject;
			tile.transform.SetParent (flagella1.transform);

			Tile t = tile.GetComponent<Tile>();
			t.setID("F1" + (i+1).ToString());
			tile.name = t.ID;

			if (prevFlagella1Tile!=null){
				Tile.connectTiles(prevFlagella1Tile,tile);
			}
			if (i+1 == flagellaSize){ //means right next to P1
				Tile.connectTiles(P1Tile,tile);
			}
			prevFlagella1Tile = tile;
		}


		//load Flagella2
		GameObject prevFlagella2Tile = null;
		for (int i=0; i<flagellaSize; i++){
			GameObject tile = Instantiate (tilePrefab) as GameObject;
			tile.transform.SetParent (flagella2.transform);
			
			Tile t = tile.GetComponent<Tile>();
			t.setID("F2" + (i+1).ToString());
			tile.name = t.ID;
			tile.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);  //for some reason local rotation was being changed to 59.999, clearly some equilateral triangle geom stuff at play, but resetting to 0 did the trick, properly orients tiles!
			if (prevFlagella2Tile!=null){
				Tile.connectTiles(prevFlagella2Tile,tile);
			}
			if (i+1 == flagellaSize){
				Tile.connectTiles(P2Tile,tile);
			}
			prevFlagella2Tile = tile;
		}

		//load Flagella3
		GameObject prevFlagella3Tile = null;
		for (int i=0; i<flagellaSize; i++){
			GameObject tile = Instantiate (tilePrefab) as GameObject;
			tile.transform.SetParent (flagella3.transform);
			
			Tile t = tile.GetComponent<Tile>();
			t.setID("F3" + (i+1).ToString());
			tile.name = t.ID;
			tile.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);  //for some reason local rotation was being changed to 59.999, clearly some equilateral triangle geom stuff at play, but resetting to 0 did the trick, properly orients tiles!
			if (prevFlagella3Tile!=null){
				Tile.connectTiles(prevFlagella3Tile,tile);
			}
			if (i+1 == flagellaSize){
				Tile.connectTiles(P3Tile,tile);
			}
			prevFlagella3Tile = tile;
		}

		//load Matrix1 Tiles
		GameObject prevMatrix1Tile = null;
		for (int i=0; i<matrixSize; i++){
			GameObject tile = Instantiate (tilePrefab) as GameObject;
			tile.transform.SetParent (matrix1.transform);
			
			Tile t = tile.GetComponent<Tile>();
			t.setID("M1" + (i+1).ToString());
			tile.name = t.ID;
			tile.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);  //for some reason local rotation was being changed to 59.999, clearly some equilateral triangle geom stuff at play, but resetting to 0 did the trick, properly orients tiles!
			if (prevMatrix1Tile!=null){
				Tile.connectTiles(prevMatrix1Tile,tile);
			}
			if (i == 0){
				Middle1Tile = tile;
			}
			if (i+1 == matrixSize){
				Tile.connectTiles(MTile,tile);
			}
			prevMatrix1Tile = tile;
		}

		//load Matrix2 Tiles
		GameObject prevMatrix2Tile = null;
		for (int i=0; i<matrixSize; i++){
			GameObject tile = Instantiate (tilePrefab) as GameObject;
			tile.transform.SetParent (matrix2.transform);
			
			Tile t = tile.GetComponent<Tile>();
			t.setID("M2" + (i+1).ToString());
			tile.name = t.ID;
			tile.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);  //for some reason local rotation was being changed to 59.999, clearly some equilateral triangle geom stuff at play, but resetting to 0 did the trick, properly orients tiles!
			if (prevMatrix2Tile!=null){
				Tile.connectTiles(prevMatrix2Tile,tile);
			}
			if (i == 0){
				Middle2Tile = tile;
			}
			if (i+1 == matrixSize){
				Tile.connectTiles(MTile,tile);
			}
			prevMatrix2Tile = tile;
		}

		//load Matrix3 Tiles
		GameObject prevMatrix3Tile = null;
		for (int i=0; i<matrixSize; i++){
			GameObject tile = Instantiate (tilePrefab) as GameObject;
			tile.transform.SetParent (matrix3.transform);
			
			Tile t = tile.GetComponent<Tile>();
			t.setID("M3" + (i+1).ToString());
			tile.name = t.ID;
			tile.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);  //for some reason local rotation was being changed to 59.999, clearly some equilateral triangle geom stuff at play, but resetting to 0 did the trick, properly orients tiles!
			if (prevMatrix3Tile!=null){
				Tile.connectTiles(prevMatrix3Tile,tile);
			}
			if (i == 0){
				Middle3Tile = tile;
			}
			if (i+1 == matrixSize){
				Tile.connectTiles(MTile,tile);
			}
			prevMatrix3Tile = tile;
		}

		//load MembraneRight1Tiles
		GameObject prevMembraneRight1Tile = null;
		for (int i=0; i<membraneSize; i++){
			GameObject tile = Instantiate (tilePrefab) as GameObject;
			tile.transform.SetParent (membraneRight1.transform);
			
			Tile t = tile.GetComponent<Tile>();
			t.setID("R1" + (i+1).ToString());
			tile.name = t.ID;
			tile.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);  //for some reason local rotation was being changed to 59.999, clearly some equilateral triangle geom stuff at play, but resetting to 0 did the trick, properly orients tiles!
			if (prevMembraneRight1Tile!=null){
				Tile.connectTiles(prevMembraneRight1Tile,tile);
			}
			if (i == 0){
				Tile.connectTiles(P1Tile,tile);
			}
			if (i+1 == membraneSize){
				Tile.connectTiles(Middle3Tile,tile);
			}
			prevMembraneRight1Tile = tile;
		}

		//load MembraneRight2Tiles
		GameObject prevMembraneRight2Tile = null;
		for (int i=0; i<membraneSize; i++){
			GameObject tile = Instantiate (tilePrefab) as GameObject;
			tile.transform.SetParent (membraneRight2.transform);
			
			Tile t = tile.GetComponent<Tile>();
			t.setID("R2" + (i+1).ToString());
			tile.name = t.ID;
			tile.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);  //for some reason local rotation was being changed to 59.999, clearly some equilateral triangle geom stuff at play, but resetting to 0 did the trick, properly orients tiles!
			if (prevMembraneRight2Tile!=null){
				Tile.connectTiles(prevMembraneRight2Tile,tile);
			}
			if (i == 0){
				Tile.connectTiles(P2Tile,tile);
			}
			if (i+1 == membraneSize){
				Tile.connectTiles(Middle1Tile,tile);
			}
			prevMembraneRight2Tile = tile;
		}

		//load MembraneRight3Tiles
		GameObject prevMembraneRight3Tile = null;
		for (int i=0; i<membraneSize; i++){
			GameObject tile = Instantiate (tilePrefab) as GameObject;
			tile.transform.SetParent (membraneRight3.transform);
			
			Tile t = tile.GetComponent<Tile>();
			t.setID("R3" + (i+1).ToString());
			tile.name = t.ID;
			tile.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);  //for some reason local rotation was being changed to 59.999, clearly some equilateral triangle geom stuff at play, but resetting to 0 did the trick, properly orients tiles!
			if (prevMembraneRight3Tile!=null){
				Tile.connectTiles(prevMembraneRight3Tile,tile);
			}
			if (i == 0){
				Tile.connectTiles(P3Tile,tile);
			}
			if (i+1 == membraneSize){
				Tile.connectTiles(Middle2Tile,tile);
			}
			prevMembraneRight3Tile = tile;
		}

		//load MembraneLeft1Tiles
		GameObject prevMembraneLeft1Tile = null;
		for (int i=0; i<membraneSize; i++){
			GameObject tile = Instantiate (tilePrefab) as GameObject;
			tile.transform.SetParent (membraneLeft1.transform);
			
			Tile t = tile.GetComponent<Tile>();
			t.setID("L1" + (i+1).ToString());
			tile.name = t.ID;
			tile.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);  //for some reason local rotation was being changed to 59.999, clearly some equilateral triangle geom stuff at play, but resetting to 0 did the trick, properly orients tiles!
			if (prevMembraneLeft1Tile!=null){
				Tile.connectTiles(prevMembraneLeft1Tile,tile);
			}
			if (i == 0){
				Tile.connectTiles(P1Tile,tile);
			}
			if (i+1 == membraneSize){
				Tile.connectTiles(Middle2Tile,tile);
			}
			prevMembraneLeft1Tile = tile;
		}

		//load MembraneLeft2Tiles
		GameObject prevMembraneLeft2Tile = null;
		for (int i=0; i<membraneSize; i++){
			GameObject tile = Instantiate (tilePrefab) as GameObject;
			tile.transform.SetParent (membraneLeft2.transform);
			
			Tile t = tile.GetComponent<Tile>();
			t.setID("L2" + (i+1).ToString());
			tile.name = t.ID;
			tile.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);  //for some reason local rotation was being changed to 59.999, clearly some equilateral triangle geom stuff at play, but resetting to 0 did the trick, properly orients tiles!
			if (prevMembraneLeft2Tile!=null){
				Tile.connectTiles(prevMembraneLeft2Tile,tile);
			}
			if (i == 0){
				Tile.connectTiles(P2Tile,tile);
			}
			if (i+1 == membraneSize){
				Tile.connectTiles(Middle3Tile,tile);
			}
			prevMembraneLeft2Tile = tile;
		}

		//load MembraneLeft3Tiles
		GameObject prevMembraneLeft3Tile = null;
		for (int i=0; i<membraneSize; i++){
			GameObject tile = Instantiate (tilePrefab) as GameObject;
			tile.transform.SetParent (membraneLeft3.transform);
			
			Tile t = tile.GetComponent<Tile>();
			t.setID("L3" + (i+1).ToString());
			tile.name = t.ID;
			tile.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);  //for some reason local rotation was being changed to 59.999, clearly some equilateral triangle geom stuff at play, but resetting to 0 did the trick, properly orients tiles!
			if (prevMembraneLeft3Tile!=null){
				Tile.connectTiles(prevMembraneLeft3Tile,tile);
			}
			if (i == 0){
				Tile.connectTiles(P3Tile,tile);
			}
			if (i+1 == membraneSize){
				Tile.connectTiles(Middle1Tile,tile);
			}
			prevMembraneLeft3Tile = tile;
		}


		//added now-initialization to addPieceToMiddleTile + cache a list of Tiles
		ba.addPieceToTile ("M");
		allTiles = GameObject.FindGameObjectsWithTag ("Tile");
	}
}
