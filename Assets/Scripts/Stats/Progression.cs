using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses = null;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            foreach (ProgressionCharacterClass progressionCharacter in characterClasses)
            {
                if (progressionCharacter.characterClass == characterClass)
                {
                    return progressionCharacter.health[level - 1];
                }
            }

            return 0;
        }
        
        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public float[] health;
        }
    }
}
