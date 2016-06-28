using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int level;
	public int health;
	bool myturn = false;

	public bool TakeDamage(DamagePackage dp){
		health -= dp.damage;
		Debug.Log ("Player took: " + dp.damage + " damage");
		if (health <= 0) {
			Debug.Log ("Player is dead");
			return true;
		}
		return false;
	}

	public void DoDmg(){
		if (myturn) {
			myturn = false;
			Debug.Log ("Player dealt damage!");
			CombatController._instance.AttackEnemy (new DamagePackage (1, 25));
		}
	}

	public void MyTurn(){
		Debug.Log ("Players turn!");
		myturn = true;
	}
}