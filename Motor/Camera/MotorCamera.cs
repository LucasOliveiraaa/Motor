using Motor.Cameras.Intern;
using UnityEngine;

namespace Motor.Cameras
{
    [Icon(MotorProperties.gizmos.camera)]
    [HelpURL(MotorProperties.m_base_url + "/Cameras/MotorCamera.html")]
    [AddComponentMenu("Motor/Camera/MotorCamera")]
    public class MotorCamera : MotorCameraBase
    {
        private void Start()
        {
            m_awakeModules = AwakeModules;
            StartModules();
        }

        public int AwakeModules()
        {
            foreach(var module in m_modules)
            {
                module.AwakeModule(this);
            }

            return 0;
        }

        /// <summary>
        ///     Zoom in the camera viewport
        /// </summary>
        /// <param name="amount">The zoom amount</param>
        public void ZoomIn(float amount)
        {
            Zoom += amount;
        }

        /// <summary>
        ///     Zoom out the camera viewport
        /// </summary>
        /// <param name="amount">The zoom amount</param>
        public void ZoomOut(float amount)
        {
            if (Zoom - amount < 0) Zoom = 0;
            else Zoom -= amount;
        }
    }
}