using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Services
{
    internal class ObjectManagerCore
    {
        private readonly Transform transform;
        internal Dictionary<EObject, GameObject> objectDict;
        internal Dictionary<EObject, ObjectPool> objectPools;

        public ObjectManagerCore(Transform transform)
        {
            this.transform = transform;
            ObjectManagerData data = Resources.Load<ObjectManagerData>("ObjectManagerData");
            objectDict = new Dictionary<EObject, GameObject>();
            foreach (EObjectPrefabPair pair in data.datas)
            {
                objectDict.Add(pair.eObject, pair.prefab);
            }
            objectPools = new Dictionary<EObject, ObjectPool>();
        }

        internal IMyObject Activate(EObject eObject, Vector3 position, Vector3 eulerAngles, Transform parent)
        {
            if (!objectPools.ContainsKey(eObject))
                CreatePool(eObject);
            IMyObject obj = objectPools[eObject].Activate(position, eulerAngles, parent);
            return obj;
        }

        internal ObjectPool CreatePool(EObject eObject)
        {
            GameObject obj_pool = new GameObject($"Pool:{eObject}");
            obj_pool.transform.parent = transform;
            ObjectPool pool = obj_pool.AddComponent<ObjectPool>();
            pool.Initialize(objectDict[eObject]);
            objectPools.Add(eObject, pool);
            return pool;
        }

        internal void PreCreate(EObject eObject, int count)
        {
            if (!objectPools.ContainsKey(eObject))
                CreatePool(eObject);
            objectPools[eObject].Create(count);
        }
    }
}

