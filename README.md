# Topaz Video AI Lab
_Topaz Video AI Lab_ is an application for trying out different _Topaz Video AI_ workflows.  
Copyright (c) 2023-2024  Mattias von Schantz

_Topaz Video AI Lab_ uses a graph based approach where you define a workflow using nodes representing different Topaz Video AI models and their settings.
One or more outputs from a model can be combined into the input to the next model. Optionally, the input can be scaled and noise can be
injected into the video before sending it to a model.

Multiple workflows can be rendered and quickly switched between, to compare different settings.

_Topaz Video AI Lab_ is not produced by, endorsed by or affiliated with _Topaz Labs_ or _Topaz Video AI_. It's just a third party application made to simplify
experimenting with _Topaz Video AI_.

![Alt text](screenshots/screenshot.png?raw=true "Screenshot of Topaz Video AI Lab")

## Note
_Topaz Video AI Lab_ is used for experimenting with _Topaz Video AI_. As such, it is in itself something of an experiment. The code is in a flux and not all features are necesserily working all the time. Some are abandoned and postponed. There are known bugs.

## System requirements
* .NET Core 8
* 64-bit AviSynth+
* 64-bit VirtualDub2
* 64-bit Avs2YUV
* LSMASHSource (LWLibavVideoSource) AviSynth plugin
* QTGMC AviSynth plugin
* Topaz Video AI (4.0 or later)	

## Optional requirements
* TIVTC (TFM and TDecimate) AviSynth plugin (Only for built-in deinterlace support. Not recommended, as this is not fully implemented yet.)

## Installation
There is no installation program. Just create a folder somewhere (like C:\Program Files\TopazVideoAILab) and copy the files into this folder.

## Configuration
The application configuration is stored in TopazVideoLab2.dll.config. This is an XML file and can be opened in any suitable text editor.

Before running the program the first time, you must edit the paths in this file to point to the correct locations on your own system.

The following paths must be set:

| Path         | Description                                                                                                                              |
|--------------|------------------------------------------------------------------------------------------------------------------------------------------|
| TopazFfmpeg  | The path to the ffmpeg.exe file in your Topaz Video AI installation folder.                                                              |
| Avs2Yuv      | The path to the avs2yuv64.exe from Avs2YUV.                                                                                              |
| VirtualDub64 | The path to the VirtualDub.exe from VirtualDub2.                                                                                         |
| ModelDataDir | The path to the Topaz Video AI models folder. Typically C:\ProgramData\Topaz Labs LLC\Topaz Video AI\models\ (note the ending backslash) |
| ModelDir     | The path to the Topaz Video AI models folder. Typically C:\ProgramData\Topaz Labs LLC\Topaz Video AI\models                              |

## Usage

Start by adding a **src** node, then double clicking on it to add a source video file and configure the project.  

**Note:** Deinterlace support is not yet fully implemented. It is suggested you deinterlace your sample files beforehand and leave this option on _None_.  

Then use the top slider in the bottom left corner to select a suitable start position within your sample.  

Create a workflow by draggin model nodes onto the work area. Then connect the nodes by dragging from the bottom of the source node onto the target node. Click on a model node to edit its properties. Click on a connection to edit the weight of that connection (if multiple source nodes are merged into a target node). Press _Delete_ to remove a node or a connection. Use the **Merge** node to create a final output by collecting and resampling the output from your source nodes into a video clip with your project resolution.  

Hold _Ctrl_ and click the left mouse button to select multiple models. Move the mouse cursor over the preview window and use the 1 through 9 keys to switch between the selected nodes. Zoom with the mouse wheel and use the left mouse button to drag the preview window.  

It is suggested you use a dual monitor setup, with the workflow editor on one screen and the preview window on another. But this is not a requirement.
