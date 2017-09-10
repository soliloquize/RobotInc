using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBuildingAttri : MonoBehaviour {
    //public int roomId; //房间id，所有生产间为2000+
    public string roomName; //房间名字
    public float buildTime; //建造所需时间
    public int buildCost; //建造所需资源,钱
    public float roomLength, roomWide;//半径
    public int defaultDirection = 0;
}
