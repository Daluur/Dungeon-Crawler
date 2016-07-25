using UnityEngine;
using System.Collections;

/// <summary>
/// Game state manager, controls entering and leaving of town/dungeon.
/// </summary>
public class GameStateManager : MonoBehaviour{

	public static GameStateManager _instance;

	public bool _inDungeon = false;
	City theTown;

	/// <summary>
	/// Makes it a singleton.
	/// </summary>
	void Awake(){
		if (_instance == null) {
			_instance = this;
		} else {
			Debug.Log ("There are 2 GameStateManagers");
		}
	}
		
	public void SetTown(City c){
		theTown = c;
	}

	public void EnterTown(){
		VisualController._instance.EnterTown ();
		theTown.EnterTown ();
	}

	public void LeaveTown(){
		VisualController._instance.LeaveTown ();
		theTown.LeaveTown ();
	}

	public void EnterDungeon(){
		VisualController._instance.EnterDungeon ();
		_inDungeon = true;
	}

	public void LeaveDungeon(){
		VisualController._instance.LeaveDungeon ();
		_inDungeon = false;
		EnterTown ();
	}
}