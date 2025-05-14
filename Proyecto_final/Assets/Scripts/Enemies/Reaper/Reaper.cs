using System.Collections;
using System.Collections.Generic;
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
        if (collision.tag == "Player" && !PlayerController.Instance.Untouchable) { 
            Debug.Log("AU REAPER");
            GameManager.Instance.TakeEnergy(damage);
        }
        else if (collision.tag == "Missile")
        {
            Debug.Log("PATAPUM");
            TakeDamage(10);
        }
    }

}
