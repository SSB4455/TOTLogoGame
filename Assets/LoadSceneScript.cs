using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadSceneScript : MonoBehaviour
{
	public GameObject levelButton;



	void Start()
	{
		levelButton.SetActive(false);
		Scene currentScene = SceneManager.GetActiveScene();
		Regex regex = new Regex(@"([^/]*/)*([\w\d\-]*)\.unity");
		for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			string sceneName = regex.Replace(SceneUtility.GetScenePathByBuildIndex(i), "$2");
			GameObject thisLevelButton = levelButton;
			if (currentScene.name != sceneName)
			{
				if (thisLevelButton.activeSelf)
				{
					thisLevelButton = Instantiate(levelButton);
				}
				thisLevelButton.SetActive(true);
				thisLevelButton.transform.SetParent(levelButton.transform.parent);
				thisLevelButton.transform.localScale = Vector3.one;
				thisLevelButton.transform.GetComponentInChildren<Text>().text = sceneName;
				thisLevelButton.transform.GetComponentInChildren<Button>().onClick.AddListener(() => { SceneManager.LoadScene(sceneName); });
			}
		}
		

	}

}