using UnityEngine;

namespace RopePhysics
{
    public class RopeVisualTwoDim : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        
        private Rope _rope;

        private const int ROPE_RESOLUTION = 30;
        private const float ROPE_LENGTH = 2f;
        
        private void Awake()
        {
            _rope = new Rope(ROPE_RESOLUTION, ROPE_LENGTH, transform.position);
            _lineRenderer.positionCount = 30;
            UpdateLineRenderer();
        }

        private void FixedUpdate()
        {
            if (_rope == null)
            {
                return;
            }
            
            _rope.UpdatePhysics(transform.position, Time.deltaTime);
            UpdateLineRenderer();
        }

        private void UpdateLineRenderer()
        {
            Vector3[] positions = _rope.GetRopePositions();

            for (int i = 0, len = positions.Length; i < len; ++i)
            {
                _lineRenderer.SetPosition(i, positions[i]);
            }
        }
    }
}