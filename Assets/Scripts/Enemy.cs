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
	List<Skill> abilties = new List<Skill>();

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
			return true;
		}
		return false;
	}

	/// <summary>
	/// Heals the Enemy
	/// </summary>
	/// <param name="hp">Hp.</param>
	public void HealUp(HealingPackage hp){
		health += hp.healing;
		Debug.Log ("Enemy recieved: " + hp.healing + " health");
	}

	/// <summary>
	/// My turn.
	/// </summary>
	public void MyTurn(){
		//TODO: Need simple AI to pick attacks.
		Skill ability = abilties[rnd.Next(0,3)];

		Debug.Log ("Enemy used " + ability.name + "!");
		if (ability.isSelfTarget ()) {
			CombatController._instance.HealEnemy (ability.CalHeal (AP));
		}
		else {
			CombatController._instance.AttackPlayer (ability.CalDmg(AP));
		}
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
		abilties.Add (new Skill ("Swarm of Butterflies", false, 1, 10, 0));

		abilties.Add (new Skill ("Elephant Stampede", false, 1, 20, 2));

		abilties.Add (new Skill ("Flock of Cows", true, 1, 10, 0));
	}
}