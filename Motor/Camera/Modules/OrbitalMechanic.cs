using UnityEngine;
using Motor.Cameras.Module.Intern;
using Motor.Cameras.Behaviour;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Motor.Cameras.Module.Intern
{
    public struct Orbit
    {
        public float height;
        public float radius;

        public Orbit(float height, float radius)
        {
            this.height = height;
            this.radius = radius;
        }
    }
}

namespace Motor.Cameras.Module
{
    [RequireComponent(typeof(FollowMechanic))]
    [Icon(MotorProperties.gizmos.comp)]
    [HelpURL(MotorProperties.m_base_url + "/Cameras/Modules/OrbitalMechanic.html")]
    public class OrbitalMechanic : BaseModule
    {
        public GameObject Target;
        public Vector3 OrbitalPoint;
        [Range(0, 1)]
        public float BulgeIndex;

        private Orbit[] Orbits = new Orbit[3]
        {
            new Orbit(1f, 0.5f),
            new Orbit(0, 1f),
            new Orbit(-1f, 0.5f)
        };
        public Orbit Top
        {
            get => Orbits[0];
            set
            {
                Orbits[0] = value;
            }
        }
        public Orbit Middle
        {
            get => Orbits[1];
            set
            {
                Orbits[1] = value;
            }
        }
        public Orbit Bottom
        {
            get => Orbits[3];
            set
            {
                Orbits[3] = value;
            }
        }

        public override void Start()
        {
            base.Start();

            if(TryGetComponent(out FollowMechanic fm))
            {
                fm.Follow = Target;
                fm.LookAt = Target;
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public void OnValidate()
        {
            if (TryGetComponent(out FollowMechanic fm))
            {
                fm.Follow = Target;
                fm.LookAt = Target;
            }

            DrawGizmos();
        }

        private void OnDrawGizmos()
        {
            DrawGizmos();
        }

        private void DrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(OrbitalPoint, 0.2f);

            DrawOrbit(Orbits[0], Color.red);
            DrawOrbit(Orbits[1], Color.red);
            DrawOrbit(Orbits[2], Color.red);
        }

        public void DrawOrbit(Orbit orbit, Color color)
        {
            if (Target == null) return;

            Vector3 position = new(Target.transform.position.x, Target.transform.position.y + orbit.height, Target.transform.position.z);

            Gizmos.color = color;

            float angleStep = 360f / 50;

            List<Vector3> segments = new List<Vector3>();

            for (int i = 0; i < 50; i++)
            {
                float angle = i * angleStep;
                float x = Mathf.Sin(Mathf.Deg2Rad * angle) * orbit.radius;
                float z = Mathf.Cos(Mathf.Deg2Rad * angle) * orbit.radius;
                Vector3 segmentPosition = position + new Vector3(x, 0f, z);
                segments.Add(segmentPosition);
            }

            for(int j = 0; j < segments.Count; j++)
            {
                var segment = segments[j];

                Vector3 nextSegment;

                if (j+1 >= segments.Count)
                {
                    nextSegment = segments[j + 1 - segments.Count];
                }else
                {
                    nextSegment = segments[j + 1];
                }

                Gizmos.DrawLine(segment, nextSegment);
            }
        }
    }
}