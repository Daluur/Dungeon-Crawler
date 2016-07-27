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

	// Enemy Damage reductions and increases
	float damageReduction;
	public float damageIncrease;
	public float additionalReductions; // From Effects, should probably be made a list

	int armor;

	//Its attack power.
	int AP;

	float critChance;
	//Its active abilities
	List<Skill> abilties = new List<Skill>();
	public List<Effect> effects = new List<Effect>();

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
	public bool TakeDamage(ref DamagePackage dp){
		dp.DamageIncrease (damageIncrease);
		dp.DamageReduction (damageReduction);
		dp.DamageReduction (additionalReductions);
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
	public void HealUp(ref DamagePackage hp){
		health += (int)Math.Floor(hp.damage);
		Debug.Log ("Enemy recieved: " + hp.damage + " health");
		//Update Healthbar
		VisualController._instance.UpdateEnemyHealthbar (health);
	}

	public void UseEffect(Skill ability) {
		if (!ability.isSelfDamage()) { // Healing
			Debug.Log ("Effect " + ability.name + "!");
			CombatController._instance.EffectHealEnemy (ability.CalDmg (AP, critChance));
		} 
		else { // Damage
			Debug.Log ("Effect " + ability.name + "!");
			CombatController._instance.EffectAttackEnemy (ability.CalDmg (AP, critChance));
		}
	}

	/// <summary>
	/// My turn.
	/// </summary>
	public void MyTurn(){
		Debug.Log ("Enemy turn!"+health);	
		RunEffects ();
		//TODO: Need simple AI to pick attacks.
		Skill ability = abilties[rnd.Next(0,3)];

		Debug.Log ("Enemy used " + ability.name + "!");
		foreach (Effect eff in ability.effects) {
			if (eff.IsSelfTar()) {
				eff.ActivateEffect (Player._instance, this);
			} else {
				eff.ActivateEffect (Player._instance);
			}
		}
		if (ability.isSelfTarget()) { // Healing
			if (ability.isSelfDamage ()) { //Damage
				CombatController._instance.EnemySelfDamage (ability.CalDmg (AP, critChance));
			} else {
				CombatController._instance.HealEnemy (ability.CalDmg (AP, critChance));
			}
		}
		else { // Damage
			CombatController._instance.AttackPlayer (ability.CalDmg (AP, critChance));
		}

	}

	void RunEffects () {
		List<Effect> toRemove = new List<Effect>();
		foreach (Effect eff in effects) {
			if (eff.IsOver ()) {
				toRemove.Add (eff);
			} else {
				eff.DoStuff (Player._instance, this);
			}
		}
		foreach (Effect eff in toRemove) {
			eff.DeactivateEffect (Player._instance, this);
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
		abilties.Add (new Skill ("Swarm of Butterflies", false, ElementalType.Earth, 10, 0));

		abilties.Add (new Skill ("Elephant Stampede", false, ElementalType.None, 10, 0));

		abilties.Add (new Skill ("Flock of Cows", false, ElementalType.None, 10, 0));
		abilties [0].AddEffect (new WeakDamageOverTime ());
	}
}