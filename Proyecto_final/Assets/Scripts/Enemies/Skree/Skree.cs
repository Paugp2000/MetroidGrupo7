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
            stunned = false;
        }
        else
        {
            positionPlayer = target.transform;
            ControlRaycast();
        }
    }

    private void ControlRaycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 50f, playerLayer);
        Debug.DrawRay(transform.position, Vector2.down * 50f, Color.red);

        if (hit.collider != null)
        {
            StartDescent();
        }
    }

    private void StartDescent()
    {
        rb.velocity = new Vector2(positionPlayer.right.x, -speed);
    }


    protected override void OnTriggerEnter2DInternal(Collider2D collision)
    {
        base.OnTriggerEnter2DInternal(collision);
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;
            Invoke("autoDestruction", 2);
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
