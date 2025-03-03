﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Cyriaque - Gere les mouvement droite / gauche de l'esprit

public class Esprit_Mouvement : MonoBehaviour
{

    public float moveSpeed;
    public Rigidbody2D rb;
    private float horizontalMovement;
    private Vector3 velocity = Vector3.zero;
    public Animator animator;
    public AudioSource audioSrc;


    public void Mouvement()
    {
        horizontalMovement = Input.GetAxis("Horizontal") * Time.fixedDeltaTime * moveSpeed;
        Vector3 wantedVelocity = new Vector2(horizontalMovement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, wantedVelocity, ref velocity, .05f);

    }



    void Update()
    {
        Mouvement();

        float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("Speed", characterVelocity);

        if ((Mathf.Abs(rb.velocity.x) > 0.3f && GetComponent<Esprit_IsGrounded>().isGrounded))
        {
            if (!audioSrc.isPlaying)
            {
                audioSrc.Play();
            }
        }
        else
        {
            audioSrc.Stop();
        }

    }

}
