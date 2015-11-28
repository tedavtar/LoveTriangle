using UnityEngine;
using System.Collections;

public class CardSelected : MonoBehaviour {

	public GameObject border;
	public bool selected;

	public GameObject cardSelected;

	void Awake () {
		selected = false;
	}

	public void setBorder(GameObject border){
		this.border = border;
	}

}
