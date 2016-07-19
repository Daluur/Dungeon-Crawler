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
			currentDungeon.NextEncounter ();
		} else {
			currentEnemy.HealHoT ();
			if (currentEnemy.TakeDoTDamage ()) {
				Debug.Log ("Enemey Died!");
				VisualController._instance.RemoveEnemyVisual ();
				currentDungeon.NextEncounter ();
			} else {
				if (dp.isOT) {
					currentEnemy.addDoT (dp);
				}
				//Says it is the enemies turn.
				currentEnemy.MyTurn (); 
			}
		}
	}

	/// <summary>
	/// Heals the Enemy
	/// </summary>
	/// <param name="hp">Hp.</param>
	public void HealEnemy(DamagePackage hp){
		currentEnemy.HealUp (hp);
		currentEnemy.HealHoT ();
		if (hp.isOT) {
			currentEnemy.addHoT (hp);
		}
		if (player.TakeDoTDamage ()) {
			Debug.Log ("Player died!");
		} else {
			//Says it is the players turn.
			player.MyTurn ();
		}
	}

	/// <summary>
	/// Attacks the Player.
	/// </summary>
	/// <param name="dp">Dp.</param>
	public void AttackPlayer(DamagePackage dp){
		if (player.TakeDamage (dp)) {
			Debug.Log ("Player died!");
		} else {
			player.HealHoT ();
			if (player.TakeDoTDamage ()) {
				Debug.Log ("Player died!");
			} else {
				if (dp.isOT) {
					player.addDoT (dp);
				}
				//Says it is the players turn.
				player.MyTurn ();
			}
		}
	}

	/// <summary>
	/// Heals the Player
	/// </summary>
	/// <param name="hp">Hp.</param>
	public void HealPlayer(DamagePackage hp){
		player.HealUp (hp);
		player.HealHoT ();
		if (hp.isOT) {
			player.addHoT (hp);
		}
		if (currentEnemy.TakeDoTDamage ()) {
			Debug.Log ("Enemy died!");
			VisualController._instance.RemoveEnemyVisual ();
			currentDungeon.NextEncounter ();
		} else {
			//Says it is the enemies turn.
			currentEnemy.MyTurn ();
		}
	}

}