using UnityEngine;
using System.Collections;

public class CardInfo : MonoBehaviour {

	public string CardAttribute = "Attack";

	public string CardName = "";

	GameState gs;

	void Awake(){
		gs = GameObject.Find ("GameState").GetComponent<GameState> ();
		CardName = gameObject.tag;
	}





	public int getAttackValue(){
		int rtn = 0;
		switch(gameObject.tag){
			case "VirtualAttack":
				rtn = gs.It + gs.Ht + gs.Cf;
			break;
			case "CurveSet":
				rtn = 30;
				break;
			case "HiddenPassion":
				rtn = 25 + gs.Ht + gs.Cf;
				break;
			case "Flowers":
				rtn = 20 + gs.Ht + gs.Cf;
				break;
			default:
					break;
		}
		return rtn;
	}


	public Vector3 getEffect(){
		Vector3 rtn = new Vector3 (0, 0, 0); //or vector3.zero basically you'll have a triple (IT,HT,CF)
		switch(gameObject.tag){
		case "Adderall":
			rtn.x = 5;
			break;
		case "CaffeinePill":
			rtn.x = 5;
			break;
		case "Hairdo":
			rtn.z = 3;
			break;
		case "Steroids":
			rtn.y = 5;
			break;
		case "WallflowerBloom":
			rtn.z = 10;
			break;
		case "Workout":
			rtn.y = 3;
			rtn.z = 3;
			break;
		case "Drunk":
			rtn.x = 5;  //Drunk does -5 IT.  The card is "SpecialNegative" so that accounts for the (-)
			break;
		case "Intimidate":
			rtn.z = 5;
			break;
		case "TrashTalk":
			rtn.y = 5;
			break;
		default:
			break;
		}
		return rtn;
			
	}

}
