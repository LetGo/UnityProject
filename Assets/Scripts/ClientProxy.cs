using UnityEngine;
using System.Collections;

public class ClientProxy : MonoBehaviour {

    [SerializeField]
    private int m_itargetFPS = 60;
    [SerializeField]
    private bool m_bshowFPS = true;
    private FPSCounter m_fpsCounter = null;

	void Awake () {
        Application.targetFrameRate = m_itargetFPS;
        GameObject.DontDestroyOnLoad(this);
        CreateFPSCounter();
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

    void OnDestroy()
    {
        GameManager.Instance.UnInitialize();
    }
}
