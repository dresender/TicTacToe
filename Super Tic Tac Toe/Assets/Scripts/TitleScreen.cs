using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour 
{
	public GameObject GameMainMenu;

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			this.gameObject.SetActive(false);
			GameMainMenu.SetActive(true);
		}
	}
}
