using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _producingObject : MonoBehaviour {

    public List<Vector3> pathDirections;
    public List<Sprite> allPartSprites;
    public int curMaCount;
    public int machineLeft;
    public Vector3 nextPosToGo;
    public float transSpeed = 0.2f;

	void Update () {
        if (Mathf.Abs(Vector3.Distance(transform.localPosition, nextPosToGo)) <= 0.2f) //如果距离很近了，判定为到达，播动画，换下一个机器位置
        {
            if (machineLeft - curMaCount == 1) //如果没有没走的机器了，就不走了
            {
                Destroy(this.gameObject);
            }
            else
            {
                //Invoke("ChangeingOutfit" , 2f ); // 等2s，增加配件；这边最后要处理成从machine读取根据动画移动的路径来走，并且与机器动画时间吻合
                Invoke("ChangeDestination" , 2f); //等2s，换方向；这边最后要与上面的动画链接。上方机器动画播放完毕，路径走到出口，此处接从出口顺着传送带走的destination
                //ChangeDestination();
            }
                    
        }
        else //向下一个机器位置移动； TODO：指定线路后，顺着指定线路移动
        {
            //Vector3 voc = nextPosToGo - transform.localPosition;
            //transform.localPosition = Vector3.SmoothDamp(transform.localPosition, nextPosToGo,ref voc ,transSpeed);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, nextPosToGo, transSpeed * Time.deltaTime);

        }

	}

    public void MoveDirections(List<Vector3> dirs)
    {
        pathDirections = dirs;
        nextPosToGo = dirs[0];
        machineLeft = dirs.Count;
        //print(nextPosToGo);
        //print(transform.position);
        print(pathDirections[0] + "," + pathDirections[1] + "," + pathDirections[2] + "," + pathDirections[3]);
        
    }

    public void ChangingOutfit()
    {

    }

    void ChangeDestination()
    {

        curMaCount = curMaCount + 1;
        nextPosToGo = pathDirections[curMaCount];
        print("the next position is " + nextPosToGo + " , and current position is " + transform.localPosition + ", and the current machine count is " + curMaCount);
    }



}
