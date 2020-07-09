using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            Dictionary<string,object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        private void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            // Will close file stream automatically
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                // Serialization and writing
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        public void Load(string loadFile)
        {
            RestoreState(LoadFile(loadFile));
        }

        private Dictionary<string,object> LoadFile(string loadFile)
        { 
            string path = GetPathFromSaveFile(loadFile); 
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            print("Loading from " + path);
           
           using (FileStream stream = File.Open(path, FileMode.Open))
           {
               BinaryFormatter formatter = new BinaryFormatter();
               return (Dictionary<string,object>)formatter.Deserialize(stream);
           }
        }


        private void CaptureState(Dictionary<string,object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }
        }
        
        private void RestoreState(Dictionary<string,object> state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>) state;
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveable.GetUniqueIdentifier();
                if (stateDict.ContainsKey(id))
                {
                    saveable.RestoreState(state[id]);
                }
                
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            // Saving ignoring the platform
            return Path.Combine(Application.persistentDataPath,saveFile + ".sav");
        }
    }
}
