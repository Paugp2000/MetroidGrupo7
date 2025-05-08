using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Reaper : Enemy
{
    [SerializeField] LayerMask floorMask;
    Rigidbody2D Rb;
    float direction = 1;
    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Rb.velocity = new Vector2(direction * speed, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (stunned)
        {
            Rb.velocity = Vector2.zero;
            stunned = false;
        }
        else
        {
            controlRaycast();
            Rb.velocity = new Vector2(direction * speed, 0f);
        }
    }
    void controlRaycast()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, -transform.right, 0.8f, floorMask);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, transform.right, 0.8f, floorMask);
        if (hitLeft)
        {
            direction *= -1;
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
        if (hitRight)
        {
            direction *= -1;
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);

        }
    }
}
