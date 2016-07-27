using UnityEngine;
using System.Collections;

abstract public class Effect {

	public bool selfTar;

	public int round = 3;
	
	abstract public void AddToSkill (Skill skill);
	abstract public void ActivateEffect(Player player, Enemy enemy, PCNPC whoUsedIt);
	abstract public void DoStuff(Player player, Enemy enemy, PCNPC whoUsedIt);
	abstract public void DeactivateEffect(Player player, Enemy enemy, PCNPC whoUsedIt);
	abstract public bool IsOver ();
}
