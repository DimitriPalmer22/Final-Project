using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCollectible : MonoBehaviour
{
    private const int REFILL_AMOUNT = 4;

    public AudioClip collectedClip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        if (controller == null) return;

        controller.ChangeCogs(REFILL_AMOUNT);

        if (collectedClip != null)
            controller.PlaySound(collectedClip);
        
        Destroy(gameObject);
    }
}
