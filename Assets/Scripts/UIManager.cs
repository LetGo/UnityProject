using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniCommon;

public class UIManager :Singleton<UIManager>
{
    //当待删除列表超过这个数，整体destroy 在GC
    const int MAX_PANEL_TODESTROY = 4;
    //加入到待删除列表的时间，超过就加入m_lstReadyDestroyPanel
    const float DESTROY_TIME = 10.0f;

    private UIMgrComponent m_UIMgrComponent;
    //窗口列表
    private Dictionary<UIPanelID, GameUIPanelBase> m_dicPanelUI = null;
    
    //每个窗口关掉的时间累计
    private Dictionary<UIPanelID, float> m_dicPanelUIDisableTimeCount = null;

    //待删除列表 当窗口关闭时加入此列表，当次列表打到一定数量一起destroy，如果中途打开则移除
    private List<UIPanelID> m_lstReadyDestroyPanel = null;

    public override void Initialize()
    {
        base.Initialize();
        m_dicPanelUI =  new Dictionary<UIPanelID, GameUIPanelBase>();
        m_dicPanelUIDisableTimeCount = new Dictionary<UIPanelID, float>();
        m_lstReadyDestroyPanel = new List<UIPanelID>();

        GameObject go = CommonTool.AddGameObject(null, "UI/UI Root", Vector3.zero, Vector3.one, Quaternion.identity, true);
        m_UIMgrComponent = go.GetComponent<UIMgrComponent>();
    }

    public override void UnInitialize()
    {
        DestroyAllUI(true);

        m_dicPanelUI.Clear();
        m_dicPanelUI = null;

        m_dicPanelUIDisableTimeCount.Clear();
        m_dicPanelUIDisableTimeCount = null;

        m_lstReadyDestroyPanel.Clear();
        m_lstReadyDestroyPanel = null;

       
        base.UnInitialize();
    }

    public void Update()
    {

    }


    public GameUIPanelBase OpenUIpanel(UIPanelID panelID)
    {
        GameUIPanelBase panel = null;
        if (ContainUI(panelID))
        {
            panel = m_dicPanelUI[panelID];
            RemoveReadyDestroyUI(panelID,true);
        } 
        else
        {
            //第一次打开执行第一次打开只执行一次的函数
            panel = InstanceUIPanel(panelID, true);
            if(panel != null) panel.InitOnce();
        }

        if (panel != null)
        {
            panel.OnOpen();
        }

        return panel;
    }

    public void CloseUIPanel(UIPanelID id, bool bDestoryNow = false)
    {
        if (ContainUI(id))
        {
             GameUIPanelBase panel = m_dicPanelUI[id];
            panel.OnClose();
            if (bDestoryNow)
            {
                GameObject.Destroy(panel.gameObject);
                m_dicPanelUI.Remove(id);
                RemoveReadyDestroyUI(id, true);
            } 
            else
            {
                panel.gameObject.SetActive(false);
                AddToReadyDestroyUI(id);
            }
        } 
    }

    /// <summary>
    /// 发送消息到指定窗口
    /// </summary>
    /// <param name="panelid"></param>
    /// <param name="msgID"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool SendMsgToUIPanel(UIPanelID panelid,PanelMsgID msgID,object obj = null)
    {
        bool isDeal = false;
        if (ContainUI(panelid))
        {
            isDeal = m_dicPanelUI[panelid].OnMsg(msgID,obj);
        } 
        else
        {
            Debug.LogWarning("there is not UIPanel which ID is " + (int)panelid);
        }
        return isDeal;
    }

    bool ContainUI(UIPanelID panelid)
    {
        return m_dicPanelUI.ContainsKey(panelid);
    }

    /// <summary>
    /// 加入计时列表
    /// </summary>
    /// <param name="id"></param>
    private void AddToReadyDestroyUI(UIPanelID id)
    {
        if (!m_dicPanelUIDisableTimeCount.ContainsKey(id))
        {
            m_dicPanelUIDisableTimeCount.Add(id, Time.realtimeSinceStartup);
        } 
        else
        {
            //从新计时
            m_dicPanelUIDisableTimeCount[id] = Time.realtimeSinceStartup;
        }

        int index = m_lstReadyDestroyPanel.IndexOf(id);
        if (index != -1)
        {
            m_lstReadyDestroyPanel.RemoveAt(index);
        }
    }

    /// <summary>
    /// 移除计时列表
    /// </summary>
    /// <param name="id"></param>
    /// <param name="bRemove"> 从待删除列表移除</param>
    private void RemoveReadyDestroyUI(UIPanelID id, bool bRemove)
    {
        if (m_dicPanelUIDisableTimeCount.ContainsKey(id))
        {
            m_dicPanelUIDisableTimeCount.Remove(id);
            if (!bRemove) return;

            int index = m_lstReadyDestroyPanel.IndexOf(id);
            if (index != -1)
            {
                m_lstReadyDestroyPanel.RemoveAt(index);
            }
        }
    }

    private GameUIPanelBase InstanceUIPanel(UIPanelID id, bool bShowWhileCreat)
    {
        //从配置获取资源路径 todo
        string resPath;
        if (!DictMgr.Instance.UIConfigDic.ContainsKey((int)id))
        {
            Debug.LogError("NO Contains key :" + (int)id);
            resPath = "UI/Panel/LoadingPanel";
        }
        else
        {
            resPath = DictMgr.Instance.UIConfigDic[(int)id].path;
        }
       


        GameObject go = CommonTool.AddGameObject(m_UIMgrComponent.CenterAnchor,resPath,
            Vector3.zero,Vector3.one,Quaternion.identity,true);

        GameUIPanelBase panel = go.GetComponent<GameUIPanelBase>();
        if (panel == null)
        {
            Debug.LogError("can not find GameUIPanelBase Component in panel which id is :" + id);
            return null;
        }

        m_dicPanelUI.Add(id,panel);

        return panel;
    }

    IEnumerator EndOfFrameUnloadAssets()
    {
        yield return new WaitForEndOfFrame();
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    void DestroyNoUseUI()
    {
        if (Time.frameCount % 10 == 0 || m_dicPanelUIDisableTimeCount.Count == 0)
        {
            return;
        }

        //将关闭时间超过DESTROY_TIME 的panle加入待删除列表
        float time = Time.realtimeSinceStartup;
        foreach (var key in m_dicPanelUIDisableTimeCount.Keys)
        {
            if (time - m_dicPanelUIDisableTimeCount[key] >= DESTROY_TIME && !m_lstReadyDestroyPanel.Contains(key) )
            {
                m_lstReadyDestroyPanel.Add(key);
            }
        }

        //检测待删除列表是否超过或等于最大值 超过就删除
        if (m_lstReadyDestroyPanel.Count >= MAX_PANEL_TODESTROY)
        {
            foreach (var key in m_lstReadyDestroyPanel)
            {
                if (m_dicPanelUI.ContainsKey(key))
                {
                    GameObject.Destroy(m_dicPanelUI[key].gameObject);
                    m_dicPanelUI.Remove(key);
                }
                RemoveReadyDestroyUI(key, false);
            }
            m_lstReadyDestroyPanel.Clear();
            GameManager.Instance.UnLoadUnuseAsset();
        }
    }

    public GameUIPanelBase GetPanelByID(UIPanelID id)
    {
        if (ContainUI(id))
        {
            return m_dicPanelUI[id];
        }
        return null;
    }

    public void DestroyAllUI(bool bRemoveAsset)
    {
        foreach ( KeyValuePair<UIPanelID,GameUIPanelBase> kvPair in m_dicPanelUI)
        {
           
            if (kvPair.Value != null)
            {
                kvPair.Value.OnClose();
                GameObject.Destroy(kvPair.Value.gameObject);
            }
           
        }
        
        m_dicPanelUI.Clear();
        m_dicPanelUIDisableTimeCount.Clear();
        m_lstReadyDestroyPanel.Clear();

        if (bRemoveAsset)
        {
            Resources.UnloadUnusedAssets();
        }
    }
}
