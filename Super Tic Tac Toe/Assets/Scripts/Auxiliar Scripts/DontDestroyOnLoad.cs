using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour 
{
	[HideInInspector]
	public static DontDestroyOnLoad Instance;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
		{
			Destroy(this.gameObject);
			return;
		}
	}

	void Start () 
	{
		DontDestroyOnLoad(this.gameObject);
	}
	
}
