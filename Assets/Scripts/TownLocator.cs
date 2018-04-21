using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using System.Linq;

public class TownLocator : MonoBehaviour 
{
	public static TownLocator instance = null;

	private bool isInitialized = false;
	private UnityARAnchorManager unityARAnchorManager;

	public Enemy enemyPrefab = null;
	public float spawnTime = 3.0f;
	private float spawnTimer = 0f;
	public float spawnRadius = .7f;

	public LayerMask lookTargetMask;
	public GameObject selectorTarget;

	List<Enemy> enemiesList = new List<Enemy>();

	// Use this for initialization
	void Start () {
		unityARAnchorManager = new UnityARAnchorManager();
		spawnTimer = spawnTime;
		instance = this;
	}

	// Update is called once per frame
	void Update () {
		if(!isInitialized)
		{
			IEnumerable<ARPlaneAnchorGameObject> arpags = unityARAnchorManager.GetCurrentPlaneAnchors ();
			var planeAnchor = arpags.FirstOrDefault();
			if(planeAnchor != null)
			{
				transform.position = planeAnchor.gameObject.transform.position;
				isInitialized = true;
			}
		}

		{
			// start spawing enemies
			spawnTimer -= Time.deltaTime;
			if(spawnTimer <= 0)
			{
				spawnTimer = spawnTime;

				Vector3 spawnPosition = transform.position;
				Vector3 randomDirection = Random.insideUnitSphere;
				randomDirection.y = 0;
				spawnPosition += randomDirection.normalized * spawnRadius;

				Enemy newEnemy = Instantiate(enemyPrefab, transform) as Enemy;
				newEnemy.Initialize(spawnPosition, transform);
				enemiesList.Add(newEnemy);
			}
		}

		UpdateLookTarget();
	}

	public void DestroyEnemy(Enemy targetEnemy)
	{
		enemiesList.Remove(targetEnemy);
		Destroy(targetEnemy.gameObject);
	}


	public void UpdateLookTarget()
	{
		RaycastHit hit;
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		if (Physics.Raycast(ray, out hit, float.MaxValue, lookTargetMask)) {
			//Debug.Log("UpdateLookTarget " + hit.collider.gameObject.name);
			selectorTarget.transform.position = hit.collider.gameObject.transform.position;
			if(!selectorTarget.activeSelf)
			{
				selectorTarget.SetActive(true);
			}

			TownCannon cannon = hit.collider.gameObject.GetComponent<TownCannon>();
			if(cannon != null)
			{
				cannon.Activate();
			}
		}else{
			if(selectorTarget.activeSelf)
			{
				selectorTarget.SetActive(false);
			}
		}
	}

	public Enemy GetClosestEnemy(Vector3 position)
	{
		float closestDist = 0;
		Enemy closestEnemy = null;
		foreach (var enemy in enemiesList) {
			float dist = Vector3.Distance(position, enemy.transform.position);

			if(closestEnemy == null || dist < closestDist)
			{
				closestEnemy = enemy;
				closestDist = dist;
			}
		}

		return closestEnemy;
	}
}
