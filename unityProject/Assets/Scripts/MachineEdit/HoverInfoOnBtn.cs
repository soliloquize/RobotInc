using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverInfoOnBtn : MonoBehaviour {

    public Canvas mainCanv;
    public GameObject infoWindow;
    public Text itemName;
    public Text itemDisc;
    public Text itemCost;

    private TransAttribute transItem;

    public void DisplayItemInfo( string partId )
    {
        //transItem.itemId = partId;
        
    }

}
