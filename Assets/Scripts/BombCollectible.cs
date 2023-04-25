using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollectible : MonoBehaviour
{
    public AudioClip collectedClip;
    private const int DAMAGE = 2;

    private void OnTriggerEnter2D(Collider2D other)
    {

        RubyController controller = other.GetComponent<RubyController>();

        if (controller == null) return;

        // GameObject temp = other.gameObject.

        controller.bombParticles.Emit(RubyController.PARTICLE_AMOUNT);

        controller.ChangeHealth(-DAMAGE);
        controller.StartFireCooldown();
        controller.PlaySound(collectedClip);
        Destroy(gameObject);
    }
}
