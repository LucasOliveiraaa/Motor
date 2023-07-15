using UnityEngine;
using System;

namespace Motor.Cameras.Module.Intern
{
    public enum EntryResponse
    {
        Success = 0,
        Error = 1
    }
    public enum ModuleStatus
    {
        Sleeping = 0,
        Starting = 1,
        Started = 2,
        Sleep = 3,
        Standby = 4
    }

    [RequireComponent(typeof(MotorCamera))]
    [Icon(MotorProperties.gizmos.main)]
    [HelpURL(MotorProperties.m_base_url + "/Cameras/Modules/intern_BaseModule.html")]
    public class BaseModule : MonoBehaviour
    {
        public Component m_this { get; private set; }
        public GameObject m_refers { get; private set; }
        public MotorCamera m_camera { get; internal set; }
        public ModuleStatus m_status { get; private set; }
        public bool Started
        {
            get
            {
                return m_status == ModuleStatus.Started && m_camera && m_refers;
            }
        }

        private void Awake()
        {
            SetStatus(ModuleStatus.Sleeping);
            if(TryGetComponent(out MotorCamera cam))
            {
                cam.StartModules();
            }
        }

        public virtual void Start()
        {
            if (!Started) return;
        }

        public virtual void Update()
        {
            if (!Started) return;
        }

        public virtual void LateUpdate()
        {
            if (!Started) return;
        }

        public virtual void FixedUpdate()
        {
            if (!Started) return;
        }

        public void AwakeModule(MotorCamera camera)
        {
            SetStatus(ModuleStatus.Starting);
            m_camera = camera;
            m_refers = camera.gameObject;
        }

        public void RemoveModule()
        {
            Destroy(m_this);
        }

        public void DestroyReference()
        {
            Destroy(m_refers);
        }

        public void StartModule()
        {
            SetStatus(ModuleStatus.Started);
            Start();
        }

        public void SetStatus(ModuleStatus status)
        {
            m_status = status;
        }
    }
}