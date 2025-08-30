using UnityEngine;

namespace ContentContent.Core
{
	public static class GameObjectExtensions
	{
		/// <summary>
		/// Get component or add component to gameObject if it does not exist
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="gameObject"></param>
		/// <returns>The component added or what was already there</returns>
		public static T GetAddComponent<T>(this GameObject gameObject) where T : MonoBehaviour
		{
			var component = gameObject.GetComponent<T>();
			if (component == null) component = gameObject.AddComponent<T>();
			return component;
		}

		/// <summary>
		/// Check if component exist on this gameObject
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		public static bool HasComponent<T>(this GameObject gameObject) where T : MonoBehaviour
		{
			return gameObject.GetComponent<T>() != null;
		}

		/// <summary>
		/// Find the nearest gameObject from a given array of targets
		/// </summary>
		/// <param name="gameObject"></param>
		/// <param name="targets"></param>
		/// <returns>The nearest gameObject from the array</returns>
		public static GameObject GetNearestTarget(this GameObject gameObject, GameObject[] targets)
		{
			if (targets == null || targets.Length == 0)
				return null;

			int nearesteIndex = 0;
			float nearestDistanceSquared = float.MaxValue;
			for (int i = 0; i < targets.Length; ++i)
			{
				GameObject target = targets[i];
				float distSqr = (target.transform.position - gameObject.transform.position).sqrMagnitude;
				if (nearestDistanceSquared >= distSqr)
				{
					nearestDistanceSquared = distSqr;
					nearesteIndex = i;
				}
			}
			return targets[nearesteIndex];
		}
	} 
}