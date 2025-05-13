using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skree : Enemy
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask mapLayer;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject target;

    private Transform positionPlayer;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (stunned)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            positionPlayer = target.transform;
            ControlRaycast();
        }
    }

    private void ControlRaycast()
    {
        RaycastHit2D playerHit = Physics2D.Raycast(transform.position, Vector2.down, 12f, playerLayer);
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, 1.05f, mapLayer);
        Debug.DrawRay(transform.position, Vector2.down * 12f, Color.red);

        if (playerHit.collider != null)
        {
            if (playerHit.collider.CompareTag("Player")){
                StartDescent();
                if (playerHit.collider.transform.position.x > transform.position.x)
                {
                    rb.velocity += new Vector2(1f, 0);
                }
                else if(playerHit.collider.transform.position.x < transform.position.x)
                {
                    rb.velocity += new Vector2(-1f, 0);
                }
            }
        }

        if (groundHit.collider != null)
        {
            Debug.Log("suelo tocao");
            rb.velocity = Vector2.zero;
            Invoke("Dye", 1);
        }
    }

    private void StartDescent()
    {
        rb.velocity = new Vector2(0, -10);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2DInternal(collision);
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke("Dye", 1);
        }
    }
}
