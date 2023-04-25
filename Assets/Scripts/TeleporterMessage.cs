using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterMessage : MonoBehaviour
{


    [TextArea]
    public string message;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player") return;

        InfoTextScript.Instance.DisplayMessage($"{message}");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player") return;

        InfoTextScript.Instance.HideMessage();
    }

}
