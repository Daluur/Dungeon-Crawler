using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class StatShop : MonoBehaviour {

	public GameObject shopCanvas;
	public RectTransform panelGroupRect;
	public GameObject panelPrefab;
	List<ShopPanel> panels = new List<ShopPanel>();
	public ShopPanelInfo[] SPIs;

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
		if (i == 3) {
			Player._instance.AddGold (5);
			SPIs [i].cost += SPIs [i].increasePerBuy;
			SPIs [i].boughtCount++;
			panels [i].UpdateValues (SPIs [i]);
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