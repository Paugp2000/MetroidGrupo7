using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidleyShoots : Enemy
{
    Rigidbody2D rb;
    [SerializeField] int minForce = 2;
    [SerializeField] int maxForce = 7;
    int bulletForce;

    private void Awake()
    { 
        bulletForce = Random.Range(minForce, maxForce);
    }
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(1,1)* bulletForce;
        Destroy(gameObject, 3f);
    }

// Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true && !PlayerController.Instance.Untouchable)
        {
            GameManager.Instance.TakeEnergy(damage);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Floor")== true)
        {
            Destroy(gameObject);
        }
    }
   
}

