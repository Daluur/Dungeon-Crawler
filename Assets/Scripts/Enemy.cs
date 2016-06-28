using UnityEngine;
using System.Collections;

public class Enemy {

	//Which type, e.g. fire, water, desert w/e.
	int type;
	//Its level.
	int level;
	//Its hp.
	int health;
	//Its attack power.
	int AP;

	/// <summary>
	/// Initializes a new instance of the <see cref="Enemy"/> class.
	/// </summary>
	/// <param name="newType">New type.</param>
	/// <param name="newLevel">New level.</param>
	/// <param name="number">Number.</param>
	public Enemy(int newType, int newLevel, int number){
		type = newType;
		level = newLevel;
		health = level * 100;
		AP = level + number;
	}

	/// <summary>
	/// Takes the damage.
	/// </summary>
	/// <returns><c>true</c>, if dead, <c>false</c> otherwise.</returns>
	/// <param name="dp">Dp.</param>
	public bool TakeDamage(DamagePackage dp){
		health -= dp.damage;
		Debug.Log ("Enemy took: " + dp.damage + " damage");
		if (health <= 0) {
			Debug.Log ("Enemy is dead");
			return true;
		}
		return false;
	}

	/// <summary>
	/// My turn.
	/// </summary>
	public void MyTurn(){
		//TODO: should have a simple AI for deciding what to do.
		Debug.Log ("Enemy dealt damage!");
		CombatController._instance.AttackPlayer (new DamagePackage (1, 10));
	}
}