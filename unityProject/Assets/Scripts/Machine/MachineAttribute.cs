using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineAttribute : MonoBehaviour {

    public int machineType;
    public int itemId;
    public bool isConnected;
    public int inItemId;
    public int outItemId;
    public int[] attriValue = new int[5]; //默认5个属性值，[0]为增加插槽，[1]~[5]分别为攻击，防御，速度，感知
    public Sprite icon;

    public GameObject machineInfo;
    public Vector3 machineInfoPosOff = new Vector3(0, 0, 0);
    public Text maNameText, maCostText, maAttriText;
    public string maName, maCost, maAttri = "";

    void OnMouseOver()
    {
        machineInfo.SetActive (true);
        
        machineInfo.transform.position =  Camera.main.WorldToScreenPoint(this.transform.localPosition) + machineInfoPosOff;
        maNameText.text = maName;
        maCostText.text = "花费 " + maCost + " 原力";
        maAttriText.text = maAttri;
        
    }

    void OnMouseExit()
    {
        machineInfo.SetActive(false);
    }

}
