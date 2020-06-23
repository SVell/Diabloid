using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
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

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }
        
        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void Animate()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            anim.SetFloat("ForwardSpeed", speed);

        }
    }
}
