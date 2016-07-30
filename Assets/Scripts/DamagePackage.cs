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

	public DamagePackage(ElementalType newType, float newDamage){
		type = newType;
		damage = newDamage;
	}

	public void DamageReduction(float reduction) { 
		damage *= ((100 - reduction) / 100);
	}

	public void DamageIncrease(float increase) {
		damage *= ((100 + increase) / 100);
	}
}