using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

abstract public class Skill{

	public string name;
	//Self Target and Self Damaging
	public bool selfTar;
	public bool selfDam;
	//SKill elemental type
	public ElementalType type;
	//Skill damage, as AP multiplier
	public float APMult;
	//Skill Cooldown length, The CDduration needs to be set to the inteded CD + 1
	public int CD = 0;
	public int CDduration;

	public List<Effect> effects = new List<Effect> ();

	/// <summary>
	/// Calculate the damage.
	/// </summary>
	/// <returns>DamagePackage</returns>
	/// <param name="AP">AP</param>
	/// <param name="critChance">Crit chance</param>
	abstract public DamagePackage CalDmg(int AP, float critChance);
	/// <summary>
	/// Adds effect
	/// </summary>
	/// <param name="newEffect">New Effect</param>
	abstract public void AddEffect (Effect newEffect);
	/// <summary>
	/// Activate CD
	/// </summary>
	abstract public void ActivateCD ();
	/// <summary>
	/// Updates the CD
	/// </summary>
	abstract public void UpdateCD();
	/// <summary>
	/// Determines whether the CD is finished
	/// </summary>
	/// <returns><c>true</c> if this CD is finished; otherwise, <c>false</c>.</returns>
	abstract public bool IsOnCD();


}