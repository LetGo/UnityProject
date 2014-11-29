using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Security.Cryptography;

namespace UniCommon
{
    public static class CommonTool
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

        public static GameObject AddGameObject(GameObject parent, 
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
            if (parent != null)
            {
                go.transform.parent = parent.transform;
                go.layer = parent.layer;
            }

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

        //加密和解密采用相同的key,可以任意数字，但是必须为32位
        //private string strkeyValue = "12345678901234567890198915689039";
        /// <summary>
        /// 内容加密
        /// </summary>
        /// <param name="ContentInfo">要加密内容</param>
        /// <param name="strkey">key值  加密和解密采用相同的key,可以任意数字，但是必须为32位</param>
        /// <returns></returns>
        public static string EncryptionContent(string ContentInfo, string strkey = "12345678901234567890198915689039")
        {

            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(strkey);

            RijndaelManaged encryption = new RijndaelManaged();

            encryption.Key = keyArray;

            encryption.Mode = CipherMode.ECB;

            encryption.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = encryption.CreateEncryptor();

            byte[] _EncryptArray = UTF8Encoding.UTF8.GetBytes(ContentInfo);

            byte[] resultArray = cTransform.TransformFinalBlock(_EncryptArray, 0, _EncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 内容解密
        /// </summary>
        /// <param name="encryptionContent">被加密内容</param>
        /// <param name="strkey">key值</param>
        /// <returns></returns>
        public static string DecipheringContent(string encryptionContent, string strkey = "12345678901234567890198915689039")
        {

            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(strkey);

            RijndaelManaged decipher = new RijndaelManaged();

            decipher.Key = keyArray;

            decipher.Mode = CipherMode.ECB;

            decipher.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = decipher.CreateDecryptor();

            byte[] _EncryptArray = Convert.FromBase64String(encryptionContent);
            Debug.LogError(_EncryptArray.Length);
            byte[] resultArray = cTransform.TransformFinalBlock(_EncryptArray, 0, _EncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);

        }
    }

}
