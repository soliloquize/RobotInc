using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlInPlayMode : MonoBehaviour {

    private Camera mainCam;
    public float moveSpd = 10f;
    public float scrollSpd = 300f;
    public float maxView = 25f;
    public float minView = 2f;

    public bool isInEditMode = false;

	// Use this for initialization
	void Start () {
        mainCam = gameObject.GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isInEditMode == false)
        {
            if (Input.GetKeyDown("q"))
            {
                gameObject.transform.eulerAngles += new Vector3(0, -45f, 0);
            }

            if (Input.GetKeyDown("e"))
            {
                gameObject.transform.eulerAngles += new Vector3(0, 45f, 0);
            }

        }
        else
        {

        }


        if (Input.GetKey("w"))
        {
            gameObject.transform.localPosition += transform.forward * moveSpd * Time.deltaTime;

        }
        if (Input.GetKey("s"))
        {
            gameObject.transform.localPosition -= transform.forward * moveSpd * Time.deltaTime;

        }
        if (Input.GetKey("a"))
        {
            gameObject.transform.localPosition -= transform.right * moveSpd * Time.deltaTime;

        }
        if (Input.GetKey("d"))
        {
            gameObject.transform.localPosition += transform.right * moveSpd * Time.deltaTime;

        }

        if (Input.GetAxis("Mouse ScrollWheel")>0)
        {
            if (mainCam.transform.localPosition.y > minView)
            {
                mainCam.transform.localPosition += new Vector3(0f, -1f, 0) * scrollSpd * Time.deltaTime;
                moveSpd = (mainCam.transform.localPosition.y - 2f) * 0.5f + 7f; //相机越远，速度越大
            }
        }else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(mainCam.transform.localPosition.y < maxView)
            {
                mainCam.transform.localPosition += new Vector3(0f, 1f, 0) * scrollSpd * Time.deltaTime;
                moveSpd = (mainCam.transform.localPosition.y - 2f) * 0.5f + 7f;
            }
        }
    }
}
