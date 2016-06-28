using UnityEngine;
using System.Collections;

public class DamagePackage {

	public int type;
	public int damage;

	public DamagePackage(int newType, int newDamage){
		type = newType;
		damage = newDamage;
	}
}