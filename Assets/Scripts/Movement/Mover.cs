using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using RPG.Core;
using RPG.Resources;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction , ISaveable
    {
        [SerializeField] private float maxSpeed = 6f;
        
        private NavMeshAgent navMeshAgent;
        private Animator anim;
    
        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
        }
    
        void Update()
        {
            navMeshAgent.enabled = !GetComponent<Health>().IsDead();
            
            Animate();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination,speedFraction);
        }
        
        public void MoveTo(Vector3 destination,float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            //navMeshAgent.enabled = !GetComponent<Health>().IsDead();
            navMeshAgent.isStopped = true;
        }

        private void Animate()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            anim.SetFloat("ForwardSpeed", speed);

        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3) state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
