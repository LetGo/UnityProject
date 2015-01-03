using UnityEngine;
using System.Collections;

public class EntityComponent : MonoBehaviour {
	public BattleEntity battleEntity;

	public void SetIdle(){
		if (battleEntity != null) {
			battleEntity.ChangeAnimStatus(EntityAnimStatus.Idel);		
		}
	}
}
