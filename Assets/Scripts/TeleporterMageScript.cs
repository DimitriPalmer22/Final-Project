using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterMageScript : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform teleportTo;
    [SerializeField] private AudioClip teleportSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Telports ruby to a specified location
    /// </summary>
    /// <param name="controller"></param>
    public void OnInteract(RubyController controller)
    {
        Vector3 currentDistance = transform.position - controller.transform.position;
        Vector3 newPosition = teleportTo.position + currentDistance * (teleportTo.localScale.x * transform.localScale.x);

        controller.transform.position = newPosition;
        controller.PlaySound(teleportSound);
    }

}
