using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RoomInMapInfo : IComparable<RoomInMapInfo> {

    public int roomid;
    public GameObject roomObj;
    public float idleEleCost;
    public float prodEleCost;
    public int currentState;

    
    public RoomInMapInfo(int id, GameObject obj, float iCost, float pCost,int s)
    {
        roomid = id;
        roomObj = obj;
        idleEleCost = iCost;
        prodEleCost = pCost;
        currentState = s;
    }
    
    public int CompareTo(RoomInMapInfo other)
    {
        if(other == null)
        {
            return 1;
        }
        else
        {
            return roomid;
        }

    }

}
