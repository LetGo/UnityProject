using UnityEngine;
using System.Collections;

public class UIMgrComponent : MonoBehaviour {
    public GameObject CenterAnchor;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
