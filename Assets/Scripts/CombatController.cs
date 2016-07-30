using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the flow of combat.
/// </summary>
public class CombatController : MonoBehaviour {

	public static CombatController _instance;

	Dungeon currentDungeon;
	public Enemy currentEnemy;
	public Player player;
	bool playersTurn = true;
	bool waitingToFinishAnimations = false;

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
		CombatText._instance.ShowInfo ("Your turn", InfoType.Unskippable);
		player.MyTurn ();
	}

	/// <summary>
	/// Attacks the enemy.
	/// </summary>
	/// <param name="dp">Dp.</param>
	public void AttackEnemy(DamagePackage dp){
		if (currentEnemy.TakeDamage (ref dp)) {
			Debug.Log ("Enemey Died!");
			VisualController._instance.RemoveEnemyVisual ();
			currentDungeon.NextEncounter ();
		} else {
			//Says it is the enemies turn.
			currentEnemy.MyTurn (); 
		}
	}

	/// <summary>
	/// Enemy attacks itself.
	/// </summary>
	/// <param name="dp">Dp.</param>
	public void EnemySelfDamage(DamagePackage dp){
		if (currentEnemy.TakeDamage (ref dp)) {
			Debug.Log ("Enemey Died!");
			VisualController._instance.RemoveEnemyVisual ();
			currentDungeon.NextEncounter ();
		} else {
<<<<<<< HEAD
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
				//CombatText._instance.ShowInfo("Enemies turn!");
				TryEndTurn();
				//currentEnemy.MyTurn ();
			}
=======
			//Says it is the enemies turn.
			player.MyTurn (); 
		}
	}

	/// <summary>
	/// Effect attack on Enemy.
	/// </summary>
	/// <param name="dp">Dp.</param>
	public void EffectAttackEnemy(DamagePackage dp){
		if (currentEnemy.TakeDamage (ref dp)) {
			Debug.Log ("Enemey Died!");
			VisualController._instance.RemoveEnemyVisual ();
			currentDungeon.NextEncounter ();
>>>>>>> refs/remotes/origin/TheQuibbler
		}
	}

	/// <summary>
	/// Heals the Enemy
	/// </summary>
	/// <param name="hp">Hp.</param>
	public void HealEnemy(DamagePackage hp){
		currentEnemy.HealUp (ref hp);
		//Says it is the players turn.
		player.MyTurn ();
	}

	/// <summary>
	/// Effect heal on Enemy
	/// </summary>
	/// <param name="hp">Hp.</param>
	public void EffectHealEnemy(DamagePackage hp){
		currentEnemy.HealUp (ref hp);
	}

	/// <summary>
	/// Attacks the Player.
	/// </summary>
	/// <param name="dp">Dp.</param>
	public void AttackPlayer(DamagePackage dp){
		if (player.TakeDamage (ref dp)) {
			Debug.Log ("Player died!");
		} else {

			TryEndTurn ();
			//player.MyTurn ();
		}
	}

	/// <summary>
	/// Player attacks itself
	/// </summary>
	/// <param name="dp">Dp.</param>
	public void PlayerSelfDamage(DamagePackage dp){
		if (player.TakeDamage (ref dp)) {
			Debug.Log ("Player died!");
		} else {
<<<<<<< HEAD
			player.HealHoT ();
			if (player.TakeDoTDamage ()) {
				Debug.Log ("Player died!");
			} else {
				if (dp.isOT) {
					player.addDoT (dp);
				}
				//Says it is the players turn.
				//CombatText._instance.ShowInfo("Your turn!");
				TryEndTurn();
				//player.MyTurn ();
			}
=======
			//Says it is the players turn.
			currentEnemy.MyTurn ();
		}
	}

	/// <summary>
	/// Effect attack on player
	/// </summary>
	/// <param name="dp">Dp.</param>
	public void EffectAttackPlayer(DamagePackage dp) {
		if (player.TakeDamage (ref dp)) {
			Debug.Log ("Player died!");
>>>>>>> refs/remotes/origin/TheQuibbler
		}
	}

	/// <summary>
	/// Heals the Player
	/// </summary>
	/// <param name="hp">Hp.</param>
	public void HealPlayer(DamagePackage hp){
<<<<<<< HEAD
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
			TryEndTurn ();
			//currentEnemy.MyTurn ();
		}
=======
		player.HealUp (ref hp);

		//Says it is the enemies turn.
		currentEnemy.MyTurn ();
>>>>>>> refs/remotes/origin/TheQuibbler
	}

	/// <summary>
	/// Effect heal on player
	/// </summary>
	/// <param name="hp">Hp.</param>
	public void EffectHealPlayer(DamagePackage hp){
		player.HealUp (ref hp);
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

	/// <summary>
	/// Tries to end turn, if animations are playing, it can't.
	/// </summary>
	void TryEndTurn(){
		if (CombatText._instance.IsPlayingAnimation ()) {
			waitingToFinishAnimations = true;
		} else {
			if (playersTurn) {
				//Says it is the enemies turn.
				CombatText._instance.ShowInfo("Enemies turn!",InfoType.Unskippable);
				currentEnemy.MyTurn ();
			} else {
				//Says it is the players turn.
				CombatText._instance.ShowInfo("Your turn!",InfoType.Unskippable);
				player.MyTurn ();
			}
			playersTurn = !playersTurn;
		}
	}

	/// <summary>
	/// Animations are finished.
	/// </summary>
	public void FinishedAnimations(){
		//If it were waiting for animations, end the turn.
		if (waitingToFinishAnimations) {
			waitingToFinishAnimations = false;
			//Invoke("TryEndTurn",1f);
			TryEndTurn();
		}
	}
}