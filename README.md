# KabschCalibrationUnity
Unity package to align different coordinate systems using the kabsch algorithm. Can be used to align real world objects with VR / AR tracking systems.

# Background
![alt text](https://github.com/MaxHeimbrock/KabschCalibrationUnity/blob/main/ezgif-2-a7eb01744123.gif)(#)
!(ezgif-2-a7eb01744123.gif)

# Usage
Import the Unity package and you will find a sample scene. 
The main components of the calibration are:

- CalibrationObject
	- lets you add source points in Unity scene to align this object with the tracking space
- CalibrationManager
	- add target points to a CalibrationObject chosen 
	- trigger Kabsch solver taken from https://github.com/zalo/MathUtilities/tree/master/Assets/Kabsch
	- display mean distance error of calibration 
	- save and load calibration to JSON 
	- switch to tooltip calibration and back again
- CalibrationPersistence 
	- specifiy saving and loading behaviour
- TooltipCalibration
	-  Further improve input precision by using https://github.com/anthonysteed/CalibrateTooltip
