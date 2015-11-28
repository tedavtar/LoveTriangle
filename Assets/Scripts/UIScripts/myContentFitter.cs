using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//well not "mine" copied from unity forums but made it custom in that added check to only alter height if the height of the text is over it's container (messages) threshold. or else the parent coerces its rect transform values/position and shifts it/weird stuff--only gurantee for scroll to work properly is when content is wider than container which this fix does!
public class myContentFitter : MonoBehaviour {

	RectTransform rt;
	Text txt;
	void Start () {
		rt = gameObject.GetComponent<RectTransform>(); // Acessing the RectTransform
		txt = gameObject.GetComponent<Text>(); // Accessing the text component
	}
	void Update () {
		if (txt.preferredHeight > 175.5){
			rt.sizeDelta = new Vector2(rt.rect.width, txt.preferredHeight); // Setting the height to equal the height of text
		}
	}
}
