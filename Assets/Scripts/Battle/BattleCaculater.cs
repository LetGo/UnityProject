using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class BattleCaculater :Singleton<BattleCaculater>
{
    public bool GetTargetEntity(bool isSelf)
    {
        bool flag = false;
        //List<BattleEntity> entityList = isSelf ? SelfTeamMgr.EntityList : TargetTeam.EntityList;
        //entityList.ApplyAll(C => {
        //    if(!C.IsDead && )})

        return flag;
    }

    public List<BattleEntity> GetTargetList(BattleEntity self)
    {
        List<BattleEntity> targetList = new List<BattleEntity>();


        return targetList;
    }
}

