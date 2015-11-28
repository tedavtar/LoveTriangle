using UnityEngine;
using System.Collections;

public class TestDrawing : MonoBehaviour {

	GameObject deck;

	void Awake(){
		deck = GameObject.Find ("Deck"); //lolol should have done this for that button interactive state (cache the gameobject here rather than keep calling it. oh wells)
	}

	// Use this for initialization
	void Start () {
		//testNextEmptySlot (); //success.  I added a bunch of cards as children to different "cardXborder" and checked printed index and in all cases, got correct result including -1 if all slots are full!
		/*
		testDrawCard();
		testDrawCard();
		testDrawCard();
		testDrawCard();
		testDrawCard();
		testDrawCard();
		testDrawCard();
		Successful tests using different # of calls + different size of deck string array
		*/
		testDrawCard();
		testDrawCard();
		testDrawCard();
	
	}



	void testDrawCard(){
		deck.GetComponent<Deck> ().drawCard ();
	}




	void testNextEmptySlot(){

		int nextEmptySlot = deck.GetComponent<Deck> ().nextEmptySlot ();
		print (nextEmptySlot.ToString());
	}
}
