# KabschCalibrationUnity
Unity package to align different coordinate systems using the Kabsch Algorithm. Can be used to align real world objects with VR / AR tracking systems.\
The C# Kabsch solver is taken from [https://github.com/zalo/MathUtilities/tree/master/Assets/Kabsch](https://github.com/zalo/MathUtilities/tree/master/Assets/Kabsch).\
The Tooltip Calibration is taken from [https://github.com/anthonysteed/CalibrateTooltip](https://github.com/anthonysteed/CalibrateTooltip).

# Background

The [Kabsch Algorithm](http://scripts.iucr.org/cgi-bin/paper?S0567739476001873) needs at least four 3D-3D correspondences from two different coordinate systems, to find the transformation between them.

<a href="#"><img src="https://github.com/MaxHeimbrock/KabschCalibrationUnity/blob/main/Documentation/kabsch_gif.gif" width="900" height="500"/>

# Usage
Import the Unity package and you will find a sample scene. 
The main components of the calibration are:

- CalibrationObject
	- lets you add source points in Unity scene to align this object with the tracking space
- CalibrationManager
	- add target points to a CalibrationObject chosen 
	- trigger Kabsch solver
	- display mean distance error of calibration 
	- save and load calibration to JSON 
	- switch to tooltip calibration and back again
- CalibrationPersistence 
	- specifiy saving and loading behaviour
- TooltipCalibration
	-  Further improve input precision by calibrating the input device


<a href="#"><img src="https://github.com/MaxHeimbrock/KabschCalibrationUnity/blob/main/Documentation/add_source_points.gif" width="900" height="511"/>
<a href="#"><img src="https://github.com/MaxHeimbrock/KabschCalibrationUnity/blob/main/Documentation/room_calibration.gif" width="900" height="500"/>
