using System;
using System.Reflection;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// ��ͨ��Get��ȡ�̳д�����������ÿ����Ķ���Ӧ����Ψһ��
    /// </summary>
    public class Service : MonoBehaviour
    {
        protected virtual void Awake()
        {
            ServiceLocator.Register(this);
        }

        protected virtual void Start()
        {
            GetOtherService();
            //Init();
            //ServiceLocator.ServiceInit?.Invoke(this);
        }

        //protected virtual void Init() { }

        /// <summary>
        /// ��ȡOther���Եķ���
        /// </summary>
        internal void GetOtherService()
        {
            FieldInfo[] infos = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo info in infos)
            {
                Type type = info.FieldType;
                if (info.GetCustomAttribute(typeof(OtherAttribute), true) 
                    is OtherAttribute attribute && type.IsSubclassOf(typeof(Service)))
                {
                    if (attribute.type != null && attribute.type.IsSubclassOf(type))
                        type = attribute.type;
                    info.SetValue(this, ServiceLocator.Get(type));
                }
            }
        }
    }

    /// <summary>
    ///�Զ���ȡ�������񣬽�����Service������
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class OtherAttribute : Attribute
    {
        internal Type type;

        /// <param name="_type">��ȡB��ʵ������ֵ��A��ʱ��B��̳�A�ࣩ����Ҫָ��B�������Ϊ������������ָ������</param>
        public OtherAttribute(Type _type = null)
        {
            type = _type;
        }
    }
}