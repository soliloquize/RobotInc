using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridFramework.Grids;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlaceObject : MonoBehaviour {

    [System.Serializable]
    public class ProdLinePart
    {
        public int itemId;
        public GameObject itemObj;
        public int type;
        //public bool isConnected;
        public int inId;
        public int outId;
        public Vector2 itemPos;
    }

    public EventSystem eventsystem;
    public GameObject gridManager;
    private int groundLayer = 8; //与layer中预设相符，不要任意修改值
    private int itemLayer = 9;

    public GameObject chosenPart = null; //被选中的，跟随鼠标走的物体；
    public GameObject placePart = null; //实际放置时新生成的物体， 新增时=被选中物体，编辑时不使用此值
    public bool isPlacingPart = false;

    private float curX = 0, curZ = 0; //当前鼠标所在坐标位置（取整），round（hit）
    private float originX, originZ; //编辑物体时，记忆物体的改动前位置，若取消修改，则返回此处

    //当前state值,0=指向空地且未有选中物体；1=指向已有物体；2=正在新建物体；3=正在放置拾起的已有物体；4=删除模式中
    public int curState = 0; 

    public List<Vector2> curLineList = new List<Vector2>(); //当前编辑中线路所占格子list
    public List<ProdLinePart> curLinePartList = new List<ProdLinePart>(); //当前编辑中线路已有部件信息
    private int curId = 0;
    private int startId = -1;
    public List<Vector2> mapLineList = new List<Vector2>(); //地图中所有格子信息（是否已占）

    public GameObject failToBuildPanel;
    //public GameObject secceedPanel;
    
    void Start () {

    }

	void Update () {
        if (EventSystem.current.IsPointerOverGameObject()) { }//指向UI时不进行计算
        else
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, 100f))
            {
                curX = Mathf.Round(hit.point.x);
                curZ = Mathf.Round(hit.point.z);
                int curLayer = hit.collider.gameObject.layer;
                //LayerMask curLayerMask = 1 << curLayer;
                //print("指向的layer" + curLayer + ", 目前的state是 " + curState);
                switch (curState)
                {
                    case 0:
                        if (curLayer == itemLayer) {curState = 1;}
                        break;
                    case 1:
                        if (curLayer == groundLayer) {curState = 0;}
                        break;
                    case 2:
                        chosenPart.transform.position = new Vector3(curX, 0, curZ);
                        break;
                    case 3:
                        chosenPart.transform.position = new Vector3(curX, 0, curZ);
                        break;
                    case 4:
                        break;
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                switch (curState)
                {
                    case 0:
                        break;
                    case 1:
                        PickExistedPart(hit.transform.gameObject);
                        originX = chosenPart.transform.position.x;
                        originZ = chosenPart.transform.position.z;
                        curState = 3;
                        break;
                    case 2:
                        if (CheckPosition(curX, curZ))
                        {
                            PlaceNewPart(chosenPart);
                        }
                        break;
                    case 3:
                        if (CheckPosition(curX, curZ))
                        {
                            PlaceExistedPart();
                            curState = 0;
                        }
                        break;
                    case 4:
                        break;
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                switch (curState)
                {
                    case 0:
                        break;
                    case 1:

                        break;
                    case 2:
                        Destroy(chosenPart);
                        curState = 0;
                        break;
                    case 3:
                        chosenPart.transform.position = new Vector3(originX, 0, originZ);
                        chosenPart = null;
                        curState = 0;
                        break;
                    case 4:
                        break;
                }
            }
        }
    }

    public void ChageCurrentState(int s)
    {
        curState = s;
    }

    public bool CheckPosition(float checkX, float checkZ)
    {
        if (curLineList.Contains(new Vector2(checkX, checkZ)) || mapLineList.Contains(new Vector2(checkX, checkZ)))
        {
            return false;
        }
        else
        {
            return true;
        }
        
    }

    public void CheckConnection(int id)
    {

    }

    public void ChooseNewPart(GameObject chosenObj)
    {
        if (chosenPart != null)
        {
            Destroy(chosenPart);
        }
        chosenPart = Instantiate(chosenObj, new Vector3(curX, 0, curZ), chosenObj.transform.rotation) as GameObject;
        chosenPart.name = chosenObj.name;
        //isPlacingPart = true;
        //print(chosenPart.name);
        curState = 2;
    }
    public void PickExistedPart(GameObject existedObj)
    {
        if (chosenPart != null)
        {
            Destroy(chosenPart);
        }
        chosenPart = existedObj;
    }

    void PlaceNewPart(GameObject chosenObj)
    {
        GameObject newPart = Instantiate(chosenObj, new Vector3(curX, 0, curZ), chosenObj.transform.rotation) as GameObject;
        newPart.name = chosenPart.name + curId.ToString();
        ProdLinePart curPart = new ProdLinePart();
        curPart.itemId = curId;
        curPart.itemObj = newPart;
        curPart.itemPos = new Vector2(curX, curZ);
        switch (chosenPart.name){
            case "pt_transfer":
                curPart.type = 0;
                break;
            case "pt_coremaker":
                curPart.type = 1;
                break;
            case "pt_skeletonmaker":
                curPart.type = 2;
                break;
            case "pt_musclemaker":
                curPart.type = 3;
                break;
            case "pt_painter":
                curPart.type = 4;
                break;
            case "pt_combiner":
                curPart.type = 5;
                break;
        }
        print("--------start to find in and out for item " + curPart.itemId + "--------------");
        if (curPart.type != 1) //不是core的部分，寻找是否有input
        {
            curPart.inId = GetInItemId(curPart.itemId, curPart.type, curPart.itemPos);
            print( curPart.itemId + "'s in id is ----------------- " + curPart.inId);
        }
        else
        {
            curPart.inId = -1;
        }
        if(curPart.type != 5) //不是combine的部分，寻找是否有output
        {
            curPart.outId = GetOutItemId(curPart.itemId, curPart.type, curPart.itemPos);
            print(curPart.itemId + "'s out id is ----------------- " + curPart.outId);
        }
        else
        {
            curPart.outId = -1;
        }
        curLinePartList.Add(curPart);
        curLineList.Add(new Vector2 (curX,curZ));
        curId++;
        if (chosenPart.name != "pt_transfer")
        {
            Destroy(chosenPart);
            curState = 0;
        }
        if(curPart.type == 1)
        {
            startId = curPart.itemId;
            print("the start item's id is " + startId);
        }
    }

    int GetInItemId(int id, int type, Vector2  curPos)
    {
        List<Vector2> dir = new List<Vector2> { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
        List<int> connectedObj = new List<int>();
        int hasConnection = 0;

        foreach (Vector2 d in dir) //对相邻的四个方向找一遍，每有一个相邻的且outId没有的，就将type加到obj列表里，并且用hasConnection计数有几个连接
        {
            //print("looking for obj on direction : " + d);
            if (curLineList.Contains(curPos + d))
            {
                //print("is looking of position " + curPos + "+" + d);
                ProdLinePart thisItem = curLinePartList.Find(item => item.itemPos == curPos + d);
                if (thisItem.outId == -1 && thisItem.inId != id)
                {
                    //print("find a not connected item around");
                    if (type == 0) //如果是trans，则所有的物体都可能是in
                    {
                        connectedObj.Add(thisItem.itemId);
                        hasConnection++;
                        //print("current part is a trans, and find a connectable item is " + thisItem.itemId);
                    }
                    else //如果是机器部件，则必须trans才可以in
                    {
                        if (thisItem.type == 0)
                        {
                            connectedObj.Add(thisItem.itemId);
                            hasConnection++;
                            //print("current part is not a trans, and find a connectable item is " + thisItem.itemId);
                        }
                    }
                }
            }
           //print("there are " + hasConnection + " objects around,");     
        }
        if (hasConnection == 0) //没有连接时候，返回-1
        {
            return -1;
        }
        else if (hasConnection == 1) //有唯一连接时，返回id
        {
            ProdLinePart thisItem = curLinePartList.Find(item => item.itemId == connectedObj[0]);
            thisItem.outId = id;
            //print(thisItem.itemId + "'s out id is " + id);
            return connectedObj[0];
        }
        else if (hasConnection > 1) //有大于一个连接物时，取id最小（最早放置的）
        {
            int min = connectedObj[0];
            for (int i = 0; i < connectedObj.Count; i++)
            {
                if (min > connectedObj[i])
                {
                    //print("the connected obj is " + connectedObj[i]);
                    min = connectedObj[i];
                }
            }
            //print("choose the obj " + min + " as connected one");
            ProdLinePart thisItem = curLinePartList.Find(item => item.itemId == min);
            thisItem.outId = id;
            print(thisItem.itemId + "'s out id is " + id);
            return min;
        }
        return -1;
    }

    int GetOutItemId(int id, int type, Vector2 curPos)
    {
        List<Vector2> dir = new List<Vector2> { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
        List<int> connectedObj = new List<int>();
        int hasConnection = 0;

        foreach (Vector2 d in dir) //对相邻的四个方向找一遍，每有一个相邻的且outId没有的，就将type加到obj列表里，并且用hasConnection计数有几个连接
        {
            //print("looking for obj on direction : " + d);
            if (curLineList.Contains(curPos + d))
            {
                //print("is looking of position " + curPos + "+" + d);
                ProdLinePart thisItem = curLinePartList.Find(item => item.itemPos == curPos + d);
                print("find item here. the item is " + thisItem.itemId + ", and it's in id is " + thisItem.inId + ", and this item's out id is " + thisItem.outId);
                if (thisItem.inId == -1 && thisItem.outId != id)
                {
                    //print("find this is a not connected item");
                    if (type == 0) //如果是trans，则所有的物体都可能是in
                    {
                        connectedObj.Add(thisItem.itemId);
                        hasConnection++;
                        //print("current part is a trans, and find a connectable item is " + thisItem.itemId);
                    }
                    else //如果是机器部件，则必须trans才可以in
                    {
                        if (thisItem.type == 0)
                        {
                            connectedObj.Add(thisItem.itemId);
                            hasConnection++;
                            //print("current part is not a trans, and find a connectable item is " + thisItem.itemId);
                        }
                    }
                }
            }
            //print("there are " + hasConnection + " objects around,");     
        }
        if (hasConnection == 0) //没有连接时候，返回-1
        {
            return -1;
        }
        else if (hasConnection == 1) //有唯一连接时，返回id
        {
            ProdLinePart thisItem = curLinePartList.Find(item => item.itemId == connectedObj[0]);
            thisItem.inId = id;
            //print(thisItem.itemId + "'s in id is" + id);
            return connectedObj[0];
        }
        else if (hasConnection > 1) //有大于一个连接物时，取id最小（最早放置的）
        {
            int min = connectedObj[0];
            for (int i = 0; i < connectedObj.Count; i++)
            {
                if (min > connectedObj[i])
                {
                    min = connectedObj[i];
                }
            }
            ProdLinePart thisItem = curLinePartList.Find(item => item.itemId == min);
            thisItem.inId = id;
            //print(thisItem.itemId + "'s in id is" + id);
            return min;
        }
        return -1;
    }

    public void PlaceExistedPart() //需要加上对in和out的修改
    {
        curLineList.Remove(new Vector2(originX, originZ));
        curLineList.Add(new Vector2(curX, curZ));
        ProdLinePart changing = curLinePartList.Find(item => item.itemPos == new Vector2(originX, originZ));
        changing.itemPos = new Vector2(curX, curZ);
        chosenPart = null;
    }

    public void RemoveExistedPart(GameObject existedObj)
    {

    }

    public void StartCheckLine()
    {
        print("start to check the line!");
        if (startId != -1) {
            ProdLinePart startPart = curLinePartList.Find(item => item.itemId == startId);
            print("find start part, and it's id is " + startPart.itemId+", and it's type is " + startPart.type+", and it's outid is " + startPart.outId);
            if (CheckProdLine(startPart, 1))
            {
                //secceedPanel.SetActive(true);
                print("succeed!");
            }
            else
            {
                failToBuildPanel.SetActive(true);
            }
        }
        else
        {
            print("no core built");
        }


    }

        bool CheckProdLine(ProdLinePart curEndPart, int curLineType)
    {
        print("------start to check-----" + curEndPart.itemId + " , " + curLineType);
        if(curEndPart.type == 5 )
        {
            if(curLineType > 1)
            {
                print("it's over!");
                return true;
            }
        }
        else
        {
            if (curEndPart.outId != -1)
            {
                print("current part out id is not -1");
                ProdLinePart nextPart = curLinePartList.Find(item => item.itemId == curEndPart.outId);
                print("find next part in list is " + nextPart.itemId);
                if (curEndPart.type == 0)
                {
                    print("current item is a transfer");
                    if (nextPart.type == 0)
                    {
                        print("next part is also a transfer");
                        return CheckProdLine(nextPart, curLineType);
                    }
                    else
                    {
                        print("next part is not a transfer");
                        if (curLineType < nextPart.type) //机器必须按照顺序的做法
                        {
                            print(" current type is " + curLineType + " , and next type is " + nextPart.type);
                            curLineType = nextPart.type;
                            return CheckProdLine(nextPart, curLineType);
                        }
                    }
                }
                else // current part type != 0
                {
                    if (nextPart.type == 0)
                    {
                        return CheckProdLine(nextPart, curLineType);
                    }
                }
            }  
        }
        return false;

    }

    

}
