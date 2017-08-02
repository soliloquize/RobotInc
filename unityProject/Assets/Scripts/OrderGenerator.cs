using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderGenerator : MonoBehaviour {

    //订单的基础属性
    public int orderId = 0;
    public string orderName;
    public int oAtk, oDef, oSpd, oFel, oProcCap;
    public int curAtk, curDef, curSpd, curFel, curCap;
    public int productCountDemand;

    //订单显示
    public Text orderNameText, oAtkText, oDefText, oSpdText, oFelText, proCounDemText;
    public Text curAtkText, curDefText, curSpdText, curFelText, curCapText;
    public Transform maIconPanel;
    public GameObject sampleIcon;
    public List<Sprite> icons;
    List<GameObject> allIcons;
    float iconPosOffset;

    //随便搞点名字
    public string[] namePartA = new string[5] { "大", "小", "瘦", "一望无际", "呆" };
    public string[] namePartB = new string[5] { "胡扯的", "天上的", "破伤风的", "惨的", "萌的" };
    public string[] namePartC = new string[5] { "史莱姆", "法师", "骑士", "刺客", "炮灰" };


    public void GenerateNewOrder()
    {
        ClearMachines();
        orderName = namePartA[Random.Range(1, 5)] + namePartB[Random.Range(1, 5)] + namePartC[Random.Range(1, 5)];
        int cap = Random.Range(2, 5);
        oProcCap = cap;
        oAtk = Random.Range(0, 3);
        oDef = Random.Range(0, cap - oAtk);
        oSpd = Random.Range(0, cap - oAtk - oDef);
        oFel = Random.Range(0, cap - 2);
        productCountDemand = Random.Range(5, 20);
        orderNameText.text  = orderName;
        oAtkText.text = oAtk.ToString();
        oDefText.text = oDef.ToString();
        oSpdText.text = oSpd.ToString();
        oFelText.text = oFel.ToString();
        curCapText.text = oProcCap.ToString();
        proCounDemText.text = productCountDemand.ToString();
    }

    //attris应为int[5],其中分别为：curAtk, curDef, curSpd, curFel, curCap，proCounDemText
    //machines中为加入路线的机器id
    public void RefreshOrderStats(int[] attris) //只加，目前不做减去单一
    {
        curAtk = curAtk + attris[1];
        curDef = curDef + attris[2];
        curSpd = curSpd + attris[3];
        curFel = curFel + attris[4];
        curCap = curCap + attris[0];

        curAtkText.text = curAtk.ToString();
        curDefText.text = curDef.ToString();
        curSpdText.text = curSpd.ToString();
        curFelText.text = curFel.ToString();
        curCapText.text = curCap.ToString();
    }

    public int CountDownProduct()
    {
        productCountDemand = productCountDemand - 1;
        proCounDemText.text = productCountDemand.ToString();
        return productCountDemand;
    }

    public void ClearMachines()
    {
        curAtk = 0;
        curDef = 0;
        curSpd = 0;
        curFel = 0;
        curCap = oProcCap;
        RefreshOrderStats(new int[5] { 0, 0, 0, 0, 0 });
        /*
        GameObject[] allIcon = maIconPanel.GetComponentsInChildren<GameObject>();
        if(allIcon != null)
        {
            foreach (GameObject c in allIcon)
            {
                Destroy(c);
            }
        }
        */
    }

    public void AddNewMachineIcon(Sprite machineIcon)
    {
        icons.Add(machineIcon);
        GameObject newIcon = Instantiate(sampleIcon) as GameObject;
        //allIcons.Add(newIcon);
        newIcon.name = "icon_" + icons.Count.ToString();
        newIcon.GetComponent<Image>().sprite = machineIcon;
        newIcon.transform.SetParent(maIconPanel, false);        
        iconPosOffset = 35f * icons.Count - 35f;
        newIcon.transform.localPosition = new Vector3(newIcon.transform.localPosition.x + iconPosOffset, newIcon.transform.localPosition.y, newIcon.transform.localPosition.z);
        
        
    }
}
