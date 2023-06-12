using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    internal class ObjectManagerData : ScriptableObject
    {
        [SerializeField]
        internal EObjectPrefabPair[] datas;
    }
}