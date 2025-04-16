using UnityEngine;

namespace RopePhysics
{
    public class RopeVisualThreeDim : MonoBehaviour
    {
        [SerializeField] private Rope _rope; // Reference to the simulated Rope object
        [SerializeField] private float ropeThickness = 0.1f; // Thickness of the rope
        [SerializeField] private int ropeSegments = 8; // Number of vertices in a circular cross-section (smoothness)
        [SerializeField] private int _radialSegments = 8; 
        [SerializeField] public float _ropeRadius = 0.1f;
        
        private Mesh _mesh;
        private Vector3[] _vertices;
        private int[] _triangles;
        private Vector2[] _uvs;

        private void Start()
        {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;
        }

        private void FixedUpdate()
        {
            if (_rope == null)
            {
                return;
            }

            _rope.UpdatePhysics(transform.position, Time.fixedDeltaTime);
            
            Vector3[] ropePositions = _rope.GetRopePositions();
            GenerateRopeMesh(ropePositions);
        }

        private void GenerateRopeMesh(Vector3[] ropePositions)
        {
            int segmentCount = ropePositions.Length - 1;
            int vertexCount = (segmentCount + 1) * _radialSegments;
            int triangleCount = segmentCount * _radialSegments * 6;

            _vertices = new Vector3[vertexCount];
            _triangles = new int[triangleCount];
            _uvs = new Vector2[vertexCount];

            // Generate vertices and UVs
            int vertexIndex = 0;
            for (int i = 0; i < ropePositions.Length; i++)
            {
                Vector3 center = ropePositions[i];

                // Determine the forward direction (for the segment direction)
                Vector3 forward = Vector3.forward; // Default forward direction
                if (i < ropePositions.Length - 1) // If not the last particle
                {
                    forward = (ropePositions[i + 1] - ropePositions[i]).normalized;
                }
                else if (i > 0) // For the last particle, use the previous direction
                {
                    forward = (ropePositions[i] - ropePositions[i - 1]).normalized;
                }

                Vector3 right = Vector3.Cross(forward, Vector3.up).normalized; // Local 'right'
                Vector3 up = Vector3.Cross(forward, right).normalized;         // Local 'up'

                for (int j = 0; j < _radialSegments; j++)
                {
                    float angle = (float)j / _radialSegments * Mathf.PI * 2; // Angle around the circle
                    Vector3 offset = (Mathf.Cos(angle) * right + Mathf.Sin(angle) * up) * _ropeRadius;

                    _vertices[vertexIndex] = center + offset;
                    _uvs[vertexIndex] = new Vector2((float)j / _radialSegments, (float)i / ropePositions.Length);

                    vertexIndex++;
                }
            }

            // Generate triangles
            int triangleIndex = 0;
            for (int i = 0; i < segmentCount; i++)
            {
                for (int j = 0; j < _radialSegments; j++)
                {
                    int current = i * _radialSegments + j;       // Current vertex
                    int next = current + _radialSegments;       // Corresponding vertex in the next cross-section
                    int nextSegment = (j + 1) % _radialSegments; // Next vertex around the circle (wrapping)

                    // First triangle
                    _triangles[triangleIndex++] = current;
                    _triangles[triangleIndex++] = nextSegment + i * _radialSegments;
                    _triangles[triangleIndex++] = next;

                    // Second triangle
                    _triangles[triangleIndex++] = nextSegment + i * _radialSegments;
                    _triangles[triangleIndex++] = nextSegment + (i + 1) * _radialSegments;
                    _triangles[triangleIndex++] = next;
                }
            }

            // Update the mesh with the new data
            _mesh.Clear();
            _mesh.vertices = _vertices;
            _mesh.triangles = _triangles;
            _mesh.uv = _uvs;
            _mesh.RecalculateNormals(); // Smooth normals for cylinder lighting
        }
    }   
}