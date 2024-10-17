using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Pooling : MonoBehaviour
{
	public List<Transform> prefabs = new List<Transform>();
	public int pool_count;

    public bool spawn_awake;

	public readonly Dictionary<object, List<Transform>> pools = new Dictionary<object, List<Transform>>();

    private void Awake()
    {
        if (spawn_awake)
        {
            Create_New_Pools();
        }
    }

    public void Set_All_Pool_To_Off()
    {
        for (int i = 0; i < pools.Count; i++)
        {
            for (int j = 0; j < pools[prefabs[i].name].Count; j++)
            {
                if (pools[prefabs[i].name][j].gameObject.activeSelf)
                {
                    pools[prefabs[i].name][j].gameObject.SetActive(false);
                }
            }
        }
    }

    public List<GameObject> Get_Activating_Pool()
    {
        List<GameObject> pool = new List<GameObject>();

        for (int i = 0; i < pools.Count; i++)
        {
            for (int j = 0; j < pools[prefabs[i].name].Count; j++)
            {
                if (pools[prefabs[i].name][j].gameObject.activeSelf)
                {
                    pool.Add(pools[prefabs[i].name][j].gameObject);
                }
            }
        }

        return pool;
    }

    public List<GameObject> Get_All_Pool(string pool_name)
    {
        List<GameObject> all_pools = new List<GameObject>();

        if (pools.ContainsKey(pool_name))
        {
            for (int i = 0; i < pools[pool_name].Count; i++)
            {
                all_pools.Add(pools[pool_name][i].gameObject);
            }

            return all_pools;
        }

        return null;
    }

    public void Off_Pools()
    {
        for (int i = 0; i < pools.Count; i++)
        {
            for (int j = 0; j < pools[prefabs[i].name].Count; j++)
            {
                if (pools[prefabs[i].name][j].gameObject.activeSelf)
                {
                    pools[prefabs[i].name][j].gameObject.SetActive(false);
                }
            }
        }
    }

    public void Create_New_Pools()
	{
		for (int i = 0; i < prefabs.Count; i++)
		{
			if (!pools.ContainsKey(prefabs[i].name))
			{
				List<Transform> new_pools = new List<Transform>();
				pools.Add(prefabs[i].name, new_pools);
			}

			for (int j = 0; j < pool_count; j++)
			{
				if (pools.ContainsKey(prefabs[i].name))
				{
					Transform new_pool = Instantiate(prefabs[i]);
                    new_pool.transform.SetParent(transform);
					new_pool.gameObject.SetActive(false);
					pools[prefabs[i].name].Add(new_pool);
				}
			}
		}
	}

    public void Set_New_Prafab_And_Create(Transform new_prefab)
    {
        if (pools.ContainsKey(new_prefab.name))
        {
            return;
        }

        prefabs.Add(new_prefab);

        Create_New_Pool(new_prefab.name);
    }

	public void Create_New_Pool(string pool_code)
	{
        if (pools.ContainsKey(pool_code))
        {
            for (int i = 0; i < prefabs.Count; i++)
            {
                if (prefabs[i].name.Equals(pool_code))
                {
                    for (int j = 0; j < pool_count; j++)
                    {
                        Transform new_pool = Instantiate(prefabs[i]);
                        new_pool.SetParent(transform);
                        new_pool.gameObject.SetActive(false);
                        pools[prefabs[i].name].Add(new_pool);
                    }

                    break;
                }
            }
        }
        else
        {
            List<Transform> new_pools = new List<Transform>();
            pools.Add(pool_code, new_pools);

            for (int i = 0; i < prefabs.Count; i++)
            {
                if (prefabs[i].name.Equals(pool_code))
                {
                    for (int j = 0; j < pool_count; j++)
                    {
                        Transform new_pool = Instantiate(prefabs[i]);
                        new_pool.parent = transform;
                        new_pool.gameObject.SetActive(false);
                        pools[pool_code].Add(new_pool);
                    }

                    break;
                }
            }
        }
	}

    public void Remove_Pool(string pool_name)
    {
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools.ContainsKey(pool_name))
            {
                for (int j = 0; j < pools[pool_name].Count; j++)
                {
                    Destroy(pools[pool_name][j].gameObject);
                    pools[pool_name].Remove(pools[pool_name][j]);
                }
            }
        }
    }

	public void Remove_Pools()
	{
		for (int i = 0; i < prefabs.Count; i++) 
		{
			if (pools.ContainsKey (prefabs [i].name)) 
			{
				for (int j = 0; j < pools[prefabs[i].name].Count; j++) 
				{
					Destroy (pools [prefabs [i].name] [j].gameObject);
				}
			}
		}
			
		pools.Clear();
		prefabs.Clear ();
	}

    public Transform Pool()
    {
        Transform pool = null;

        for (int i = 0; i < pools[prefabs[0].name].Count; i++)
        {
            if (!pools[prefabs[0].name][i].gameObject.activeSelf)
            {
                pool = pools[prefabs[0].name][i];
                return pool;
            }
        }

        Create_New_Pools();

        pool = pools[prefabs[0].name][pools[prefabs[0].name].Count - pool_count];

        return pool;
    }

    public Transform Pool(string pool_code)
	{
		Transform pool = null;

        if (pools.ContainsKey(pool_code))
        {
            for (int i = 0; i < pools[pool_code].Count; i++)
            {
                if (!pools[pool_code][i].gameObject.activeSelf)
                {
                    pool = pools[pool_code][i];
                    return pool;
                }
            }

            Create_New_Pool(pool_code);

            pool = pools[pool_code][pools[pool_code].Count - pool_count];
        }
        else
        {
            Create_New_Pool(pool_code);

            pool = pools[pool_code][pools[pool_code].Count - pool_count];
        }

		return pool;
	}
}