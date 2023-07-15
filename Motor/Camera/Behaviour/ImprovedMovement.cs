using UnityEngine;
using System.Collections;
using Motor.Cameras.Module.Intern;

namespace Motor.Cameras.Behaviour
{
    [Icon(MotorProperties.gizmos.comp)]
    [HelpURL(MotorProperties.m_base_url + "/Cameras/Modules/AprimoredMovements.html")]
    [AddComponentMenu("Motor/Camera/Behaviour/ImprovedMovements")]
    public static class ImprovedMovement
    {
        public static void LookAt(this MotorCamera instance, Vector3 at)
        {
            instance.m_camera.transform.LookAt(at);
        }
        public static void LookAt(this MotorCamera instance, GameObject at)
        {
            instance.m_camera.transform.LookAt(at.transform);
        }
        public static void LookAt(this MotorCamera instance, Transform at)
        {
            instance.m_camera.transform.LookAt(at);
        }

        public static void Offset(this MotorCamera instance, Vector3 offset)
        {
            instance.transform.position += offset;
        }

        public static void MoveReality(this MotorCamera instance, Vector3 position, Vector3 offset)
        {
            instance.MoveReality(position + offset);
        }
    }
}