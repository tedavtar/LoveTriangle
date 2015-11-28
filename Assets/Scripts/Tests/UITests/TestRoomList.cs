using UnityEngine;
using System.Collections;

public class TestRoomList : MonoBehaviour {

	GameObject rmList;
	public GameObject roomRow;
	// Use this for initialization
	void Start () {
		rmList = GameObject.Find ("RoomList");
		addRow ();
		addRow ();

	}
	
	void addRow(){
		GameObject newRow = Instantiate (roomRow) as GameObject;

		newRow.transform.SetParent (rmList.transform);
		newRow.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);  //very important that this line takes place AFTER set parent!
	}

}
