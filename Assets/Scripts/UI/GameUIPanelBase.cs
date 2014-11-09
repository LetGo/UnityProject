using UnityEngine;
using System.Collections;
/// <summary>
/// ui窗口基类
/// </summary>
public abstract class GameUIPanelBase : MonoBehaviour
{
    public virtual void InitOnce()
    {

    }

    protected virtual void InitWhileOpen()
    {

    }

    public virtual void OnOpen()
    {
        this.InitWhileOpen();
    }

    public virtual void OnClose()
    {
        this.UnInitWhileClose();
    }

    public virtual void UnInitWhileClose()
    {

    }

    /// <summary>
    /// 通过窗口ID发送消息
    /// </summary>
    /// <param name="msgID"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public abstract bool OnMsg(PanelMsgID msgID, object obj = null);
}
