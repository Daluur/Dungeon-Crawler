using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour {

	//Players level
	int level = 1;
	//Players current health.
	int health;
	//Players current AP
	int AP;
	//Whether or not it is the players turn.
	bool myTurn = false;
	//Players active abilties
	List<Skill> abilties;

	/// <summary>
	/// Ínitialises data
	/// </summary>
	void Awake () {
		health = 100 * level;
		AP = 20 * level;
		abilties = new List<Skill>();
		addAbilities ();
	}


	/// <summary>
	/// Takes the damage.
	/// </summary>
	/// <returns><c>true</c>, if dead, <c>false</c> otherwise.</returns>
	/// <param name="dp">Dp.</param>
	public bool TakeDamage(DamagePackage dp){
		health -= dp.damage;
		Debug.Log ("Player took: " + dp.damage + " damage");
		if (health <= 0) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// Does the dmg.
	/// </summary>
	public void DoDmg(Skill attack){
		if (myTurn) {
			if (attack.onCD ()) {
				Debug.Log (attack.name + " is on Cooldown");
			} else {
				myTurn = false;
				Debug.Log ("Player used " + attack.name + "!");
				CombatController._instance.AttackEnemy (attack.CalDmg (AP));
				updateCD ();
				attack.setCD ();
			}
		}
	}

	/// <summary>
	/// Allows the player to perform his turn.
	/// </summary>
	public void MyTurn(){
		Debug.Log ("Players turn!");
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

	// Temporary solution, because I was unable to get info from button's in canvas.
	// But.. it's an easy way to make the attack buttons.
	public void Attack1(){
		DoDmg (abilties [0]);
	}

	public void Attack2(){
		DoDmg (abilties [1]);
	}

	public void Attack3(){
		DoDmg (abilties [2]);
	}

	// Temporary solution, until we get another way to keep abiltiies.
	private void addAbilities(){
		abilties.Add (new Skill ("Mega Punch", 1, 1, 0));

		abilties.Add (new Skill ("Fireball", 3, 2, 1));

		abilties.Add (new Skill (".38 Caliber", 1, 2, 2));
	}
}