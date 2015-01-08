using UnityEngine;
using System.Collections;

public class BattleEntity  
{
	public GameObject entityGo;
	public EntityComponent entityComponent;
	public EntityBattleMgr entityBattleMgr;
	public EntityProperties entityProperties;

    public bool IsSelfTeam { get; set; }
	public bool IsDead{ get; set;}
    public int Position { get; set; }
	EntityAnimStatus animStatus = EntityAnimStatus.None;
	float currentTime = 0;

	public BattleEntity(GameObject go,bool self){
		IsDead = false;
        entityGo = go;
		animStatus = EntityAnimStatus.None;
		currentTime = 0;
        IsSelfTeam = self;
		entityBattleMgr = new EntityBattleMgr (this);
		entityProperties = new EntityProperties (this);
	}

	public void DestroyEntityObj(){
        if (entityGo != null)
		    GameObject.Destroy (entityGo);
	}

    public void DestroyEntity()
    {
        DestroyEntityObj();
        entityBattleMgr = null;
        entityProperties = null;
    }
    /// <summary>
    /// 更新实体状态并返回是否死亡
    /// </summary>
    /// <param name="deltaTime"></param>
    /// <returns>是否死亡</returns>
	public bool Update(float deltaTime){
        if (!IsDead)
        {
            UpdateAttackProgress(deltaTime);
            return false;
        }

        Debug.LogError("IsDead");

        return true;
	}

    public void Dead()
    {
        IsDead = true;
        DestroyEntityObj();
    }

	void UpdateAttackProgress(float deltaTime){
		currentTime += deltaTime;
		if(currentTime >= entityProperties.AttackSpeed){
			currentTime = 0;
			entityBattleMgr.AddAttackRound();
		}

		entityBattleMgr.skillBeanPlayer.Update (Time.realtimeSinceStartup);	
	}

	public void ChangeAnimStatus(EntityAnimStatus status){
		animStatus = status;
		switch (animStatus) {
		case EntityAnimStatus.Idel:
			entityGo.animation["idle"].wrapMode = WrapMode.Loop;
			entityGo.animation.Play("idle");
			break;
		case EntityAnimStatus.Move:
			entityGo.animation["run"].wrapMode = WrapMode.Loop;
			entityGo.animation.Play("run");
			break;
		case EntityAnimStatus.Dead:
			break;
		}
	}
}
