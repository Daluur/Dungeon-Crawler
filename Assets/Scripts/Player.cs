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
	public int maxHealth;
	int bonusHealth;

	//Players current AP
	public int AP;
	int bonusAP;

	//Players current crit
	float critChance;
	int critRating;

	// Player Damage reductions and increases
	float damageReduction;
	int armor;

	public float damageIncrease;
	public float additionalReductions; 



	//Whether or not it is the players turn.
	bool myTurn = false;
	public bool isStun;
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
		maxHealth = 300;
		health = maxHealth;
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
	/// <param name="dp">DamagePackage.</param>
	public bool TakeDamage(ref DamagePackage dp){
		dp.DamageIncrease (damageIncrease);
		dp.DamageReduction (damageReduction);
		dp.DamageReduction (additionalReductions);
		health -= (int)Math.Floor(dp.damage);
		Debug.Log ("Player took: " + dp.damage + " damage");
		//Updates the healthbar
		CombatText._instance.PlayerTakesDamage(dp.damage, false, false, "Damage", health);
		//VisualController._instance.UpdatePlayerHealthbar (health);
		if (health <= 0) {
			return true;
		}
		return false;
	}

<<<<<<< HEAD
	public bool TakeDoTDamage(){
		List<DamagePackage> toRemove = new List<DamagePackage> ();
		foreach (DamagePackage DoT in DoTS) {
			if (DoT.rounds > 0) {
				health -= DoT.damage;
				//Update Healthbar
				CombatText._instance.PlayerTakesDamage(DoT.OTDamage, false, false, "DOT", health);
				//VisualController._instance.UpdatePlayerHealthbar (health);
				Debug.Log ("Player took: " + DoT.OTDamage + " damage, from DoT");
				DoT.updateTimeLeft();
=======
	/// <summary>
	/// Heals the Player
	/// </summary>
	/// <param name="hp">DamagePackage.</param>
	public void HealUp(ref DamagePackage hp){
		health += (int)Math.Floor(hp.damage);
		Debug.Log ("Player recieved: " + hp.damage + " health");
		//Updates the healthbar
		VisualController._instance.UpdatePlayerHealthbar (health);
	}

	/// <summary>
	/// Uses ability.
	/// </summary>
	/// <param name="ability">Ability.</param>
	public void UseAbility(Skill ability){
		if (myTurn) {
			if (ability.IsOnCD ()) {
				Debug.Log (ability.name + " is on Cooldown");
>>>>>>> refs/remotes/origin/TheQuibbler
			} else {
				myTurn = false;
				Debug.Log ("Player used " + ability.name + "!");
				ability.ActivateCD ();
				foreach (Effect eff in ability.effects) {
					eff.ActivateEffect (this, CC.currentEnemy, PCNPC.PC);
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

	/// <summary>
	/// Uses effect.
	/// </summary>
<<<<<<< HEAD
	/// <param name="hp">Hp.</param>
	public void HealUp(DamagePackage hp){
		health += hp.damage;
		//Updates the healthbar
		CombatText._instance.PlayerTakesDamage(hp.damage, false, true, "Heal", health);
		//VisualController._instance.UpdatePlayerHealthbar (health);
		Debug.Log ("Player recieved: " + hp.damage + " health");
	}

	public bool HealHoT(){
		List<DamagePackage> toRemove = new List<DamagePackage> ();
		foreach (DamagePackage HoT in HoTS) {
			if (HoT.rounds > 0) {
				health += HoT.damage;
				//Update Healthbar
				CombatText._instance.PlayerTakesDamage(HoT.damage, false, true, "HOT", health);
				//VisualController._instance.UpdatePlayerHealthbar (health);
				Debug.Log ("Player recieved: " + HoT.OTDamage + " health, from HoT");
				HoT.updateTimeLeft();
=======
	/// <param name="ability">Ability</param>
	/// <param name="tempAP">Temporary AP</param>
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
		Debug.Log ("Players turn!" + health);
		UpdateCD ();
		RunEffects ();
		if (isStun) {
			Debug.Log ("Player is Stunned");
			CC.currentEnemy.MyTurn ();
		} else {
			myTurn = true;
		}
	}

	// <summary>
	/// Runs the effects.
	/// </summary>/
	void RunEffects () {
		List<Effect> toRemove = new List<Effect>();
		foreach (Effect eff in effects) {
			if (eff.IsOver ()) {
				toRemove.Add (eff);
>>>>>>> refs/remotes/origin/TheQuibbler
			} else {
				eff.DoStuff (this, CC.currentEnemy, PCNPC.PC);
			}
		}
		foreach (Effect eff in toRemove) {
			eff.DeactivateEffect (this, CC.currentEnemy, PCNPC.PC);
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
						effects [i].ResetEffect (this, CC.currentEnemy, PCNPC.PC);
					}
				}
			}
<<<<<<< HEAD
			else { // Damage
				if (ability.onCD ()) {
					Debug.Log (ability.name + " is on Cooldown");
					CombatText._instance.ShowInfo (ability.name + " is on cooldown!", InfoType.Error);
				} else {
					myTurn = false;
					Debug.Log ("Player used " + ability.name + "!");
					CombatController._instance.AttackEnemy (ability.CalDmg (AP));
					updateCD ();
					ability.setCD ();
				}
=======
			if (!nameMatch) {
				effects.Add (eff);
>>>>>>> refs/remotes/origin/TheQuibbler
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
		maxHealth = (3000 * level) + bonusHealth;
		health = maxHealth;
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