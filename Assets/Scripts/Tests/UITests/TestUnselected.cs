using UnityEngine;
using System.Collections;

public class TestUnselected : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//testDisablePlayButton ();  Success
		//testDisableDrawButton ();  Success
		//testEnableDrawButton ();  Success
		//testEnablePlayButton ();  Success
	}








	void testDisablePlayButton(){
		GameObject uiTests = GameObject.Find ("UIScripts");
		uiTests.GetComponent<Buttons> ().DisablePlayButton ();
	}

	void testDisableDrawButton(){
		GameObject uiTests = GameObject.Find ("UIScripts");
		uiTests.GetComponent<Buttons> ().DisableDrawButton ();
	}

	void testEnablePlayButton(){
		GameObject uiTests = GameObject.Find ("UIScripts");
		uiTests.GetComponent<Buttons> ().EnablePlayButton ();
	}
	
	
	void testEnableDrawButton(){
		GameObject uiTests = GameObject.Find ("UIScripts");
		uiTests.GetComponent<Buttons> ().EnableDrawButton ();
	}
}
