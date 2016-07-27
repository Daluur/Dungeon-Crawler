using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

abstract public class Skill{

	public string name;
	//Skill type, self target or enemy target.
	public bool selfTar;
	public bool selfDam;
	//SKill elemental type
	public ElementalType type;
	//Skill damage, as AP multiplier
	public float APMult;
	//Skill Cooldown length
	public int CD;

	public Effect mainEffect;
	public List<Effect> effects = new List<Effect> ();


	abstract public DamagePackage CalDmg(int AP, float critChance);
	abstract public void AddEffect (Effect newEffect);
	abstract public void SetCD ();
	abstract public void UpdateCD();
	abstract public bool OnCD();


}