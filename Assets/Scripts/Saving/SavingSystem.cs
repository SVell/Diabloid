﻿using System;
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
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            // Will close file stream automatically
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                Transform playerTransform = GetPlayerTransform();

                // Serialization and writing
                BinaryFormatter formatter = new BinaryFormatter();
                SerializableVector3 position = new SerializableVector3(playerTransform.position);
                formatter.Serialize(stream, position);
            }
        }
        
        public void Load(string loadFile)
        {
            string path = GetPathFromSaveFile(loadFile);
            print("Loading from " + path);
            
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                Transform playerTransform = GetPlayerTransform();
                BinaryFormatter formatter = new BinaryFormatter();
                SerializableVector3 position = (SerializableVector3)formatter.Deserialize(stream);
                playerTransform.position = position.ToVector();
            }
        }

        private byte[] SerializeVector(Vector3 vector)
        {
            // each 4 byte is a float
            byte[] vectorBytes = new byte[3 * 4];
            BitConverter.GetBytes(vector.x).CopyTo(vectorBytes,0);
            BitConverter.GetBytes(vector.y).CopyTo(vectorBytes,4);
            BitConverter.GetBytes(vector.z).CopyTo(vectorBytes,8);
            return vectorBytes;
        }

        private Vector3 DeserializeVector(byte[] buffer)
        {
            Vector3 result = new Vector3();
            result.x = BitConverter.ToSingle(buffer, 0);
            result.y = BitConverter.ToSingle(buffer, 4);
            result.z = BitConverter.ToSingle(buffer, 8);
            return result;
        }

        private Transform GetPlayerTransform()
        {
            return GameObject.FindWithTag("Player").transform;
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            // Saving ignoring the platform
            return Path.Combine(Application.persistentDataPath,saveFile + ".sav");
        }
    }
}
