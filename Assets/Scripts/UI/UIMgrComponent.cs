using UnityEngine;
using System.Collections;

public class UIMgrComponent : MonoBehaviour {
    public GameObject CenterAnchor;

    public UITextList textList;
    public void Add(string text)
    {
        textList.Add(text);
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
