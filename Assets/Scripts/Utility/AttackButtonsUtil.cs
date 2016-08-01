using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Attack buttons utility.
/// </summary>
public class AttackButtonsUtil : MonoBehaviour {

	public Button attOneB;
	public Text attOneT;
	public Button attTwoB;
	public Text attTwoT;
	public Button attThreeB;
	public Text attThreeT;
	Skill skillOne;
	Skill skillTwo;
	Skill skillThree;

	/// <summary>
	/// Updates which skills the player has.
	/// </summary>
	public void UpdateSkills(){
		skillOne = Player._instance.GetSkill (0);
		skillTwo = Player._instance.GetSkill (1);
		skillThree = Player._instance.GetSkill (2);
		UpdateButtons ();
	}

	/// <summary>
	/// Updates the buttons.
	/// </summary>
	public void UpdateButtons(){
		if (skillOne == null) {
			UpdateSkills ();
			return;
		}

		//If the skill is on CD make it not interactable, change the color and text of the text.
		if (skillOne.IsOnCD ()) {
			attOneB.interactable = false;
			attOneT.color = Color.red;
			attOneT.text = "Cooldown: " + skillOne.CD;
		} else { //If not on CD make it interactable, change the color and text.
			attOneB.interactable = true;
			attOneT.color = Color.black;
			attOneT.text = skillOne.name;
		}

		if (skillTwo.IsOnCD()) {
			attTwoB.interactable = false;
			attTwoT.color = Color.red;
			attTwoT.text = "Cooldown: " + skillTwo.CD;
		}
		else {
			attTwoB.interactable = true;
			attTwoT.color = Color.black;
			attTwoT.text = skillTwo.name;
		}

		if (skillThree.IsOnCD()) {
			attThreeB.interactable = false;
			attThreeT.color = Color.red;
			attThreeT.text = "Cooldown: " + skillThree.CD;
		}
		else {
			attThreeB.interactable = true;
			attThreeT.color = Color.black;
			attThreeT.text = skillThree.name;
		}
	}
}