using UnityEngine;
using System.Collections;

public class Message : MonoBehaviour {

	ParseDataManage parse;
	// Use this for initialization
	void Start () {
		parse = GameObject.Find ("ParseLogin").GetComponent<ParseDataManage> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (parse.validCredentials) {
			parse.setMessage("Login successful");	
			return;
		}
		if (parse.invalid) {
			parse.setMessage("Invalid username or password");		
		}
	}
}
