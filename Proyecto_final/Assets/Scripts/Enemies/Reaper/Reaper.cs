using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Reaper : Enemy
{
    [SerializeField] LayerMask floorMask;
    Rigidbody2D Rb2D;
    [SerializeField] Transform raycastOrigin;

    void Start()
    {
        Rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
            Rb2D.velocity = transform.right * speed;
            controlRaycast();

    }
    void controlRaycast()
    {
        RaycastHit2D hitRight = Physics2D.Raycast(raycastOrigin.position, transform.right, 0.1f, floorMask);
        if (hitRight)
        {
            Debug.Log("hitRight");
            if (transform.rotation.y == 0)
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            }
            else
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            }
        }
    }

    protected override void OnTriggerEnter2DInternal(Collider2D collision)
    {
        base.OnTriggerEnter2DInternal(collision);
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Player"))
        {
            Rb2D.velocity = Vector2.zero;
            Invoke("autoDestruction", 2);
        }
    }



}
