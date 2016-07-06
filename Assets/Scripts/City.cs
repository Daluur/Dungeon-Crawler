using UnityEngine;
using System.Collections;

public class City : MonoBehaviour {

	Dungeon testDungeon;

	// Use this for initialization
	void Start () {
		GameStateManager._instance.SetTown (this);
		GameStateManager._instance.EnterTown ();
	}

	void EnterTestDungeon(){
		Debug.Log ("started a new dungeon");
		testDungeon = new Dungeon (1,5,1);
	}

	//TODO check if the player unlocked things from the dungeon etc. in order to decide if new things should be visible in the town.
	public void EnterTown(){
		//TODO fully heal the player.
	}

	public void LeaveTown(){
		
	}

	/// <summary>
	/// Enters the dungeon.
	/// </summary>
	//TODO: needs to be able to specify which dungeon.
	public void EnterDungeon(){
		GameStateManager._instance.LeaveTown ();
		GameStateManager._instance.EnterDungeon ();
		EnterTestDungeon ();
	}
}