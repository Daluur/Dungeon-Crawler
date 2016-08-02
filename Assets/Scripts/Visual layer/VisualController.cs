using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VisualController : MonoBehaviour {

	public static VisualController _instance;

	//The current gameobject of the visual enemy.
	GameObject currentObj;
	//The current info about the enemy.
	EnemyInfo currentEnemyInfo;

	//Things for the enemy healthbar.
	public Slider enemyHealthbar;
	public Image enemyHealthbarColor;
	int enemyMaxHealth;
	public Text enemyHealthText;
	public Text enemyName;

	//Things for the player healthbar.
	public Slider playerHealthbar;
	public Image playerHealthbarColor;
	int playerMaxHealth;
	public Text playerHealthText;

	//The background.
	public SpriteRenderer background;

	public GameObject combatCanvas;
	public GameObject townCanvas;

	public GameObject lootButton;
	public GameObject nextEncounterButton;

	/// <summary>
	/// Makes it a singleton.
	/// </summary>
	void Awake(){
		if (_instance == null) {
			_instance = this;
		} else {
			Debug.Log ("There are 2 VisualControllers");
		}
	}

	/// <summary>
	/// Creates the enemy visual.
	/// </summary>
	/// <param name="e">E.</param>
	public void CreateEnemyVisual(EnemyInfo e){
		if (currentObj != null) {
			Debug.LogError ("There already is an enemy!");
		}
		currentEnemyInfo = e;
		currentObj = Instantiate(e.visual,new Vector3 (0, 2.5F, 0),Quaternion.identity) as GameObject;
	}

	/// <summary>
	/// Removes the enemy visual.
	/// </summary>
	public void RemoveEnemyVisual(){
		if (currentObj == null) {
			Debug.Log ("Enemy visual was already destroyed!");
			return;
		}
		Destroy (currentObj);
		currentEnemyInfo = null;
		currentObj = null;
	}
		
	public void CreateEnemyHealthbar(int maxHealth){
		//Initialize healthbar
		enemyHealthbar.enabled = true;
		enemyHealthbar.value = 1;
		enemyMaxHealth = maxHealth;
		enemyHealthbarColor.color = Color.green;
		enemyHealthText.text = "" + maxHealth + "/" + maxHealth;

		//Initialize name
		enemyName.enabled = true;
		enemyName.text = currentEnemyInfo.name;
	}

	public void UpdateEnemyHealthbar(int currHealth){
		//Updates the healthbar
		enemyHealthbar.value = (float)currHealth/enemyMaxHealth;
		enemyHealthbarColor.color = Color.Lerp (Color.red, Color.green, enemyHealthbar.value);
		enemyHealthText.text = "" + currHealth + "/" + enemyMaxHealth;
	}
		
	public void RemoveEnemyHealthbar(){
		//Disables the healthbar
		enemyHealthbar.enabled = false;
		enemyName.enabled = false;
	}

	public void CreatePlayerHealthbar(int maxHealth){
		//Initialize healthbar
		playerHealthbar.enabled = true;
		playerHealthbar.value = 1;
		playerMaxHealth = maxHealth;
		playerHealthbarColor.color = Color.green;
		playerHealthText.text = "" + maxHealth + "/" + maxHealth;
	}

	public void UpdatePlayerMaxHealth(int maxHealth){
		playerMaxHealth = maxHealth;
	}

	public void UpdatePlayerHealthbar(int currHealth){
		//Updates the healthbar
		playerHealthbar.value = (float)currHealth/playerMaxHealth;
		playerHealthbarColor.color = Color.Lerp (Color.red, Color.green, playerHealthbar.value);
		playerHealthText.text = "" + currHealth + "/" + playerMaxHealth;
	}

	public void ChangebackgroundColor(Color col){
		//Changes the background color.
		background.color = col;
	}

	public void ShowLootButton(){
		lootButton.SetActive (true);
	}

	public void RemoveLootButton(){
		lootButton.SetActive (false);
	}

	public void ShowNextEncounterButton(){
		nextEncounterButton.SetActive (true);
	}

	public void RemoveNextEncounterButton(){
		nextEncounterButton.SetActive (false);
	}

	public void LeaveDungeon(){
		combatCanvas.SetActive (false);
	}

	public void EnterDungeon(){
		combatCanvas.SetActive (true);
	}

	public void LeaveTown(){
		townCanvas.SetActive (false);
	}

	public void EnterTown(){
		townCanvas.SetActive (true);
	}
}