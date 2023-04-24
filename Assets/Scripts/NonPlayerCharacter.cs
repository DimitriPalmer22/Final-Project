using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NonPlayerCharacter : MonoBehaviour
{

    public float displayTime = 4.0f;
    public GameObject dialogBox;
    float timerDisplay;

    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
    }

    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
                dialogBox.SetActive(false);
        }
    }

    public void DisplayDialog()
    {
        // change level
        if (RobotCounter.Instance.gameWon && 
            !RobotCounter.Instance.finalLevel && 
            gameObject.name == "Jambi")
        {
            SceneManager.LoadScene("Level 2");
            return;
        }

        timerDisplay = displayTime;
        dialogBox.SetActive(true);
    }

}
