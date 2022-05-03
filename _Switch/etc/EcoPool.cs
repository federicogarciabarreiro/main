using System.Collections.Generic;
using System.Text;
using UnityEngine;


public partial class EcoPool<T> {
	public static T ExpandPercent(EcoPool<T> pool, float percent)
    {
		pool.Expand(Mathf.Max(1, (int)(pool.Capacity * percent / 100f)));
		return pool.Acquire();
	}

	public static T ReuseRandom(EcoPool<T> pool)
    {
		int index = Random.Range(0, pool._used.Count);
		var rand = pool._used[index];
		pool._used.RemoveAt(index);
		var poolable = rand as IPoolable;

		if(poolable != null)
        {
			poolable.OnRelease();
			poolable.OnAcquire();
		}

		return rand;
	}

	public static T ReturnNull(EcoPool<T> pool)
    {
		return default(T);
	}

}

public partial class EcoPool<T> where T : IPoolable
{
	public delegate T ExpandPolicy(EcoPool<T> pool);
	public delegate T Factory();

	Stack<T> _free;
	List<T> _used;
	Factory _factory;
	ExpandPolicy _exPolicy;

	public int Capacity { get; private set; }

	public EcoPool(Factory factory, ExpandPolicy exPolicy, int initialAmount = 400) {
		_factory = factory;
		_exPolicy = exPolicy;
		_free = new Stack<T>(initialAmount);
		_used = new List<T>(initialAmount);
		Capacity = 0;
		Expand(initialAmount);
	}

	public T Acquire()
    {
		T obj;
		if(_free.Count > 0) obj = _free.Pop();
        else
        {
			if (Recycle() > 0) obj = _free.Pop();
            else obj = _exPolicy(this);
        }

		if(obj != null)
        {
			obj.OnAcquire();
			_used.Add(obj);
		}

		return obj;
	}

	public int Recycle()
    {
		int nc = 0;

		for (int i = 0; i < _used.Count; i++)
        {
			var obj = _used[i];

			if (obj.CanRecycle())
			{
				obj.OnRelease();
				_free.Push(obj);
			}

            else _used[nc++] = obj;
        }

		int recycled = _used.Count - nc;
		_used.RemoveRange(nc, recycled); 
		return recycled;
	}

	public void Release(T obj)
    {
		var poolable = obj as IPoolable;

		if(poolable != null) poolable.OnRelease();

		_used.Remove(obj);
		_free.Push(obj);
	}

	public void Expand(int amount)
    {
		Capacity += amount;

		while(amount-- != 0)
        {
			var obj = _factory();
			_free.Push(obj);
		}
	}
}
