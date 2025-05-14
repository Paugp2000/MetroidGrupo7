using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int lives;
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    protected Boolean stunned = false;
    private float stunnedTime = 0.4f;
    private float randomNumber;

    [SerializeField] GameObject powerBeamPickeable, missilePickeable;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter2DInternal(collision);
    }

    protected virtual void OnTriggerEnter2DInternal(Collider2D collision)
    {
        if (collision.tag == "Player" && !PlayerController.Instance.Untouchable) { 
        Debug.Log("AU REAPER");
        GameManager.Instance.TakeEnergy(damage);
        }
        else if (collision.tag == "PowerBeam")
        {
            TakeDamage(1);
        }
        else if (collision.tag == "Missile")
        {
            Debug.Log("PATAPUM");
            TakeDamage(10);
        }
    }

    protected void TakeDamage(int damage)
    {
        StartCoroutine(stunnedCoroutine());
        lives -= damage;
        if (lives <= 0)
        {
            Dye();
        }
    }

    IEnumerator stunnedCoroutine()
    {
        stunned = true;
        GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f,1);
        yield return new WaitForSeconds(stunnedTime);
        GetComponent<SpriteRenderer>().color = Color.white;
        stunned = false;
    }

    void DropItem()
    {
        randomNumber = UnityEngine.Random.Range(1,3);
        Debug.Log(randomNumber);
        switch (randomNumber)
        {
            case 1:
                Instantiate(powerBeamPickeable, transform.position, transform.rotation);
                break;
            case 2:
                if (GameManager.Instance.enableMissiles)
                {
                Instantiate(missilePickeable, transform.position, transform.rotation);
                }
                break;
        }
    }

    protected void Dye()
    {
        AnaliticsManager.Instance.AddKill();
        DropItem();
        Destroy(gameObject);
    }
}
