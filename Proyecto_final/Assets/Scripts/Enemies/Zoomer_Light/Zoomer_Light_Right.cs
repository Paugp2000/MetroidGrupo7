using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Zoomer_Light_Right : Enemy
{
    [SerializeField] LayerMask floorLayer;
    [SerializeField] Transform rayCastDownLeft;
    [SerializeField] Transform rayCastDownRightOut;
    [SerializeField] Transform rayCastDownRightIn;
    [SerializeField] Transform rayCastRight;
    Rigidbody2D RbWE;
    Vector3 direction;

    void Start()
    {
        RbWE = GetComponent<Rigidbody2D>();
        direction = transform.right;
    }

    void Update()
    {
        controlRaycast();
        RbWE.velocity = direction * speed;

    }

    void controlRaycast()
    {
        RaycastHit2D hitDownRigthOut = Physics2D.Raycast(rayCastDownRightOut.position, -transform.up, 0.2f, floorLayer);
        RaycastHit2D hitDownRigthIn = Physics2D.Raycast(rayCastDownRightIn.position, -transform.up, 0.2f, floorLayer);
        RaycastHit2D hitDownLeft = Physics2D.Raycast(rayCastDownLeft.position, -transform.up, 0.2f, floorLayer);
        RaycastHit2D hitDownRight = Physics2D.Raycast(rayCastRight.position, transform.right, 0.105f, floorLayer);

        if (hitDownRight) {
            changeRotationL();
            changeDirectionL();
        }

        if (!hitDownLeft)
        {
            if (!hitDownRigthOut && !hitDownRigthIn) { 
                changeRotationR();
                changeDirectionR();
            }
        }

    }
    
    void changeDirectionR()
    {
        if (direction == new Vector3(1, 0, 0))
        {
            direction = new Vector3(0, -1, 0);
        }
        else if (direction == new Vector3(0, -1, 0))
        {
            direction = new Vector3(-1, 0, 0);
        }
        else if (direction == new Vector3(-1, 0, 0))
        {
            direction = new Vector3(0, 1, 0);
        }
        else if (direction == new Vector3(0, 1, 0))
        {
            direction = new Vector3(1, 0, 0);
        }
    }

    void changeDirectionL()
    {
        if (direction == new Vector3(1, 0, 0))
        {
            direction = new Vector3(0, 1, 0);
        }
        else if (direction == new Vector3(0, -1, 0))
        {
            direction = new Vector3(1, 0, 0);
        }
        else if (direction == new Vector3(-1, 0, 0))
        {
            direction = new Vector3(0, -1, 0);
        }
        else if (direction == new Vector3(0, 1, 0))
        {
            direction = new Vector3(-1, 0, 0);
        }
    }

    void changeRotationR()
    {
        if (direction == new Vector3(1, 0, 0))
        {
            transform.rotation = Quaternion.AngleAxis(90, -transform.forward);
        }
        else if (direction == new Vector3(0, -1, 0))
        {
            transform.rotation = Quaternion.AngleAxis(180, -transform.forward);
        }
        else if (direction == new Vector3(-1, 0, 0))
        {
            transform.rotation = Quaternion.AngleAxis(-90, -transform.forward);
        }
        else if (direction == new Vector3(0, 1, 0))
        {
            transform.rotation = Quaternion.AngleAxis(0, -transform.forward);
        }
    }

    void changeRotationL()
    {
        if (direction == new Vector3(1, 0, 0))
        {
            transform.rotation = Quaternion.AngleAxis(-90, transform.forward);
        }
        else if (direction == new Vector3(0, -1, 0))
        {
            transform.rotation = Quaternion.AngleAxis(0, transform.forward);
        }
        else if (direction == new Vector3(-1, 0, 0))
        {
            transform.rotation = Quaternion.AngleAxis(90, transform.forward);
        }
        else if (direction == new Vector3(0, 1, 0))
        {
            transform.rotation = Quaternion.AngleAxis(180, transform.forward);
        }
    }
}
