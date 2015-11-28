using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Card : MonoBehaviour {

	CardSelected info;

	GameObject cardOnDisplay;

	GameObject f1;
	GameObject f2;
	GameObject f3;
	Vector3 cardInfoScale;
	Vector3 cardInfoScaleZeroed;

	// Use this for initialization
	void Awake () {
		//selected = false;
		info = GameObject.Find("UIScripts").GetComponent<CardSelected>();
		cardOnDisplay = GameObject.Find("CardBlownUp");

		f1 = GameObject.Find("Field1attackorspecial");
		f2 = GameObject.Find("Field2attack");
		f3 = GameObject.Find("Field3target");
		cardInfoScale = new Vector3 (.6675f, .6675f, .6675f);
		cardInfoScaleZeroed = new Vector3 (0, .6675f, .6675f);

		f2.GetComponent<RectTransform> ().localScale = cardInfoScaleZeroed;  //Only needed for Special Attack--atm, don't think implementing that



	}
	
	// Update is called once per frame
	void Update () {
	
	}



	/*Will need to add more here.
	 *Use the card's name to access a card info repository 
	 * Load card info below card + target if applicable
	 * store card info in a gamestate so that on play can use this to carry out move
	 */
	public void DisplayCard(){
		cardOnDisplay.GetComponent<Image> ().sprite = gameObject.GetComponent<Image> ().sprite;

		info.cardSelected = gameObject; //so now can do a destroy on Play to destroy cardSelected


		CardInfo ci = gameObject.GetComponent<CardInfo> ();
		//super redundant but oh well I'm passing in displayed card info to GameState
		GameObject.Find ("GameState").GetComponent<GameState> ().informationAboutSelectedCard = ci;
		if (ci.CardAttribute == "Attack") {
			if (!GameObject.Find ("GameState").GetComponent<GameState> ().amCounterAttacking){
			//ensure Target is visible only when not counterattacking, if counterattacking, no need for a target field because you must be attacking player which initiated attack against you
				f3.GetComponent<RectTransform> ().localScale = cardInfoScale;
			} else {
				f3.GetComponent<RectTransform> ().localScale = cardInfoScaleZeroed;
			}
			int attackVal = ci.getAttackValue ();
			f1.GetComponent<Text>().text = "Attack Value: " + attackVal.ToString();

		}
		if (ci.CardAttribute == "SpecialPositive") {
			//ensure Target is not visible since this special doesn't target anyone
			f3.GetComponent<RectTransform> ().localScale = cardInfoScaleZeroed;

			Vector3 effect = ci.getEffect();
			string display;
			display = "Effect: ";
			if (effect.x != 0) {
				display += "+" + effect.x + " IT ";
			}
			if (effect.y != 0) {
				display += "+" + effect.y + " HT ";
			}
			if (effect.z != 0) {
				display += "+" + effect.z + " CF ";
			}
			f1.GetComponent<Text>().text = display;
		}

		if (ci.CardAttribute == "SpecialNegative") {
			//ensure Target is not visible since this special doest target other
			f3.GetComponent<RectTransform> ().localScale = cardInfoScale;
			
			Vector3 effect = ci.getEffect();
			string display;
			display = "Effect: ";
			if (effect.x != 0) {
				display += "-" + effect.x + " IT ";
			}
			if (effect.y != 0) {
				display += "-" + effect.y + " HT ";
			}
			if (effect.z != 0) {
				display += "-" + effect.z + " CF ";
			}
			f1.GetComponent<Text>().text = display;
		}

		if (ci.CardAttribute == "Portal") {
			//ensure Target is not visible since this special doesn't target anyone
			f3.GetComponent<RectTransform> ().localScale = cardInfoScaleZeroed;

			f1.GetComponent<Text>().text = "Teleports her to your portal!";
		}
	}

	public void ToggleSelection(){

		if (!info.selected){ //no card is selected at the moment, select this card, add border to it

			info.border = transform.parent.gameObject;
			SelectorHelper(info.border);

			DisplayCard();

		} else { //so a card is already selected 
			if (info.border.name != transform.parent.gameObject.name){ //some other card selected, 1st deselect it
				DeselectorHelper(info.border);

				//gets the red border visible for the card just clicked
				info.border = transform.parent.gameObject;
				SelectorHelper(info.border);

				DisplayCard();
			} else{
				DeselectorHelper(info.border);  //so this card already selected, deselect it
			}

		}
	}

	public void SelectorHelper(GameObject border){
		info.setBorder(border);

		Color c = border.GetComponent<Image> ().color;
		c.a = 37;
		border.GetComponent<Image> ().color = c;
		info.selected = true;
	}

	public void DeselectorHelper(GameObject border){
		Color c = border.GetComponent<Image> ().color;
		c.a = 0;
		border.GetComponent<Image> ().color = c;
		info.selected = false;
	}
}

/*public void ToggleBorder(){

		if (!selected){

			//gets the red border visible for the card just clicked
			border = transform.parent.gameObject;
			Color c = border.GetComponent<Image> ().color;
			c.a = 37;
			border.GetComponent<Image> ().color = c;
			selected = true;
		} else {
			border = transform.parent.gameObject;
			Color c = border.GetComponent<Image> ().color;
			c.a = 0;
			border.GetComponent<Image> ().color = c;
			selected = false;
		}
	}*/
