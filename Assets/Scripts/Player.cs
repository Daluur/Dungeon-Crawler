using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class Player : MonoBehaviour {

	public static Player _instance;

	//Players level
	public int level;

	public int experience;

	public int XPforLevel;

	//Players current health.
	public int health;

	int bonusHealth;

	//Players current AP
	int AP;

	int bonusAP;

	float critChance;

	int critRating;

	float damageReduction;

	public float additionReductions; // From Effects

	int armor;


	//Whether or not it is the players turn.
	bool myTurn = false;

	//Players active abilties
	List<Skill> abilties = new List<Skill>();
	List<Effect> effects = new List<Effect>();

	public int gold;

	int rubies;



	/// <summary>
	/// Ínitialises data
	/// </summary>
	void Awake () {
		if (_instance == null) {
			_instance = this;
		} else {
			Debug.Log ("There are 2 players... somehow.. does it make sense?");
		}

		if (SaveLoad.SaveExist ()) {
			loadPlayerData (SaveLoad.Load ());
		} else {
			NewPlayer ();
		}
		AddAbilities ();
		VisualController._instance.CreatePlayerHealthbar (health);
	}

	void OnApplicationQuit() {
		SaveLoad.Save (Player._instance.savePlayerData());
	}

	public void NewPlayer() {
		level = 1;
		experience = 0;
		XPforLevel = 250;
		health = 300;
		bonusHealth = 0;
		AP = 40;
		bonusAP = 0;
		critChance = 5.0F;
		critRating = 0;
		damageReduction = 0.0F;
		armor = 0;
		gold = 0;
		rubies = 0;
		VisualController._instance.CreatePlayerHealthbar (health);
	}

	public void AddExperience(int Enemylevel) {
		experience += Enemylevel * 100;
		if (XPforLevel < experience) {
			level++;
			experience -= XPforLevel;
			XPforLevel *= 2;
		}
	}	

	public void AddGold(int Enemylevel) {
		gold += Enemylevel * 5;
	}

	public void AddRubies(int Enemylevel) {
		rubies += Enemylevel * 2;
	}

	/// <summary>
	/// Takes the damage.
	/// </summary>
	/// <returns><c>true</c>, if dead, <c>false</c> otherwise.</returns>
	/// <param name="dp">Dp.</param>
	public bool TakeDamage(DamagePackage dp){
		dp.DamageReduction (damageReduction);
		dp.DamageReduction (additionReductions);
		health -= (int)Math.Floor(dp.damage);
		Debug.Log ("Player took: " + dp.damage + " damage");
		//Updates the healthbar
		VisualController._instance.UpdatePlayerHealthbar (health);
		if (health <= 0) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// Heals the Player
	/// </summary>
	/// <param name="hp">Hp.</param>
	public void HealUp(DamagePackage hp){
		health += (int)Math.Floor(hp.damage);
		Debug.Log ("Player recieved: " + hp.damage + " health");
		//Updates the healthbar
		VisualController._instance.UpdatePlayerHealthbar (health);
	}

	/// <summary>
	/// Uses an Ability.
	/// </summary>
	public void UseAbility(Skill ability){
		if (myTurn) {
			if (ability.isSelfTarget()) { // Healing
				if (ability.onCD ()) {
					Debug.Log (ability.name + " is on Cooldown");
				} else {
					myTurn = false;
					foreach (Effect eff in ability.effects) {
						eff.ActivateEffect (this);
						effects.Add (eff);
					}
					Debug.Log ("Player used " + ability.name + "!");
					CombatController._instance.HealPlayer (ability.CalDmg (AP, critChance));
					UpdateCD ();
					ability.setCD ();
				}
			}
			else { // Damage
				if (ability.onCD ()) {
					Debug.Log (ability.name + " is on Cooldown");
				} else {
					myTurn = false;
					foreach (Effect eff in ability.effects) {
						eff.ActivateEffect (this);
						effects.Add (eff);
					}
					Debug.Log ("Player used " + ability.name + "!");
					CombatController._instance.AttackEnemy (ability.CalDmg (AP, critChance));
					UpdateCD ();
					ability.setCD ();
				}
			}

		}
	}

	public void UseEffect(Skill ability) {
		if (ability.isSelfTarget()) { // Healing
			Debug.Log ("Player used " + ability.name + "!");
			CombatController._instance.HealPlayer (ability.CalDmg (AP, critChance));
		} 
		else { // Damage
			Debug.Log ("Player used " + ability.name + "!");
			CombatController._instance.EffectEnemy (ability.CalDmg (AP, critChance));
		}


	}

	public void RemoveEffectsOnEnemy() {
		List<Effect> toRemove = new List<Effect>();
		foreach (Effect eff in effects) {
			if (!eff.IsSelfTar()) {
				toRemove.Add (eff);
			}
		}
		foreach (Effect eff in toRemove) {
			effects.Remove (eff);
		}
	}

	/// <summary>
	/// Allows the player to perform his turn.
	/// </summary>
	public void MyTurn(){
		Debug.Log ("Players turn!"+health);
		List<Effect> toRemove = new List<Effect>();
		foreach (Effect eff in effects) {
			if (eff.IsOver ()) {
				eff.DeactivateEffect (this);
				toRemove.Add (eff);
			} else {
				eff.DoStuff (this);
			}
		}
		foreach (Effect eff in toRemove) {
			effects.Remove (eff);
		}
		myTurn = true;
	}

	/// <summary>
	/// Decrement the CD for all Skills
	/// </summary>
	public void UpdateCD() {
		foreach (Skill attack in abilties) {
			attack.updateCD();
		}
	}
		
	// Wrappers for attacking
	public void Attack1(){
		UseAbility(abilties [0]);
	}

	public void Attack2(){
		UseAbility(abilties [1]);
	}

	public void Attack3(){
		UseAbility(abilties [2]);
	}

	private void AddAbilities(){
		abilties.Add (new Skill ("Mega Punch", false, ElementalType.Earth, 1, 0));
		abilties.Add (new Skill ("Fireball", false, ElementalType.Fire, 2, 1));
		abilties.Add (new Skill ("Holy Hand", true, ElementalType.None, 2, 2));
		abilties [1].AddEffect (new WeakOverTime());
		abilties [2].AddEffect (new ReduceDamage ());
	} 

	public PlayerData savePlayerData() {
		PlayerData data = new PlayerData ();
		data.level = level;
		data.experience = experience;
		data.XPforLevel = XPforLevel;
		data.bonusHealth = bonusHealth;
		data.bonusAP = bonusAP;
		data.critRating = critRating;
		data.armor = armor;
		data.gold = gold;
		data.rubies = rubies;

		return data;
	}

	public void loadPlayerData(PlayerData data) {
		level = data.level;
		experience = data.experience;
		XPforLevel = data.XPforLevel;
		bonusHealth = data.bonusHealth;
		health = (3000 * level) + bonusHealth;
		bonusAP = data.bonusAP;
		AP = (level * 40) + bonusAP;
		critRating = data.critRating;
		critChance = 5.0F + (critRating / 22.5F); 
		armor = data.armor;
		damageReduction = 10.0F + (armor / 50);
		gold = data.gold;
		rubies = data.rubies;
	}
}