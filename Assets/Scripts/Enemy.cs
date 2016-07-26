using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Enemy {

	//Which type, e.g. fire, water, desert w/e.
	ElementalType type;
	//Its level.
	int level;
	//Its hp.
	int health;

	float damageReduction;

	int armor;

	//Its attack power.
	int AP;

	float critChance;
	//Its active abilities
	List<Skill> abilties = new List<Skill>();
	List<DamagePackage> DoTS = new List<DamagePackage>();
	List<DamagePackage> HoTS = new List<DamagePackage>();

	System.Random rnd = new System.Random();


	/// <summary>
	/// Initializes a new instance of the <see cref="Enemy"/> class.
	/// </summary>
	/// <param name="newType">New type.</param>
	/// <param name="newLevel">New level.</param>
	/// <param name="number">Number.</param>
	public Enemy(ElementalType newType, int newLevel, int number){
		type = newType;
		level = newLevel;
		health = level * 1000;
		damageReduction = 11.0F;
		armor = 0;
		critChance = level;
		//Instansiate healthbar with max hp.
		VisualController._instance.CreateEnemyHealthbar (health);
		AP = level + number;
		addAbilities();
	}

	/// <summary>
	/// Takes the damage.
	/// </summary>
	/// <returns><c>true</c>, if dead, <c>false</c> otherwise.</returns>
	/// <param name="dp">Dp.</param>
	public bool TakeDamage(DamagePackage dp){
		dp.DamageReduction (damageReduction);
		health -= (int)Math.Floor(dp.damage);
		Debug.Log ("Enemy took: " + dp.damage + " damage");
		//Update Healthbar
		VisualController._instance.UpdateEnemyHealthbar(health);
		if (health <= 0) {
			Player._instance.AddGold (level);
			Player._instance.AddExperience (level);
			return true;
		}
		return false;
	}

	/// <summary>
	/// Heals the Enemy
	/// </summary>
	/// <param name="hp">Hp.</param>
	public void HealUp(DamagePackage hp){
		health += (int)Math.Floor(hp.damage);
		Debug.Log ("Enemy recieved: " + hp.damage + " health");
		//Update Healthbar
		VisualController._instance.UpdateEnemyHealthbar (health);
	}

	/// <summary>
	/// My turn.
	/// </summary>
	public void MyTurn(){
		Debug.Log ("Enemy turn!"+health);
		//TODO: Need simple AI to pick attacks.
		Skill ability = abilties[rnd.Next(0,3)];

		Debug.Log ("Enemy used " + ability.name + "!");
		if (ability.isSelfTarget ()) {
			CombatController._instance.HealEnemy (ability.CalDmg (AP, critChance));
		}
		else {
			CombatController._instance.AttackPlayer (ability.CalDmg(AP, critChance));
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

	public void addDoT(DamagePackage dp) {
		DoTS.Add (dp);
	}

	public void addHoT(DamagePackage dp) {
		HoTS.Add (dp);
	}

	//Temporary solution, until we get another way to keep abilties.
	private void addAbilities(){
		abilties.Add (new Skill ("Swarm of Butterflies", false, ElementalType.Earth, 10, 0));

		abilties.Add (new Skill ("Elephant Stampede", false, ElementalType.None, 10, 0));

		abilties.Add (new Skill ("Flock of Cows", false, ElementalType.None, 10, 0));
	}
}