using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    public int maxHealth = 5;
    int currentHealth;
    public int health { get { return currentHealth; } }
    public float timeInvincible = 1.0f;
    float invincibleTimer;

    new Rigidbody2D rigidbody2D;
    float horizontal;
    float vertical;
    private bool isInvincible;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    public GameObject projectilePrefab;

    public ParticleSystem get_hitParticle;
    public AudioSource audioController;
    // Start is called before the first frame update
    public AudioClip cog_audio;
    public AudioClip get_hit;
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        audioController= GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal"); // A, D, Left, Right
        vertical = Input.GetAxis("Vertical"); // W, S, Up, Down
        Vector2 move = new Vector2(horizontal, vertical);
        if(Mathf.Approximately(move.x,0.0f)|| Mathf.Approximately(move.y, 0.0f))
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
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2D.position + (Vector2.up * 0.2f), lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }

    }
    private void FixedUpdate()
    {
        Vector2 position = rigidbody2D.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2D.MovePosition(position);
    }
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            
        }
        if(get_hitParticle != null)
        {
            get_hitParticle.Play();

            audioController.PlayOneShot(get_hit);
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        audioController.PlayOneShot(cog_audio);
        animator.SetTrigger("Launch");
    }
    public void PlaySound(AudioClip clip)
    {
        audioController.PlayOneShot(clip);
    }
}
