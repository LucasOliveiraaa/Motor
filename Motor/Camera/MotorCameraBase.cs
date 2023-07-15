using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using Motor.Cameras.Module.Intern;
using System;

namespace Motor.Cameras.Intern
{
    public enum Cameras
    {
        Perspective = 0,
        Orthographic = 1
    };

    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [HelpURL(MotorProperties.m_base_url+"/Cameras/intern_MotorCameraBase.html")]
    [Icon(MotorProperties.gizmos.main)]
    public class MotorCameraBase : MonoBehaviour
    {
        [SerializeField]
        public Cameras Type
        {
            get => m_type;
            set
            {
                m_type = value;
                m_camera_comp.orthographic = value == Cameras.Orthographic;
            }
        }
        private Cameras m_type;
        [HideInInspector] public bool m_started { get; internal set; }
        [HideInInspector] public GameObject m_camera { get; internal set; }
        [HideInInspector] public Camera m_camera_comp { get; internal set; }

        [HideInInspector] public Vector3 Position;

        [HideInInspector]
        public float Left
        {
            get
            {
                return m_camera_comp.ViewportToWorldPoint(new Vector3(0, 0, Position.z)).x;
            }
            internal set => m_left = value;
        }
        private float m_left;

        [HideInInspector]
        public float Right
        {
            get
            {
                return m_camera_comp.ViewportToWorldPoint(new Vector3(1, 0, Position.z)).x;
            }
            internal set => m_right = value;
        }
        private float m_right;

        [HideInInspector]
        public float Top
        {
            get
            {
                return m_camera_comp.ViewportToWorldPoint(new Vector3(0, 1, Position.z)).y;
            }
            internal set => m_top = value;
        }
        private float m_top;

        [HideInInspector]
        public float Bottom
        {
            get
            {
                return m_camera_comp.ViewportToWorldPoint(new Vector3(0, 0, Position.z)).y;
            }
            internal set => m_bottom = value;
        }
        private float m_bottom;

        [HideInInspector]
        public float Width
        {
            get
            {
                float left = m_camera_comp.ViewportToWorldPoint(new Vector3(0, 0, Position.z)).x;
                float right = m_camera_comp.ViewportToWorldPoint(new Vector3(1, 0, Position.z)).x;
                return Mathf.Abs(right - left);
            }
            set
            {
                float currentWidth = Width;

                float ratio = value / currentWidth;

                Vector3 newPosition = m_camera_comp.transform.position;
                newPosition.x *= ratio;
                m_camera_comp.transform.position = newPosition;
            }
        }
        [HideInInspector]
        public float Height
        {
            get
            {
                float top = m_camera_comp.ViewportToWorldPoint(new Vector3(0, 1, Position.z)).y;
                float botton = m_camera_comp.ViewportToWorldPoint(new Vector3(0, 0, Position.z)).y;
                return Mathf.Abs(top - botton);
            }
            set
            {
                float currentHeight = Height;

                float ratio = value / currentHeight;

                Vector3 newPosition = m_camera_comp.transform.position;
                newPosition.y *= ratio;
                m_camera_comp.transform.position = newPosition;
            }
        }

        [HideInInspector]
        public float Zoom
        {
            get => m_zoom;
            set
            {
                if (Type == Cameras.Orthographic)
                {
                    m_camera_comp.orthographicSize = value;
                }
                else
                {
                    float fov = Mathf.Deg2Rad * 2 * Mathf.Atan(Mathf.Tan(Mathf.Deg2Rad * m_camera_comp.fieldOfView / 2) / value);
                    m_camera_comp.fieldOfView = fov;
                }
                m_zoom = value;
            }
        }
        private float m_zoom;

        [HideInInspector]
        public float FieldOfView
        {
            get => m_fieldOfView;
            set
            {
                if (Type == Cameras.Orthographic)
                {
                    m_camera_comp.orthographicSize = value;
                }
                else
                {
                    m_camera_comp.fieldOfView = value;
                }
                m_fieldOfView = value;
            }
        }
        private float m_fieldOfView;

        [HideInInspector]
        public List<BaseModule> m_modules;

        [HideInInspector]
        public Func<int> m_awakeModules = null;

        public void Awake()
        {
            if (Camera.main)
            {
                m_camera = Camera.main.gameObject;
                m_camera_comp = Camera.main;
            }
            else
            {
                m_camera = new GameObject("Main Camera");
                m_camera_comp = m_camera.AddComponent<Camera>();
            }

            m_started = true;
            Type = IsThisA3DProject() ? Cameras.Perspective : Cameras.Orthographic;
            Zoom = 60;
            FieldOfView = 60;
        }

        /// <summary>
        ///     Reload all modules instancied in this Game Object
        /// </summary>
        public void ReloadModules()
        {
            m_modules = new List<BaseModule>();
            List<BaseModule> modules = GetComponentsInChildren<BaseModule>().ToList();

            m_modules = modules;
        }

        /// <summary>
        ///     Start all loaded modules instancied in this Game Object
        /// </summary>
        public void StartAllModules()
        {
            foreach (var module in m_modules)
            {
                module.StartModule();
            }
        }

        /// <summary>
        ///     Loads and starts all modules instancied in this Game Object
        /// </summary>
        public void StartModules()
        {
            ReloadModules();

            if(m_awakeModules != null)
                m_awakeModules();

            StartAllModules();
        }

        /// <summary>
        ///     Verify if the current project is 3D or 2D(Inaccurate).
        /// </summary>
        /// <returns>True if is a 3D Project, false if not.</returns>
        public bool IsThisA3DProject()
        {
            int sceneCount = SceneManager.sceneCount;

            if (sceneCount == 1)
            {
                Scene scene = SceneManager.GetSceneAt(0);

                UnityEngine.Tilemaps.Tilemap[] tilemaps = scene.GetRootGameObjects()[0].GetComponentsInChildren<UnityEngine.Tilemaps.Tilemap>();
                if (tilemaps.Length > 0)
                    return false;
            }

            return true;
        }

        /// <summary>
        ///     Move the camera in reality.
        /// </summary>
        /// <param name="to">A Vector3 of the position</param>
        /// <param name="smooth">The Smoothness</param>
        public void MoveReality(Vector3 to, float smooth = 100f)
        {
            m_camera.transform.position = Vector3.Lerp(m_camera.transform.position, to, smooth / 100);
        }

        /// <summary>
        ///     Rotate the camera in reality.
        /// </summary>
        /// <param name="totation">A Vector3 of the rotation</param>
        /// <param name="smooth">The Smoothness</param>
        public void Rotate(Vector3 rotation, float smooth = 100f)
        {
            Quaternion rot = new(rotation.x, rotation.y, rotation.z, 0);
            Rotate(rot, smooth);
        }

        /// <summary>
        ///     Move the camera in reality.
        /// </summary>
        /// <param name="to">A Quaternion of the rotation</param>
        /// <param name="smooth">The Smoothness</param>
        public void Rotate(Quaternion rotation, float smooth = 100f)
        {
            m_camera.transform.rotation = Quaternion.Lerp(m_camera.transform.rotation, rotation, smooth / 100);
        }

        /// <summary>
        ///     Calcule the constant velocity from two points in a given duration
        /// </summary>
        /// <param name="start">A Vector3 of the start position</param>
        /// <param name="end">A Vector3 of the end position</param>
        /// <param name="duration">The duration in seconds</param>
        public float Velocity(Vector3 start, Vector3 end, float duration)
        {
            float distance = Vector3.Distance(start, end);

            return distance / duration;
        }

        /// <summary>
        ///     Interpolate the camera position between two points in a given duration
        /// </summary>
        /// <param name="start">A Vector3 of the start position</param>
        /// <param name="end">A Vector3 of the end position</param>
        /// <param name="duration">The duration in seconds</param>
        public void Interpole(Vector3 start, Vector3 end, float duration)
        {
            StartCoroutine(InterpoleCorroutine(start, end, duration));
        }

        private IEnumerator InterpoleCorroutine(Vector3 start, Vector3 end, float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                Vector3 currentPosition = Vector3.Lerp(start, end, t);

                transform.position = currentPosition;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = end;
        }

        /// <summary>
        ///     Make a transition of the current camera to other camera in a given duration
        /// </summary>
        /// <param name="targetCamera">The target camera</param>
        /// <param name="transitionDuration">The transition durationn</param>
        public void TransitionToCamera(Camera targetCamera, float transitionDuration)
        {
            StartCoroutine(TransitionCoroutine(targetCamera, transitionDuration));
        }

        private IEnumerator TransitionCoroutine(Camera targetCamera, float transitionDuration)
        {
            float elapsedTime = 0f;
            Camera startingCamera = m_camera_comp;

            while (elapsedTime < transitionDuration)
            {
                float t = elapsedTime / transitionDuration;

                m_camera_comp.fieldOfView = Mathf.Lerp(startingCamera.fieldOfView, targetCamera.fieldOfView, t);
                m_camera_comp.transform.position = Vector3.Lerp(startingCamera.transform.position, targetCamera.transform.position, t);
                m_camera_comp.transform.rotation = Quaternion.Lerp(startingCamera.transform.rotation, targetCamera.transform.rotation, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            m_camera_comp.fieldOfView = targetCamera.fieldOfView;
            m_camera_comp.transform.position = targetCamera.transform.position;
            m_camera_comp.transform.rotation = targetCamera.transform.rotation;
        }
    }
}