using UnityEngine;
using System.Collections;

public class GameEvent  {

    public GameEventID m_GEID = GameEventID.GEUnDefine;
    public object m_parameter;
    public void Reset()
    {
        if (this.m_parameter != null)
            this.m_parameter = null;
        this.m_GEID = GameEventID.GEUnDefine;
    }

    public GameEvent(GameEventID id, object parameter) {
        m_GEID = id;
        m_parameter = parameter;
    }

    public GameEvent()
    {
    }
}

public enum GameEventID
{
    GEUnDefine = 0,
    GEIdle,             // 等待
    GEMove,             // 移动到某点
    GEMoveAlong,        // 沿着某方向移动
    GEAttack,
}