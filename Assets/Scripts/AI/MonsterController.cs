using UnityEngine;
using System.Collections;

public enum state{
	idle,
	run,
	attack,
}
public class MonsterController : MonoBehaviour {
	public float HatredZone = 4f;//仇恨区
	public float AttackRange = 1f;
	public GameObject player;

	public Transform[] idleTransform; //待机巡逻位置。
	public NavMeshAgent navMeshAgent;//Robot的导航网格代理
	public state m_state = state.idle;
	public int index = 0;
	Vector3 HatredZonePos;
	public LayerMask maskTarget = LayerMask.NameToLayer("role");

	void Start () {
		HatredZonePos = transform.position;
		animation.wrapMode = WrapMode.Loop;
		animation.Play ("idle");
		navMeshAgent.SetDestination(transform.position);
	}

	bool isidle = false;
	void FixedUpdate () {

		Collider[] cols = Physics.OverlapSphere(HatredZonePos, HatredZone,maskTarget);
		if(cols.Length > 0){
			Collider[] Attackcols = Physics.OverlapSphere(transform.position, AttackRange,maskTarget);
			if(Attackcols.Length > 0){
				navMeshAgent.Stop();
				m_state = state.attack;
			}else{
				Collider currentCollider = cols[Random.Range(0, cols.Length)];
				GameObject targetTemp= currentCollider.gameObject;
				if(targetTemp != null){
					navMeshAgent.SetDestination(targetTemp.transform.position);
				}
			}
		}else{
			if(navMeshAgent.hasPath && navMeshAgent.remainingDistance != 0){
				m_state = state.run;
			}else{
				m_state = state.idle;
			}
		}

		if (m_state == state.idle) {
			if(!isidle){
				isidle = true;
				StopAllCoroutines();
				StartCoroutine(Idle());
			}
		}else if(m_state == state.run){
			if(isidle){
				animation.wrapMode = WrapMode.Loop;
				animation.CrossFade ("run");
				isidle = false;
			}
		}
		else if(m_state == state.attack){
			animation.wrapMode = WrapMode.Loop;
			animation.CrossFade ("baseattack2");
		}
	}


	IEnumerator Idle() {
		animation.wrapMode = WrapMode.Loop;
		animation.CrossFade ("idle");
		yield return new WaitForSeconds (1.5f);

		if (index >= idleTransform.Length) {
			index = 0;
		}
		navMeshAgent.SetDestination(idleTransform[index++].position);

	}
}
