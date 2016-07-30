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

	public DamagePackage(ElementalType newType, float newDamage){
		Debug.Log("You should not use this constructor, include name and crit");
		type = newType;
		damage = newDamage;
	}

	public DamagePackage(ElementalType newType, float newDamage, string nName, bool crit){
		type = newType;
		damage = newDamage;
		name = nName;
		isCrit = crit;
	}

	public void DamageReduction(float reduction) { 
		damage *= ((100 - reduction) / 100);
	}

	public void DamageIncrease(float increase) {
		damage *= ((100 + increase) / 100);
	}
}