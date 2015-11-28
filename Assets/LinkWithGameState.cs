using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LinkWithGameState : MonoBehaviour {


	GameState gs;
	GameObject statsText;

	// Use this for initialization
	void Start () {
		gs = GameObject.Find ("GameState").GetComponent<GameState> ();
		statsText = gameObject.transform.GetChild(2).gameObject;
	}
	
	// Update is called once per frame
	//Make this Script DISABLED so that onJoinRoom, depending on order you enter (detmnd by plist len) enable this
	void Update () {
		string it = gs.It.ToString();
		string cf = gs.Cf.ToString();
		string ht = gs.Ht.ToString();

		if (it != "10") {
			it = "0" + it;
		}
		if (cf != "10") {
			cf = "0" + cf;
		}
		if (ht != "10") {
			ht = "0" + ht;
		}
		string refreshed = it + " " + cf + " " + ht;
		statsText.GetComponent<Text> ().text = refreshed;
	}
}
