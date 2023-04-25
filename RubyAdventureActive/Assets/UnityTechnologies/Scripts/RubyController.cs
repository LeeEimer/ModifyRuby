using System;
using UnityEngine;
using Unity.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class RubyController : MonoBehaviour
{
    // ========= MOVEMENT =================
    public float currentSpeed;

    // ======== HEALTH ==========
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    public Transform respawnPosition;
    public ParticleSystem hitParticle;

    // ======== PROJECTILE ==========
    public GameObject projectilePrefab;
    public GameObject slowProjectilePrefab;

    // ======== AUDIO ==========
    public AudioClip hitSound;
    public AudioClip shootingSound;
    public AudioClip winmusic;
    public AudioClip losemusic;
    public AudioClip backgroundMusic;


    // ======== HEALTH ==========
    public int health
    {
        get { return currentHealth; }
    }

    // =========== MOVEMENT ==============
    Rigidbody2D rigidbody2d;
    Vector2 currentInput;

    // ======== HEALTH ==========
    int currentHealth;
    float invincibleTimer;
    bool isInvincible;

    // ==== ANIMATION =====
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    // ================= SOUNDS =======================
    AudioSource audioSource;



    public static int totalBots;
    public static int JambiTalks;
    public TextMeshProUGUI WinText;
    public TextMeshProUGUI LoseText;
    public TextMeshProUGUI FixedBots;
    public TextMeshProUGUI AmmoCount;
    public TextMeshProUGUI SlowCogCount;
    public TextMeshProUGUI DashCount;
    public GameObject DashCountObj;
    public GameObject SlowCogTextObj;
    public GameObject AmmoTextObj;
    public GameObject WinTextObj;
    public GameObject LoseTextObj;
    public GameObject BotTextObj;
    public static int cogLimit;
    public static int slowCogLimit;
    public static int dashLimit;
    public Boolean slowCog = false;
    public Boolean winEnabled;
    public Boolean restartAllowed = false;
    public Boolean inLevel2 = false;
    public Boolean finDash = true;

    void Start()
    {
        // =========== MOVEMENT ==============
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentSpeed = 4;

        // ======== HEALTH ==========
        invincibleTimer = -1.0f;
        currentHealth = maxHealth;
        cogLimit = 5;
        slowCogLimit = 0;
        dashLimit = 0;
        hitParticle.Stop();

        // ==== ANIMATION =====
        animator = GetComponent<Animator>();

        // ==== AUDIO =====
        audioSource = GetComponent<AudioSource>();

        WinTextObj.SetActive(false);
        LoseTextObj.SetActive(false);
        BotTextObj.SetActive(true);
        setSlowAmmoText();
        setAmmoText();
        setBotText();
        setDashText();
        winEnabled = false;

        audioSource.PlayOneShot(backgroundMusic);
    }

    void Update()
    {
        //move to level 2
        if (JambiTalks > 1 && totalBots == 7)
        {
            SceneManager.LoadScene("Level2");
            totalBots = 0;
            inLevel2 = true;
        }

        //Respawn
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (restartAllowed == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                totalBots = 0;
                ChangeHealth(maxHealth);
                cogLimit = 5;
                transform.position = respawnPosition.position;
                WinTextObj.SetActive(false);
                LoseTextObj.SetActive(false);
                currentSpeed = 4;
                restartAllowed = false;
            }
            if (winEnabled == true)
            {
                inLevel2 = false;
                SceneManager.LoadScene("MainScene");
            }
        }

        setBotText();
        setAmmoText();
        setSlowAmmoText();
        setDashText();

        // ================= HEALTH ====================
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        // ============== MOVEMENT ======================
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        currentInput = move;

        // ================ DASH ==========================

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dash());
            Dash();
            dashLimit--;
        }
        // ============== ANIMATION =======================

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // ============== PROJECTILE ======================

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (cogLimit >= 1)
            {
                cogLimit--;
                LaunchProjectile();
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            if (slowCogLimit >= 1)
            {
                slowCog = true;
                slowCogLimit--;
                LaunchProjectile();
            }
            if (slowCogLimit < 1)
            {
                slowCog = false;
            }
        }

        // ======== DIALOGUE ==========
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, 1 << LayerMask.NameToLayer("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                NPCOtherScript otherchar = hit.collider.GetComponent<NonPlayerCharacter()>;
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }

    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;

        position = position + currentInput * currentSpeed * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    // ===================== HEALTH ==================
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            animator.SetTrigger("Hit");
            hitParticle.Play();
            audioSource.PlayOneShot(hitSound);
            hitParticle.Stop();

            Instantiate(hitParticle, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (currentHealth == 0)
        {
            currentSpeed = 0;
            LoseTextObj.SetActive(true);
            audioSource.PlayOneShot(losemusic);
            restartAllowed = true;
        }

        UIHealthBar.Instance.SetValue(currentHealth / (float)maxHealth);
    }


    // =============== PROJECTICLE ========================
    void LaunchProjectile()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        GameObject slowprojectileObject = Instantiate(slowProjectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        if (slowCog == true)
        {
            SlowProjectile slowprojectile = slowprojectileObject.GetComponent<SlowProjectile>();
            slowprojectile.Launch(lookDirection, 300);
            animator.SetTrigger("Launch");
        }
        else
        {
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300);
            animator.SetTrigger("Launch");
        }

        audioSource.PlayOneShot(shootingSound);


    }

    // =============== SOUND ==========================

    //Allow to play a sound on the player sound source. used by Collectible
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void setBotText()
    {
        FixedBots.text = "Fixed Bots: " + totalBots.ToString();
        if (totalBots == 6)
        {
            FixedBots.text = "Talk to Jambi!";
        }
        if (totalBots == 7)
        {
            currentSpeed = 0;
            WinTextObj.SetActive(true);
            BotTextObj.SetActive(false);
            AmmoTextObj.SetActive(false);
            SlowCogTextObj.SetActive(false);
            DashCountObj.SetActive(false);
            audioSource.PlayOneShot(winmusic);
        }
    }

    public void setAmmoText()
    {
        AmmoCount.text = "Ammo: " + cogLimit.ToString();
    }

    public void setSlowAmmoText()
    {
        SlowCogCount.text = "Slow Ammo: " + slowCogLimit.ToString();
    }

    public void setDashText()
    {
        DashCount.text = "Dashes: " + dashLimit.ToString();
    }

    IEnumerator Dash()
    {
        if (dashLimit >= 1)
        {
            isInvincible = true;
            currentSpeed = 8;
            yield return new WaitForSecondsRealtime(2);
            currentSpeed = 4;
        }
    }

    

}
