using System;
using UnityEngine;

/// <summary>
/// This class handle Enemy behaviour. It make them walk back & forth as long as they aren't fixed, and then just idle
/// without being able to interact with the player anymore once fixed.
/// </summary>
public class HardEnemy : MonoBehaviour
{
    // ====== ENEMY MOVEMENT ========
    public float speed;
    public float timeToChange;
    public bool horizontal;

    public ParticleSystem smokeParticleEffect;
    public ParticleSystem fixedParticleEffect;

    public AudioClip hitSound;
    public AudioClip fixedSound;

    Rigidbody2D rigidbody2d;
    float remainingTimeToChange;
    Vector2 direction = Vector2.right;
    bool repaired = false;
    public int pointsWorth;

    // ===== ANIMATION ========
    Animator animator;

    // ================= SOUNDS =======================
    AudioSource audioSource;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        remainingTimeToChange = timeToChange;

        direction = horizontal ? Vector2.right : Vector2.down;

        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
        smokeParticleEffect.Play();
        setSpeed(4); 
    }

    void Update()
    {
        if (repaired)
            return;

        remainingTimeToChange -= Time.deltaTime;

        if (remainingTimeToChange <= 0)
        {
            remainingTimeToChange += timeToChange;
            direction *= -1;
        }

        animator.SetFloat("ForwardX", direction.x);
        animator.SetFloat("ForwardY", direction.y);
    }

    void FixedUpdate()
    {
        rigidbody2d.MovePosition(rigidbody2d.position + direction * speed * Time.deltaTime);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (repaired)
            return;

        RubyController controller = other.collider.GetComponent<RubyController>();

        if (controller != null)
            controller.ChangeHealth(-2);
    }

    public void Fix()
    {
        animator.SetTrigger("Fixed");
        repaired = true;

        RubyController.killedBots += pointsWorth;

        smokeParticleEffect.Stop();

        Instantiate(fixedParticleEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);

        //we don't want that enemy to react to the player or bullet anymore, remove its reigidbody from the simulation
        rigidbody2d.simulated = false;

        audioSource.Stop();
        audioSource.PlayOneShot(hitSound);
        audioSource.PlayOneShot(fixedSound);
    }

    public void setSpeed(int x)
    {
        speed = x;
    }
}
