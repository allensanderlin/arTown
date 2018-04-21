using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour {

	float speed = 0.1f;
	Enemy targetEnemy;

	public void Fire(Vector3 launchPos, Enemy target)
	{
		transform.position = launchPos;
		targetEnemy = target;
	}
	
	// Update is called once per frame
	void Update () {
		// for now, just seek to enemy
		Vector3 vectorToEnemy = targetEnemy.transform.position - transform.position;
		if(vectorToEnemy.magnitude <= 0.1f)
		{
			// kill us both
			Destroy(gameObject);
			TownLocator.instance.DestroyEnemy(targetEnemy);
		}
		vectorToEnemy.y = 0;

		Vector3 newPos = transform.position + vectorToEnemy.normalized * Time.deltaTime * speed;
		transform.position = newPos;
	}
}
