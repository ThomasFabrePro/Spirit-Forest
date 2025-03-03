﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
    // Cyriaque - Utilise tous les booléens renvoyé par les scripts hitbox pour gerer les déplacements de l'ennemie
    Le comportement de l'ennemi est décrit par le fait que le personnage se trouve ou non dans es différentes hitbox de l'ennemi
    L'ennemi peut avoir plusieurs comportements suivant la position du joueur autour de lui, il peut reculer, frapper,
        se retourner ou avancer. 
    De plus, comme expliqués dans un précédent script, si en attaquant la lance touche le joueur, alors l'ennemi inflige des dégàts. 
**/

public class EnemyLance_Launcher : MonoBehaviour
{
    public int CoolDown;
    public int CDAttacking;
    public EnemyLance_WalkBackward goBack;
    public EnemyLance_Agro agro;
    public EnemyLance_TurnAround turn;
    public Transform gate;
    public EnemyLance_AttackArea attackArea;
    public Rigidbody2D rb;

    private Animator anim;
    public Transform cible;
    private Vector3 scaleChange;
    private Vector3 vecZero = new Vector3(0, 0, 0);

    public bool inCoolDown = false;
    public bool isAttacking = false;
    private bool faceLeft;
    public Animator animator;


    //fonction attaque
    void attack()
    {
        isAttacking = true;
        inCoolDown = true;
        StartCoroutine(TimerCDAttacking());
        StartCoroutine(TimerCoolDown());
    }

    //coroutine cd attack
    IEnumerator TimerCDAttacking()
    {
        yield return new WaitForSeconds(CDAttacking);
        isAttacking = false;
    }

    //coroutine cd CoolDown
    IEnumerator TimerCoolDown()
    {
        yield return new WaitForSeconds(CoolDown);
        inCoolDown = false;
    }

    //fonction mouvement
    void mouvement()
    {
        Vector3 vecAgro = cible.position.Y() - transform.position.Y();
        transform.Translate(vecAgro.normalized * 4 * Time.deltaTime, Space.World);
        anim.SetFloat("speed", 1.0f);

    }

    //fonction retour a la base
    void backToBase()
    {
        if (transform.position.x > gate.position.x)
        {
            Vector3 vecGate = gate.position.Y() - transform.position.Y();
            transform.Translate(vecGate.normalized * 2 * Time.deltaTime, Space.World);
            anim.SetFloat("speed", 1.0f);
        }
        else
        {
            Vector3 vecGate = gate.position.Y() - transform.position.Y();
            transform.Translate(vecGate.normalized * 2 * Time.deltaTime, Space.World);
            anim.SetFloat("speed", 1.0f);
        }
    }

    //fonction recule si joueur trop proche
    void goBackward()
    {
        Vector3 vecAgro = transform.position.Y() - cible.position.Y();
        transform.Translate(vecAgro.normalized * 4 * Time.deltaTime, Space.World);
        anim.SetFloat("speed", 1.0f);

    }

    //fonction rotation vers le joueur
    void rotationPlayer()
    {
        if (faceLeft == true)
        {
            scaleChange = new Vector3(0.4f, 0f, 0f);
            faceLeft = false;
        }
        else
        {
            scaleChange = new Vector3(-0.4f, 0f, 0f);
            faceLeft = true;
        }

        transform.localScale += scaleChange;
        turn.needTurn = false;
    }

    //fonction rotation vers la gate
    void rotationGate()
    {
        if (faceLeft == true)
        {
            scaleChange = new Vector3(0.4f, 0f, 0f);
        }
        else
        {
            scaleChange = new Vector3(-0.4f, 0f, 0f);
        }

        transform.localScale += scaleChange;
        turn.needTurn = false;
    }


    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        //rb.velocity.x = vitesse du personnage sur axe X
        float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("speed", characterVelocity);
        float ecareGate = Vector2.Distance(gate.position, transform.position);
        if (isAttacking)
        {
            animator.SetBool("attacking", true);
        }
        else
        {
            animator.SetBool("attacking", false);
        }


        //if attack
        if (attackArea.inRange == true && inCoolDown == false)
        {
            attack();
        }

        //if mouvement
        if (attackArea.inRange == false && agro.seePlayer == true && isAttacking == false && goBack.needBack == false)
        {

            mouvement();

        }

        //if retour a la base
        if (agro.seePlayer == false && isAttacking == false && ecareGate > 1)
        {
            backToBase();

        }

        //if recul
        if (goBack.needBack == true && isAttacking == false)
        {

            goBackward();

        }

        //if rotation player
        if (turn.needTurn == true && isAttacking == false)
        {

            rotationPlayer();
        }



    }
}