using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Motor.Editor.GUI.Cameras
{
    [CustomEditor(typeof(Motor.Cameras.MotorCamera))]
    public class MotorCamera : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            Motor.Cameras.MotorCamera camera = (Motor.Cameras.MotorCamera)target;

            //camera.Awake();

            //camera.Zoom = EditorGUILayout.IntSlider("Zoom", (int)camera.Zoom, 0, 100);
        }

    }
}