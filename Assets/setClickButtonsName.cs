using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class setClickButtonsName : MonoBehaviour {

	GameObject target;

	GameObject gs;

	void Start(){
		target = GameObject.Find ("Click");
		gs = GameObject.Find ("GameState");
	}

	public void UpdateClickButtonText(){
		string toSetTo = gameObject.GetComponentInChildren<Text> ().text;
		target.GetComponentInChildren<Text> ().text = toSetTo;
		gs.GetComponent<GameState> ().playerBeingTargeted = toSetTo;
	}
}
