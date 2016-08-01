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
		if (currentEnemy.TakeDamage (dp)) {
			Debug.Log ("Enemey Died!");
			VisualController._instance.RemoveEnemyVisual ();
			currentDungeon.NextEncounter ();
		} else {
			//Says it is the enemies turn.
			TryEndTurn();
		}
	}

	/// <summary>
	/// Enemy attacks itself.
	/// </summary>
	/// <param name="dp">Dp.</param>
	public void EnemySelfDamage(DamagePackage dp){
		if (currentEnemy.TakeDamage (dp)) {
			Debug.Log ("Enemey Died!");
			VisualController._instance.RemoveEnemyVisual ();
			currentDungeon.NextEncounter ();
		} else {
			TryEndTurn();
		}
	}

	/// <summary>
	/// Effect attack on Enemy.
	/// </summary>
	/// <param name="dp">Dp.</param>
	public void EffectAttackEnemy(DamagePackage dp){
		if (currentEnemy.TakeDamage (dp)) {
			Debug.Log ("Enemey Died!");
			VisualController._instance.RemoveEnemyVisual ();
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
		TryEndTurn();
	}

	/// <summary>
	/// Effect heal on Enemy
	/// </summary>
	/// <param name="hp">Hp.</param>
	public void EffectHealEnemy(DamagePackage hp){
		currentEnemy.HealUp (hp);
	}

	/// <summary>
	/// Attacks the Player.
	/// </summary>
	/// <param name="dp">Dp.</param>
	public void AttackPlayer(DamagePackage dp){
		if (player.TakeDamage (dp)) {
			Debug.Log ("Player died!");
		} else {
			TryEndTurn ();
		}
	}

	/// <summary>
	/// Player attacks itself
	/// </summary>
	/// <param name="dp">Dp.</param>
	public void PlayerSelfDamage(DamagePackage dp){
		if (player.TakeDamage (dp)) {
			Debug.Log ("Player died!");
		} else {
			//Says it is the players turn.
			TryEndTurn();
		}
	}

	/// <summary>
	/// Effect attack on player
	/// </summary>
	/// <param name="dp">Dp.</param>
	public void EffectAttackPlayer(DamagePackage dp) {
		if (player.TakeDamage (dp)) {
			Debug.Log ("Player died!");
		}
	}

	/// <summary>
	/// Heals the Player
	/// </summary>
	/// <param name="hp">Hp.</param>
	public void HealPlayer(DamagePackage hp){
		player.HealUp (hp);
		//Says it is the enemies turn.
		TryEndTurn();
	}

	/// <summary>
	/// Effect heal on player
	/// </summary>
	/// <param name="hp">Hp.</param>
	public void EffectHealPlayer(DamagePackage hp){
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
		Debug.Log ("Player recieved Two Rubies per level of the dungeon");
		Player._instance.AddRubies (currentDungeon.level);
		//Leaves the dungeon.
		Debug.Log ("Dungeon is finished");
		GameStateManager._instance.LeaveDungeon ();
	}

	/// <summary>
	/// Tries to end turn, if animations are playing, it can't.
	/// </summary>
	public void TryEndTurn(){
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
			TryEndTurn();
		}
	}
}