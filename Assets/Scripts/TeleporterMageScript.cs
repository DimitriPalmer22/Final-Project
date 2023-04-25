using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterMageScript : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform teleportTo;
    [SerializeField] private AudioClip teleportSound;

    [SerializeField] private List<EnemyController> linkedRobots = new();

    private float displayTime = 4.0f;
    public GameObject dialogBox;
    private float timerDisplay;

    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerDisplay > 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay <= 0)
                dialogBox.SetActive(false);
        }
    }

    private void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
    }

    /// <summary>
    /// Teleports ruby to a specified location or shows a dialogue box
    /// </summary>
    /// <param name="controller"></param>
    public void OnInteract(RubyController controller)
    {
        if (!linkedRobotsCompleted())
        {
            DisplayDialog();
            return;
        }

        Vector3 currentDistance = controller.transform.position - transform.position;
        Vector3 newPosition = teleportTo.position + currentDistance * (teleportTo.localScale.x * transform.localScale.x);

        controller.transform.position = newPosition;
        controller.PlaySound(teleportSound);
    }

    private bool linkedRobotsCompleted()
    {
        foreach (var robot in linkedRobots)
            if (robot.broken) return false;

        return true;
    }

}
