using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        private Experience exp;
        
        private void Awake()
        {
            exp = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }
    
        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}",exp.GetExperience());
        }
    }
}
