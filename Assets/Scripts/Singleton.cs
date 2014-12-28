using UnityEngine;
using System.Collections;
//class 类约束指定类型T必须是引用类型
//new() 这是一个构造函数约束,指定类型T必须有一个默认构造函数
public class Singleton<T> where T :class, new()
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }

            return _instance;
        }  
    }

    public virtual void Initialize() { }

    public virtual void UnInitialize() { }
}