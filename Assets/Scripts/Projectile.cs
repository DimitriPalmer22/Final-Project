using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    Rigidbody2D rigidbody2d;
    private const float maxAliveDuration = 5f;
    private float aliveDuration;

    private Vector3 startPos;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    private void Update()
    {
        if ((transform.position - startPos).magnitude > 1000.0f)
            Destroy(gameObject);

        aliveDuration += Time.deltaTime;

        if (aliveDuration >= maxAliveDuration)
            Destroy(gameObject);

    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Projectile") return;

        EnemyController eController = other.collider.GetComponent<EnemyController>();

        if (eController != null)
            eController.Fix();

        Destroy(gameObject);
    }

}
