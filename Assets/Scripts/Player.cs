using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour {

	//Players level
	int level = 2;
	//Players current health.
	int health;
	//Players current AP
	int AP;
	//Whether or not it is the players turn.
	bool myTurn = false;
	//Players active abilties
	List<Skill> abilties = new List<Skill>();
	List<DamagePackage> DoTS = new List<DamagePackage>();
	List<DamagePackage> HoTS = new List<DamagePackage>();


	/// <summary>
	/// Ínitialises data
	/// </summary>
	void Start () {
		health = 100 * level;
		AP = 20 * level;
		addAbilities ();
		VisualController._instance.CreatePlayerHealthbar (health);
	}

	/// <summary>
	/// Takes the damage.
	/// </summary>
	/// <returns><c>true</c>, if dead, <c>false</c> otherwise.</returns>
	/// <param name="dp">Dp.</param>
	public bool TakeDamage(DamagePackage dp){
		health -= dp.damage;
		Debug.Log ("Player took: " + dp.damage + " damage");
		//Updates the healthbar
		VisualController._instance.UpdatePlayerHealthbar (health);
		if (health <= 0) {
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
				VisualController._instance.UpdatePlayerHealthbar (health);
				Debug.Log ("Player took: " + DoT.OTDamage + " damage, from DoT");
				DoT.updateTimeLeft();
			} else {
				toRemove.Add (DoT);
			}
		}
		foreach (DamagePackage DoT in toRemove) {
			DoTS.Remove (DoT);
		}
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
		health += hp.damage;
		//Updates the healthbar
		VisualController._instance.UpdatePlayerHealthbar (health);
		Debug.Log ("Player recieved: " + hp.damage + " health");
	}

	public bool HealHoT(){
		List<DamagePackage> toRemove = new List<DamagePackage> ();
		foreach (DamagePackage HoT in HoTS) {
			if (HoT.rounds > 0) {
				health += HoT.damage;
				//Update Healthbar
				VisualController._instance.UpdatePlayerHealthbar (health);
				Debug.Log ("Player recieved: " + HoT.OTDamage + " health, from HoT");
				HoT.updateTimeLeft();
			} else {
				toRemove.Add (HoT);
			}
		}
		foreach (DamagePackage HoT in toRemove) {
			HoTS.Remove (HoT);
		}
		if (health <= 0) {
			return true;
		}
		return false;
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
					Debug.Log ("Player used " + ability.name + "!");
					CombatController._instance.HealPlayer (ability.CalDmg (AP));
					updateCD ();
					ability.setCD ();
				}
			}
			else { // Damage
				if (ability.onCD ()) {
					Debug.Log (ability.name + " is on Cooldown");
				} else {
					myTurn = false;
					Debug.Log ("Player used " + ability.name + "!");
					CombatController._instance.AttackEnemy (ability.CalDmg (AP));
					updateCD ();
					ability.setCD ();
				}
			}
		}
	}

	/// <summary>
	/// Allows the player to perform his turn.
	/// </summary>
	public void MyTurn(){
		Debug.Log ("Players turn!"+health);
		myTurn = true;
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

	// Temporary solution, because I was unable to get info from button's in canvas.
	// But.. it's an easy way to make the attack buttons.
	public void attack1(){
		UseAbility(abilties [0]);
	}

	public void attack2(){
		UseAbility(abilties [1]);
	}

	public void attack3(){
		UseAbility(abilties [2]);
	}

	// Temporary solution, until we get another way to keep abiltiies.
	private void addAbilities(){
		abilties.Add (new Skill ("Mega Punch", false, 1, 1, 0));

		abilties.Add (new Skill ("Fireball", false, 3, 2, 1, 3, 1));

		abilties.Add (new Skill ("Holy Hand", true, 1, 2, 2));
	}
}