using UnityEngine;
using System.Collections;

public class Stun : Effect {

	new public string name = "Stun";
	bool selfTar;

	new int round = 1;

	public Stun(Skill skill) {
		effectFromSkill = skill.name;
		selfTar = skill.selfTar;
	}

	public override void AddToSkill(Skill skill) {
		effectFromSkill = skill.name;
		selfTar = skill.selfTar;
	}

	public override void ActivateEffect(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (selfTar) {
			if (whoUsedIt == PCNPC.NPC) {
				enemy.AddEffect (this);
				enemy.isStun = true;
			} else {
				player.AddEffect (this);
				player.isStun = true;
			}
		} else {
			if (whoUsedIt == PCNPC.NPC) {
				player.AddEffect (this);
				player.isStun = true;
			} else {
				enemy.AddEffect (this);
				enemy.isStun = true;
			}
		}
	}

	public override void DoStuff(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (whoUsedIt == PCNPC.NPC) {
			enemy.isStun = true;
		} else {
			player.isStun = true;
		}
		round--;
	}

	public override void DeactivateEffect(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (whoUsedIt == PCNPC.NPC) {
			enemy.RemoveEffect (this);
			enemy.isStun = false;
		} else {
			player.RemoveEffect (this);
			player.isStun = false;
		}
		round = 1;
	}

	public override void ResetEffect (Player player, Enemy enemy, PCNPC whoUsedIt) {
		round = 1;
	}

	public override bool IsOver () {
		if (round > 0) {
			return false;
		}
		return true;
	} 
}
