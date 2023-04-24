using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {

        RubyController controller = other.GetComponent<RubyController>();

        if (controller == null) return;

        controller.ChangeHealth(-1);
    }
}
