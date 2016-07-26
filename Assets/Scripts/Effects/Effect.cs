using UnityEngine;
using System.Collections;

public interface Effect {

	void AddToSkill (Skill skill);
	void ActivateEffect(Player player);
	void DoStuff(Player player);
	void DeactivateEffect(Player player);
	bool IsOver ();
	bool IsSelfTar ();
}
