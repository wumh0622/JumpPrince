using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BgTools.CastVisualizer
{
    [DefaultExecutionOrder(-32000)]
    public abstract class BaseVisulizer<T, U> : MonoBehaviour where T : System.Enum
    {
        private List<(Vector3, Vector3)> hitsToRender = new List<(Vector3, Vector3)>();
        private List<(Ray, float)> raysToRender = new List<(Ray, float)>();
        private List<(T, Matrix4x4, Vector3)> meshesToRender = new List<(T, Matrix4x4, Vector3)>();
        private List<(T, Matrix4x4)> meshHitsToRender = new List<(T, Matrix4x4)>();
        private List<U> colliderHitsToRender = new List<U>();
        private List<U> colliderCastsToRender = new List<U>();

        private int lastframe;

        #region Add Render Data Functions
        internal static bool AddHitToRender((Vector3, Vector3) hitData)
        {
            if(Instance != null)
            {
                Instance.hitsToRender.Add(hitData);
                return true;
            }
            return false;
        }

        internal static bool AddRayToRender((Ray, float) rayData)
        {
            if (Instance != null)
            {
                Instance.raysToRender.Add(rayData);
                return true;
            }
            return false;
        }

        internal static bool AddMeshToRender((T, Matrix4x4, Vector3) meshData)
        {
            if (Instance != null)
            {
                Instance.meshesToRender.Add(meshData);
                return true;
            }
            return false;
        }

        internal static bool AddMeshHitToRender((T, Matrix4x4) meshHitData)
        {
            if (Instance != null)
            {
                Instance.meshHitsToRender.Add(meshHitData);
                return true;
            }
            return false;
        }

        internal static bool AddColliderHitToRender(U colliderData)
        {
            if (Instance != null)
            {
                Instance.colliderHitsToRender.Add(colliderData);
                return true;
            }
            return false;
        }

        internal static bool AddColliderCastToRender(U colliderData)
        {
            if (Instance != null)
            {
                Instance.colliderCastsToRender.Add(colliderData);
                return true;
            }
            return false;
        }
        #endregion

        protected static BaseVisulizer<T, U> Instance { get; private set; }

        private void Awake()
        {
            Instance = this;

            //Debug.Log($"[RV] Init {GetType().Name}");
        }

        private void FixedUpdate()
        {
            if (lastframe != Time.frameCount)
            {
                lastframe = Time.frameCount;

                hitsToRender.Clear();
                raysToRender.Clear();
                meshesToRender.Clear();
                meshHitsToRender.Clear();
                colliderHitsToRender.Clear();
                colliderCastsToRender.Clear();
            }
        }

        protected virtual Color RayColor() { return Color.magenta; }
        protected virtual bool DrawCondition() { return true; }
        protected abstract void DrawMeshes((T, Matrix4x4, Vector3) meshData);
        protected abstract void DrawHitMeshes((T, Matrix4x4) meshData);
        protected abstract void DrawColliders(U collider);

        // Necessary for Gizmos Menu entry
        //private void OnDrawGizmos()
        //{
        //    // Code in DrawGizmos() fucntion
        //}

        // Use DrawGizmo annotation to avoid the pickable Gizmos in OnDrawGizmos()
        //[DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        protected static void DrawGizmos(BaseVisulizer<T, U> script, GizmoType gizmoType)
        {
            if (!script.DrawCondition())
                return;

            // Casts
            Color orgGizmosColor = Gizmos.color;
            Gizmos.color = Instance.RayColor();

            foreach ((Ray, float) rayValues in script.raysToRender)
            {
                Gizmos.DrawRay(rayValues.Item1.origin, rayValues.Item1.direction * rayValues.Item2);
            }

            Gizmos.color = orgGizmosColor;

            using (new Handles.DrawingScope(Instance.RayColor()))
            {
                foreach ((T, Matrix4x4, Vector3) meshData in script.meshesToRender)
                {
                    script.DrawMeshes(meshData);
                }

                foreach (U collHit in script.colliderCastsToRender)
                {
                    if(collHit != null)
                        script.DrawColliders(collHit);
                }
            }
            // Hits
            if (!CastVisualizerManager.Instance.ShowHits)
                return;

            using (new Handles.DrawingScope(CastVisualizerManager.Instance.HitMarkerColor))
            {
                // HitPoints
                foreach ((Vector3, Vector3) hitData in script.hitsToRender)
                {
                    RenderUtil.DrawCross(hitData.Item1, hitData.Item2);
                }

                // Meshes
                foreach ((T, Matrix4x4) meshData in script.meshHitsToRender)
                {
                    script.DrawHitMeshes(meshData);
                }

                // Collider
                foreach (U collHit in script.colliderHitsToRender)
                {
                    script.DrawColliders(collHit);
                }
            }
        }
    }
}