using UnityEngine;
using System.Collections;

/// <summary>
/// Damage package.
/// </summary>
public class DamagePackage {

	//Which type, e.g. fire, water, desert w/e.
	public int type;
	//The damage.
	public int damage;

	public bool isOT;

	public int rounds;

	public int OTDamage;

	public DamagePackage(int newType, int newDamage){
		type = newType;
		damage = newDamage;
		isOT = false;
	}

	public DamagePackage(int newType, int newDamage, int newRounds, int newOTDamage) {
		type = newType;
		damage = newDamage;
		isOT = true;
		rounds = newRounds;
		OTDamage = newOTDamage;
	}

	public void updateTimeLeft() {
		rounds--;
	}
}