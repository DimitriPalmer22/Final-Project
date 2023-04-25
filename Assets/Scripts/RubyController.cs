using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum FiringMode
{
    Normal,
    Spread
}

public class RubyController : MonoBehaviour
{

    private Rigidbody2D rigidbody2d;
    Animator animator;

    // audio
    AudioSource audioSource;
    public AudioClip cogLaunched;
    public AudioClip damaged;
    public float timeInvincible = 2.0f;
    private bool isInvincible;
    private float invincibleTimer;

    // Movement
    private float horizontal;
    private float vertical;
    Vector2 lookDirection = new Vector2(1, 0);
    public float speed = 3f;

    public int maxHealth = 5;
    private int currentHealth;
    public int health => currentHealth;

    private int cogs;


    public GameObject projectilePrefab;

    // particles
    public ParticleSystem healthParticles;
    public ParticleSystem damageParticles;
    public ParticleSystem bombParticles;
    public const int PARTICLE_AMOUNT = 30;

    public FiringMode firingMode = default;
    private const int SPREAD_SHOT_COUNT = 3;
    private const float SPREAD_SHOT_ANGLE = 180f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

        cogs = RobotCounter.Instance.GetRobotCount();

        CogCounter.Instance.SetCount(cogs);
        SetFiringMode(default);
    }

    private void UpdateInput()
    {
        // restart
        if (RobotCounter.Instance.gameOver && Input.GetKeyDown(KeyCode.R))
            Restart();

        // disable input if dead
        if (health <= 0)
        {
            horizontal = vertical = 0;
            return;
        }

        // Movement
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // flip firing mode
        if (Input.GetButtonDown("Fire2"))
            SwitchFiringMode();

        // shoot
        if (Input.GetButtonDown("Fire1"))
        {
            switch (firingMode)
            {
                case FiringMode.Normal:
                    Launch();
                    break;
                case FiringMode.Spread:
                    LaunchSpread();
                    break;
                default:
                    break;
            }

        }

        // interact with NPCs
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                rigidbody2d.position + Vector2.up * 0.2f,
                lookDirection,
                1.5f,
                LayerMask.GetMask("NPC"));

            if (hit.collider != null)
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                    interactable.OnInteract(this);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0) isInvincible = false;
        }

    }

    void FixedUpdate()
    {
        Vector2 position = transform.position;
        position.x += speed * horizontal * Time.deltaTime;
        position.y += speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        // taking damage            
        if (amount < 0 && health > 0)
        {
            if (isInvincible) return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
            PlaySound(damaged);

            damageParticles.Emit(-amount * PARTICLE_AMOUNT);

            if (RobotCounter.Instance.gameWon) amount = 0;

        }

        // healing
        else if (amount > 0)
        {
            healthParticles.Emit(amount * PARTICLE_AMOUNT);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

        // dead
        if (currentHealth <= 0)
        {
            RobotCounter.Instance.gameLost = true;
            RobotCounter.Instance.DisplayEndMessage("You Lose! Press R to Restart!");
            BackgroundMusicController.Instance.PlayLoseMusic();
        }
    }

    public void ChangeCogs(int amount)
    {
        cogs += amount;
        if (cogs < 0) cogs = 0;
        CogCounter.Instance.SetCount(cogs);
    }

    void Launch()
    {
        if (cogs <= 0) return;

        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        PlaySound(cogLaunched);

        ChangeCogs(-1);
    }

    void LaunchSpread()
    {
        if (cogs <= 0) return;

        float angleSize = SPREAD_SHOT_ANGLE / (SPREAD_SHOT_COUNT - 1);

        for (int i = 0; i < SPREAD_SHOT_COUNT; i++)
        {
            float cAngle = -(SPREAD_SHOT_ANGLE / 2) + (i * angleSize);
            Vector2 rotatedVector = RotateVector(lookDirection, cAngle);

            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();

            projectile.maxDistance = 4f;
            float scaleAmt = .8f;
            projectileObject.transform.localScale = new Vector3(scaleAmt , scaleAmt , scaleAmt );

            projectile.Launch(rotatedVector, 300 * (1.5f / 2));

        }

        animator.SetTrigger("Launch");
        PlaySound(cogLaunched);

        ChangeCogs(-1);
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null) return;

        audioSource.PlayOneShot(clip);
    }

    private void Restart()
    {
        if (RobotCounter.Instance.gameWon)
        {
            if (RobotCounter.Instance.finalLevel) SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
            else SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        else SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    private void SetFiringMode(FiringMode mode)
    {
        this.firingMode = mode;
        FiringModeTextScript.Instance.SetText(firingMode);
    }

    public void SwitchFiringMode()
    {
        if (firingMode == FiringMode.Normal)
            SetFiringMode(FiringMode.Spread);
        else SetFiringMode(FiringMode.Normal);
    }

    private Vector2 RotateVector(Vector2 vec, float rotation)
    {
        float originalX = vec.x;
        float originalY = vec.y;
        float sine = Mathf.Sin(rotation * Mathf.Deg2Rad);
        float cosine = Mathf.Cos(rotation * Mathf.Deg2Rad);

        Vector2 newVec = new Vector2();
        newVec.x = (cosine * originalX) - (sine * originalY);
        newVec.y = (sine * originalX) + (cosine * originalY);

        return newVec;
    }

}
