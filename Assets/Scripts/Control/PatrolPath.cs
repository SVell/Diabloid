using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;

            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWaipoint(i),0.2f);
                Gizmos.DrawLine(GetWaipoint(i),GetWaipoint(j));
            }
        }

        public int GetNextIndex(int i)
        {
            int j = i + 1;
            if (j >= transform.childCount)
                return 0;
            return j;
        }

        public Vector3 GetWaipoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
