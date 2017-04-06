Circuit Shock Instillation Instructions

Prerequisite software:
This projects required the Kinect SDK 1.8.0, the Oculus Rift SDK, and Unity to be installed on the system beforehand

Setting up the hardware:
This project requires 3 descrete pieces of hardware, the Oculus Rift, The Microsoft Kinect, and the custom arduino controller.
Make sure the kinect is a good distance from the area the player will be standing to ensure it will pick the player up correctly.
I reccomend having the player stand near the PC as the gun controller and Oculus Rift have relativly short cables

Calibrating the gun:
When you run the software you will find yourself in a room with no gun and the string 'NotConfigured:0,0,0,0' in front of you, this
is the current calibration level. These numbers range from 0, not configured, to 3, fully configured. The first number is the system
calibration level and this is controlled by the other three levels. The second number is the gyroscope calibration level and can be
calibrated by placing the gun on a flat stable surface for a few seconds. The third number is the accelerometer and can be calibrated
by holding the gun steady for seven seconds, then turning it 45 degrees along its Z axis and repeating, it usually takes 5-10 turns to
calibrate this. The final number is the magnometer calibration level, this can be calibrated by moving the gun in a figure eight for a
number of seconds. I suggest using the order gyroscope, magnometer, acceletmoeter for best results.

