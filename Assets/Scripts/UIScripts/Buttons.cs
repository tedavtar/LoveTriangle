using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Buttons : Photon.MonoBehaviour {

	public string description = "Button functionality";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}




	//PLAY AND DRAW

	//Methods to Toggle Button interactable state
	public void DisablePlayButton(){
		GameObject playBtn = GameObject.Find ("PlayButton");
		playBtn.GetComponent<Button> ().interactable = false;
	}

	public void EnablePlayButton(){
		GameObject playBtn = GameObject.Find ("PlayButton");
		playBtn.GetComponent<Button> ().interactable = true;
	}

	public void DisableDrawButton(){
		GameObject playBtn = GameObject.Find ("DrawButton");
		playBtn.GetComponent<Button> ().interactable = false;
	}
	
	public void EnableDrawButton(){
		GameObject playBtn = GameObject.Find ("DrawButton");
		playBtn.GetComponent<Button> ().interactable = true;
	}






	//SEND

	//adds text on a new line to chatbox
	public void AddLine(string message){
		GameObject container = GameObject.Find ("Content");
		string text = container.GetComponent<Text> ().text;
		//print (text.Length.ToString());
		text += "\n";
		text += message;
		text += "\n";
		text.Replace("\n", System.Environment.NewLine);
		container.GetComponent<Text> ().text = text;

	}

	[RPC]
	public void AddLineRemote(string message){
		AddLine(message);
	}

	public void broadcastMyMessage(){
		string inputText = GameObject.Find ("InputField").GetComponent<InputField>().text;
		string username = GameObject.Find ("ParseLogin").GetComponent<ParseDataManage> ().validUsername;
		photonView.RPC ("AddLineRemote", PhotonTargets.All, inputText, username);
	}

	[RPC]
	public void AddLineRemote(string message, string username){
		string line = "";
		line = username + ": " + message;
		AddLine (line);
	}

	//a test/demo of what the rpc will use to add lines to chat wired from the input
	public void AddLineFromInput(){
		string inputText = GameObject.Find ("InputField").GetComponent<InputField>().text;
		AddLine ("me: " + inputText);
	}
}
