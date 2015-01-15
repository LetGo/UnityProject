using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniCommon;

public class BattleManager : Singleton<BattleManager> {

	public BattlePositionMgr battlePositionMgr;
	public BattleTeamMgr SelfTeamMgr;
	public BattleTeamMgr TargetTeam;
	public BattleStatus CurrentBattleStatus = BattleStatus.None;
	public bool IsBattleOver{ get; set;}
	EntityMoveMgr entityMoveMgr = null;
	//TEST
	List<HeroDada> selfEntity;
    List<HeroDada> targetEntity;
	public override void Initialize ()
	{
		base.Initialize ();
		IsBattleOver = false;
		Camera.main.transform.position = new Vector3 (9,4,-1);
		Camera.main.transform.rotation = Quaternion.Euler (new Vector3 (20, -83, 0));

        selfEntity = new List<HeroDada>();
		for (int i = 0; i < 1; i++) {
            HeroDada data = HerosManager.Instance.Clone(1001,10);
            if (data != null)
            {
                data.pos = (uint)(i + 1);
                selfEntity.Add(data);
            }
		}

        targetEntity = new List<HeroDada>();
		for (int i = 0; i < 3; i++) {
            HeroDada data = HerosManager.Instance.Clone(1001,Random.Range(1,11));

            if (data != null)
            {
                data.pos = (uint)(i + 1);
                targetEntity.Add(data);
            }
		}

		entityMoveMgr = new EntityMoveMgr ();

		SelfTeamMgr = new BattleTeamMgr ();
		TargetTeam =  new BattleTeamMgr ();

		//get battlePositionMgr
		GameObject prefab = Resources.Load ("FightPostion") as GameObject;
		if (prefab != null) {
			battlePositionMgr = (GameObject.Instantiate(prefab) as GameObject).GetComponent<BattlePositionMgr>();
		}
		battlePositionMgr.SetPosition ();
	}

	public override void UnInitialize ()
	{
		base.UnInitialize ();
	}

	public void InitScene(){

	}

	public void InitBattleEntitys(){
		CurrentBattleStatus = BattleStatus.Preparing;
		//self
		for (int i = 0; i < selfEntity.Count; i++) {
			BattleEntity temp = EntityCreator.Create (selfEntity[i],true);
			SelfTeamMgr.AddEntity (temp);
		}
		
		//target
		targetEntity.ApplyAll (C => TargetTeam.AddEntity (EntityCreator.Create (C,false)));

		//set idle
		SelfTeamMgr.SetAllIdel ();
		TargetTeam.SetAllIdel ();
		//move to position
		entityMoveMgr.BeginMoveToPostion (SelfTeamMgr);
		entityMoveMgr.BeginMoveToPostion (TargetTeam);
	}

    public List<BattleEntity> GetTargetEntity(bool isSelf)
    {
        List<BattleEntity> targetList = new List<BattleEntity>();
        List<BattleEntity> entityList = isSelf ? TargetTeam.EntityList : SelfTeamMgr.EntityList;
        entityList.ApplyAll(C =>
        {
            if (!C.IsDead && !C.entityBattleMgr.isFighting)
            {
                targetList.Add(C);
            }
        });

        return targetList;
    }

	public void Update(float deltaTime){
        if (SelfTeamMgr != null)
        {
            SelfTeamMgr.EntityList.ApplyAll(C => { 
                if (!C.IsDead)
                {
                    C.Update(deltaTime);
                }
            });
        }

        if (TargetTeam != null)
        {
            TargetTeam.EntityList.ApplyAll(C =>
            {
                if (!C.IsDead)
                {
                    C.Update(deltaTime);
                }
            });
        }
	}

	public void CheckBattleOver(bool IsSelfTeam){
		if (IsSelfTeam && SelfTeamMgr.CheckIfAllDead()) {
			Debug.Log("BattleOver enimy win");
			TargetTeam.SetAllWin();
			IsBattleOver = true;
		}else if(TargetTeam.CheckIfAllDead()){
			Debug.Log("BattleOver self win");
			SelfTeamMgr.SetAllWin();
			IsBattleOver = true;
		}
	}
}
