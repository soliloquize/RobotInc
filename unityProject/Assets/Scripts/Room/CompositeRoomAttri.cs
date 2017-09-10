using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeRoomAttri : MonoBehaviour {

    public int roomId = 1001;
    //public string roomName; //房间名字
    //public float efficience = 30f; //生产所需秒数
    public int produceCost = 50; //生产所需电力/min
    public int idleCost = 10; //闲置所需电力
    public float startToRunCost = 5f; //开始/停止运转所需时间,更换配置（重启）=*2，关停/重开=*1
    //public int buildCost = 500; //建造所需资源,钱
    //public int prodcutAttriBonus = 2; //生产额外加成,敏捷加成
    //public float roomLength = 4, roomWide = 4;
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
