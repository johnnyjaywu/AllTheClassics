using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace ContentContent.Core
{
	public partial class PooledBehaviour : MonoBehaviour
	{
		[Header("Pool Settings")]
		[SerializeField]
		protected int defaultCapacity = 10;

		[SerializeField]
		protected int maxCapacity = 1000;

		[SerializeField]
		protected bool dontDestroyOnLoad;

		[SerializeField, Tooltip("0 is infinite"), Min(0)]
		protected float lifeTime;

		Pool pool;
		protected bool isActiveInPool;

		public PooledBehaviour GetPooledInstance()
		{
			if (!pool)
			{
				pool = Pool.Initialize(this, dontDestroyOnLoad,
					OnGetFromPool, OnReturnToPool, OnDestroyedFromPool,
					defaultCapacity, maxCapacity);
			}
			return pool.Pull();
		}

		public T GetPooledInstance<T>() where T : PooledBehaviour
		{
			if (!pool)
			{
				pool = Pool.Initialize(this, dontDestroyOnLoad,
					OnGetFromPool, OnReturnToPool, OnDestroyedFromPool,
					defaultCapacity, maxCapacity);
			}
			return (T)pool.Pull();
		}

		public void ReturnToPool()
		{
			if (pool)
			{
				if (isActiveInPool)
					pool.Push(this);
				else
				{
					Debug.LogWarning("Object is already returned to pool!");
				}
			}
			else
			{
				Debug.LogWarning("This object does not belong in a pool! Destroying it instead");
				Destroy(gameObject);
			}
		}

		protected virtual void OnGetFromPool(PooledBehaviour pooledObject)
		{
			pooledObject.isActiveInPool = true;
			pooledObject.gameObject.SetActive(true);
			if (lifeTime > 0)
				pooledObject.StartCoroutine(RunLifetime(pooledObject));
		}

		protected virtual void OnReturnToPool(PooledBehaviour pooledObject)
		{
			pooledObject.isActiveInPool = false;
			pooledObject.gameObject.SetActive(false);
			pooledObject.StopAllCoroutines();
		}

		protected virtual void OnDestroyedFromPool(PooledBehaviour pooledObject)
		{
			Destroy(pooledObject.gameObject);
		}

		private IEnumerator RunLifetime(PooledBehaviour pooledObject)
		{
			yield return new WaitForSeconds(lifeTime);
			pooledObject.ReturnToPool();
		}
	}

	public partial class PooledBehaviour : MonoBehaviour
	{
		class Pool : MonoBehaviour
		{
			private PooledBehaviour prefab;
			private ObjectPool<PooledBehaviour> objectPool;

			public static Pool Initialize(PooledBehaviour prefab, bool dontDestroyOnLoad = false,
				Action<PooledBehaviour> actionOnGet = null, Action<PooledBehaviour> actionOnRelease = null, Action<PooledBehaviour> actionOnDestroy = null,
				int defaultCapacity = 10, int maxSize = 10000)
			{
				GameObject poolGameObject = GameObject.Find(prefab.name + " Pool");
				Pool pool = null;
				if (poolGameObject)
				{
					pool = poolGameObject.GetComponent<Pool>();
				}
				else
				{
					poolGameObject = new GameObject(prefab.name + " Pool");
					if (dontDestroyOnLoad)
						DontDestroyOnLoad(poolGameObject);
					pool = poolGameObject.AddComponent<Pool>();
					pool.prefab = prefab;

					bool collectionCheck = false;
#if UNITY_EDITOR
					collectionCheck = true;
#endif
					pool.objectPool = new ObjectPool<PooledBehaviour>(pool.OnCreateFromPool, actionOnGet, actionOnRelease, actionOnDestroy, collectionCheck, defaultCapacity, maxSize);
				}
				return pool;
			}

			private PooledBehaviour OnCreateFromPool()
			{
				PooledBehaviour pooledObject = GameObject.Instantiate(prefab);
				pooledObject.transform.SetParent(transform, false);
				pooledObject.pool = this;
				return pooledObject;
			}

			public PooledBehaviour Pull()
			{
				return objectPool?.Get();
			}

			public void Push(PooledBehaviour pooled)
			{
				objectPool?.Release(pooled);
			}
		}
	} 
}