using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownCannon : MonoBehaviour {

	public CannonBall cannonBallPrefab;
	public Transform launchPoint;
	float attackDistance = 0.5f;
	float activationTimer = 0.0f;

	float attackTimer = 0.0f;

	public void Activate()
	{
		activationTimer = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(activationTimer > 0)
		{
			activationTimer -= Time.deltaTime;
			FindAndAttackEnemies();
		}
	}

	void FindAndAttackEnemies()
	{
		Enemy closestEnemy = TownLocator.instance.GetClosestEnemy(transform.position);
		if(closestEnemy != null)
		{
			float distance = Vector3.Distance(closestEnemy.transform.position, transform.position);
			if(distance <= attackDistance)
			{
				transform.LookAt(closestEnemy.transform.position);

				attackTimer -= Time.deltaTime;
				if(attackTimer <= 0)
				{
					attackTimer = 3.0f;

					CannonBall newCannonBall = Instantiate(cannonBallPrefab) as CannonBall;
					newCannonBall.Fire(launchPoint.transform.position, closestEnemy);
				}
			}
		}
	}
}
