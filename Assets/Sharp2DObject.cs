using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Sharp2DObject : MonoBehaviour
{
	internal RectTransform rectTrans { get; private set; }
	public float waitDistance = 1000;
	Vector2 goalPosition;
	float goalRotation;
	Vector2 startPosition;
	Vector2 disappearPosition;

	public float DistanceToGoal { get { return Vector2.Distance(rectTrans.position, goalPosition); } }



	void Start()
	{
		rectTrans = GetComponent<RectTransform>();
		if (rectTrans == null)
		{
			Debug.LogError("Sharp2DObject lost RectTransform " + name);
		}

		goalPosition = rectTrans.position;
		goalRotation = rectTrans.rotation.eulerAngles.z;

		float k = Mathf.Tan(goalRotation * Mathf.Deg2Rad);
		float moveX = -1 * Mathf.Sign(k) * (waitDistance / Mathf.Sqrt(1 + k * k));
		if (goalRotation > 180)
		{
			moveX = Mathf.Sign(k) * (waitDistance / Mathf.Sqrt(1 + k * k));
		}
		switch (goalRotation)
		{
			case 0: moveX = -waitDistance; break;
			case 180: moveX = waitDistance; break;
		}
		float moveY = k * moveX;
		startPosition = goalPosition + new Vector2(moveX, moveY);
		disappearPosition = goalPosition + new Vector2(-moveX, -moveY);
		Debug.Log("Sharp2d " + name + "\tgoalPosition = " + goalPosition + "\tstartPosition = " + startPosition +
				"\tdisappearPosition = " + disappearPosition + "\tgoalRotation = " + goalRotation);
	}

	public void MoveProgress(float progress)
	{
		progress = Mathf.Clamp(progress, -1, 1);

		if (progress < 0)
		{
			rectTrans.position = Vector2.Lerp(startPosition, goalPosition, progress + 1);
		}
		else if (progress > 0)
		{
			rectTrans.position = Vector2.Lerp(goalPosition, disappearPosition, progress);
		}
		else
		{
			rectTrans.position = goalPosition;
		}

	}


}