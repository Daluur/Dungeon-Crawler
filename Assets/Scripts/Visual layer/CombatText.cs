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

	public Text info;

	Queue<AnimationQueue> eQ = new Queue<AnimationQueue>();
	Queue<AnimationQueue> pQ = new Queue<AnimationQueue>();
	bool playingEAnim = false;
	bool playingPAnim = false;

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

	public bool IsPlayingAnimation(){
		if (playingEAnim || playingPAnim) {
			return true;
		}
		return false;
	}

	public void EnemyTakesDamage(int d, bool c, bool h, string name, int health){
		eQ.Enqueue(new AnimationQueue(d, c, h, name, health));
		Debug.Log ("added an enemy animation: " + d + " damage. " + c + " crit. " + h + " heal");
		if (eQ.Count == 1 && !playingEAnim) {
			EnemyAnimEnd ();
		}
	}

	public void PlayerTakesDamage(int d, bool c, bool h, string name, int health){
		pQ.Enqueue (new AnimationQueue (d, c, h, name, health));
		Debug.Log ("added an player animation: " + d + " damage. " + c + " crit. " + h + " heal");
		if (pQ.Count == 1 && !playingPAnim) {
			PlayerAnimEnd ();
		}
	}

	public void EnemyAnimEnd(){
		if (playingPAnim) {
			return;
		}
		playingEAnim = false;
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
			VisualController._instance.UpdateEnemyHealthbar (temp.health);
		} else {
			if (pQ.Count > 0) {
				PlayerAnimEnd ();
			} else {
				CombatController._instance.FinishedAnimations ();
			}
		}
	}

	public void PlayerAnimEnd(){
		if (playingEAnim) {
			return;
		}
		playingPAnim = false;
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
			VisualController._instance.UpdatePlayerHealthbar (temp.health);
		} else {
			if (eQ.Count > 0) {
				EnemyAnimEnd ();
			}
			else {
				CombatController._instance.FinishedAnimations ();
			}
		}
	}

	public void ShowInfo(string message){
		ShowInfo (message, InfoType.Info);
	}

	public void ShowInfo(string message, InfoType type){
		if (type == InfoType.Info) {
			info.color = Color.yellow;
		} else if (type == InfoType.Error) {
			info.color = Color.red;
		}
		info.text = message;
		Invoke ("ClearInfo",0.5f);
	}

	public void ClearInfo(){
		info.text = "";
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
	Error
}