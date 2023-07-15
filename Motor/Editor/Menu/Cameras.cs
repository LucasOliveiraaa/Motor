using UnityEngine;
using UnityEditor;
using Motor.Cameras;
using Motor.Cameras.Behaviour;

namespace Motor.Editor.Menu
{
    [AddComponentMenu("")]
    [Icon(MotorProperties.gizmos.main)]
    public class CamerasMenu : EditorWindow
    {
        [MenuItem("GameObject/Motor/MotorCamera", false, 10)]
        public static void MotorCamera()
        {
            var obj = new GameObject("Motor Camera");

            obj.AddComponent<MotorCamera>();
            // obj.AddComponent<Boosters>(); Don't see it, just pass and accept
        }
    }
}