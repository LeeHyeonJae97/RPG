using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TigerForge
{
    public static class EasyFileSaveJSONExtension
    {
        public static void AddSerialized<T>(this EasyFileSave easyFileSave, string key, T data, bool ignoreExistingKey = false)
        {
            string json = easyFileSave.Serialize<T>(data);
            easyFileSave.Add(key, json, ignoreExistingKey);
        }

        public static T GetDeserialized<T>(this EasyFileSave easyFileSave, string key)
        {
            try
            {
                string data = (string)easyFileSave.GetData(key);
                if (data != null)
                    return easyFileSave.Deserialize<T>(data);
                else
                    return default(T);
            }
            catch (System.Exception)
            {
                easyFileSave.Warning("[Easy File Save] GetDeserializedObject error using key: " + key);
                return default(T);
            }
        }

        internal static string Serialize<T>(this EasyFileSave easyFileSave, T data)
        {
            return JsonUtility.ToJson(data);
        }

        internal static T Deserialize<T>(this EasyFileSave easyFileSave, string data)
        {
            return JsonUtility.FromJson<T>(data);
        }
    }
}
