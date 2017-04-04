using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

public class gunPlacer : MonoBehaviour
{
    protected KinectManager kinectManager;
    public GameObject gun;
    
    private Vector3 oldPos;
    private Vector3 gunPos;
    
    public float xpos;
    public float ypos;
    public float zpos;
    public float handXpos;
    public float handYpos;
    public float handZPos;
    public float moveSpeed;
    public bool leftHanded = false;
    public bool handSelected = false;
    private bool selectionPaused = false;
    private Vector3 standardPos = new Vector3(0, -100, 0);

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (kinectManager == null)
        {
            kinectManager = KinectManager.Instance;
        }
        else
        {
            if (kinectManager.IsUserDetected())
            {
                Vector3 handPos = new Vector3(0, 0, 0);
                Vector3 playerPos = kinectManager.GetUserPosition(kinectManager.GetPlayer1ID());
                Vector3 worldHandPos = kinectManager.GetJointPosition(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight);
                Vector3 worldLeftHandPos = kinectManager.GetJointPosition(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandLeft);
                Vector3 worldHeadPos = kinectManager.GetJointPosition(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.Head);
                if(leftHanded)
                {
                    handPos = worldLeftHandPos - playerPos;
                }
                else
                {
                    handPos = worldHandPos - playerPos;
                }

                handPos.z = -handPos.z;
                handZPos = handPos.z;
                handXpos = handPos.x;
                handYpos = handPos.y;
                

                if(handSelected)
                {
                    if (kinectManager.IsJointTracked(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight) && kinectManager.IsJointTracked(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.Head))
                    {
                        Quaternion rot = gun.transform.localRotation;

                        gunPos = new Vector3(0, 0, 0) + (rot * new Vector3(0, gun.GetComponent<MeshFilter>().mesh.bounds.extents.y * 0.2f, -gun.GetComponent<MeshFilter>().mesh.bounds.extents.z * 0.4f));
                        handPos = handPos + gunPos;
                        gun.transform.localPosition = handPos;
                        oldPos = handPos;
                    }
                    else
                    {
                        gun.transform.localPosition = oldPos;
                    }
                }
                else
                {
                    gun.transform.localPosition = standardPos;
                }
            }
        }
    }
    

    public bool getHandSelected()
    {
        return handSelected;
    }

    public void setHandSelected(bool _setting)
    {
        handSelected = _setting;
    }

    public bool getSelectionPaused()
    {
        return selectionPaused;
    }

    public void setSelectionPaused(bool _setting)
    {
        selectionPaused = _setting;
    }
}