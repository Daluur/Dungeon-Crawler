using UnityEngine;
using System.Collections;

/// <summary>
/// Damage package.
/// </summary>
public class HealingPackage {

	//Which type, e.g. fire, water, desert w/e.
	public int type;
	//The damage.
	public int healing;

	public HealingPackage(int newType, int newHealing){
		type = newType;
		healing = newHealing;
	}
}