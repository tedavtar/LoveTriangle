using UnityEngine;
using System.Collections;
using Parse;
using UnityEngine.UI;

public class ParseDataManage : MonoBehaviour {
		//many disclaimers. idk how to do dis stuff.  worst practices here guyzzz  zyzzz
		//prob shuold have used the /* thing but oh well. K so ideally want to put all d UI buttons as prefabs and instantiate them.  However, I used Framer and well no disable or render visibility so my lazy workaround will be to set scale to zero
		//the parseUser thing for parseUser on the password is not working correctly--so I'll be super insecurity/stuff I don't know about--will just get the job done
	GameObject message;

	GameObject nameField;
	GameObject passField;

	GameObject login1;
	GameObject signup1;

	GameObject login;
	GameObject signup;

	public string validUsername = "";

	public string username;
	public string password;

	int maxUsernameLength = 7;
	int minUsernameLength = 3;

	public bool invalid = false;
	public bool validCredentials = false;

	public bool canProceed = true; //This switch is CRITICAL--basically I was continously loading next level-photon was complaining cuz alredy connected

	void Awake(){
		DontDestroyOnLoad(gameObject);  //want to make validUsername persist
	}

	void Update(){
		if (validUsername.Length > 0 && canProceed) {
			canProceed = false;
			Application.LoadLevel("Game");
		}
	}

	void Start(){
		validCredentials = false;
		message = GameObject.Find ("Message");

		nameField = GameObject.Find ("Username");
		passField = GameObject.Find ("Password");

		login1 = GameObject.Find ("Login1");
		signup1 = GameObject.Find ("SignUp1");

		login = GameObject.Find ("Login");
		signup = GameObject.Find ("SignUp");
	}

	public void setMessage(string s){
		message.GetComponent<Text> ().text = s;
	}


	public void attemptLogin(){
		ParseObject retrieved = null;

		username = nameField.GetComponent<InputField> ().text;
		password = passField.GetComponent<InputField> ().text;

		var query = ParseObject.GetQuery ("Account").WhereEqualTo ("username", username).WhereEqualTo("password",password);

		query.FirstAsync().ContinueWith(t => {
			//print (t.IsFaulted); ///HAHA so that's what happens when error

			//see i'm setting boolean flags because this is non UI thread, so there blocking/won't let me set messages text, so I use a script attached to message and use it's update w/ these boolean flags to appropriately set message
			if (t.IsFaulted|| !t.IsCompleted) {
				invalid = true;
				return;
			} else {
				validCredentials = true;

			}
			retrieved = t.Result;
			validUsername = retrieved.Get<string> ("username");
		});

		

	}

	public void attemptSignUp(){
		username = nameField.GetComponent<InputField> ().text;
		password = passField.GetComponent<InputField> ().text;

		if (username.Equals ("")) {
			setMessage("No empty usernames!");
			return;
		}
		if (username.Length > maxUsernameLength || username.Length < minUsernameLength) {
			setMessage("Names must be " + minUsernameLength + "-" + maxUsernameLength + " letters!" );
			return;
		}

		ParseObject account = new ParseObject("Account");
		account ["username"] = username;
		account ["password"] = password;
		account.SaveAsync ();
		setMessage ("Success!");

		//K so once signed in, want to let them login.
		//should make signup disappear
		Vector3 sscale1 = signup.GetComponent<RectTransform> ().localScale;
		sscale1.x = 0;
		signup.GetComponent<RectTransform> ().localScale = sscale1;
		displayLogin ();
	}







	public void makeFieldsVisible(){
		Vector3 nscale = nameField.GetComponent<RectTransform> ().localScale;
		nscale.x = 1;
		nameField.GetComponent<RectTransform> ().localScale = nscale;
		
		
		Vector3 pscale = passField.GetComponent<RectTransform> ().localScale;
		pscale.x = 1;
		passField.GetComponent<RectTransform> ().localScale = pscale;
	}

	public void displayLogin(){
		Vector3 lscale = login1.GetComponent<RectTransform> ().localScale;
		lscale.x = 0;
		login1.GetComponent<RectTransform> ().localScale = lscale;


		Vector3 sscale = signup1.GetComponent<RectTransform> ().localScale;
		sscale.x = 0;
		signup1.GetComponent<RectTransform> ().localScale = sscale;

		makeFieldsVisible ();

		Vector3 lscale1 = login.GetComponent<RectTransform> ().localScale;
		lscale1.x = 1;
		login.GetComponent<RectTransform> ().localScale = lscale1;
	}

	public void displaySignUp(){
		Vector3 lscale = login1.GetComponent<RectTransform> ().localScale;
		lscale.x = 0;
		login1.GetComponent<RectTransform> ().localScale = lscale;
		
		
		Vector3 sscale = signup1.GetComponent<RectTransform> ().localScale;
		sscale.x = 0;
		signup1.GetComponent<RectTransform> ().localScale = sscale;
		
		makeFieldsVisible ();
		
		Vector3 sscale1 = signup.GetComponent<RectTransform> ().localScale;
		sscale1.x = 1;
		signup.GetComponent<RectTransform> ().localScale = sscale1;
	}


}
