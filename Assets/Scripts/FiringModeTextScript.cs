using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FiringModeTextScript : MonoBehaviour
{

    public static FiringModeTextScript Instance { get; private set; }
    private TMP_Text firingModeText;

    private void Awake()
    {
        Instance = this;
        firingModeText = GetComponent<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(FiringMode mode)
    {
        firingModeText.text = $"Firing Mode:\n{mode}";
    }

}
