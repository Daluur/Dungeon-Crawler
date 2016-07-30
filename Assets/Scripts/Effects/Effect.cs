using UnityEngine;
using System.Collections;

abstract public class Effect {

	public string name;
	public string effectFromSkill;
	public int round;
	public bool stackable = false;

	/// <summary>
	/// Adds this effect to skill.
	/// </summary>
	/// <param name="skill">Skill</param>
	abstract public void AddToSkill (Skill skill);
	/// <summary>
	/// Activates the effect
	/// </summary>
	/// <param name="player">PlayerFuck</param>
	/// <param name="enemy">Enemy</param>
	/// <param name="whoUsedIt">Who used it</param>
	abstract public void ActivateEffect(Player player, Enemy enemy, PCNPC whoUsedIt);
	/// <summary>
	/// Do the stuff.
	/// </summary>
	/// <param name="player">Player.</param>
	/// <param name="enemy">Enemy.</param>
	/// <param name="whoUsedIt">Who used it.</param>
	abstract public void DoStuff(Player player, Enemy enemy, PCNPC whoUsedIt);
	/// <summary>
	/// Deactivates the effect
	/// </summary>
	/// <param name="player">Player</param>
	/// <param name="enemy">Enemy</param>
	/// <param name="whoUsedIt">Who used it</param>
	abstract public void DeactivateEffect(Player player, Enemy enemy, PCNPC whoUsedIt);
	/// <summary>
	/// Resets the effect
	/// </summary>
	/// <param name="player">Player</param>
	/// <param name="enemy">Enemy</param>
	/// <param name="whoUsedIt">Who used it</param>
	abstract public void ResetEffect (Player player, Enemy enemy, PCNPC whoUsedIt);
	/// <summary>
	/// Determines whether this instance is over.
	/// </summary>
	/// <returns><c>true</c> if this instance is over; otherwise, <c>false</c>.</returns>
	abstract public bool IsOver ();
		


}
