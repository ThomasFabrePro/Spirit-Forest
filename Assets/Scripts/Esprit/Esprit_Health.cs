﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Cyriaque - dans 'public Healthbar' il faut mettre la health bar qui est dans canvas
// si le joueur rentre en collision avec qql chose du layer Enemy alors
// colide = true et il prend des dmgs , quand il arrete de toucher un ennemi colide = false
// quand il se fait toucher une coroutine de 2 s start et pendant ce temps il est invincible
public class Esprit_Health : MonoBehaviour
{
    private int maxHealth = 100;
    private int currentHealth;
    public HealthBar healthBar;
    private bool invicibility = false;
    private bool colide = false;
    public GameObject esprit;
    public AudioSource audioSource;
    public DeathMenu deathMenu;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }


    void OnCollision2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            colide = true;
        }
    }



    void Update()
    {
        if (Input.GetKeyDown("h"))
        {
            TakeDamage(-2000);
        }
        if (colide && !invicibility)
        {
            TakeDamage(20);
            StartCoroutine(Delay());
            colide = false;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        audioSource.Play();

        StartCoroutine(Clignotement());

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());

        }
    }

    //ca devrait pas etre la mais ca marche pas quand c'est mis sur le gameObject spike
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Pique")
        {
            TakeDamage(50);
        }
    }

    IEnumerator Delay()
    {
        invicibility = true;
        yield return new WaitForSeconds(1.5f);
        invicibility = false;
    }
    IEnumerator Clignotement()
    {
        for (int i = 0; i < 3; i++)
        {

            GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(0.25f);
            GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(0.25f);
        }
    }


    IEnumerator Die()
    {

        Debug.Log("Le joueur est dead");
        //bloquer les action
        GetComponent<Esprit_Mouvement>().enabled = false;
        GetComponent<Esprit_Jump>().enabled = false;
        GetComponent<Esprit_Health>().enabled = false;
        GetComponent<Esprit_Dash>().enabled = false;
        GetComponent<Esprit_Combat>().enabled = false;
        //jouer animation de mort
        esprit.GetComponent<Animator>().Play("PlayerDie");
        yield return new WaitForSeconds(1.5f);
        deathMenu.ToggleEndMenu ();

    }
}
