using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class Player : MonoBehaviour {

	public static Player _instance;
	CombatController CC;
	//Used to see the cooldown and skill names.
	public AttackButtonsUtil ABU;

	//Players level
	public int level;
	public int experience;
	public int XPforLevel;

	//Players current health.
	public int health;
	public int maxHealth;
	public int bonusHealth;

	//Players current AP
	public int AP;
	public int bonusAP;

	//Players current crit
	public float critChance;
	public int critRating;

	// Player Damage reductions and increases
	public float damageReduction;
	public int armor;

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

	// Currency
	public int gold;
	public int rubies;



	/// <summary>
	/// Ínitialises data
	/// </summary>
	void Awake () {
		if (_instance == null) {
			_instance = this;
		} else {
			Debug.Log ("There are 2 players... somehow.. does it make sense?");
		}

		if (SaveLoad.PlayerExist ()) {
			loadPlayerData (SaveLoad.LoadPlayer ());
		} else {
			NewPlayer ();
		}
		AddAbilities ();
		VisualController._instance.CreatePlayerHealthbar (maxHealth);
	}

	void Start(){
		CC = CombatController._instance;
	}

	/// <summary>
	/// Saves PlayerData on exit
	/// </summary>
	void OnApplicationQuit() {
		SaveLoad.SavePlayer (savePlayerData());
	}

	/// <summary>
	/// Creates new Player
	/// </summary>
	public void NewPlayer() {
		level = 1;
		experience = 0;
		XPforLevel = 250;
		maxHealth = 2000;
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
		VisualController._instance.CreatePlayerHealthbar (maxHealth);
		HUD._instance.UpdateHUD();
		StatShop._instance.ResetShop ();

	}

	/// <summary>
	/// Adds experience.
	/// </summary>
	/// <param name="Enemylevel">Enemylevel.</param>
	public void AddExperience(int enemyLevel) {
		experience += enemyLevel * 100;
		if (XPforLevel < experience) {
			DING ();
		}
		HUD._instance.UpdateHUD ();
	}	

	/// <summary>
	/// Adds gold.
	/// </summary>
	/// <param name="Enemylevel">Enemylevel.</param>
	public void AddGold(int enemyLevel) {
		gold += enemyLevel * 5;
		HUD._instance.UpdateHUD ();
	}

	public void SubstractGold(int amount) {
		gold -= amount;
		HUD._instance.UpdateHUD ();
	}

	/// <summary>
	/// Adds rubies.
	/// </summary>
	/// <param name="Enemylevel">Enemylevel.</param>
	public void AddRubies(int dungeonLevel) {
		rubies += dungeonLevel * 2;
		HUD._instance.UpdateHUD ();
	}

	public void SubstractRubies(int amount) {
		rubies -= amount;
		HUD._instance.UpdateHUD ();
	}

	public void AddBonusHealth(int amount) {
		maxHealth += amount;
		bonusHealth += amount;
	}

	public void AddBonusAP(int amount) {
		AP += amount;
		bonusAP += amount;
	}

	/// <summary>
	/// Takes the damage.
	/// </summary>
	/// <returns><c>true</c>, if dead, <c>false</c> otherwise.</returns>
	/// <param name="dp">DamagePackage.</param>
	public bool TakeDamage(DamagePackage dp){
		dp.DamageIncrease (damageIncrease); // From Effects
		dp.DamageReduction (additionalReductions); // From Effects 
		dp.DamageReduction (damageReduction); // From Armor
		health -= (int)Math.Floor(dp.damage);
		Debug.Log ("Player took: " + dp.damage + " damage");
		//Updates the healthbar
		CombatText._instance.PlayerTakesDamage((int)Math.Floor(dp.damage), dp.isCrit, false, dp.name, health);
		//VisualController._instance.UpdatePlayerHealthbar (health);
		if (health <= 0) {
			return true;
		}
		return false;
	}

		
	/// <summary>
	/// Heals the Player
	/// </summary>
	/// <param name="hp">DamagePackage.</param>
	public void HealUp(DamagePackage hp){
		health += (int)Math.Floor(hp.damage);
		if (health > maxHealth) {
			health = maxHealth;
		}
		Debug.Log ("Player recieved: " + hp.damage + " health");
		//Updates the healthbar
		CombatText._instance.PlayerTakesDamage((int)Math.Floor(hp.damage), hp.isCrit, true, hp.name, health);
	}

	public void HealToFull() {
		health = maxHealth;
	}

	/// <summary>
	/// Uses ability.
	/// </summary>
	/// <param name="ability">Ability.</param>
	public void UseAbility(Skill ability){
		if (myTurn) {
			if (ability.IsOnCD ()) {
				Debug.Log (ability.name + " is on Cooldown");
			} else {
				myTurn = false;
				Debug.Log ("Player used " + ability.name + "!");
				ability.ActivateCD ();

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
				foreach (Effect eff in ability.effects) {
					eff.ActivateEffect (this, CC.currentEnemy, PCNPC.PC);
				}
			}
			//Updates the visual CD effect.
			ABU.UpdateButtons ();
		}
	}

	/// <summary>
	/// Uses effect.
	/// </summary>
	/// <param name="ability">Ability</param>
	/// <param name="tempAP">Temporary AP</param>
	public void UseHealEffect(Skill ability) {
		Debug.Log ("Effect " + ability.name + "!");
		CombatController._instance.EffectHealPlayer (ability.CalDmg (AP, critChance));
	}

	public void UseAttackEffect(Skill ability, int tempAP, float tempCrit) {
		Debug.Log ("Effect " + ability.name + "!");
		CombatController._instance.EffectAttackPlayer (ability.CalDmg (tempAP, tempCrit));
	}

	public void DING() {
		level++;
		experience -= XPforLevel;
		XPforLevel *= 2;
		maxHealth += 2000; //Whatever much health you get per level
		health = maxHealth;
		VisualController._instance.UpdatePlayerMaxHealth (maxHealth);
		VisualController._instance.UpdatePlayerHealthbar (health);
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
			CombatText._instance.ShowInfo ("You are stunned!", InfoType.UnskippableError);
			CC.TryEndTurn ();
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
			for (int i = 0; i < effects.Count; i++) {
				if(eff.name == effects[i].name) {
					if (eff.effectFromSkill == effects [i].effectFromSkill) {
						effects [i].ResetEffect (this, CC.currentEnemy, PCNPC.PC);
						CombatText._instance.AddPlayerEffect (eff.name, true);
						CombatText._instance.AddPlayerEffect (eff.name, false);
						return;
					} else {
						effects.Add (eff);
						CombatText._instance.AddPlayerEffect (eff.name, false);
						return;
					}
				}
			}
			effects.Add (eff);
			CombatText._instance.AddPlayerEffect (eff.name, false);
		}
	}

	/// <summary>
	/// Removes the effect.
	/// </summary>
	/// <param name="eff">Effect</param>
	public void RemoveEffect(Effect eff) {
		effects.Remove (eff);
		CombatText._instance.AddPlayerEffect (eff.name, true);
	}

	/// <summary>
	/// Decrement the CD for all Skills
	/// </summary>
	void UpdateCD() {
		foreach (Skill attack in abilties) {
			attack.UpdateCD();
		}
		//Updates the visual CD effect.
		ABU.UpdateButtons ();
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
		//Updates the onscreen buttons.
		ABU.UpdateSkills ();
	} 

	/// <summary>
	/// Returns the skill (there are only 0-2).
	/// </summary>
	/// <returns>The skill.</returns>
	/// <param name="i">The index.</param>
	public Skill GetSkill(int i){
		return abilties [i];
	}

	PlayerData savePlayerData() {
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

	void loadPlayerData(PlayerData data) {
		level = data.level;
		experience = data.experience;
		XPforLevel = data.XPforLevel;
		bonusHealth = data.bonusHealth;
		maxHealth = (2000 * level) + bonusHealth;
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

[Serializable]
public class PlayerData{
	public int level;
	public int experience;
	public int XPforLevel;
	public int bonusHealth;
	public int bonusAP;
	public int critRating;
	public int armor;
	public int gold;
	public int rubies;
	//public List<int> activeAbilities = new List<int>();
	//public List<int> unlockedAbilities = new List<int>();
}