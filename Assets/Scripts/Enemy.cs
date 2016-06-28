using UnityEngine;
using System.Collections;

public class Enemy {

	int type;
	int level;
	int health;
	int AP;

	public Enemy(int newType, int newLevel, int number){
		type = newType;
		level = newLevel;
		health = level * 100;
		AP = level + number;
	}

	public bool TakeDamage(DamagePackage dp){
		health -= dp.damage;
		Debug.Log ("Enemy took: " + dp.damage + " damage");
		if (health <= 0) {
			Debug.Log ("Enemy is dead");
			return true;
		}
		return false;
	}

	public void MyTurn(){
		Debug.Log ("Enemy dealt damage!");
		CombatController._instance.AttackPlayer (new DamagePackage (1, 10));
	}
}