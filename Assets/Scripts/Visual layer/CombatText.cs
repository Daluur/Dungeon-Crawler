using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CombatText : MonoBehaviour {
	
	public static CombatText _instance;
	public Animator enemyA;
	public Text enemyT;
	public Animator playerA;
	public Text playerT;

	public Animator infoA;
	public Text info;

	Queue<AnimationQueue> eQ = new Queue<AnimationQueue>();
	Queue<AnimationQueue> pQ = new Queue<AnimationQueue>();
	bool playingEAnim = false;
	bool playingPAnim = false;
	bool playingInfoMessage = false;
	bool playingUnskippableMessage = false;
	bool eFirst = false;
	bool pFirst = false;

	/// <summary>
	/// Makes it a singleton.
	/// </summary>
	void Awake(){
		if (_instance == null) {
			_instance = this;
		} else {
			Debug.Log ("There are 2 CombatTexts");
		}
	}

	/// <summary>
	/// Determines whether this instance is playing animation.
	/// </summary>
	/// <returns><c>true</c> if this instance is playing animation; otherwise, <c>false</c>.</returns>
	public bool IsPlayingAnimation(){
		if (playingEAnim || playingPAnim || playingInfoMessage) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// Enemy takes damage, plays animation.
	/// </summary>
	/// <param name="d">D.</param>
	/// <param name="c">If set to <c>true</c> c.</param>
	/// <param name="h">If set to <c>true</c> h.</param>
	/// <param name="name">Name.</param>
	/// <param name="health">Health.</param>
	public void EnemyTakesDamage(int d, bool c, bool h, string name, int health){
		eQ.Enqueue(new AnimationQueue(d, c, h, name, health));
		Debug.Log ("added an enemy animation: " + d + " damage. " + c + " crit. " + h + " heal");
		if (eQ.Count == 1 && !playingEAnim) {
			EnemyAnimEnd ();
		}
	}

	/// <summary>
	/// Players takes damage, plays animation.
	/// </summary>
	/// <param name="d">D.</param>
	/// <param name="c">If set to <c>true</c> c.</param>
	/// <param name="h">If set to <c>true</c> h.</param>
	/// <param name="name">Name.</param>
	/// <param name="health">Health.</param>
	public void PlayerTakesDamage(int d, bool c, bool h, string name, int health){
		pQ.Enqueue (new AnimationQueue (d, c, h, name, health));
		Debug.Log ("added an player animation: " + d + " damage. " + c + " crit. " + h + " heal");
		if (pQ.Count == 1 && !playingPAnim) {
			PlayerAnimEnd ();
		}
	}

	/// <summary>
	/// An enemy animation finished, starts the next (if there is one).
	/// </summary>
	public void EnemyAnimEnd(){
		if (playingPAnim) {
			return;
		}
		if (playingUnskippableMessage) {
			if (pFirst) {
				return;
			} else {
				eFirst = true;
				return;
			}
		}
		playingEAnim = false;
		//If there is an animation in the queue, start the next animation.
		if (eQ.Count > 0) {
			AnimationQueue temp = eQ.Dequeue ();
			playingEAnim = true;
			enemyT.text = temp.name + "\n";
			if (temp._isCrit) {
				enemyT.text += "-" + temp.damage;
				enemyA.SetTrigger ("Crit");
				Debug.Log ("played a crit animation: " + temp.damage);
			} else if (temp._isHeal) {
				enemyT.text += "+" + temp.damage;
				enemyA.SetTrigger ("Heal");
				Debug.Log ("played a heal animation: " + temp.damage);
			} else {
				enemyT.text += "-" + temp.damage;
				enemyA.SetTrigger ("Hit");
				Debug.Log ("played a hit animation: " + temp.damage);
			}
			//Updates the healthbar.
			VisualController._instance.UpdateEnemyHealthbar (temp.health);
		} else {
			if (pQ.Count > 0) {
				PlayerAnimEnd ();
			} else {
				pFirst = false;
				eFirst = false;
				//Finish the animation, as it cannot change turns if animations are playing.
				CombatController._instance.FinishedAnimations ();
			}
		}
	}


	/// <summary>
	/// A player animation finished, starts the next (if there is one).
	/// </summary>
	public void PlayerAnimEnd(){
		if (playingEAnim) {
			return;
		}
		if (playingUnskippableMessage) {
			if (eFirst) {
				return;
			} else {
				pFirst = true;
				return;
			}
		}
		playingPAnim = false;
		//If there is an animation in the queue, start the next animation.
		if (pQ.Count > 0) {
			AnimationQueue temp = pQ.Dequeue ();
			playingPAnim = true;
			playerT.text = temp.name + "\n";
			if (temp._isCrit) {
				playerT.text += "-" + temp.damage;
				playerA.SetTrigger ("Crit");
				Debug.Log ("played a crit animation: " + temp.damage);
			} else if (temp._isHeal) {
				playerT.text += "+" + temp.damage;
				playerA.SetTrigger ("Heal");
				Debug.Log ("played a heal animation: " + temp.damage);
			} else {
				playerT.text += "-" + temp.damage;
				playerA.SetTrigger ("Hit");
				Debug.Log ("played a hit animation: " + temp.damage);
			}
			//Updates the healthbar.
			VisualController._instance.UpdatePlayerHealthbar (temp.health);
		} else {
			if (eQ.Count > 0) {
				EnemyAnimEnd ();
			}
			else {
				pFirst = false;
				eFirst = false;
				//Finish the animation, as it cannot change turns if animations are playing.
				CombatController._instance.FinishedAnimations ();
			}
		}
	}

	/// <summary>
	/// Shows an info message to the player.
	/// </summary>
	/// <param name="message">Message.</param>
	public void ShowInfo(string message){
		ShowInfo (message, InfoType.Info);
	}

	/// <summary>
	/// Shows an info message to the player.
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="type">Type.</param>
	public void ShowInfo(string message, InfoType type){
		info.text = message;
		if (type == InfoType.Info) {
			infoA.SetTrigger ("Info");
		} else if (type == InfoType.Error) {
			infoA.SetTrigger ("Error");
		} else if (type == InfoType.Unskippable) {
			infoA.SetTrigger ("Info");
			playingUnskippableMessage = true;
		}
		playingInfoMessage = true;
	}

	/// <summary>
	/// Ends the info message.
	/// </summary>
	public void EndInfoMessage(){
		playingInfoMessage = false;
		if (playingUnskippableMessage) {
			playingUnskippableMessage = false;
			if (pFirst) {
				PlayerAnimEnd ();
			} else {
				EnemyAnimEnd ();
			}
		}
	}
}

public struct AnimationQueue
{
	public int damage;
	public bool _isCrit;
	public bool _isHeal;
	public int health;
	public string name;

	public AnimationQueue(int d, bool c, bool h, string newName, int newHealth){
		damage = d;
		_isCrit = c;
		_isHeal = h;
		health = newHealth;
		name = newName;
	}
}

public enum InfoType
{
	Info,
	Error,
	Unskippable
}