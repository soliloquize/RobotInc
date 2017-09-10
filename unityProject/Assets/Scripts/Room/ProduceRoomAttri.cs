using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceRoomAttri : MonoBehaviour {

    public int roomId; //房间id，所有生产间为2000+
    //public string roomName; //房间名字
    public float efficience; //生产所需秒数
    public int produceCost; //生产所需电力
    public int idleCost; //闲置所需电力
    public float startToRunCost; //开始/停止运转所需时间,更换配置（重启）=*2，关停/重开=*1
    //public int buildCost; //建造所需资源,钱
    public int prodcutAttriBonus; //生产额外加成,敏捷加成
    //public float roomLength, roomWide;//半径
    //public int defaultDirection = 0;

    public void ChangeCurrentState(int s)
    {
        //用switch在idle\producting\stop\maintain中切换，并调取相应动画
        
    }

    public void ProducingPath()
    {
        //房间内生产部件的移动轨迹

    }

    public void ProductNewPart()
    {
        //调取要求生产次数，按次开始生产动画
        //idle-生产-idle-生产

    }
}
