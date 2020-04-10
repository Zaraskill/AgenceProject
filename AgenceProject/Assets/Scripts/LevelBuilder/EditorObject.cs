using UnityEngine;
using System;

public class EditorObject : MonoBehaviour
{
    public enum ObjectType { Player, Ennemi, Sticky_Brick, Heavy_Brick, Light_Brick, Destroyable_Brick };

    [Serializable] // serialize the Data struct
    public struct Data
    {
        public Vector3 pos;
        public Quaternion rot;
        public ObjectType objectType;
    }

    public Data data;
}
