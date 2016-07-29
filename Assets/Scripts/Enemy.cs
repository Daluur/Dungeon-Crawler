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
	public int AP;

	float critChance;



	// Enemy Damage reductions and increases
	float damageReduction;
	int armor;

	public float damageIncrease;
	public float additionalReductions; 


	public bool isStun;
	public bool isMultiRoundAttack = false;
	public Skill multiRoundAttack = null;

	//Its active abilities
	List<Skill> abilties = new List<Skill>();
	public List<Effect> effects = new List<Effect>();

	System.Random rnd = new System.Random();


	/// <summary>
	/// Initializes a new instance of the <see cref="Enemy"/> class.
	/// </summary>
	/// <param name="newType">New type.</param>
	/// <param name="newLevel">New level.</param>
	/// <param name="number">Number</param>
	public Enemy(ElementalType newType, int newLevel, int number){
		type = newType;
		level = newLevel;
		health = level * 100000;
		damageReduction = 11.0F;
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
	/// <param name="dp">DamagePackage</param>
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
	/// <param name="hp">DamagePackage</param>
	public void HealUp(ref DamagePackage hp){
		health += (int)Math.Floor(hp.damage);
		Debug.Log ("Enemy recieved: " + hp.damage + " health");
		//Update Healthbar
		VisualController._instance.UpdateEnemyHealthbar (health);
	}

	/// <summary>
	/// Uses effect.
	/// </summary>
	/// <param name="ability">Ability.</param>
	/// <param name="tempAP">Temporary AP</param>
	public void UseEffect(Skill ability, int tempAP) {
		if (!ability.selfDam) { // Healing
			Debug.Log ("Effect " + ability.name + "!");
			CombatController._instance.EffectHealEnemy (ability.CalDmg (tempAP, critChance));
		} 
		else { // Damage
			Debug.Log ("Effect " + ability.name + "!");
			CombatController._instance.EffectAttackEnemy (ability.CalDmg (tempAP, critChance));
		}
	}

	/// <summary>
	/// My turn.
	/// </summary>
	public void MyTurn(){
		Debug.Log ("Enemy turn!"+health);	
		UpdateCD ();
		RunEffects ();
		if (isStun) {
			Debug.Log ("Enemy is Stunned");
			Player._instance.MyTurn ();
		} else {
			UseAttack ();
		}


	}

	/// <summary>
	/// Pick and use an attack
	/// </summary>
	void UseAttack() {
		//TODO: Need simple AI to pick attacks.
		Skill ability = abilties[2];

		while (ability.IsOnCD ()) {
			ability = abilties [rnd.Next (0, 2)];
		}
		Debug.Log ("Enemy used " + ability.name + "!");

		ability.ActivateCD ();
		foreach (Effect eff in ability.effects) {
			eff.ActivateEffect (Player._instance, this, PCNPC.NPC);
		}
		if (ability.selfTar) { // Healing
			if (ability.selfDam) { //Damage
				CombatController._instance.EnemySelfDamage (ability.CalDmg (AP, critChance));
			} else {
				CombatController._instance.HealEnemy (ability.CalDmg (AP, critChance));
			}
		} else { // Damage
			CombatController._instance.AttackPlayer (ability.CalDmg (AP, critChance));
		}
	}

	/// <summary>
	/// Runs the effects.
	/// </summary>
	void RunEffects () {
		List<Effect> toRemove = new List<Effect>();
		foreach (Effect eff in effects) {
			if (eff.IsOver ()) {
				toRemove.Add (eff);
			} else {
				eff.DoStuff (Player._instance, this, PCNPC.NPC);
			}
		}
		foreach (Effect eff in toRemove) {
			eff.DeactivateEffect (Player._instance, this, PCNPC.NPC);
		}
	}

	/// <summary>
	/// Adds the effect.
	/// </summary>
	/// <param name="eff">Effect</param>
	public void AddEffect(Effect eff) {
		if (eff.stackable) {
			effects.Add (eff);
		} else {
			bool nameMatch = false;
			for (int i = 0; i < effects.Count; i++) {
				if(eff.name == effects[i].name) {
					nameMatch = true;
					if (eff.effectFromSkill == effects [i].effectFromSkill) {
						effects [i].ResetEffect (Player._instance, this, PCNPC.NPC);
					}
				}
			}
			if (!nameMatch) {
				effects.Add (eff);
			}
		}
	}

	/// <summary>
	/// Removes the effect.
	/// </summary>
	/// <param name="eff">Effect</param>
	public void RemoveEffect(Effect eff) {
		effects.Remove (eff);
	}

	/// <summary>
	/// Decrement the CD for all Skills
	/// </summary>
	public void UpdateCD() {
		foreach (Skill ability in abilties) {
			ability.UpdateCD();
		}
	}

	//Temporary solution, until we get another way to keep abilties.
	private void addAbilities(){
		abilties.Add (new SwarmOfButterflies());
		abilties.Add (new FlockOfCows());
		abilties.Add (new ElephantStampede());


	}
}