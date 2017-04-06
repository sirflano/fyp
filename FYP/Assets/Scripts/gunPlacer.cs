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
        //This makes sure the kinectManager is initialized
        if (kinectManager == null)
        {
            kinectManager = KinectManager.Instance;
        }
        else
        {
            
            if (kinectManager.IsUserDetected())
            {
                //Initialize hand, head, leftHand and rightHand vectors
                Vector3 handPos = new Vector3(0, 0, 0);
                Vector3 playerPos = kinectManager.GetUserPosition(kinectManager.GetPlayer1ID());
                Vector3 worldHandPos = kinectManager.GetJointPosition(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight);
                Vector3 worldLeftHandPos = kinectManager.GetJointPosition(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandLeft);

                //if the player is left handed set handPos to the relative position of the players left hand, else to the same for the players right hand
                if(leftHanded)
                {
                    handPos = worldLeftHandPos - playerPos;
                }
                else
                {
                    handPos = worldHandPos - playerPos;
                }

                //flip the Z positon to match the cubemanController
                handPos.z = -handPos.z;
                

                if(handSelected)
                {
                    //This generates the position of the gun controller. It gets the rotation of the gun objects as well as its bounds. It multiplies these to get the position whereby the handle of the gun is at 0,0,0
                    //It then adds this to the hand position and updates the gun controllers position. It also stores this position incase the kinect cannot find it next frame
                    if (!leftHanded && kinectManager.IsJointTracked(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight) && kinectManager.IsJointTracked(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.Head))
                    {
                        Quaternion rot = gun.transform.localRotation;

                        gunPos = new Vector3(0, 0, 0) + (rot * new Vector3(0, gun.GetComponent<MeshFilter>().mesh.bounds.extents.y * 0.2f, -gun.GetComponent<MeshFilter>().mesh.bounds.extents.z * 0.4f));
                        handPos = handPos + gunPos;
                        gun.transform.localPosition = handPos;
                        oldPos = handPos;
                    }
                    else if (leftHanded && kinectManager.IsJointTracked(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandLeft) && kinectManager.IsJointTracked(kinectManager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.Head))
                    {
                        Quaternion rot = gun.transform.localRotation;

                        gunPos = new Vector3(0, 0, 0) + (rot * new Vector3(0, gun.GetComponent<MeshFilter>().mesh.bounds.extents.y * 0.2f, -gun.GetComponent<MeshFilter>().mesh.bounds.extents.z * 0.4f));
                        handPos = handPos + gunPos;
                        gun.transform.localPosition = handPos;
                        oldPos = handPos;
                    }
                    else
                    {
                        //If the kinect cannot tack the players hand set the gun to the last known positon of the gun
                        gun.transform.localPosition = oldPos;
                    }
                }
                else
                {
                    //If the gun has not been initialized set it to a standard position outside the players field of view
                    gun.transform.localPosition = standardPos;
                }
            }
        }
    }
    
    //return true if the players hand has been selected, used by the hand selection controller
    public bool getHandSelected()
    {
        return handSelected;
    }

    //sets the players preferred hand to matched an input bool, This is used by the menu controller to allow me to fix the player having accidentally chosen the wrong hand
    public void setHandSelected(bool _setting)
    {
        handSelected = _setting;
    }

    //return true if the hand selection process has been paused, used by the hand selection controller to allow me to pause the selection process when switching players
    public bool getSelectionPaused()
    {
        return selectionPaused;
    }

    //set weather or not the hand selection process has been paused, used by the menu controller to allow me to pause the selection process when switching players
    public void setSelectionPaused(bool _setting)
    {
        selectionPaused = _setting;
    }
}