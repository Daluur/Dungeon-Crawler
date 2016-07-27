using UnityEngine;
using System.Collections;

public interface Effect {

	void AddToSkill (Skill skill);
	void ActivateEffect(Player player, Enemy enemy = null);
	void DoStuff(Player player, Enemy enemy = null);
	void DeactivateEffect(Player player, Enemy enemy = null);
	bool IsOver ();
	bool IsSelfTar ();
}
