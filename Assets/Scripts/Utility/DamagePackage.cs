using UnityEngine;
using System.Collections;

/// <summary>
/// Damage package.
/// </summary>
public class DamagePackage {

	//Which type, e.g. fire, water, desert w/e.
	public ElementalType type;
	//The damage.
	public float damage;
	//The name
	public string name;
	//Is crit?
	public bool isCrit;

	//Hvis null, er der en type forskel, true = strong agaisnt, false = weak against
	public bool? isStrong = null;

	public DamagePackage(ElementalType newType, float newDamage, string newName, bool newCrit){
		type = newType;
		damage = newDamage;
		name = newName;
		isCrit = newCrit;
	}

	public void DamageReduction(float reduction) { 
		damage *= ((100 - reduction) / 100);
	}

	public void DamageIncrease(float increase) {
		damage *= ((100 + increase) / 100);
	}
}