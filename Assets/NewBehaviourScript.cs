using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public RectTransform[] sharpList;
    float distance = 2000;
    public float speed = 800;
    RectTransform ggg;
    Vector2[] galbalPositionsVector2;
    Vector2[] startPositionsVector2;
    //bool[] sharpsReverse;
    public Slider moveSilder;
    public Toggle autoToggle;
    public InputField checkInputField;
    public Text distinceText;
    float checkDid;
    public ParticleSystem effect;



    // Start is called before the first frame update
    void Start()
    {
        startPositionsVector2 = new Vector2[sharpList.Length];
        galbalPositionsVector2 = new Vector2[sharpList.Length];
        for (int i = 0; i < sharpList.Length; i++)
        {
            galbalPositionsVector2[i] = sharpList[i].anchoredPosition;
            float k = Mathf.Tan(sharpList[i].rotation.eulerAngles.z * Mathf.Deg2Rad);
            float moveX = -1 * Mathf.Sign(k) * (distance / Mathf.Sqrt(1 + k * k));
            if (sharpList[i].rotation.eulerAngles.z > 180)
            {
                moveX = Mathf.Sign(k) * (distance / Mathf.Sqrt(1 + k * k));
            }
            switch (sharpList[i].rotation.eulerAngles.z)
            {
                case 0: moveX = -distance; break;
                case 180: moveX = distance; break;
            }
            startPositionsVector2[i] = new Vector2(sharpList[i].anchoredPosition.x + moveX, sharpList[i].anchoredPosition.y + k * moveX);
            galbalPositionsVector2[i] = new Vector2(sharpList[i].anchoredPosition.x - moveX, sharpList[i].anchoredPosition.y + k * (-moveX));
            Debug.Log(sharpList[i].name + "\t" + sharpList[i].anchoredPosition + "\t" + sharpList[i].rotation.eulerAngles + "\n" + k + "\t" + startPositionsVector2[i]);
        }
        moveSilder.onValueChanged.AddListener(MoveProgress);

        ggg = Instantiate<RectTransform>(sharpList[0]);
        ggg.gameObject.SetActive(false);

        float.TryParse(checkInputField.text, out checkDid);
        checkInputField.onValueChanged.AddListener((textValue) => { float.TryParse(checkInputField.text, out checkDid); });
        effect?.gameObject.SetActive(false);
    }

    float touchWaitShift;
    bool touched;
    float moveF = 0;
    // Update is called once per frame
    void Update()
    {
        float ddt1 = Vector2.Distance(ggg.anchoredPosition, sharpList[0].anchoredPosition);
        distinceText.text = ddt1.ToString("f3");
        if (autoToggle.isOn && !touched)
        {
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(1))
            {
                touched = true;
                Debug.Log("touch");
                if (ddt1 <= checkDid)
                {
                    effect?.gameObject.SetActive(true);
                    effect?.Play();
                }
            }
            moveF += speed * Time.deltaTime / distance;
            moveSilder.value = moveF;
            if (moveF > 2)
            {
                moveF = 0;
            }
        }
        if (touched)
        {
            touchWaitShift += Time.deltaTime;
            if (touchWaitShift > 2)
            {
                touched = false;
                touchWaitShift = 0;
                effect?.gameObject.SetActive(false);
            }
        }
    }

    void MoveProgress(float progress)
    {
        for (int i = 0; i < sharpList.Length; i++)
        {
            sharpList[i].anchoredPosition = Vector2.Lerp(startPositionsVector2[i], galbalPositionsVector2[i], progress);
        }
    }
}
