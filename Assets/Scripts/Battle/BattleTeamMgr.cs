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

	public void ResetData(){
		if (EntityList != null) {
			EntityList.ApplyAll(C => C.Destroy());
			EntityList.Clear ();
		}
	}

	public void SetAllIdel(){
		EntityList.ApplyAll(C => C.ChangeAnimStatus(EntityAnimStatus.Idel));
	}

	public void SetAllRun(){
		EntityList.ApplyAll(C => C.ChangeAnimStatus(EntityAnimStatus.Move));
	}
}
