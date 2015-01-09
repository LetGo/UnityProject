using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniCommon;

public class EntityTest{
	public int pos;//1-6
	public string model;
	public bool self;
	public EntityTest(int p,string m,bool s){
		pos = p;
		model = m;
		self = s;
	}
}

public class BattleManager : Singleton<BattleManager> {

	public BattlePositionMgr battlePositionMgr;
	public BattleTeamMgr SelfTeamMgr;
	public BattleTeamMgr TargetTeam;
	public BattleStatus CurrentBattleStatus = BattleStatus.None;
	public bool IsBattleOver{ get; set;}
	EntityMoveMgr entityMoveMgr = null;
	//TEST
	List<EntityTest> selfEntity;
	List<EntityTest> targetEntity;
	public override void Initialize ()
	{
		base.Initialize ();
		IsBattleOver = false;
		Camera.main.transform.position = new Vector3 (9,4,-1);
		Camera.main.transform.rotation = Quaternion.Euler (new Vector3 (20, -83, 0));

		selfEntity = new List<EntityTest> ();
		for (int i = 0; i < 1; i++) {
			EntityTest temp = new EntityTest(i,"player_1_1",true);
			selfEntity.Add(temp);
		}

		targetEntity = new List<EntityTest> ();
		for (int i = 0; i < 3; i++) {
			EntityTest temp = new EntityTest(i,"player_1_2",false);
			targetEntity.Add(temp);
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
			BattleEntity temp = EntityCreator.Create (selfEntity[i]);
			SelfTeamMgr.AddEntity (temp);
		}
		
		//target
		targetEntity.ApplyAll (C => TargetTeam.AddEntity (EntityCreator.Create (C)));

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
