using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CogCounter : MonoBehaviour
{

    public static CogCounter Instance { get; private set; }

    private TMP_Text cogText;

    private void Awake()
    {
        if (Instance != this) Instance = this;

        cogText = GetComponent<TMP_Text>();


    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCount(int amt)
    {
        cogText.text = $"Cogs: {amt}";
    }
}
