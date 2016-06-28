using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	//Players level
	public int level;
	//Players current health.
	public int health;
	//Whether or not it is the players turn.
	bool myTurn = false;

	/// <summary>
	/// Takes the damage.
	/// </summary>
	/// <returns><c>true</c>, if dead, <c>false</c> otherwise.</returns>
	/// <param name="dp">Dp.</param>
	public bool TakeDamage(DamagePackage dp){
		health -= dp.damage;
		Debug.Log ("Player took: " + dp.damage + " damage");
		if (health <= 0) {
			Debug.Log ("Player is dead");
			return true;
		}
		return false;
	}

	/// <summary>
	/// Does the dmg.
	/// </summary>
	public void DoDmg(){
		if (myTurn) {
			myTurn = false;
			Debug.Log ("Player dealt damage!");
			CombatController._instance.AttackEnemy (new DamagePackage (1, 25));
		}
	}

	/// <summary>
	/// Allows the player to perform his turn.
	/// </summary>
	public void MyTurn(){
		Debug.Log ("Players turn!");
		myTurn = true;
	}
}