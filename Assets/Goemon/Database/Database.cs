using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Database<Type, Data> : MonoBehaviour, IEnumerable<KeyValuePair<Type, Data>>
	where Data : Component, IDatabaseKey<Type>
{
    public List<Data> dataPrfs;

    private readonly Dictionary<Type, Data> m_Datas = new Dictionary<Type, Data>();

    public Data this[Type _type]
    {
        get
        {
            Data data;
            if (m_Datas.TryGetValue(_type, out data))
            {
                return data;
            }
            else
            {
                Debug.LogWarning("Trying to access " + _type + ", but data does not exist. Return null.");
                return default(Data);
            }
        }
    }

    public IEnumerator<KeyValuePair<Type, Data>> GetEnumerator()
    {
        return m_Datas.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return m_Datas.GetEnumerator();
    }

    void Start()
    {
	    Build();
    }

	public void Build()
	{
		m_Datas.Clear();
		foreach (var _dataPrf in dataPrfs)
			m_Datas.Add(_dataPrf.Key(), _dataPrf);
	}
}
