using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonInteractivity : MonoBehaviour {

	public InputField username;
	public InputField password;

	// Use this for initialization
	void Start () {
		//username = GameObject.Find ("Username").GetComponent<InputField> ();
		password = GameObject.Find ("Password").GetComponent<InputField> ();
		gameObject.GetComponent<Button>().interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (password.text.Length > 0) {
			gameObject.GetComponent<Button>().interactable = true;
		}
	}
}
