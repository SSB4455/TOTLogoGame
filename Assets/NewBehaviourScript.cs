using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
	public Sharp2DObject[] sharp2dList;
	public float speed;
	public float Speed { get { return speed; } set{ speed = value; } }
	float checkArea;
	public ParticleSystem effect;

	#region DebugUI
	public Slider progressSilder;
	public Toggle demoToggle;
	public Toggle freeToggle;
	public Toggle playToggle;
	public InputField checkInputField;
	public Text distinceText;
	#endregion



	// Start is called before the first frame update
	void Start()
	{
		Application.targetFrameRate = 60;

		progressSilder?.onValueChanged.AddListener(OnProgressChanged);
		demoToggle?.onValueChanged.AddListener((value) => { progressSilder.interactable = !value; });
		demoToggle.isOn = true;

		float.TryParse(checkInputField.text, out checkArea);
		checkInputField.onValueChanged.AddListener((textValue) => { float.TryParse(checkInputField.text, out checkArea); });
		effect?.gameObject.SetActive(false);
	}

	float timeWaitShift;
	float touchWaitTime = 5;
	bool stopToWait = false;
	float moveF = -1;
	// Update is called once per frame
	void Update()
	{
		float distance = sharp2dList[0].DistanceToGoal;

		if (!freeToggle.isOn)
		{
			if (stopToWait && timeWaitShift < touchWaitTime)
			{
				timeWaitShift += Time.deltaTime;
			}
			else if (demoToggle.isOn)
			{
				moveF += speed * Time.deltaTime;
				progressSilder.value = moveF;
				distance = sharp2dList[0].DistanceToGoal;
				if (Mathf.Abs(distance) < (checkArea / 2) && !stopToWait)
				{
					stopToWait = true;
					timeWaitShift = 0;
					progressSilder.value = 0;
					moveF = 0;
				}
				if (moveF > 1)
				{
					moveF = -1;
					stopToWait = false;
				}
			}
			else if (playToggle.isOn)
			{
				moveF += speed * Time.deltaTime;
				progressSilder.value = moveF;
				distance = sharp2dList[0].DistanceToGoal;
				effect?.gameObject.SetActive(false);
				if (Input.touchCount > 0 || Input.GetMouseButtonDown(1))
				{
					stopToWait = true;
					timeWaitShift = 1;
					Debug.Log("touch");
					if (distance <= checkArea)
					{
						effect?.gameObject.SetActive(true);
						effect?.Play();
					}
				}
				if (moveF > 1)
				{
					moveF = -1;
				}
			}
		}

		distinceText.text = "speed:" + speed.ToString("f3") + "\t\tdistance:" + distance.ToString("f3");
	}

	void OnProgressChanged(float progress)
	{
		for (int i = 0; i < sharp2dList.Length; i++)
		{
			sharp2dList[i].MoveProgress(progress);
		}
	}
}
