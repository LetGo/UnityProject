using UnityEngine;
using System.Collections;

public class EntityCreator  {
	static string modelPath = "Character/player_1";
	public static BattleEntity Create(EntityTest data){

        BattleEntity entity = new BattleEntity(data.self);
		entity.entityGo = LoadModle (data.model);
		Transform positionTransform = BattleManager.Instance.battlePositionMgr.GetPositionTransform ((BattlePositionType)data.pos, data.self);
		entity.entityGo.transform.parent = positionTransform;
		entity.entityGo.transform.localPosition = Vector3.zero;
		entity.entityGo.transform.localEulerAngles = Vector3.zero;
        entity.Position = data.pos;
        entity.IsSelfTeam = data.self;
// 		EntityComponent component = entity.entityGo.AddComponent<EntityComponent> ();
// 		component.battleEntity = entity;
// 		entity.entityComponent = component;
		return entity;
	}

	static GameObject LoadModle(string model){
		string path = string.Format ("{0}/{1}", modelPath, model);
		GameObject prefab = Resources.Load(path) as GameObject;
		GameObject go = null;
		if (prefab != null)
		{
			go = GameObject.Instantiate(prefab) as GameObject;
		}
		else
		{
			Debug.LogError("load error :" + path);
		}
		return go;
	}
}
