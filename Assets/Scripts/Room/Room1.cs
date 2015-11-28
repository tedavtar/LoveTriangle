using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Room1 : MonoBehaviour {
	
	GameObject welcomeText;
	string username;
	bool enabled = true;

	void Start(){
		//UNCOMMENT LINE BELOW!!!!!  (only commented out for UI testing purposes, but need it to get username)
		//username = GameObject.Find ("ParseLogin").GetComponent<ParseDataManage> ().validUsername;


		//load default board dimentions (3,11,4)
		GameObject.Find ("CreateMatrix").GetComponent<InputField> ().text = "3";
		GameObject.Find ("CreateMembrane").GetComponent<InputField> ().text = "11";
		GameObject.Find ("CreateFlagella").GetComponent<InputField> ().text = "4";
	}
	
	public void greyOutRow(){
		float r = 204 / 255.0f;
		float g = 204 / 255.0f;
		float b = 204 / 255.0f;
		Color setColor = new Color (r,g,b,1);
		gameObject.GetComponent<Image> ().color = setColor;
		enabled = false;

		Text[] texts = gameObject.GetComponentsInChildren<Text> ();
		foreach (Text t in texts) {
			t.color = Color.black; //to get the color better on a grey background		
		}
	}
	
	public void highlightRow(GameObject row){
		if (enabled){
		float r = 46 / 255.0f;
		float g = 48 / 255.0f;
		float b = 146 / 255.0f;
		Color setColor = new Color (r,g,b,1);
		row.GetComponent<Image> ().color = setColor;
		}
	}
	
	public void revertRow(GameObject row){
		if (enabled){
		float r = 0;
		float g = 113 / 255.0f;
		float b = 188 / 255.0f;
		Color setColor = new Color (r,g,b,1);
		row.GetComponent<Image> ().color = setColor;
		}
	}

	public void JoinClickedRoom(GameObject row){
		if (enabled) {
			string roomName = row.GetComponent<Transform> ().GetChild (0).GetComponent<Text> ().text;

			//get the boardsize child's boardsize component and load the matrix,m,f vals there and then load the board size
			int matrix = row.GetComponent<Transform> ().GetChild (2).GetComponent<boardsize> ().matrix;
			int membrane = row.GetComponent<Transform> ().GetChild (2).GetComponent<boardsize> ().membrane;
			int flagella = row.GetComponent<Transform> ().GetChild (2).GetComponent<boardsize> ().flagella;

			GameObject.Find ("UIScripts").GetComponent<LoadBoard> ().setUpBoard (matrix,membrane,flagella);
			PhotonNetwork.JoinRoom (roomName);
		}
	}
	
}
