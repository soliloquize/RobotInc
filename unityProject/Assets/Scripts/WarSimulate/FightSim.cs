using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightSim : MonoBehaviour {

    public float HpA, AtkA, DefA, SpdA, HpB, AtkB, DefB, SpdB;
    public InputField HpAText, AtkAText, DefAText, SpdAText, HpBText, AtkBText, DefBText, SpdBText;
    public float powerA, powerB;
    public Text fightResult;

	public void SimFight() {
        HpA = float.Parse(HpAText.text);
        AtkA = float.Parse(AtkAText.text);
        DefA = float.Parse(DefAText.text);
        SpdA = float.Parse(SpdAText.text);
        HpB = float.Parse(HpBText.text);
        AtkB = float.Parse(AtkBText.text);
        DefB = float.Parse(DefBText.text);
        SpdB = float.Parse(SpdBText.text);

        powerA = HpA * (AtkA - DefB) * SpdA;
        powerB = HpB * (AtkB - DefA) * SpdB;

        EndFight(powerA, powerB);
	}

    public void SimFightBasedonLv()
    {
        for (int i = 1; i < 200; ++i)
        {
            int HpALv = int.Parse(HpAText.text);
            int AtkALv = int.Parse(AtkAText.text);
            int DefALv = int.Parse(DefAText.text);
            int SpdALv = int.Parse(SpdAText.text);
            int HpBLv = int.Parse(HpBText.text);
            int AtkBLv = int.Parse(AtkBText.text);
            int DefBLv = int.Parse(DefBText.text);
            int SpdBLv = int.Parse(SpdBText.text);

            HpA = 50f * HpALv * HpALv * 3f * ( 1f + Random.Range(-0.1f, 0.1f));
            AtkA = 8f * AtkALv * AtkALv * 2f * (1f + Random.Range(-0.1f, 0.1f));
            DefA = 8f * DefALv * DefALv * 2f * (1f + Random.Range(-0.1f, 0.1f));
            SpdA = (1f + (SpdALv - 1f) * 0.5f) * HpALv * 2f * (1f + Random.Range(-0.1f, 0.1f));
            HpB = 50f * HpBLv * HpBLv * 3f * (1f + Random.Range(-0.1f, 0.1f));
            AtkB = 8f * AtkBLv * AtkBLv * 2f * (1f + Random.Range(-0.1f, 0.1f));
            DefB = 8f * DefBLv * DefBLv * 2f * (1f + Random.Range(-0.1f, 0.1f));
            SpdB = (1f + (SpdBLv - 1f) * 0.5f) * HpALv * 2f * (1f + Random.Range(-0.1f, 0.1f));

            if ((AtkA - DefB) <= 0)
            {
                powerA = HpA * SpdA;
            }else
            {
                powerA = HpA * (AtkA - DefB) * SpdA;
            }
           
            if ((AtkB - DefA) <= 0)
            {
                powerB = HpB * SpdB;
            }
            else
            {
                powerB = HpB * (AtkB - DefA) * SpdB;
            }
            

            EndFight(powerA, powerB);
        }
    }

    public void EndFight( float a, float b)
    {
        if (a > b)
        {
            fightResult.text = "Group A Win!";
            print("a wins");
        }else if ( a < b)
        {
            fightResult.text = "Group B Win!";
            print("b wins");
        }else if (a == b)
        {
            fightResult.text = "Draw!";
            //print("no winner");
        }
    }
}
