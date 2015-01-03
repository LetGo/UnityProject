using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattlePositionMgr : MonoBehaviour {
	public List<Transform> SelfPostions = new List<Transform>(); //一共7个包括中心位置 index= 6为中心位置
	public List<Transform> TargetPostions = new List<Transform>();

	public void SetPosition(){

	}

	public Transform GetPositionTransform(BattlePositionType posType,bool bself){
		Transform pos = null;
		if (posType == BattlePositionType.None || posType == BattlePositionType.Count)
		{			
			Debug.LogError("传入位置类型出错!");
			return pos;
		}
		
		if (!bself) {
			pos	= TargetPostions[(int)posType];	
		}else{ 
			pos	= SelfPostions[(int)posType];
		}
		return pos;
	}
}
