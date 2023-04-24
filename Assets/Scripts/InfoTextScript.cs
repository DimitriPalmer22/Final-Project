using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoTextScript : MonoBehaviour
{

    public static InfoTextScript Instance { get; private set; }
    private TMP_Text infoText;

    private const float MESSAGE_DISPLAY_LENGTH = 5;
    private float timer;

    private void Awake()
    {
        Instance = this;
        infoText = GetComponent<TMP_Text>();
        infoText.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                infoText.enabled = false;
            }
        }

    }

    public void DisplayMessage(string text)
    {
        infoText.text = text;
        infoText.enabled = true;
        timer = MESSAGE_DISPLAY_LENGTH;
    }

    public void HideMessage()
    {
        infoText.enabled = false;
        timer = 0;
    }

}
