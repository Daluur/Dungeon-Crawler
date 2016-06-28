using UnityEngine;
using System.Collections;

public class CombatController : MonoBehaviour {

	public static CombatController _instance;

	Dungeon currentDungeon;
	Enemy currentEnemy;
	public Player player;

	// Use this for initialization
	void Awake () {
		if (_instance == null) {
			_instance = this;
		} else {
			Debug.Log ("There are 2 combat controllers in the scene");
		}
	}

	public void NewDungeon(Dungeon newD){
		currentDungeon = newD;
	}

	public void NewEnemy(Enemy newE){
		currentEnemy = newE;
		Debug.Log ("has an enemy");
		player.MyTurn ();
	}
	
	public void AttackEnemy(DamagePackage dp){
		if (currentEnemy.TakeDamage (dp)) {
			Debug.Log ("Enemey Died!");
			currentDungeon.NextEncounter ();
		} else {
			currentEnemy.MyTurn ();
		}
	}

	public void AttackPlayer(DamagePackage dp){
		if (player.TakeDamage (dp)) {
			Debug.Log ("Player died!");
		} else {
			player.MyTurn ();
		}
	}
}