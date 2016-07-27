using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class Player : MonoBehaviour {

	public static Player _instance;
	CombatController CC;

	//Players level
	public int level;
	public int experience;
	public int XPforLevel;

	//Players current health.
	public int health;
	int bonusHealth;

	//Players current AP
	public int AP;
	int bonusAP;

	//Players current crit
	float critChance;
	int critRating;

	// Player Damage reductions and increases
	float damageReduction;
	public float damageIncrease;
	public float additionalReductions; // From Effects, should probably be made a list
	int armor;


	//Whether or not it is the players turn.
	bool myTurn = false;
	bool isStun;
	public bool isMultiRoundAttack = false;
	public Skill multiRoundAttack = null;

	//Players active abilties
	List<Skill> abilties = new List<Skill>();
	public List<Effect> effects = new List<Effect>();

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
		CC = CombatController._instance;
		AddAbilities ();
		VisualController._instance.CreatePlayerHealthbar (health);
	}

	/// <summary>
	/// Saves PlayerData on exit
	/// </summary>
	void OnApplicationQuit() {
		SaveLoad.Save (Player._instance.savePlayerData());
	}

	/// <summary>
	/// Creates new Player
	/// </summary>
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

	/// <summary>
	/// Adds experience.
	/// </summary>
	/// <param name="Enemylevel">Enemylevel.</param>
	public void AddExperience(int Enemylevel) {
		experience += Enemylevel * 100;
		if (XPforLevel < experience) {
			level++;
			experience -= XPforLevel;
			XPforLevel *= 2;
		}
	}	

	/// <summary>
	/// Adds gold.
	/// </summary>
	/// <param name="Enemylevel">Enemylevel.</param>
	public void AddGold(int Enemylevel) {
		gold += Enemylevel * 5;
	}

	/// <summary>
	/// Adds rubies.
	/// </summary>
	/// <param name="Enemylevel">Enemylevel.</param>
	public void AddRubies(int Enemylevel) {
		rubies += Enemylevel * 2;
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
	public void HealUp(ref DamagePackage hp){
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
			if (ability.OnCD ()) {
				Debug.Log (ability.name + " is on Cooldown");
			} else {
				myTurn = false;
				Debug.Log ("Player used " + ability.name + "!");
				UpdateCD ();
				ability.SetCD ();
				foreach (Effect eff in ability.effects) {
					if (eff.selfTar) {
						eff.ActivateEffect (this, CC.currentEnemy, PCNPC.PC);
					} else {
						eff.ActivateEffect (this, CC.currentEnemy, PCNPC.PC);
					}
				}
				if (ability.selfTar) { // Healing
					if (ability.selfDam) { //Damage
						CombatController._instance.PlayerSelfDamage (ability.CalDmg (AP, critChance));
					} else {
						CombatController._instance.HealPlayer (ability.CalDmg (AP, critChance));
					}
				}
				else { // Damage
					CombatController._instance.AttackEnemy (ability.CalDmg (AP, critChance));

				}
			}
		}
	}

	public void UseEffect(Skill ability, int tempAP) {
		if (!ability.selfDam) { // Healing
			Debug.Log ("Effect " + ability.name + "!");
			CombatController._instance.EffectHealPlayer (ability.CalDmg (AP, critChance));
		} 
		else { // Damage
			Debug.Log ("Effect " + ability.name + "!");
			CombatController._instance.EffectAttackPlayer (ability.CalDmg (tempAP, critChance));
		}
}


	/// <summary>
	/// Allows the player to perform his turn.
	/// </summary>
	public void MyTurn(){
		Debug.Log ("Players turn!"+health);
		RunEffects ();
		myTurn = true;
	}

	void RunEffects () {
		List<Effect> toRemove = new List<Effect>();
		foreach (Effect eff in effects) {
			if (eff.IsOver ()) {
				toRemove.Add (eff);
			} else {
				eff.DoStuff (this, CC.currentEnemy, PCNPC.PC);
			}
		}
		foreach (Effect eff in toRemove) {
			eff.DeactivateEffect (this, CC.currentEnemy, PCNPC.PC);
		}
	}

	/// <summary>
	/// Decrement the CD for all Skills
	/// </summary>
	void UpdateCD() {
		foreach (Skill attack in abilties) {
			attack.UpdateCD();
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
		abilties.Add (new MegaPunch());
		abilties.Add (new Fireball());
		abilties.Add (new HolyHand());
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