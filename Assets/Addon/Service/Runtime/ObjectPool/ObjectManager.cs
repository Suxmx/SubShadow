using UnityEngine;

namespace Services
{
    public class ObjectManager : Service
    {
        private ObjectManagerCore core;

        protected override void Awake()
        {
            base.Awake();
            core = new ObjectManagerCore(transform);
        }

        /// <summary>
        /// ����һ����Ϸ���壬��������еĶ������꣬����һ��������ӵ�������У��ټ���
        /// </summary>
        /// <param name="eObject">Ҫ�������Ϸ�����Ӧ��ö��</param>
        /// <param name="position">λ��</param>
        /// <param name="eulerAngles">ŷ����</param>
        /// <param name="parent">���������Ϸ������Ϊĳ����Ϸ����������壬Ĭ��������Ƕ���ص�������</param>
        /// <returns>���������Ϸ����</returns>
        public IMyObject Activate(EObject eObject, Vector3 position, Vector3 eulerAngles, Transform parent = null)
            => core.Activate(eObject, position, eulerAngles, parent);

        /// <summary>
        /// (����2D��Ϸ)����һ����Ϸ���壬��������еĶ������꣬����һ��������ӵ�������У��ټ���
        /// </summary>
        /// <param name="eObject">Ҫ�������Ϸ�����Ӧ��ö��</param>
        /// <param name="position">λ��</param>
        /// <param name="eulerAngleZ">z����ŷ����</param>
        /// <param name="parent">���������Ϸ������Ϊĳ����Ϸ����������壬Ĭ��������Ƕ���ص�������</param>
        /// <returns>���������Ϸ����</returns>
        public IMyObject Activate(EObject eObject, Vector3 position, float eulerAngleZ = 0f, Transform parent = null)
            => core.Activate(eObject, position, new Vector3(0f, 0f, eulerAngleZ), parent);
        public IMyObject Activate(EObject eObject, Vector3 position, Transform parent)
            => core.Activate(eObject, position, Vector3.zero, parent);

        /// <summary>
        /// Ԥ��������
        /// </summary>
        /// <param name="eObject">Ҫ���ɵ���Ϸ�����Ӧ��ö��</param>
        /// <param name="count">����</param>
        public void PreCreate(EObject eObject, int count)
            => core.PreCreate(eObject, count);

        public GameObject GetPrefab(EObject eObject) => core.objectDict[eObject];
        public GameObject[] GetPrefabs(params EObject[] eObjects)
        {
            GameObject[] gameObjects = new GameObject[eObjects.Length];
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i] = GetPrefab(eObjects[i]);
            }
            return gameObjects;
        }

        public GameObject Clone(EObject eObject) => 
            ObjectPoolUtility.Clone(GetPrefab(eObject)).gameObject;
    }
}

