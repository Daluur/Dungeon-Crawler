using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ShopPanel : MonoBehaviour {

	public Text infoText;
	public Text buttonText;
	public Button button;
	ShopPanelInfo SPI;

	public void SetInfo (ShopPanelInfo info, Action<int> a){
		SPI = info;
		infoText.text = info.description;
		//int temp = 
		button.onClick.AddListener (()=> a(SPI.ID));
		UpdateInfo ();
	}

	public void UpdateInfo(){
		if (SPI.currencyType == 0) {
			if (SPI.cost <= Player._instance.gold) {
				buttonText.text = "Buy: " + SPI.cost;
				button.interactable = true;
			} else {
				buttonText.text = "Costs: " + SPI.cost;
				button.interactable = false;
			}
		} else {
			if (SPI.cost <= Player._instance.rubies) {
				buttonText.text = "Buy: " + SPI.cost;
				button.interactable = true;
			} else {
				buttonText.text = "Costs: " + SPI.cost;
				button.interactable = false;
			}
		}
	}

	public void UpdateValues(ShopPanelInfo info){
		SPI = info;
	}
}
