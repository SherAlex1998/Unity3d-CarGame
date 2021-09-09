using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoolManager  {

	private static Dictionary<string , LinkedList<GameObject>> poolsDictionary;
	private static Transform deactivatedObjectsParent;

	public static void init ( Transform pooledObjectsContainer)
	{
		Debug.Log ("init");
		deactivatedObjectsParent = pooledObjectsContainer;
		poolsDictionary = new Dictionary<string, LinkedList<GameObject>> ();
	}

	public static GameObject getGameObjectFromPool(GameObject prefab)
	{
		if (!poolsDictionary.ContainsKey (prefab.name)) {
			poolsDictionary [prefab.name] = new LinkedList<GameObject> ();
			Debug.Log ("pool create");
		}

		GameObject result;

		if (poolsDictionary [prefab.name].Count > 0) {
			result = poolsDictionary [prefab.name].First.Value;
			poolsDictionary [prefab.name].RemoveFirst ();
			result.SetActive (true);
			Debug.Log ("object from pool");
			return result;
		}

		result = GameObject.Instantiate (prefab);
		result.name = prefab.name;
		
		return result;
	}

	public static void putGameObjectToPool (GameObject target)
	{
		if (target) {
			if (poolsDictionary [target.name] != null) {
				poolsDictionary [target.name].AddFirst (target);
				target.transform.parent = deactivatedObjectsParent;
				target.SetActive (false);
				Debug.Log ("put to pool");
			} else
				Debug.Log ("dictionary fail");
		}else
			Debug.Log ("target fail");
		
	}

}
