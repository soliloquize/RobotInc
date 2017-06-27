using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class shopItem
{
    public string itemName;
    public string itemDisc;
    public string itemCost;
    public bool isBuyable;      
}

public class MachineShopList : MonoBehaviour {

    public List<shopItem> shopList;
    public Transform contentPanel;
    
	void Start () {
		
	}
	
    void RefreshShopList()
    {

    }

    void AddTranShopButtons()
    {
        foreach (var shopItem in shopList)
        {
            
        }

    }


}
