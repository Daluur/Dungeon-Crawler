using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the flow of combat.
/// </summary>
public class CombatController : MonoBehaviour {

	public static CombatController _instance;

	Dungeon currentDungeon;
	Enemy currentEnemy;
	public Player player;

	/// <summary>
	/// Makes it a singleton.
	/// </summary>
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

	/// <summary>
	/// Creates a new enemy, gives the player the turn.
	/// </summary>
	/// <param name="newE">New e.</param>
	public void NewEnemy(Enemy newE){
		currentEnemy = newE;
		Debug.Log ("A new Enemy as appeared");
		player.MyTurn ();
	}

	/// <summary>
	/// Attacks the enemy.
	/// </summary>
	/// <param name="dp">Dp.</param>
	public void AttackEnemy(DamagePackage dp){
		if (currentEnemy.TakeDamage (dp)) {
			Debug.Log ("Enemey Died!");
			VisualController._instance.RemoveEnemyVisual ();
			player.RemoveEffectsOnEnemy ();
			currentDungeon.NextEncounter ();
		} else {
			//Says it is the enemies turn.
			currentEnemy.MyTurn (); 
		}
	}

	public void EffectEnemy(DamagePackage dp){
		if (currentEnemy.TakeDamage (dp)) {
			Debug.Log ("Enemey Died!");
			VisualController._instance.RemoveEnemyVisual ();
			player.RemoveEffectsOnEnemy ();
			currentDungeon.NextEncounter ();
		}
	}

	/// <summary>
	/// Heals the Enemy
	/// </summary>
	/// <param name="hp">Hp.</param>
	public void HealEnemy(DamagePackage hp){
		currentEnemy.HealUp (hp);

		//Says it is the players turn.
		player.MyTurn ();
	}

	/// <summary>
	/// Attacks the Player.
	/// </summary>
	/// <param name="dp">Dp.</param>
	public void AttackPlayer(DamagePackage dp){
		if (player.TakeDamage (dp)) {
			Debug.Log ("Player died!");
		} else {
			//Says it is the players turn.
			player.MyTurn ();
		}
	}

	/// <summary>
	/// Heals the Player
	/// </summary>
	/// <param name="hp">Hp.</param>
	public void HealPlayer(DamagePackage hp){
		player.HealUp (hp);

		//Says it is the enemies turn.
		currentEnemy.MyTurn ();
	}

	public void EffectPlayer(DamagePackage hp) {
		player.HealUp (hp);
	}

	/// <summary>
	/// Starts the next encounter.
	/// </summary>
	public void NextEncounter(){
		VisualController._instance.RemoveNextEncounterButton ();
		currentDungeon.NextEncounter ();
	}

	/// <summary>
	/// Gives loot to the player.
	/// </summary>
	public void DoLoot(){
		VisualController._instance.RemoveLootButton ();
		Debug.Log ("Player recieved some loot (NYI)");
		VisualController._instance.ShowNextEncounterButton ();
	}
}