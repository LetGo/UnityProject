using UnityEngine;
using System.Collections;

public class ShowScopeOnGizmos : MonoBehaviour {
	public float HatredZone = 4f;//仇恨区
	public float AttackRange = 1f;
	public Transform target;
	Transform m_trans;
	Vector3 HatredZonePos;
	void Awake(){
		if (target == null) {
			m_trans = transform;		
		}else{
			m_trans = target;
		}
		HatredZonePos = m_trans.position;
	}

	void OnDrawGizmos(){
		if (m_trans != null) {
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere (HatredZonePos, HatredZone);

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere (m_trans.position, AttackRange);
		}
	}
}
