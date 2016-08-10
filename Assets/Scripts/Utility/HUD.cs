using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

	public Text HUDText;
	public static HUD _instance;

	/// <summary>
	/// Makes it a singleton.
	/// </summary>
	void Awake () {
		if (_instance == null) {
			_instance = this;
		} else {
			Debug.Log ("There are 2 HUDs in the scene");
		}
	}

	void Start(){
		UpdateHUD ();
	}
	
	public void UpdateHUD(){
		HUDText.text = "Level: " + Player._instance.level + "\t\tXP: X" /*+ Player._instance.XPforLevel*/+"/" + Player._instance.XPforLevel + "\t\tGold: " + Player._instance.gold + "\t\tRubies: X" /*+ Player._instance.rubies*/;
	}
}
