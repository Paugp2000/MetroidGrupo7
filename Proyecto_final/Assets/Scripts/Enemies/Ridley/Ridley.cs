using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Ridley : Enemy
{
    Rigidbody2D RbR;
    [SerializeField] private float jumpImpulse;
    [SerializeField] private float timeBetweenJumps;
    [SerializeField] private float timeBetweenShootsR;
    [SerializeField] private float frequency;
    [SerializeField] private GameObject shoot;
    private float currentTimeBetweenJumps;
    private float currentTimeBetweenShootsR;


    void Start()
    {
        RbR = GetComponent<Rigidbody2D>();
        currentTimeBetweenJumps = timeBetweenJumps;
        currentTimeBetweenShootsR = timeBetweenShootsR;
        
    }

    void Update()
    {
        currentTimeBetweenJumps -= Time.deltaTime;
        if (currentTimeBetweenJumps <= 0)
        {
            currentTimeBetweenJumps = timeBetweenJumps;
            RidleyJump();
        }
            currentTimeBetweenShootsR -= Time.deltaTime;
        if(currentTimeBetweenShootsR <= 0)
        {
            currentTimeBetweenShootsR = timeBetweenShootsR;
            StartCoroutine("RidleyAttack");
        }
       
    }
    void RidleyJump()
    {
        
        RbR.AddForce(new Vector2(0, 1) * jumpImpulse, ForceMode2D.Impulse);
    }
    IEnumerator RidleyAttack()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(shoot, transform.position + new Vector3(2, 2, 0), Quaternion.identity);
            yield return new WaitForSeconds(frequency);
        }
    }
}
