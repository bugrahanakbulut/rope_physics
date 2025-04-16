using UnityEngine;

namespace RopePhysics
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed = 2.5f;
        
        void Update()
        {
            if (_target == null)
            {
                return;
            }

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(horizontal, vertical, 0);

            if (movement.sqrMagnitude > 1)
            {
                movement.Normalize();
            }

            _target.position += movement * (_speed * Time.deltaTime);
        }
    }
}