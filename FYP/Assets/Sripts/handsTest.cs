using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

public class handsTest : MonoBehaviour {

    protected KinectManager kinectManager;
    public GameObject gun;
    //public GameObject camera;
    private Vector3 oldPos;
    private Vector3 gunPos;

    //public GameObject BodySrcManager;
    public float xpos;
    public float ypos;
    public float zpos;
    public float handXpos;
    public float handYpos;
    public float handZPos;
    public float moveSpeed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (kinectManager == null)
        {
            kinectManager = KinectManager.Instance;
        }
        else
        {
            if (kinectManager.IsUserDetected())
            {
                Vector3 playerPos = kinectManager.GetUserPosition(kinectManager.GetPlayer1ID());
                //Vector3 handPos = kinectManager.GetJointLocalPosition(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight);
                //Vector3 headPos = kinectManager.GetJointLocalPosition(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.Head);
                Vector3 worldHandPos = kinectManager.GetJointPosition(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight);
                Vector3 worldLeftHandPos = kinectManager.GetJointPosition(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandLeft);
                Vector3 worldHeadPos = kinectManager.GetJointPosition(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.Head);
                Vector3 handPos = worldHandPos - playerPos;
                Vector3 leftHandPos = worldLeftHandPos - playerPos;
                //Vector3 headPos = worldHeadPos - playerPos;
                //handPos.x = handPos.x;
                //handPos.y = -handPos.y;
                //leftHandPos.x = leftHandPos.x;
                //leftHandPos.y = -leftHandPos.y;
                //Vector3 relPos = handPos - headPos;
                //Vector3 jointDir = kinectManager.GetDirectionBetweenJoints(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandLeft, (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight, true, false);
                //Vector3 jointDir = Vector3.Dot(handPos, leftHandPos);

                handPos.z = -handPos.z;
                handZPos = handPos.z;
                handXpos = handPos.x;
                handYpos = handPos.y;

                if (kinectManager.IsJointTracked(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight) && kinectManager.IsJointTracked(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.Head))
                {
                    //Quaternion angle = new Quaternion();
                    //angle.SetFromToRotation(handPos, leftHandPos);
                    //angle.SetLookRotation(worldHandPos - worldLeftHandPos);

                    //angle.x = angle.x * -1;
                    //xpos = angle.x;
                    //ypos = angle.y;
                    //zpos = angle.z;
                    //angle.SetEulerAngles(xpos, ypos, zpos);
                    //Vector3 gunPos = angle * handPos;
                    Quaternion rot = gun.transform.rotation;

                    //gunPos = handPos + gun.transform.rotation * new Vector3(0, 0, 0.5f);// * -0.5f;
                    Debug.Log(handPos + " Becomes " + gunPos);
                    gun.transform.localPosition = handPos;
                    //camera.transform.localPosition = headPos;
                    //gun.transform.rotation = angle;
                    oldPos = handPos;
                }
                else
                {
                    gun.transform.localPosition = oldPos;
                }
            }
        }
    }
}
