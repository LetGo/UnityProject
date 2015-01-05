using UnityEngine;
using System.Collections;

public class EntityComponent : MonoBehaviour {
	public BattleEntity battleEntity;

	public void SetIdle(){
		if (battleEntity != null) {
			battleEntity.ChangeAnimStatus(EntityAnimStatus.Idel);		
		}
	}

    public void Hurt(int a)
    {

    }

    public void OnAnimationMsg()
    {
        Debug.Log("OnAnimationMsg");
    }
}
