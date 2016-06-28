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

	public DamagePackage(int newType, int newDamage){
		type = newType;
		damage = newDamage;
	}
}