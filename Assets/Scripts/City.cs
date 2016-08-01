using UnityEngine;
using System.Collections;

public class City : MonoBehaviour {

	public int level = 1;
	public int length = 5;
	Player player; //I expect we need this, when one of us starts implementing the code for what dungeons appear

	// Use this for initialization
	void Start () {
		player = Player._instance;
		GameStateManager._instance.SetTown (this);
		GameStateManager._instance.EnterTown ();
	}

	//TODO check if the player unlocked things from the dungeon etc. in order to decide if new things should be visible in the town.
	public void EnterTown(){
		Player._instance.HealToFull();
	}

	public void LeaveTown(){
		
	}

	/// <summary>
	/// Enters a dungeon.
	/// </summary>
	public void EnterDungeon(int i){
		GameStateManager._instance.LeaveTown ();
		GameStateManager._instance.EnterDungeon ();
		new Dungeon ((ElementalType)i, length, player.level);
	}
}