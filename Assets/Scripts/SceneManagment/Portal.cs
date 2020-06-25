using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;
using UnityEngine.Diagnostics;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A,B,C,D,E
        }
        
        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private DestinationIdentifier destination;
        [SerializeField] private float fadeOutTime = 1f;
        [SerializeField] private float fadeInTime = 1f;
        [SerializeField] private float fadeWaitTime = 0.5f;
        private void Start()
        {
            
        }

        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(Transition());
            }
        }
        
        private IEnumerator Transition()
        {
           
            if (sceneToLoad < 0)
            {
                yield break;
            }

            Fader fader = FindObjectOfType<Fader>();
            
            DontDestroyOnLoad(gameObject);
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.transform.position);
            player.transform.rotation = otherPortal.spawnPoint.transform.rotation;

        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if(portal.destination != destination) continue;

                return portal;
            }

            return null;
        }
    }
}
