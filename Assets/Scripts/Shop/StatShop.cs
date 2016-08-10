using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class StatShop : MonoBehaviour {


	public static StatShop _instance;

	public GameObject shopCanvas;
	public RectTransform panelGroupRect;
	public GameObject panelPrefab;
	List<ShopPanel> panels = new List<ShopPanel>();
	public ShopPanelInfo[] SPIs;

	void Awake () {
		if (_instance == null) {
			_instance = this;
		} else {
			Debug.Log ("There are 2 shops... somehow.. does it make sense?");
		}

		if (SaveLoad.ShopExist ()) {
			LoadShopData (SaveLoad.LoadShop ());
		}
	}

	void OnApplicationQuit() {
		SaveLoad.SaveShop (SaveShopData());
	}

	void CreatePanels(){
		float num = (SPIs.Length * 100 + 10);
		panelGroupRect.sizeDelta = new Vector2 (panelGroupRect.sizeDelta.x, num);
		panelGroupRect.anchoredPosition = new Vector2 (panelGroupRect.anchoredPosition.x, -(num / 2));
		for (int i = 0; i < SPIs.Length; i++) {
			GameObject tempGame = Instantiate (panelPrefab) as GameObject;
			ShopPanel temp = tempGame.GetComponent<ShopPanel> ();
			temp.transform.SetParent (panelGroupRect.transform);
			temp.SetInfo (SPIs [i], ButtonGotPressed);
			panels.Add (temp);
		}
	}

	public void ResetShop() {
		for (int i = 0; i < SPIs.Length; i++) {
			SPIs [i].cost = 0;
			SPIs [i].boughtCount = 0;
		}

	}

	void RemovePanels(){
		for (int i = 0; i < panels.Count; i++) {
			Destroy (panels [i].gameObject);
		}
		panels.Clear ();
	}

	void UpdatePanels(){
		foreach (ShopPanel item in panels) {
			item.UpdateInfo ();
		}
	}

	public void ButtonGotPressed(int i){
		if (i == 0) {
			Player._instance.AddBonusHealth (100);
			Player._instance.SubstractGold (SPIs [i].cost);
			SPIs [i].cost += SPIs [i].increasePerBuy;
			SPIs [i].boughtCount++;
			panels [i].UpdateValues (SPIs [i]);
		} else if (i == 1) {
			Player._instance.AddBonusAP (5);
			Player._instance.SubstractGold (SPIs [i].cost);
			SPIs [i].cost += SPIs [i].increasePerBuy;
			SPIs [i].boughtCount++;
			panels [i].UpdateValues (SPIs [i]);
		} else if (i == 2) {
			Player._instance.AddGold (20);
			Player._instance.SubstractRubies (SPIs [i].cost);
			SPIs [i].cost += SPIs [i].increasePerBuy;
			SPIs [i].boughtCount++;
			panels [i].UpdateValues (SPIs [i]);
		} else if (i == 3) {
			//Player._instance.AddGold (20);
			//Player._instance.SubstractRubies (SPIs [i].cost);
			//SPIs [i].cost += SPIs [i].increasePerBuy;
			//SPIs [i].boughtCount++;
			//panels [i].UpdateValues (SPIs [i]);
		}

		UpdatePanels ();
		HUD._instance.UpdateHUD ();
	}

	public void LeaveShop(){
		RemovePanels ();
		shopCanvas.SetActive (false);
		VisualController._instance.EnterTown ();
	}

	public void EnterShop(){
		shopCanvas.SetActive (true);
		Invoke ("CreatePanels", 0.1f);
		//CreatePanels();
	}

	List<int> SaveShopData() {
		List<int> data = new List<int> ();
		foreach (ShopPanelInfo info in SPIs) {
			data.Add (info.boughtCount);
		}

		return data;
	}

	void LoadShopData(List<int> data) {
		for (int i = 0; i < SPIs.Length; i++) {
			SPIs [i].boughtCount = data [i];
			SPIs [i].cost = SPIs [i].increasePerBuy * SPIs [i].boughtCount;
		}
	}

}

[Serializable]
public struct ShopPanelInfo
{
	public string description;
	public int cost;
	public int currencyType;
	public int ID;
	public int increasePerBuy;
	public int boughtCount;
}