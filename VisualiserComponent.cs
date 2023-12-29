using Invector.vMelee;
using UnityEngine;

namespace HitboxVisualiser
{

    // like half of the shit here was done by chatgpt (update(), drawtrail()) but who gives a shit it works
    internal class VisualiserComponent: MonoBehaviour
    {
        Collider trigger;
        vHitBox hitBox;
        private LineRenderer lineRenderer;

        internal void Start()
        {
            trigger = gameObject.GetComponent<Collider>();
            hitBox = gameObject.GetComponent<vHitBox>();
            if (!trigger)
            {
                trigger = gameObject.AddComponent<Collider>();
            }
        }

        private void Update()
        {
            if (Visualiser.active && lineRenderer == null)
            {
                DrawTrail();
            }
            else if (!Visualiser.active && lineRenderer != null)
            {
                Destroy(lineRenderer);
            }
        }

        private void DrawTrail()
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.useWorldSpace = false; // Use local space
            lineRenderer.loop = true; // Close the loop for trail
            lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Change the material

            Color color = ((hitBox.triggerType & vHitBoxType.Damage) != (vHitBoxType)0 && (hitBox.triggerType & vHitBoxType.Recoil) == (vHitBoxType)0) ? Color.green : (((hitBox.triggerType & vHitBoxType.Damage) != (vHitBoxType)0 && (hitBox.triggerType & vHitBoxType.Recoil) != (vHitBoxType)0) ? Color.yellow : (((hitBox.triggerType & vHitBoxType.Recoil) != (vHitBoxType)0 && (hitBox.triggerType & vHitBoxType.Damage) == (vHitBoxType)0) ? Color.red : Color.black));
            color.a = 0.6f;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            Collider colliderComp = trigger;

            if (colliderComp is MeshCollider)
            {
                MeshCollider meshCollider = trigger as MeshCollider;

                if (meshCollider != null)
                {
                    Mesh mesh = meshCollider.sharedMesh;
                    if (mesh != null)
                    {
                        Vector3[] vertices = mesh.vertices;
                        int[] triangles = mesh.triangles;

                        lineRenderer.positionCount = triangles.Length * 2; // Each edge has 2 points

                        for (int i = 0, j = 0; i < triangles.Length; i += 3)
                        {
                            // Draw lines for each triangle edge
                            Vector3 v0 = vertices[triangles[i]];
                            Vector3 v1 = vertices[triangles[i + 1]];
                            Vector3 v2 = vertices[triangles[i + 2]];

                            // Edge 0-1
                            lineRenderer.SetPosition(j++, v0);
                            lineRenderer.SetPosition(j++, v1);

                            // Edge 1-2
                            lineRenderer.SetPosition(j++, v1);
                            lineRenderer.SetPosition(j++, v2);

                            // Edge 2-0
                            lineRenderer.SetPosition(j++, v2);
                            lineRenderer.SetPosition(j++, v0);
                        }
                    }
                }
            }
            else
            {
                BoxCollider collider = colliderComp as BoxCollider;
                Vector3 center = collider.center;
                Vector3 size = collider.size;

                Vector3[] corners =
                {
                    center + new Vector3(size.x, size.y, size.z) * 0.5f,
                    center + new Vector3(size.x, size.y, -size.z) * 0.5f,
                    center + new Vector3(-size.x, size.y, -size.z) * 0.5f,
                    center + new Vector3(-size.x, size.y, size.z) * 0.5f,
                    center + new Vector3(size.x, -size.y, size.z) * 0.5f,
                    center + new Vector3(size.x, -size.y, -size.z) * 0.5f,
                    center + new Vector3(-size.x, -size.y, -size.z) * 0.5f,
                    center + new Vector3(-size.x, -size.y, size.z) * 0.5f
                };

                    // Define indices to draw the edges of the box
                int[] indices =
                {
                    0, 1, 1, 2, 2, 3, 3, 0, // Top face
                    4, 5, 5, 6, 6, 7, 7, 4, // Bottom face
                    0, 4, 1, 5, 2, 6, 3, 7  // Vertical edges
                };

                lineRenderer.positionCount = indices.Length;

                for (int i = 0; i < indices.Length; i++)
                {
                    lineRenderer.SetPosition(i, corners[indices[i]]);
                }
            }
        }

        private void OnDestroy()
        {
            if (lineRenderer) Destroy(lineRenderer);
        }
    }
}
