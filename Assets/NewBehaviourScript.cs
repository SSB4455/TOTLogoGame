using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
	public Sharp2DObject[] sharp2ds;
	public float[] stepTimes;
	Dictionary<int, List<Sharp2DObject>> stepSequence = new Dictionary<int, List<Sharp2DObject>>();
	List<Sharp2DObject> currStepSharp2dList;
	float stepTime;
	int stepIndex = 0;
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

		for (int i = 0; i < sharp2ds.Length; i++)
		{
			if (!stepSequence.ContainsKey(sharp2ds[i].stepId))
			{
				stepSequence.Add(sharp2ds[i].stepId, new List<Sharp2DObject>());
			}
			stepSequence[sharp2ds[i].stepId].Add(sharp2ds[i]);
			sharp2ds[i].gameObject.SetActive(false);
		}
		
		currStepSharp2dList = stepSequence[(new List<int>(stepSequence.Keys))[stepIndex]];
		stepTime = stepTimes[stepIndex];
	}

	float timeWaitShift;
	float touchWaitTime = 5;
	bool stopToWait = false;
	float moveF = -1;
	// Update is called once per frame
	void Update()
	{
		float distance = currStepSharp2dList[0].DistanceToGoal;

		if (!freeToggle.isOn)
		{
			if (stopToWait && timeWaitShift < touchWaitTime)
			{
				timeWaitShift += Time.deltaTime;
			}
			else if (demoToggle.isOn)
			{
				moveF += Time.deltaTime * (1 / stepTime);
				progressSilder.value = moveF;
				distance = currStepSharp2dList[0].DistanceToGoal;
				if (Mathf.Abs(distance) < (checkArea / 2) && !stopToWait)
				{
					stopToWait = true;
					timeWaitShift = 0;
					progressSilder.value = 0;
					moveF = 0;

					if (stepIndex < 0)
					{
						stepIndex++;
					}
					stepIndex++;
					if (stepIndex < stepSequence.Keys.Count && stepIndex < stepTimes.Length)
					{
						currStepSharp2dList = stepSequence[(new List<int>(stepSequence.Keys))[stepIndex]];
						stepTime = stepTimes[stepIndex];
						moveF = -1;
						stopToWait = false;
					}
				}
				if (moveF > 1)
				{
					moveF = -1;
					stopToWait = false;

					if (stepIndex >= stepSequence.Keys.Count)
					{
						stepIndex--;
					}
					stepIndex--;
					if (stepIndex >= 0 && stepIndex < stepSequence.Keys.Count && stepIndex < stepTimes.Length)
					{
						currStepSharp2dList = stepSequence[(new List<int>(stepSequence.Keys))[stepIndex]];
						stepTime = stepTimes[stepIndex];
						moveF = 0;
						stopToWait = true;
					}
				}
			}
			else if (playToggle.isOn)
			{
				moveF += Time.deltaTime * (1 / stepTime);
				progressSilder.value = moveF;
				distance = currStepSharp2dList[0].DistanceToGoal;
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

					if (stepIndex < 0)
					{
						stepIndex++;
					}
					stepIndex++;
					if (stepIndex < stepSequence.Keys.Count && stepIndex < stepTimes.Length)
					{
						currStepSharp2dList = stepSequence[(new List<int>(stepSequence.Keys))[stepIndex]];
						stepTime = stepTimes[stepIndex];
						moveF = -1;
						stopToWait = false;
					}
				}
				if (stopToWait && (stepIndex >= stepSequence.Keys.Count || stepIndex >= stepTimes.Length))
				{
					for (int i = 0; i < sharp2ds.Length; i++)
					{
						sharp2ds[i].gameObject.SetActive(false);
					}
					stepIndex = 0;
					currStepSharp2dList = stepSequence[(new List<int>(stepSequence.Keys))[stepIndex]];
					stepTime = stepTimes[stepIndex];
					moveF = -1;
					stopToWait = false;
				}
				if (moveF > 1)
				{
					moveF = -1;
					stopToWait = false;
				}
			}
		}

		distinceText.text = "stepTime:" + stepTime.ToString("f1") + "\t\tdistance:" + distance.ToString("f3");
	}

	void OnProgressChanged(float progress)
	{
		for (int i = 0; i < currStepSharp2dList.Count; i++)
		{
			currStepSharp2dList[i].gameObject.SetActive(true);
			currStepSharp2dList[i].MoveProgress(progress);
		}
	}
}
