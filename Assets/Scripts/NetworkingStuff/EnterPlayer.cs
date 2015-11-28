using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

//mostly copying from fragmas/marco polo tut
public class EnterPlayer : Photon.MonoBehaviour {

	GameObject rmList;
	public GameObject roomRow;
	GameState gs;

	/*
	void OnLevelWasLoaded(int level) {
		if (level == 1)
			print("Woohoo");
		
	}*/

	// Use this for initialization
	void Start () {
		//print ("called once");
		gs = GameObject.Find ("GameState").GetComponent<GameState> ();
		rmList = GameObject.Find ("RoomList");
		if (!PhotonNetwork.connected){
			PhotonNetwork.ConnectUsingSettings("0.1");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}

	void OnJoinedLobby(){}

	public void makeRoom(){
		GameObject rname = GameObject.Find ("CreateName");
		string roomName = rname.GetComponent<InputField> ().text;

		GameObject ma = GameObject.Find ("CreateMatrix");
		string mat = ma.GetComponent<InputField> ().text;
		//print (mat);
		int matrix = int.Parse (mat);

		GameObject f = GameObject.Find ("CreateFlagella");
		string fl = f.GetComponent<InputField> ().text;
		int flagella = int.Parse (fl);

		GameObject m = GameObject.Find ("CreateMembrane");
		string me = m.GetComponent<InputField> ().text;
		int membrane = int.Parse (me);

		string[] roomPropsInLobby = { "matrix", "membrane", "flagella" };
		Hashtable customRoomProperties = new Hashtable() { { "matrix", matrix },{ "membrane", membrane },{ "flagella", flagella } };
		//change board size here
		GameObject.Find ("UIScripts").GetComponent<LoadBoard> ().setUpBoard (matrix,membrane,flagella);
		PhotonNetwork.CreateRoom (roomName, true, true, 3, customRoomProperties, roomPropsInLobby);
	}




	void OnReceivedRoomListUpdate(){
		foreach (Transform child in rmList.GetComponent<Transform>()) { //refresh list => deleting then adding (to avoid repeats!)
			GameObject.Destroy(child.gameObject);
		}

		foreach (RoomInfo room in PhotonNetwork.GetRoomList()) {

			Hashtable roomProps = room.customProperties;
			int matrix = (int)roomProps["matrix"];
			int membrane = (int)roomProps["membrane"];
			int flagella = (int)roomProps["flagella"];
			string matrix1 = matrix.ToString();
			string membrane1 = membrane.ToString();
			string flagella1 = flagella.ToString();
			string boardSize = "";
			boardSize += matrix1;
			boardSize += ", ";
			boardSize += membrane1;
			boardSize += ", ";
			boardSize += flagella1;
			GameObject newRow = Instantiate (roomRow) as GameObject;
			newRow.transform.SetParent (rmList.transform);
			newRow.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
			newRow.GetComponent<Transform> ().GetChild(0).GetComponent<Text>().text = room.name;
			newRow.GetComponent<Transform> ().GetChild(1).GetComponent<Text>().text = room.playerCount.ToString() + "/3";
			newRow.GetComponent<Transform> ().GetChild(2).GetComponent<Text>().text = boardSize;
			newRow.GetComponent<Transform> ().GetChild(2).GetComponent<boardsize>().matrix = matrix;
			newRow.GetComponent<Transform> ().GetChild(2).GetComponent<boardsize>().flagella = flagella;
			newRow.GetComponent<Transform> ().GetChild(2).GetComponent<boardsize>().membrane = membrane;

			//add a check to disable click action of joining room when room already has three players + grey it out
			if (room.playerCount == 3){
				newRow.GetComponent<Room1>().greyOutRow();
				newRow.GetComponent<Room1>().enabled = false; //now get rid of functionality when clicked
			}
		}

	}
	[RPC]
	void SetPlayerInfoBoxName(int player, string name){
		switch (player) {
		case (int)Player.first:
			GameObject.Find("Player1Name").GetComponent<Text>().text = name;
			break;
		case (int)Player.second:
			GameObject.Find("Player2Name").GetComponent<Text>().text = name;
			break;
		default:
			GameObject.Find("Player3Name").GetComponent<Text>().text = name;
			break;
		}
	}

	void OnJoinedRoom(){
		GameObject.Find ("RoomManager").SetActive (false);
		int numPlayers = PhotonNetwork.playerList.Length;
		string username = GameObject.Find ("ParseLogin").GetComponent<ParseDataManage> ().validUsername;
		GameObject sendBtn = GameObject.Find ("SendButton");
		string joinMessage = username + " has entered the room.";
		switch (numPlayers) {
		case 1:
			gs.myIdentity = Player.first;
			gs.isMyTurn = false;
			gs.myName = username;

			gs.myPortal = "P1";
			gs.myHome = "F11";

			//the all buffered means that the rpc will be relayed to later clients--exactly needed
			gs.GetComponent<PhotonView>().RPC("registerPlayer", PhotonTargets.AllBuffered,(int)Player.first,username);

			sendBtn.GetComponent<PhotonView>().RPC("AddLineRemote", PhotonTargets.AllBuffered, joinMessage);

			GameObject.Find("Player2InfoZone").GetComponent<LinkWithGameState>().enabled = false;
			GameObject.Find("Player3InfoZone").GetComponent<LinkWithGameState>().enabled = false;
			photonView.RPC("SetPlayerInfoBoxName", PhotonTargets.AllBuffered, (int)Player.first, username);
			break;
		case 2:
			gs.myIdentity = Player.second;
			gs.isMyTurn = false;
			gs.myName = username;

			gs.myPortal = "P2";
			gs.myHome = "F21";

			gs.GetComponent<PhotonView>().RPC("registerPlayer", PhotonTargets.AllBuffered,(int)Player.second,username);
			sendBtn.GetComponent<PhotonView>().RPC("AddLineRemote", PhotonTargets.AllBuffered, joinMessage);

			GameObject.Find("Player1InfoZone").GetComponent<LinkWithGameState>().enabled = false;
			GameObject.Find("Player3InfoZone").GetComponent<LinkWithGameState>().enabled = false;
			photonView.RPC("SetPlayerInfoBoxName", PhotonTargets.AllBuffered, (int)Player.second, username);
			break;
		case 3:
			gs.myIdentity = Player.third;
			gs.isMyTurn = false;
			gs.myName = username;

			gs.myPortal = "P3";
			gs.myHome = "F31";

			gs.GetComponent<PhotonView>().RPC("registerPlayer", PhotonTargets.AllBuffered,(int)Player.third,username);
			sendBtn.GetComponent<PhotonView>().RPC("AddLineRemote", PhotonTargets.AllBuffered, joinMessage);

			GameObject.Find("Player2InfoZone").GetComponent<LinkWithGameState>().enabled = false;
			GameObject.Find("Player1InfoZone").GetComponent<LinkWithGameState>().enabled = false;

			photonView.RPC("SetPlayerInfoBoxName", PhotonTargets.AllBuffered, (int)Player.third, username);
			//some initialization here
			//start first turn
			photonView.RPC("StartTurnRemote", PhotonTargets.All,(int)Player.first);


			break;
			
		}
	}

}
