
# KabschCalibrationUnity
Unity package to align different coordinate systems using the Kabsch Algorithm. Can be used to align real world objects with VR / AR tracking systems.\
The C# Kabsch solver is taken from [https://github.com/zalo/MathUtilities/tree/master/Assets/Kabsch](https://github.com/zalo/MathUtilities/tree/master/Assets/Kabsch).\
The Tooltip Calibration is taken from [https://github.com/anthonysteed/CalibrateTooltip](https://github.com/anthonysteed/CalibrateTooltip).

# Background

The [Kabsch Algorithm](http://scripts.iucr.org/cgi-bin/paper?S0567739476001873) needs at least four 3D-3D correspondences from two different coordinate systems, to find the transformation between them.

<a href="#"><img src="https://github.com/MaxHeimbrock/KabschCalibrationUnity/blob/main/Documentation/kabsch_gif.gif" width="900" height="511"/>

The algorithm is more stable, the more correspondences are provided. Also the points should be spread as much as possible.

# Components
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

# 1. Calibration prefab

The easiest way to use the calibration is to use the calibration prefab. It holds the CalibrationManager, CalibrationPersistence and TooltipCalibration scripts, which are only needed once per scene. 

<a href="#"><img src="https://github.com/MaxHeimbrock/KabschCalibrationUnity/blob/main/Documentation/Calibration.png" width="400" height="700"/>

# 2. Add Source Points
To calibrate an object, add the CalibrationObject script to the parent GameObject in the hierarchy. Now you can add source points, by clicking on the GameObject in the scene view. Make sure, that the GameObject (and its children) have (mesh) colliders and are not occluded, since the source point adding mechanism is based on raycasting. Source points should be added in Edit Mode.

# 3. Tooltip Calibration
For better accuracy, the input device should be calibrated first using the Tooltip Calibration. The input device has to be referenced as the controller in the inspector. Go into Play Mode and press "Start Calibration". Follow the insructions from 
You can press "T" as input on the keyboard or add a custom input.

# 4. Add Custom Input
Add custom input for adding source points to the CalibrationManagers update method. There is an example for SteamVR 1.0 and for testing, you can remove the error thrown and press "M" to add dummy source points.

# 5. Add Target Points
In Play Mode, select the current object to calibrate to add target points for calibration. Once for every source point a target point was added, the kabsch solver is triggered and the resulting transformation (in this case a 4x4 Matrix) is applied to the object to align it with the tracking coordinate system.

# 6. Check Calibration Quality
The Calibration Distance Error is the mean distance between each source point and target point pair in cm. Try different runs and setups to improve the measured calibration quality.

# 7. Saving and Loading
By pressing the Save button, for every object to calibrate the current transformation is saved to a JSON file with the path specified by the script TransformPersistance. If the tracking system is static (like SteamVR), the saved transforms can be used again. You can activate "Load calibration at startup" to immediatly load and apply the transformations from the specified file path.


