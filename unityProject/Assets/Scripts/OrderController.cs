using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderController : MonoBehaviour {

    public List<MachineAttribute> usingMachine =  new List<MachineAttribute>(); 
    public List<MachineAttribute> mapList = new List<MachineAttribute>(); //目前直接把所有machine拖进去
    public List<Vector3> orderProLine;
    public List<int> orderMachineLine;
    public GameObject product;
    public Transform productParent;

    public int atk, def, spd, fel = 0; //当前已配置属性
    public int cap; // 当前剩余可配置数
    public int produceRemain; //订单剩余未生产数（未生成core）

    public int curMachineCount;
    public bool isAddingNewMachineToLine = false;

    bool hasStartPos = false;
    Vector3 startPos;

    public GameObject orderSettingPanel;

	void Update () {
		if(isAddingNewMachineToLine == true)
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, 1000f, 512))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject h = GameObject.Find(hit.transform.name);
                    AddMachineToOrder(h.GetComponent<MachineAttribute>());
                }
            }
        }
	}

    //整体思路： 点击界面为order指定线路，弹出配置的窗口；
    //  在窗口中，点击+按钮（addMachineBtn），弹出可加入的machine图标列表，点选加入的machine；
    //  点选后，该machine即加入list
    //  后方继续出现新的+按钮，点击后再次弹出可加入的machine图表列表；
    //  每次弹出列表需要检测已进行的加工次数（剩余加工次数）和当前已进行的加工类别（类别有顺序不可逆）

    public void isAddingNewMachine()
    {
        isAddingNewMachineToLine = true;
    }

    void StructureLineForOrder()
    {
        foreach (MachineAttribute m in usingMachine)
        {
            Vector3 machinePos = m.transform.localPosition;
            orderProLine.Add(machinePos);
            int t = m.machineType;
            orderMachineLine.Add(t);
            //print(machinePos);
        }
        
    }

    public void StartToProduce() //为新的order开始一个新的list 
    {
        StructureLineForOrder();

        InvokeRepeating("InsNewProduct", 1f, 2f);
        
    }

    void InsNewProduct()
    {
        GameObject newProduct = Instantiate(product,startPos,product.transform.localRotation) as GameObject;
        newProduct.transform.SetParent(productParent);
        newProduct.GetComponent<_producingObject>().MoveDirections(orderProLine);
        //newProduct.GetComponent<_producingObject>().ChangingOutfit(orderMachineLine);
        produceRemain = orderSettingPanel.GetComponent<OrderGenerator>().CountDownProduct();
        if (produceRemain == 0)
        {
            this.CancelInvoke();
        }
    }

    public void AddMachineToOrder(MachineAttribute newMachine)
    {
        usingMachine.Add(newMachine);
        orderSettingPanel.GetComponent<OrderGenerator>().RefreshOrderStats(newMachine.attriValue);
        orderSettingPanel.GetComponent<OrderGenerator>().AddNewMachineIcon(newMachine.icon);
        if(newMachine.machineType == 0 && hasStartPos == false)
        {
            Vector3 startMaPos = newMachine.transform.position;
            startPos = new Vector3(startMaPos.x, 0, startMaPos.z);
            hasStartPos = true;
        }
    }

    public void StartNewOrder()
    {
        hasStartPos = false;
    }



}
