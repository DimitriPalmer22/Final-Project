using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector2 movement;
    protected Rigidbody2D rigidbody2D;
    protected Animator animator;
    public ParticleSystem smokeEffect;

    protected float moveSpeed = 2;
    protected float changeTime => movement.magnitude / moveSpeed;
    protected float timer;
    protected int direction = 1;

    protected int damage = 1;

    private bool broken = true;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (!broken) return;

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        var normalMoveVec = movement.normalized * direction;
        animator.SetFloat("Move X", normalMoveVec.x);
        animator.SetFloat("Move Y", normalMoveVec.y);
    }

    protected virtual void FixedUpdate()
    {
        if (!broken) return;

        Vector2 position = rigidbody2D.position;
        position += movement * direction / changeTime * Time.deltaTime;

        rigidbody2D.MovePosition(position);
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {

        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player == null) return;

        player.ChangeHealth(-damage);
    }

    public virtual void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        RobotCounter.Instance.AddFixedRobot();
    }

}
