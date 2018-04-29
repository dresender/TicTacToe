using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

	void Start()
	{
		StartCoroutine(LoadMainScene());
	}
	IEnumerator LoadMainScene()
	{
		yield return new WaitForSeconds(3.9f);

		SceneManager.LoadScene("Menu");
	}
}
