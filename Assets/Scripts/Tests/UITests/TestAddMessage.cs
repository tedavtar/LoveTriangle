using UnityEngine;
using System.Collections;

public class TestAddMessage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		for (int i = 1; i<= 16; i++) {
			testAddLine (i.ToString());
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void testAddLine(string i){
		GameObject uiTests = GameObject.Find ("UIScripts");
		uiTests.GetComponent<Buttons> ().AddLine(i);
	}
}
