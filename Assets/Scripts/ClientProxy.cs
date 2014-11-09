using UnityEngine;
using System.Collections;

public static class ProgramRoot
{
    private static GameObject ms_node = null;

    public static Transform Node
    {
        get
        {
            if (ms_node == null)
            {
                ms_node = new GameObject("ProgramRoot");
                GameObject.DontDestroyOnLoad(ms_node);
            }
            return ms_node.transform;
        }
    }
}

public class ClientProxy : MonoBehaviour {

    [SerializeField]
    private int m_itargetFPS = 60;
    [SerializeField]
    private bool m_bshowFPS = true;
    private FPSCounter m_fpsCounter = null;

	void Awake () 
    {
        Application.targetFrameRate = m_itargetFPS;
        GameObject.DontDestroyOnLoad(this);
        CreateFPSCounter();
        transform.parent = ProgramRoot.Node;
	}

    void Start()
    {
        GameManager.Instance.Initialize();
        GameManager.Instance.ClientProxy = this;
        m_fpsCounter.Start();
    }

	// Update is called once per frame
	void Update () {
        m_fpsCounter.Update();
        GameManager.Instance.MainUpdate();
	}

    private void CreateFPSCounter()
    {
        m_fpsCounter = new FPSCounter();
    }

    void OnGUI()
    {
        if (m_bshowFPS)
        {
            GUI.Label(new Rect(0, Screen.height - 30, 200, 200), "FPS:" + m_fpsCounter.FPS.ToString("f2"));
        }
    }

    public void UnLoadUnuseAsset()
    {
        StartCoroutine(EndOfFrameGC());
    }

    IEnumerator EndOfFrameGC()
    {
        yield return new WaitForEndOfFrame();
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }


    void OnApplicationQuit()
    {
        GameManager.Instance.UnInitialize();
    }
}
