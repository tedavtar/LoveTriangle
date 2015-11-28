using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HandleTurn : Photon.MonoBehaviour {

	GameState gs;

	GameObject sendBtn;

	GameObject deck;

	public int eventsLeftInStage2 = 2;




	//Really bad redundant thing I'm about to do, forgive me gods of redunda..no
	public Sprite[] cardSprites = new Sprite[14];

	// Use this for initialization
	void Start () {
		deck = GameObject.Find ("Deck");
		gs = GameObject.Find ("GameState").GetComponent<GameState> ();
		sendBtn = GameObject.Find ("SendButton");
	}
	
	void StartTurn(){
		gs.isMyTurn = true;
		gs.myStage = Stage.first;

		GameObject.Find ("DrawButton").GetComponent<Button>().interactable = true;

		string broadcastThatItIsMyTurn = "It is " + gs.myName + "'s turn";
		sendBtn.GetComponent<PhotonView>().RPC("AddLineRemote", PhotonTargets.All, broadcastThatItIsMyTurn);
		//similar to what's directly above except displaying it somewhere else
		string turnDisplay = gs.myName + "'s turn";
		photonView.RPC ("DisplayPlayerTurnRemote", PhotonTargets.All, turnDisplay);
	}

	[RPC]
	void StartTurnRemote(int next){ //Originally argument was Player next, but was some type issues so used casting/stick to int

		/* won't work since last player to enter doesn't have the playerwhoseturn info
		//regardless if whose turn it just became, want to update certain elements to reflect whose turn it now is
		Player playerWhoseTurnItIs = (Player)next;
		string p = gs.players[(int)playerWhoseTurnItIs];
		GameObject.Find ("PlayerWhoseTurnItIs").GetComponent<Text> ().text = p + "'s turn";*/

		//if it's your turn, then change appropriate GameState Vars
		if ((int)gs.myIdentity == next){
			//print ("it's my turn");
			
			StartTurn();
		}
	}



	[RPC]
	void allowPlayerToMoveSpacesInBoard(string player, int spaces, int redundantPlayerWhoseTurnIsNext){
		//print ("yobin");
		//print (player);
		print (player + " now prompted to move " + spaces + " spaces");
		if (gs.myName.Equals (player)) { 
			gs.playerWhoseTurnItIsAfterAttack = redundantPlayerWhoseTurnIsNext;
			gs.canMovePieceOnBoard = true;
			gs.spacesICanMove = spaces;
		}
	}




	[RPC]
	void activateCounterAttackMode(string target, string attacker, int playerNumberOfAttacker, int attackVal){
		//print (target + " is being targeted by " + attacker + " with an Attack of " + attackVal);
		//print (target);
		//print (attacker);
		//print (playerNumberOfAttacker.ToString ());
		gs.attackingMeWith = attackVal;
		gs.myattacker = attacker;
		switch (playerNumberOfAttacker) {
		case (int)Player.first:
			gs.playerWhoseTurnItIsAfterAttack = (int)Player.second;
			break;
		case (int)Player.second:
			gs.playerWhoseTurnItIsAfterAttack = (int)Player.third;
			break;
		default:
			gs.playerWhoseTurnItIsAfterAttack = (int)Player.first;
			break;
		}

		//print (playerWhoseTurnItIsAfterAttack.ToString());

		string myNameWithAnExtraSpace = " " + gs.myName;
		if (string.Equals (target, myNameWithAnExtraSpace)) {
			//so I'm being attacked
			gs.amCounterAttacking = true;
			GameObject.Find ("PlayButton").GetComponent<Button>().interactable = true;
			//ADD NOTE CHATBOX TO PLAYER just him/her not all to counterattack and that card automatically will automatically direct to attacking player (regardless of what you choose)
			sendBtn.GetComponent<Buttons>().AddLine("Time for you to counterattack");
		}
	}

	[RPC]
	void ChangePlayerStats(string target, Vector3 effect){  //like start turn remote in how though dierected at all, selectively respond
		/*
		print ("called");
		print (gs.myName);
		print (effect);
		print (target);*/
		//WOW it was a matter of spacing. target has an extra space
		string myNameWithAnExtraSpace = " " + gs.myName;
		//print (target);
		//print (myNameWithAnExtraSpace);
		if (string.Equals(target,myNameWithAnExtraSpace)) {
			//print ("reached here");
			gs.It = gs.It + (int)effect.x;
			gs.Ht = gs.Ht + (int)effect.y;
			gs.Cf = gs.Cf + (int)effect.z;

			if (gs.It < 0){
				gs.It = 0;
			}
			if (gs.Ht < 0){
				gs.Ht = 0;
			}if (gs.Cf < 0){
				gs.Cf = 0;
			}

			if (gs.It > 10){
				gs.It = 10;
			}
			if (gs.Ht > 10){
				gs.Ht = 10;
			}
			if (gs.Cf > 10){
				gs.Cf = 10;
			}
			int me = (int)gs.myIdentity;

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

			string myNewStats = it + " " + cf + " " + ht;
			photonView.RPC("UpdateMyStats",PhotonTargets.All,me,myNewStats);
		}
	}


	[RPC]
	void UpdateMyStats(int sender, string toSetTo){
		switch (sender) {
		case (int)Player.first:
			GameObject.Find("Player1Stats").GetComponent<Text>().text = toSetTo;
			break;
		case (int)Player.second:
			GameObject.Find("Player2Stats").GetComponent<Text>().text = toSetTo;
			break;
		default:
			GameObject.Find("Player3Stats").GetComponent<Text>().text = toSetTo;
			break;
			
		}
	}



	public void drawButton(){
		if (gs.myStage == Stage.first) {
			gs.myStage = Stage.second;
			//right here need to put checks for empty deck or full hand

			if (deck.GetComponent<Deck>().nextEmptySlot() < 0){ //so your hand is full
				GameObject.Find ("SendButton").GetComponent<Buttons>().AddLine("Your hand is full!");
				photonView.RPC ("DisplayStageCompleteRemote", PhotonTargets.All, 1);
				GameObject.Find ("PlayButton").GetComponent<Button>().interactable = true;
				return;
			}

			if (deck.GetComponent<Deck>().deck.Count == 0){
				GameObject.Find ("SendButton").GetComponent<Buttons>().AddLine("You are out of cards!");
				photonView.RPC ("DisplayStageCompleteRemote", PhotonTargets.All, 1);
				GameObject.Find ("PlayButton").GetComponent<Button>().interactable = true;
				return;
			}

			deck.GetComponent<Deck> ().drawCard ();
			string message = gs.myName + " has drawn a card.";
			sendBtn.GetComponent<PhotonView>().RPC("AddLineRemote", PhotonTargets.All, message);
			photonView.RPC ("DisplayStageCompleteRemote", PhotonTargets.All, 1);
			GameObject.Find ("PlayButton").GetComponent<Button>().interactable = true;  //since you can play in stage2
			return;
		}

		if (gs.myStage == Stage.second) {

			if (deck.GetComponent<Deck>().nextEmptySlot() < 0){ //so your hand is full
				GameObject.Find ("SendButton").GetComponent<Buttons>().AddLine("Your hand is full! Trying to draw counts as an event. If you don't have any specials to play (or don't want to), then keep trying to draw till Stage2 ends");
				eventsLeftInStage2 = eventsLeftInStage2 - 1;
				if (eventsLeftInStage2 == 0){
					eventsLeftInStage2 = 2;//reset
					GameObject.Find ("DrawButton").GetComponent<Button>().interactable = false; //addednow, so can't draw in 3rd phase
					gs.myStage = Stage.third;
					photonView.RPC ("DisplayStageCompleteRemote", PhotonTargets.All, 2);
				}
				return;
			}
			if (deck.GetComponent<Deck>().deck.Count == 0){
				GameObject.Find ("SendButton").GetComponent<Buttons>().AddLine("You are out of cards! Trying to draw counts as an event. If you don't have any specials to play (or don't want to), then keep trying to draw till Stage2 ends");
				eventsLeftInStage2 = eventsLeftInStage2 - 1;
				if (eventsLeftInStage2 == 0){
					eventsLeftInStage2 = 2;//reset
					GameObject.Find ("DrawButton").GetComponent<Button>().interactable = false; //addednow, so can't draw in 3rd phase
					gs.myStage = Stage.third;
					photonView.RPC ("DisplayStageCompleteRemote", PhotonTargets.All, 2);
				}
				return;
			}

			deck.GetComponent<Deck> ().drawCard ();
			string message = gs.myName + " has drawn a card.";
			sendBtn.GetComponent<PhotonView>().RPC("AddLineRemote", PhotonTargets.All, message);
			eventsLeftInStage2 = eventsLeftInStage2 - 1;

			if (eventsLeftInStage2 == 0){
				eventsLeftInStage2 = 2;//reset
				GameObject.Find ("DrawButton").GetComponent<Button>().interactable = false; //addednow, so can't draw in 3rd phase
				gs.myStage = Stage.third;
				photonView.RPC ("DisplayStageCompleteRemote", PhotonTargets.All, 2);
			}

			return;
		}
	}



	public void playButton(){

		//get name of card this is currently selected
		string cardName = gs.informationAboutSelectedCard.CardName;

		//get the attribute of the card
		string cardAttr = gs.informationAboutSelectedCard.CardAttribute;

		//get your username
		string username = GameObject.Find ("ParseLogin").GetComponent<ParseDataManage> ().validUsername;




		//here we get reference to the card we will destory and make sure that it is not empty
		GameObject cardToDestroy = GameObject.Find ("UIScripts").GetComponent<CardSelected> ().cardSelected;
		if (cardToDestroy == null) {
			sendBtn.GetComponent<Buttons>().AddLine("Please select a card first!");
			return;
		}

		/*
		//LATER ADD THIS TO STAGE3 (since only dealing with specials--not attacks right now
		if (!cardName.Equals("VirtualAttack")){
			Color c = cardToDestroy.transform.parent.GetComponent<Image> ().color;
			c.a = 0;
			cardToDestroy.transform.parent.GetComponent<Image> ().color = c;
			Destroy (cardToDestroy); //destory card selected 
		}*/


		if (gs.amCounterAttacking) {

			if (cardAttr.Equals("SpecialPositive")||cardAttr.Equals("SpecialNegative")||cardAttr.Equals("Portal")){//should have just done !=attack type card...
				sendBtn.GetComponent<Buttons>().AddLine("You must counterattack with an Attack Card!");
				return;
			}
			photonView.RPC("LoadImageToOtherPlayer", PhotonTargets.All, cardName);
			int attackVal = gs.informationAboutSelectedCard.getAttackValue();
			string display = "Attack value: " + attackVal.ToString();
			photonView.RPC("LoadEffect", PhotonTargets.All, display);
			string movedesc = username + " counterattacked";
			photonView.RPC("LoadMoveDescription", PhotonTargets.All, movedesc);
			int difference = attackVal - gs.attackingMeWith;
			string resultMessage = "";
			if (difference > 0){
				resultMessage = username + " gets to move " + difference.ToString() + " spaces.";
				photonView.RPC("allowPlayerToMoveSpacesInBoard",PhotonTargets.All, username,difference,gs.playerWhoseTurnItIsAfterAttack);
			}
			if (difference < 0){
				resultMessage = gs.myattacker + " gets to move " + (-1*difference).ToString() + " spaces.";
				photonView.RPC("allowPlayerToMoveSpacesInBoard",PhotonTargets.All, gs.myattacker,(-1*difference),gs.playerWhoseTurnItIsAfterAttack);
			}
			//NEED TO ADD CASE WHERE DIFFERENCE IS 0 (then immediately state next person's turn
			if (difference == 0){
				resultMessage = gs.myattacker + "'s and " + username + "'s attacks canceled each other out. ";
				photonView.RPC("StartTurnRemote", PhotonTargets.All,gs.playerWhoseTurnItIsAfterAttack);
			}

			sendBtn.GetComponent<PhotonView>().RPC("AddLineRemote", PhotonTargets.All, resultMessage);

			if (!cardName.Equals("VirtualAttack")){
				Color c = cardToDestroy.transform.parent.GetComponent<Image> ().color;
				c.a = 0;
				cardToDestroy.transform.parent.GetComponent<Image> ().color = c;
				Destroy (cardToDestroy); //destory card selected 
			}
			
			gs.amCounterAttacking = false; //reset
			GameObject.Find ("PlayButton").GetComponent<Button>().interactable = false;//reset
			
			return;
		}



		if (gs.myStage == Stage.second) {

			//make sure that player trying to play a card with attribute special
			if (cardAttr.Equals("Attack")||cardAttr.Equals("Portal")){
				sendBtn.GetComponent<Buttons>().AddLine("You can only play a special in Stage2!");
				return;//return right here since don't want to count this as an event
			}

			//LoadImageToOtherPlayer (cardName); So moved it here since now in stage 2 and trying to play a special card, so we're good-can broadcast it's image
			photonView.RPC("LoadImageToOtherPlayer", PhotonTargets.All, cardName);


			if (cardAttr.Equals ("SpecialPositive")) {
				string desc = username + " played";
				//LoadMoveDescription(desc);
				photonView.RPC("LoadMoveDescription", PhotonTargets.All, desc);
				Vector3 effect = gs.informationAboutSelectedCard.getEffect();
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
				photonView.RPC("LoadEffect", PhotonTargets.All, display);
				string mynamewithextraspace = " " + gs.myName;

				photonView.RPC ("ChangePlayerStats", PhotonTargets.All, mynamewithextraspace, effect);
			}

			if (cardAttr.Equals ("SpecialNegative")) {
				//need to figure out who is being targeted
				string myTarget = gs.playerBeingTargeted;
				//catch to ensure that someone is in fact being targeted
				if (myTarget == ""){
					sendBtn.GetComponent<Buttons>().AddLine("Choose a target!");
					return;
				}
				string desc = username + " targeted " + myTarget;
				//LoadMoveDescription(desc);
				photonView.RPC("LoadMoveDescription", PhotonTargets.All, desc);
				Vector3 effect = gs.informationAboutSelectedCard.getEffect();
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
				photonView.RPC("LoadEffect", PhotonTargets.All, display);
				photonView.RPC ("ChangePlayerStats", PhotonTargets.All, myTarget, -1* effect);
			}
			//destroy the card/take card of it's red border
			Color c = cardToDestroy.transform.parent.GetComponent<Image> ().color;
			c.a = 0;
			cardToDestroy.transform.parent.GetComponent<Image> ().color = c;
			Destroy (cardToDestroy); //destory card selected

			eventsLeftInStage2 = eventsLeftInStage2 - 1;

			if (eventsLeftInStage2 == 0){
				eventsLeftInStage2 = 2; //reset
				GameObject.Find ("DrawButton").GetComponent<Button>().interactable = false; //addednow, so can't draw in 3rd phase

				gs.myStage = Stage.third;
				photonView.RPC ("DisplayStageCompleteRemote", PhotonTargets.All, 2);
			}
			
			return;
		}

		if (gs.myStage == Stage.third) {
			 
			if (gs.isMyTurn){ //prob unneded since possiblity of myStage being 3 is there only when ismyturn

				//make sure they are trying to play attack or
				if (cardAttr.Equals("SpecialPositive")||cardAttr.Equals("SpecialNegative")){
					sendBtn.GetComponent<Buttons>().AddLine("You cannot play specials in stage3!");
					return;//return right here since don't want to count this as an event
				}
				//now that special check passed since reached here, can display image (be it attack or portal)
				photonView.RPC("LoadImageToOtherPlayer", PhotonTargets.All, cardName);

				if (cardAttr.Equals ("Attack")) {
					string myTarget = gs.playerBeingTargeted;

					if (myTarget == ""){
						sendBtn.GetComponent<Buttons>().AddLine("Choose a target!");
						return;
					}

					//print ("person I'm targeting: " + myTarget);
					int attackVal = gs.informationAboutSelectedCard.getAttackValue();
					string display = "Attack value: " + attackVal.ToString();
					photonView.RPC("LoadEffect", PhotonTargets.All, display);

					//need to figure out who is being targeted
					//string myTarget = gs.playerBeingTargeted;
					//catch to ensure that someone is in fact being targeted

					string desc = username + " is targeting " + myTarget;
					//LoadMoveDescription(desc);
					photonView.RPC("LoadMoveDescription", PhotonTargets.All, desc);
					//add RPC call here that will result in specifically that player that's targeted to be able to interact his/her play button and counterattack 

					photonView.RPC ("activateCounterAttackMode", PhotonTargets.All, myTarget, username, (int)gs.myIdentity, attackVal);
				}

				if (cardAttr.Equals ("Portal")) {
					string display = "Effect: Go to portal";
					photonView.RPC("LoadEffect", PhotonTargets.All, display);

					string desc = username + " teleported";
					photonView.RPC("LoadMoveDescription", PhotonTargets.All, desc);

					//do the teleport
					GameObject.Find("UIScripts").GetComponent<PhotonView>().RPC("ClearAllTilesAndAdd", PhotonTargets.All, gs.myPortal);
					//now that teleported, should chat to all that teleported + RPC turn end
					sendBtn.GetComponent<PhotonView>().RPC("AddLineRemote", PhotonTargets.All, desc);

					//to handle transition to next player
					switch ((int)gs.myIdentity) {
					case (int)Player.first:
						gs.playerWhoseTurnItIsAfterAttack = (int)Player.second;
						break;
					case (int)Player.second:
						gs.playerWhoseTurnItIsAfterAttack = (int)Player.third;
						break;
					default:
						gs.playerWhoseTurnItIsAfterAttack = (int)Player.first;
						break;
					}
					photonView.RPC("StartTurnRemote", PhotonTargets.All,gs.playerWhoseTurnItIsAfterAttack);

				}

				if (!cardName.Equals("VirtualAttack")){
					Color c = cardToDestroy.transform.parent.GetComponent<Image> ().color;
					c.a = 0;
					cardToDestroy.transform.parent.GetComponent<Image> ().color = c;
					Destroy (cardToDestroy); //destory card selected 
				}

				GameObject.Find ("PlayButton").GetComponent<Button>().interactable = false;//reset
			}
		}

	}




	[RPC]
	void LoadEffect(string effect){
		GameObject.Find ("OtherCardInfo").GetComponent<Text> ().text = effect;
	}

	[RPC]
	void LoadMoveDescription(string desc){
		GameObject.Find("MoveDescription").GetComponent<Text>().text = desc;
	}

	[RPC]
	void LoadImageToOtherPlayer(string cardName){  //might as well make the method name a misnomer and load the name to other players too here

		GameObject.Find ("OtherCardName").GetComponent<Text> ().text = cardName;
		foreach (Sprite s in cardSprites){
			if (s.name.Equals(cardName.ToLower())){
				GameObject.Find("OtherCardImage").GetComponent<Image>().sprite = s;
			}
		}
	}





	void DisplayStageComplete(int stageNum){
		string stageObjectName = "Stage" + stageNum.ToString ();
		GameObject.Find (stageObjectName).GetComponent<Text> ().text = "Stage " + stageNum.ToString() + " complete";
	}

	[RPC]
	public void DisplayStageCompleteRemote(int stageNum){
		DisplayStageComplete(stageNum);
	}




	void DisplayPlayerTurn(string message){
		GameObject.Find ("PlayerWhoseTurnItIs").GetComponent<Text> ().text = message;
		GameObject.Find ("Stage1").GetComponent<Text> ().text = "Stage 1"; //resets to make sure start display at non-complete stages
		GameObject.Find ("Stage2").GetComponent<Text> ().text = "Stage 2";
	} 

	[RPC]
	void DisplayPlayerTurnRemote(string message){
		DisplayPlayerTurn (message);
	}

}
