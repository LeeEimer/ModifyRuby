using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollectible : MonoBehaviour
{
    public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            
            RubyController.cogLimit += 4;
            Destroy(gameObject);

            controller.PlaySound(collectedClip);
            
        }

    }
}