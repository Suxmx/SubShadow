using System;
using System.Reflection;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// 可通过Get获取继承此类的子类对象，每个类的对象应当是唯一的
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
        /// 获取Other特性的服务
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
    ///自动获取其他服务，仅用于Service的子类
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class OtherAttribute : Attribute
    {
        internal Type type;

        /// <param name="_type">获取B类实例并赋值给A类时（B类继承A类），需要指定B类的类型为参数，否则不用指定参数</param>
        public OtherAttribute(Type _type = null)
        {
            type = _type;
        }
    }
}