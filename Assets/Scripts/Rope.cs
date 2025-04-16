using UnityEngine;

namespace RopePhysics
{
    public class Rope
    {
        private class RopeParticle
        {
            public Vector3 Position;
            public Vector3 PrevPosition;
            public Vector3 Acceleration;
        }

        private int _resolution;
        private float _ropeLength = 3f;
        
        private float _particleDistance;
        private RopeParticle[] _particles;
        private Vector3[] _particlePositions;
        
        private const float AirFriction = 0.035f;

        public Rope(int resolution, float ropeLength, Vector3 anchorPos)
        {
            _resolution = resolution;
            _ropeLength = ropeLength;
            _particleDistance = _ropeLength / _resolution;
            
            _particles = new RopeParticle[_resolution];
            _particlePositions = new Vector3[_resolution];

            for (int i = 0; i < _resolution; ++i)
            {
                RopeParticle particle = new RopeParticle();
                particle.Position = particle.PrevPosition = anchorPos + new Vector3(0, -i * _particleDistance, 0);
                particle.Acceleration = i == 0 ? Vector3.zero : Physics.gravity;
                _particles[i] = particle;
            }
        }

        public void UpdatePhysics(Vector3 newAnchorPos, float deltaTime)
        {
            if (_particles == null)
            {
                return;
            }
            
            // update first particle by input
            RopeParticle fp = _particles[0];
            fp.PrevPosition = fp.Position;
            fp.Position = newAnchorPos;
            // turn input to acceleration
            fp.Acceleration = (fp.Position - fp.PrevPosition) / Time.fixedDeltaTime;

            ApplyVerletIntegration(deltaTime);
            RelaxConstraints();
        }

        public Vector3[] GetRopePositions()
        {
            for (int i = 0; i < _resolution; ++i)
            {
                _particlePositions[i] = _particles[i].Position;
            }

            return _particlePositions;
        }

        private void ApplyVerletIntegration(float deltaTime)
        {
            float sqredTime = deltaTime * deltaTime;
            for (int i = 1; i < _resolution; ++i)
            {
                RopeParticle p = _particles[i];
                Vector3 velocity = p.Position - p.PrevPosition;
                velocity *= (1 - AirFriction);
                
                Vector3 newPosition = p.Position + velocity + p.Acceleration * sqredTime;
                p.PrevPosition = p.Position;
                p.Position = EncounterCollision(p.PrevPosition, newPosition);
            }
        }

        private Vector3 EncounterCollision(Vector3 position, Vector3 nextPosition)
        {
            float collisionRad = 0.1f;
            
            Collider[] colliders = Physics.OverlapSphere(nextPosition, collisionRad);
            
            if (colliders == null)
            {
                return nextPosition;
            }

            foreach (Collider collider in colliders)
            {
                Vector3 closestPoint = collider.ClosestPoint(nextPosition);
                
                if (Vector3.Distance(nextPosition, closestPoint) < collisionRad)
                {
                    // Push the particle out of the collider
                    Vector3 direction = (nextPosition - closestPoint).normalized;
                    nextPosition = closestPoint + direction * collisionRad;
                }
            }

            return nextPosition;
        }
        
        private void RelaxConstraints()
        {
            for (int j = 0; j < 2; ++j)
            {
                if (j % 2 == 0)
                {
                    for (int i = 0; i < _resolution - 1; ++i)
                    {
                        RelaxConstraint(_particles[i], _particles[i + 1]);
                    }
                }
                else
                {
                    for (int i = _resolution - 2; i >= 0; --i)
                    {
                        RelaxConstraint(_particles[i], _particles[i + 1]);
                    }
                }
            }
        }

        private void RelaxConstraint(RopeParticle p1, RopeParticle p2, float weight1 = 0.5f, float weight2 = 0.5f)
        {
            Vector3 deltaVec = p2.Position - p1.Position;
            Vector3 direction = deltaVec.normalized;
            float delta = deltaVec.magnitude - _particleDistance;
            
            if (delta is < 0.001f and > - 0.001f)
            {
                return;
            }

            if (p1 == _particles[0])
            {
                weight1 = 0;
                weight2 = 1;
            }

            p1.Position += direction * (delta * weight1);
            p2.Position -= direction * (delta * weight2);
        }
    }
}