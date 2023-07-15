using UnityEngine;
using Motor.Cameras.Module.Intern;
using Motor.Cameras.Behaviour;

namespace Motor.Cameras.Module
{
    [Icon(MotorProperties.gizmos.comp)]
    [AddComponentMenu("Motor/Camera/Modules/FollowMechanic")]
    [DisallowMultipleComponent]
    [HelpURL(MotorProperties.m_base_url + "/Cameras/Modules/FollowMechanic.html")]
    public class FollowMechanic : BaseModule
    {
        public GameObject Follow;
        public GameObject LookAt;
        public Vector3 CameraOffset;

        public override void Update()
        {
            base.Update();

            m_camera.MoveReality(Follow.transform.position, CameraOffset);
            m_camera.LookAt(Follow);
        }
    }
}