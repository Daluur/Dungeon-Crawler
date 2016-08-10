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
	int maxHealth;

	//Its attack power.
	public int AP;

	public float critChance;



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
		maxHealth = level * 1000;
		health = maxHealth;
		damageReduction = 11.0F;
		critChance = level;
		//Instansiate healthbar with max hp.
		VisualController._instance.CreateEnemyHealthbar (health);
		AP = (level*10) + (number*5);
		addAbilities();
	}

	/// <summary>
	/// Takes the damage.
	/// </summary>
	/// <returns><c>true</c>, if dead, <c>false</c> otherwise.</returns>
	/// <param name="dp">DamagePackage</param>
	public bool TakeDamage(DamagePackage dp){
		dp.DamageIncrease (damageIncrease); // From effects
		dp.DamageReduction (additionalReductions); // From effects
		TypeChecks(ref dp); // Type check, strong against, weak against
		dp.DamageReduction (damageReduction); // From armor
		health -= (int)Math.Floor(dp.damage);
		Debug.Log ("Enemy took: " + dp.damage + " damage");
		//Update Healthbar
		CombatText._instance.EnemyTakesDamage((int)Math.Floor(dp.damage), dp.isCrit, false, dp.name, health);
		if (health <= 0) {
			Player._instance.AddGold (level);
			Player._instance.AddExperience (level);
			return true;
		}
		return false;
	}

	//Change this however, also might need to add magic, since we have talked about magci resist.
	void TypeChecks(ref DamagePackage dp) {
		if (dp.type == ElementalType.Earth) {
			if (type == ElementalType.Water) {
				dp.DamageIncrease (20);
				return;
			} else if (type == ElementalType.Fire) {
				dp.DamageReduction (20);
				return;
			}
		} else if (dp.type == ElementalType.Fire) {
			if(type == ElementalType.Earth) {
				dp.DamageIncrease (20);
				return;
			} else if (type == ElementalType.Water) {
				dp.DamageReduction (20);
				return;
			}
		} else if (dp.type == ElementalType.Water) {
			if(type == ElementalType.Fire) {
				dp.DamageIncrease (20);
				return;
			} else if (type == ElementalType.Earth) {
				dp.DamageReduction (20);
				return;
			}
		}
	}

	/// <summary>
	/// Heals the Enemy
	/// </summary>
	/// <param name="hp">DamagePackage</param>
	public void HealUp(DamagePackage hp){
		health += (int)Math.Floor(hp.damage);
		if (health > maxHealth) {
			health = maxHealth;
		}
		Debug.Log ("Enemy recieved: " + hp.damage + " health");
		//Update Healthbar
		CombatText._instance.EnemyTakesDamage((int)Math.Floor(hp.damage), hp.isCrit, true, hp.name, health);
	}

	/// <summary>
	/// Uses effect.
	/// </summary>
	/// <param name="ability">Ability.</param>
	/// <param name="tempAP">Temporary AP</param>
	public void UseHealEffect(Skill ability) {
		Debug.Log ("Effect " + ability.name + "!");
		CombatController._instance.EffectHealEnemy (ability.CalDmg (AP, critChance));
	}

	public void UseAttackEffect(Skill ability, int tempAP, float tempCrit) {
		Debug.Log ("Effect " + ability.name + "!");
		CombatController._instance.EffectAttackEnemy (ability.CalDmg (tempAP, tempCrit));
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
			CombatText._instance.ShowInfo ("Enemy is stunned!", InfoType.UnskippableError);
			CombatController._instance.TryEndTurn ();
		} else {
			UseAttack ();
		}


	}

	/// <summary>
	/// Pick and use an attack
	/// </summary>
	void UseAttack() {
		//TODO: Need simple AI to pick attacks.
		//Skill ability = abilties[2];
		Skill ability = abilties [rnd.Next (0, 3)];

		while (ability.IsOnCD ()) {
			ability = abilties [rnd.Next (0, 3)];
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
						CombatText._instance.AddEnemyEffect (eff.name, true);
						CombatText._instance.AddEnemyEffect (eff.name, false);
					} else {
						effects.Add (eff);
						CombatText._instance.AddEnemyEffect (eff.name, false);
					}
				}
			}
			if (!nameMatch) {
				effects.Add (eff);
				CombatText._instance.AddEnemyEffect (eff.name, false);
			}
		}
	}

	/// <summary>
	/// Removes the effect.
	/// </summary>
	/// <param name="eff">Effect</param>
	public void RemoveEffect(Effect eff) {
		effects.Remove (eff);
		CombatText._instance.AddEnemyEffect (eff.name, true);
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