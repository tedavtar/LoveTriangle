using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class createBtnInteractivity : MonoBehaviour {

	public InputField roomName;
	// Use this for initialization
	void Start () {
		roomName = GameObject.Find ("CreateName").GetComponent<InputField> ();
		gameObject.GetComponent<Button>().interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (roomName.text.Length > 0) {
			gameObject.GetComponent<Button>().interactable = true;
		}
	}
}
