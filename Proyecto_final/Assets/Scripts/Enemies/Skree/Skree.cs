using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skree : Enemy
{
    [SerializeField] private LayerMask playerLayer;
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 12f, playerLayer);
        Debug.DrawRay(transform.position, Vector2.down * 50f, Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player")){
                StartDescent();
                if (hit.collider.transform.position.x > transform.position.x)
                {
                    rb.velocity += new Vector2(1f, 0);
                }
                else if(hit.collider.transform.position.x < transform.position.x)
                {
                    rb.velocity += new Vector2(-1f, 0);
                }
            }
        }
    }

    private void StartDescent()
    {
        rb.velocity = new Vector2(0, -10);
    }


    protected override void OnTriggerEnter2DInternal(Collider2D collision)
    {
        base.OnTriggerEnter2DInternal(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke("autoDestruction", 1);
        }
        if (collision.gameObject.CompareTag("Floor"))
        {
            rb.velocity = Vector2.zero;
            Invoke("autoDestruction", 1);
        }
    }

    private void autoDestruction()
    {
        Instantiate(explosion, transform.position + new Vector3(1, 1, 0), Quaternion.identity);
        Instantiate(explosion, transform.position + new Vector3(-1, 1, 0), Quaternion.identity);
        Instantiate(explosion, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
        Instantiate(explosion, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
        Destroy(gameObject);
    }
}
