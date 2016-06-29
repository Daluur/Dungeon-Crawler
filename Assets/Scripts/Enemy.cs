using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Enemy {

	//Which type, e.g. fire, water, desert w/e.
	int type;
	//Its level.
	int level;
	//Its hp.
	int health;
	//Its attack power.
	int AP;
	//Its active abilities
	List<Skill> abilties;

	System.Random rnd = new System.Random();


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
		abilties = new List<Skill>();
		addAbilities();
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
		//TODO: Need simple AI to pick attacks.
		Skill attack = abilties[rnd.Next(0,3)];

		Debug.Log ("Enemy used " + attack.name + "!");
		CombatController._instance.AttackPlayer (attack.CalDmg(AP));
	}

	/// <summary>
	/// Decrement the CD for all Skills
	/// </summary>
	public void updateCD() {
		foreach (Skill attack in abilties) {
			attack.updateCD();
		}
	}

	//Temporary solution, until we get another way to keep abilties.
	private void addAbilities(){
		abilties.Add (new Skill ("Swarm of Butterflies", 1, 1, 0));

		abilties.Add (new Skill ("Elephant Stampede", 1, 2, 2));

		abilties.Add (new Skill ("Flock of Cows", 1, 1, 1));
	}
}