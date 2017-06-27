using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProdLineTest : MonoBehaviour {

    public int[] lineSketch = new int[10] {1,0,1,0,1,0,1,0,1,0 };
    public bool testMe = false;
    public bool goodConnection = false;


	void Update () {
		if (testMe == true)
        {
            print ( TestLine(0, lineSketch[0], 0));
            testMe = false;
        }
	}


    int TestLine (int n, int type, int a)
    {
        print("start test, the 3 var are: " + n + "," + type + "," + a);
        if (a == 5 && type == 2)
        {
            print("在这停顿！" + a );

        }else
        {
            if (lineSketch[n] == 0)
            {
                if (lineSketch[n + 1] == 0)
                {
                    int m = n + 1;
                    print("当前是传送带，且下个还是传送带: " + n + "," + type + "," + a);
                    return TestLine(m, 0, a);
                }
                else if (lineSketch[n + 1] == 1)
                {
                    int m = n + 1;
                    int b = a + 1;
                    print("当前是传送带，下个是机器: " + n + "," + type + "," + a);
                    return TestLine(m, 1, b);
                }
                else
                {
                    print("下一个不是传送带也不是机器: " + n + "," + type + "," + a);
                }
            }
            else if (lineSketch[n] == 1)
            {
                if (lineSketch[n + 1] == 0)
                {
                    int m = n + 1;
                    print("当前是机器，且下个是传送带: " + n + "," + type + "," + a);
                    return TestLine(m, 0, a);
                }
                else
                {
                    print("当前是机器，下个不是传送带: " + n + "," + type + "," + a);
                }
            }
            else if (lineSketch[n] == 2)
            {
                int m = n + 1;
                type = 2;
                return TestLine(m, type, a);            }
            else
            {
                print("当前不是传送带也不是机器: " + n + "," + type + "," + a);
            }
            print("不知为什么直接结束了" + n + "," + type + "," + a);
        }
        int c = a;
        print("结束在了else后面" + n + "," + type + "," + c);
        return c;
    }

}
