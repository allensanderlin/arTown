using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	float speed = 0.03f;
	public Animator enemyAnimator;
	public float attackDistance = .1f;
	Transform attackTarget;

	public void Initialize(Vector3 spawnPosition, Transform target)
	{
		transform.position = spawnPosition;
		transform.LookAt(target.position);
		attackTarget = target;
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance(transform.position, attackTarget.position);
		if(distance > attackDistance)
		{
			Vector3 newPos = transform.position;
			newPos += transform.forward * Time.deltaTime * speed;
			transform.position = newPos;
		}else{
			enemyAnimator.SetBool("isAttacking", true);
		}

	}
}
