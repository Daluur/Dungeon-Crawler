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
	//Its attack power.
	int AP;
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
		health -= dp.damage;
		//Update Healthbar
		VisualController._instance.UpdateEnemyHealthbar(health);
		Debug.Log ("Enemy took: " + dp.damage + " damage");
		if (health <= 0) {
			Player._instance.addGold (level);
			return true;
		}
		return false;
	}

	public bool TakeDoTDamage(){
		List<DamagePackage> toRemove = new List<DamagePackage> ();
		foreach (DamagePackage DoT in DoTS) {
			if (DoT.rounds > 0) {
				health -= DoT.damage;
				//Update Healthbar
				VisualController._instance.UpdateEnemyHealthbar (health);
				Debug.Log ("Enemy took: " + DoT.OTDamage + " damage, from DoT");
				DoT.updateTimeLeft();
			} else {
				toRemove.Add (DoT);
			}
		}
		foreach (DamagePackage DoT in toRemove) {
			DoTS.Remove (DoT);
		}
		if (health <= 0) {
			Player._instance.addGold (level);
			return true;
		}
		return false;
	}

	/// <summary>
	/// Heals the Enemy
	/// </summary>
	/// <param name="hp">Hp.</param>
	public void HealUp(DamagePackage hp){
		health += hp.damage;
		//Update Healthbar
		VisualController._instance.UpdateEnemyHealthbar (health);
		Debug.Log ("Enemy recieved: " + hp.damage + " health");
	}
		
	public void HealHoT(){
		List<DamagePackage> toRemove = new List<DamagePackage> ();
		foreach (DamagePackage HoT in HoTS) {
			if (HoT.rounds > 0) {
				health += HoT.damage;
				//Update Healthbar
				VisualController._instance.UpdateEnemyHealthbar (health);
				Debug.Log ("Enemy recieved: " + HoT.OTDamage + " health, from HoT");
				HoT.updateTimeLeft();
			} else {
				toRemove.Add (HoT);
			}
		}
		foreach (DamagePackage HoT in toRemove) {
			HoTS.Remove (HoT);
		}
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
			CombatController._instance.HealEnemy (ability.CalDmg (AP));
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

	public void addDoT(DamagePackage dp) {
		DoTS.Add (dp);
	}

	public void addHoT(DamagePackage dp) {
		HoTS.Add (dp);
	}

	//Temporary solution, until we get another way to keep abilties.
	private void addAbilities(){
		abilties.Add (new Skill ("Swarm of Butterflies", false, 1, 10, 0, 2, 10));

		abilties.Add (new Skill ("Elephant Stampede", false, 1, 20, 2));

		abilties.Add (new Skill ("Flock of Cows", true, 1, 10, 0, 2, 10));
	}
}