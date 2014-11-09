using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UniCommon
{
    public static class UniCommon
    {
        public static void ApplyAll<T>(this List<T> sourList, Action<T> action)
        {
            int count = sourList.Count;
            for (int i = 0; i < count; ++i)
            {
                action(sourList[i]);
            }
        }

        public static void ApplyAll<T>(this T[] sourList, Action<T> action)
        {
            int count = sourList.Length;
            for (int i = 0; i < count; ++i)
            {
                action(sourList[i]);
            }
        }

        public static void ApplyAllByIenumerable<T>(this IEnumerable<T> sourList, Action<T> action)
        {
            foreach (var child in sourList)
            {
                action(child);
            }
        }

        public static void CleanUpChild(this UnityEngine.Transform parent)
        {
            foreach (UnityEngine.Transform child in parent)
            {
                UnityEngine.GameObject.Destroy(child.gameObject);
            }
        }

        public static void CleanUpChildImmediate(this UnityEngine.Transform parent)
        {
            foreach (UnityEngine.Transform child in parent)
            {
                UnityEngine.GameObject.DestroyImmediate(child.gameObject);
            }
        }

        public static GameObject AddGameObject(this GameObject parent, 
            string path,Vector3 pos,Vector3 scale,
            Quaternion q,bool bLocalPos,string name = null)
        {
            GameObject prefab = Resources.Load(path) as GameObject;
            if (prefab == null)
            {
                Debug.LogError("AddGameObject failed path :" + path);
                return null;
            }

            GameObject go = GameObject.Instantiate(prefab) as GameObject;
            go.layer = parent.layer;

            if (!string.IsNullOrEmpty(name))
            {
                go.name = name;
            }

            Transform t = go.transform;
            if (bLocalPos)
            {
                t.localPosition = pos;
                t.localRotation = q;
            } 
            else
            {
                t.position = pos;
                t.rotation = q;
            }
            t.localScale = scale;
            return go;
        }
    
        /// <summary>
        /// 使用此方法设置text 方便后期本地化
        /// </summary>
        /// <param name="label"></param>
        /// <param name="content"></param>
        public static void SetString(this UILabel label,string content)
        {
            //TODO 根据content获取相应的text
            label.text = content;
        }
    }

}
