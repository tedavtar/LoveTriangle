using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Deck : MonoBehaviour {

	public List<string> deck;


	GameObject virtualAttack;
	GameObject border;

	public GameObject[] cardSlots = new GameObject[6];

	// Use this for initialization
	void Awake () {
		deck = new List<string>();
		setUpSlots ();
		initDeck();
		initVirtualAttack ();

		for (int i=0; i<4; i++) {
			drawCard();
		}


	}



	public void drawCard(){

		int deckLength = deck.Count;
		int whereToAdd = nextEmptySlot();
		if (deckLength > 0){
			if (whereToAdd >= 0){
				int index = Random.Range(0,deckLength);
				string card = deck[index];
				deck.RemoveAt(index);
				GameObject cardToAdd = Instantiate(Resources.Load(card)) as GameObject;
				cardToAdd.GetComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.None;
				cardToAdd.transform.SetParent (cardSlots[whereToAdd].transform);
				cardToAdd.GetComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.FitInParent;
				cardToAdd.GetComponent<RectTransform>().localScale = new Vector3(.9f,1,1); //apparently need the "f" other else double type complain
			} else {
				GameObject.Find ("SendButton").GetComponent<Buttons>().AddLine("Your hand is full!");
				print ("Hand is full!");
				//so i think later should make this function return an int --like a result callback and only if properly done as in not hand is full nor out of cards would the event get decremented.  now you can waste the stage futile attempting to draw cards--or duh! place a deck size/hand size YEAH these checks on the calling method itself to prevent this from happening in the first place
			}
		} else {
			GameObject.Find ("SendButton").GetComponent<Buttons>().AddLine("You're out of cards!");
			print ("Out of Cards!");
		}
	}


	void initVirtualAttack(){
		//loads the card in first slot
		virtualAttack = Instantiate(Resources.Load("VirtualAttack")) as GameObject;

		//this is so that it can be "played" the first time--even though i think the game would protest not being a special
		GameObject.Find ("UIScripts").GetComponent<CardSelected> ().cardSelected = virtualAttack;

		//to prevent nullpointer in the beginnig if try play Virtual Attack, got to initialize GameState's selectedcardinfo
		GameObject.Find ("GameState").GetComponent<GameState> ().informationAboutSelectedCard = virtualAttack.GetComponent<CardInfo> ();

		virtualAttack.transform.SetParent (cardSlots[0].transform);
		virtualAttack.GetComponent<RectTransform>().localScale = new Vector3(.9f,1,1);

		//adds the red  border
		border = virtualAttack.GetComponent<Transform>().parent.gameObject;
		virtualAttack.GetComponent<Card> ().SelectorHelper(border);
	}

	//give the deck some cards
	void initDeck(){
		//let's add some attacks
		deck.Add ("CurveSet");
		deck.Add ("CurveSet");
		deck.Add ("Flowers");
		deck.Add ("HiddenPassion");
		deck.Add ("HiddenPassion");

		for (int i = 0; i<2; i++){
			//some specials
			deck.Add ("Adderall");
			deck.Add ("CaffeinePill");
			deck.Add ("Drunk");
			deck.Add ("Hairdo");
			deck.Add ("Intimidate");
			deck.Add ("TrashTalk");
			deck.Add ("Steroids");
			deck.Add ("WallflowerBloom");
			deck.Add ("Workout");
		}

		//some portals
		deck.Add ("Teleport");
		deck.Add ("Teleport");
		deck.Add ("Teleport");

	}



	//fills cardSlots with containers/borders to place cards into
	void setUpSlots() {
		string prefix = "Card";
		string suffix = "Border";
		for (int i = 1; i<7; i++){
			string name = prefix + i.ToString() + suffix;
			cardSlots[i - 1] = GameObject.Find(name);
		}
	}

	/*
	 *This will return the index of the next empty slot or -1 if all slots are full. 
	 *Assumes cards added from left (left aligned, no gaps)
	*/
	public int nextEmptySlot(){
		for (int i = 0; i<6; i++) {
			if (cardSlots[i].transform.childCount == 0){ //so if has children! (1st was testing for presence of containers themselves! realized had to use awake not start to make sure they're already initialized
				return i;
			}
		}
		return -1;
	}


	/*
GameObject cardToAdd = Instantiate(Resources.Load("CurveSet")) as GameObject;
		cardToAdd.transform.SetParent (cardSlots[0].transform);
		*/





}
