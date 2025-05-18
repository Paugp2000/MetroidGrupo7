using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //============ ENEMY ATTRIBUTES ============\\
    [SerializeField] protected int lives;                      // ENEMY LIVES/HEALTH
    [SerializeField] protected float speed;                    // ENEMY MOVEMENT SPEED
    [SerializeField] protected int damage;                     // DAMAGE DEALT TO PLAYER
    protected bool stunned = false;                            // STUNNED STATE BOOL
    private float stunnedTime = 0.4f;                          // DURATION OF STUN
    private float randomNumber;                               // RANDOM VALUE FOR ITEM DROP

    //============ ITEM DROPS ============\\
    [SerializeField] GameObject powerBeamPickeable;            // POWER BEAM PICKUP PREFAB
    [SerializeField] GameObject missilePickeable;              // MISSILE PICKUP PREFAB

    //============ TRIGGER DETECTION ============\\
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter2DInternal(collision);
    }

    //============ COLLISIONS ============\\
    protected virtual void OnTriggerEnter2DInternal(Collider2D collision)
    {
        if (collision.tag == "Player" && !PlayerController.Instance.Untouchable)
        {
            // DAMAGE PLAYER IF NOT UNTOUCHABLE
            Debug.Log("AU REAPER");
            GameManager.Instance.TakeEnergy(damage);
        }
        else if (collision.tag == "PowerBeam")
        {
            // TAKE DAMAGE FROM POWER BEAM
            TakeDamage(1);
        }
        else if (collision.tag == "Missile")
        {
            // TAKE HIGH DAMAGE FROM MISSILE
            Debug.Log("PATAPUM");
            TakeDamage(10);
        }
    }

    //============ APPLY DAMAGE TO ENEMY ============\\
    protected void TakeDamage(int damage)
    {
        StartCoroutine(stunnedCoroutine());  // START STUN ANIMATION
        lives -= damage;
        if (lives <= 0)
        {
            Dye(); // DESTROY ENEMY IF HEALTH <= 0
        }
    }

    //============ STUN VISUAL FEEDBACK ============\\
    IEnumerator stunnedCoroutine()
    {
        stunned = true;
        GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f, 1); // FLASH RED
        yield return new WaitForSeconds(stunnedTime);
        GetComponent<SpriteRenderer>().color = Color.white; // RESET COLOR
        stunned = false;
    }

    //============ RANDOM ITEM DROP ON DEATH ============\\
    void DropItem()
    {
        randomNumber = UnityEngine.Random.Range(1, 3); // GET RANDOM NUMBER (1 OR 2)
        Debug.Log(randomNumber);

        switch (randomNumber)
        {
            case 1:
                Instantiate(powerBeamPickeable, transform.position, transform.rotation); // DROP POWER BEAM
                break;
            case 2:
                if (GameManager.Instance.enableMissiles)
                {
                    Instantiate(missilePickeable, transform.position, transform.rotation); // DROP MISSILE IF ENABLED
                }
                break;
        }
    }

    //============ HANDLE ENEMY DEATH ============\\
    protected void Dye()
    {
        AnaliticsManager.Instance.AddKill(); // REGISTER KILL
        DropItem();                           // ATTEMPT ITEM DROP
        Destroy(gameObject);                  // REMOVE ENEMY
    }
}
