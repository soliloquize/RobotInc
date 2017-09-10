using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaceRoomToNode : MonoBehaviour {

    private List<Vector2> idleNodeList = new List<Vector2>();//当前空闲node点位
    private List<Vector2> roomPosList = new List<Vector2>();//已有room所覆盖的点位
    private List<Vector2> currentCoveredNode = new List<Vector2>();
    private List<Vector2> currentRoomNodePos = new List<Vector2>();
    private List<Vector2> currentRoomPos = new List<Vector2>();
    private int curId; //当前新建room的id

    private float cellSize = 1;
    public float curX, curY; //鼠标当前坐标位置（格子
    public UnityEventQueueSystem eventsystem;
    public GameObject chosenRoom;
    public GameObject curRoomBlueprint;
    public bool isBuilding = false;
    private int groundLayer = 8;

    //public InputField lT, wT, centerXT,centerYT; //输入文本,待删
    //public Text posResult, curRoomDir; //输出文本，待删
    private float l,w; //房间以centerX，y为中心点，l，w为长与宽， 
    public int roomDirection = 0; //默认水平
    private Camera editorCamera;
    public GameObject nodeTest; //测试用点，待删
    public GameObject roomSpotTest;
    public GameObject coveredNodeTest;

    void Start()
    {
        editorCamera = GameObject.FindWithTag("EditorCamera").GetComponent<Camera>();
        //NewFactoryWithCompositeRoom();
    }

    public void NewFactoryWithCompositeRoom()
    {
        chosenRoom = (GameObject)Resources.Load("Prefabs/RoomPrefab/CompositeRoom", typeof(GameObject));
        curRoomBlueprint = Instantiate(chosenRoom, new Vector3(curX, curY, 0), chosenRoom.transform.rotation) as GameObject;
        l = chosenRoom.GetComponent<RoomBuildingAttri>().roomLength;
        w = chosenRoom.GetComponent<RoomBuildingAttri>().roomWide;
        curX = 0;
        curY = 0;
        //CheckCurrentPosAvailability();
        CheckNodePosAvailability();
        BuildChosenRoomAtCurrentPos();
        //Destroy(chosenRoom);
    }

    //共有两种方向水平0，竖直1,待删
    //public void GetAllCoordOfNewRoom() 
    //{
    //    posResult.text = ""; //重置一下
    //    //读取输入的4个值,todo 读取room.attri
    //    l = float.Parse(lT.text);
    //    w = float.Parse(wT.text);
    //    //centerX = float.Parse(centerXT.text);
    //    //centerY = float.Parse(centerYT.text);
    //    if (roomDirection == 0) //水平矩形，x1y1为左下，2为右上
    //    {
    //        /*
    //        x1 = -l;
    //        y1 = -w;
    //        x2 = l;
    //        y2 = w;

    //        for(int i=0; i <= (x2 - x1); i++)
    //        {
    //            for(int j=0; j<= (y2 - y1); j++)
    //            {
    //                posResult.text += ("(" + (centerX + x1 + i) + "," + (centerY + y1 + ) + ")");
    //            }
    //        }
    //        */
    //        for(int i =0; i <= l * 2; i++)
    //        {
    //            for (int j=0; j<= w * 2; j++)
    //            {
    //                //posResult.text += ("(" + (centerX - l + i) + "," + (centerY - w + j ) + ") ， ");

    //            }
    //        }

    //    }else if(roomDirection == 1)//竖直矩形，
    //    {
    //        for (int i = 0; i <= w * 2; i++)
    //        {
    //            for (int j = 0; j <= l * 2; j++)
    //            {
    //                //posResult.text += ("(" + (centerX - w + i) + "," + (centerY -l + j) + ") ， ");
    //            }
    //        }
    //    }
    //    //TODO:将所有位置add到RoomPosList
    //    //TODO:将涉及到的node位置添加到闲置/已连接nodelist
    //}

    public void ChangeRoomDirection() //在0和1之间切换，并旋转蓝图方向，也可以换成switch
    {
        if (roomDirection == 0)
        {
            roomDirection = 1;
            curRoomBlueprint.transform.localEulerAngles = new Vector3(0,0,90);
           
        }else if (roomDirection == 1)
        {
            roomDirection = 0;
            curRoomBlueprint.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void InitRoomBlueprintToMousePos(string roomPrefabName) //点击按钮，当满足建造条件时，生成蓝图room并跟随鼠标移动
    {
        if(chosenRoom != null)
        {
            Destroy(curRoomBlueprint);
        }
        chosenRoom = (GameObject)Resources.Load( "Prefabs/RoomPrefab/" + roomPrefabName, typeof(GameObject));
        curRoomBlueprint = Instantiate(chosenRoom, new Vector3(curX, curY, 0), chosenRoom.transform.rotation) as GameObject;
        l = chosenRoom.GetComponent<RoomBuildingAttri>().roomLength;
        w = chosenRoom.GetComponent<RoomBuildingAttri>().roomWide;
        roomDirection = chosenRoom.GetComponent<RoomBuildingAttri>().defaultDirection;
        isBuilding = true;       
    }

    private void CheckCurrentPosAvailability() //计算当前房间位置所占的pos
    {
        currentRoomPos.Clear();
        float x, y;
        if (roomDirection == 0)
        {
            x = l;
            y = w;
        }else //if (roomDirection == 1)
        {
            x = w;
            y = l;
        }
        for (int i = 0; i < x * 2-1; i++)
        {
            for (int j = 0; j < y * 2-1; j++)
            {
                currentRoomPos.Add(new Vector2(curX - x + i+1, curY - y + j+1));
            }
        }
    }

    private bool CheckNodePosAvailability() //计算当前房间是否和已有房间链接、是否有重叠；返回true则当前位置可建立房间
    {
        currentRoomNodePos.Clear();
        currentCoveredNode.Clear();
        float x, y; 
        if (roomDirection == 0)
        {
            x = l;
            y = w;
        }
        else //if (roomDirection == 1)
        {
            x = w;
            y = l;
        }
        for (int i = 0; i < x * 2; i++) 
        {
            //GameObject newnodeA = Instantiate(nodeTest, new Vector3(curX - x + i, curY + y, 0), transform.rotation);
            //newnodeA.name = "newNodeA" + i.ToString();
            //GameObject newnodeB = Instantiate(nodeTest, new Vector3(curX + x - i, curY - y, 0), transform.rotation);
            //newnodeB.name = "newNodeB" + i.ToString();
            currentRoomNodePos.Add(new Vector2(curX - x + i, curY + y));
            currentRoomNodePos.Add(new Vector2(curX + x - i, curY - y));
        }
        for (int j = 0; j < y * 2; j++)
        {
            //GameObject newnodeC = Instantiate(nodeTest, new Vector3(curX - x, curY - y + j, 0), transform.rotation);
            //newnodeC.name = "newNodeC" + j.ToString();
            //GameObject newnodeD = Instantiate(nodeTest, new Vector3(curX + x, curY + y - j, 0), transform.rotation);
            //newnodeD.name = "newNodeD" + j.ToString();
            currentRoomNodePos.Add(new Vector2(curX - x, curY - y + j));
            currentRoomNodePos.Add(new Vector2(curX + x, curY + y - j));
        }
        foreach (Vector2 n in roomPosList) //周圈位置是否与已有房间位置冲突（是否重叠
        {
            if (currentRoomNodePos.Contains(n))
            {
                return false;
            }
        }
        foreach (Vector2 node in idleNodeList) //周圈位置是否有与可用节点相同位置，并记录个数
        {
            if (currentRoomNodePos.Contains(node))
            {
                currentCoveredNode.Add(node);
                //print(currentCoveredNode.Count);
                //GameObject covNode = Instantiate(coveredNodeTest, new Vector3(node.x, node.y, 0), transform.rotation);
                currentRoomNodePos.Remove(node);
            }
        }
        if(currentCoveredNode.Count >= 2) //连接节点个数大于等于2时，视为可链接
        {
            return true;
        }
        return false;
    }

    private void BuildChosenRoomAtCurrentPos() //在当前位置（检测可放置）生成所选Room，加入map中room列表
    {
        GameObject newRoom = Instantiate(chosenRoom, new Vector3(curX, curY, 0), curRoomBlueprint.transform.rotation) as GameObject;
        newRoom.name = chosenRoom.GetComponent<RoomBuildingAttri>().roomName + curId;
        curId = curId + 1;
        CheckCurrentPosAvailability();
        foreach(Vector2 n in currentCoveredNode)
        {
            idleNodeList.Remove(n);
        }
        foreach(Vector2 m in currentRoomNodePos)
        {
            idleNodeList.Add(m);
        }
        foreach(Vector2 r in currentRoomPos)
        {
            roomPosList.Add(r);
        }
        Destroy(curRoomBlueprint);
        isBuilding = false;
    }

    public void loadExistedRoomPosList() // 载入初始图中已覆盖位置
    {
        
    }

    private void CombineCompositeRooms() //合并组合室
    {

    }

    public void CheckResourceAvailability(string roomPrefabName) //所选房间是否满足建造要求
    {

    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { }//指向UI时不进行计算
        else
        {
            Ray r = editorCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, 1000f))
            {
                curX = Mathf.Round(hit.point.x / cellSize) * cellSize;
                curY = Mathf.Round(hit.point.y / cellSize) * cellSize;
                int curLayer = hit.collider.gameObject.layer;
                if (isBuilding == true)
                {
                    curRoomBlueprint.transform.position = new Vector3(curX, curY, 0);
                    if (Input.GetKeyUp("q") || Input.GetKeyUp("e"))
                    {
                        ChangeRoomDirection();
                    }
                    if (/*CheckCurrentPosAvailability() == true &&*/ CheckNodePosAvailability() == true)
                    {
                        curRoomBlueprint.GetComponent<SpriteRenderer>().color = Color.green;
                        if (Input.GetMouseButtonDown(0))
                        {
                            BuildChosenRoomAtCurrentPos();
                        }
                    }
                    else
                    {
                        curRoomBlueprint.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                    if (Input.GetMouseButtonDown(1))
                    {
                        Destroy(curRoomBlueprint);
                        isBuilding = false;
                    }
                }                              
            }
        }
    }
}
