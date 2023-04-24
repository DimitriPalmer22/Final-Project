using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterMessage : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player") return;

        InfoTextScript.Instance.DisplayMessage($"Interact with the Mage to teleport back to the start of the level!");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player") return;

        InfoTextScript.Instance.HideMessage();
    }

}
