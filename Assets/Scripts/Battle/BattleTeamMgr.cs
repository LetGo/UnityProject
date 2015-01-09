using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniCommon;

public class BattleTeamMgr  {

	public List<BattleEntity> EntityList;

	public BattleTeamMgr(){
		EntityList = new List<BattleEntity> ();
	}

	public void AddEntity(BattleEntity entity){
		EntityList.Add (entity);
	}

    public void RemoveEntity(BattleEntity entity)
    {
        Debug.Log("RemoveEntity :" + entity.IsSelfTeam);
    }

	public bool CheckIfAllDead(){
		for (int i= 0; i< EntityList.Count; ++i) {
		if(!EntityList[i].IsDead)		
			return false;
		}
		return true;
	}

	public void ResetData(){
		if (EntityList != null) {
			EntityList.ApplyAll(C => C.DestroyEntityObj());
			EntityList.Clear ();
		}
	}

	public void SetAllIdel(){
		EntityList.ApplyAll(C => C.ChangeAnimStatus(EntityAnimStatus.Idel));
	}

	public void SetAllRun(){
		EntityList.ApplyAll(C => C.ChangeAnimStatus(EntityAnimStatus.Move));
	}

	 public void SetAllWin(){
		EntityList.ApplyAll(C => {if(!C.IsDead)C.ChangeAnimStatus(EntityAnimStatus.Win);});
	}
}
