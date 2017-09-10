using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour {

    //玩家相关设定值
    public string playerName;

    //工厂所含物品列表
    //public List<ProduceRoom> prodRoomList;
    //public List<CompRoom> compRoomList;

    //工厂当前基本状态值
    public int curCash;
    public float curTime;
    private float curEnergyPoint;
    private float curEnergyCostInMin;
    private int curFamePoint;

    //工厂研究、解锁状态
    //private List<Tech> unlockedTech;
    

    //工厂各类记录值
    private int orderFinishedCount;
    private int orderFailedCount;

    private float factoryValue;

}
