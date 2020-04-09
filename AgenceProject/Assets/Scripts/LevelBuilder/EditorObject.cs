using UnityEngine;
using System;

public class EditorObject : MonoBehaviour
{
    public enum ObjectType { Platform_1, Platform_2, Platform_3, Platform_4 };

    [Serializable] // serialize the Data struct
    public struct Data
    {
        public Vector3 pos;
        public Quaternion rot;
        public ObjectType objectType;
    }

    public Data data;
}
