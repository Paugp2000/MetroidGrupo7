using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //============SPEEDS & FORCES===========\\
    [SerializeField]
    float speed, jumpImpulse;
    //==========END SPEEDS & FORCES==========//



    //============RIGIDBODY===========\\
    Rigidbody2D rb2D;
    //==========END RIGIDBODY==========//

    Animator anim;


    //============MOVEMENT===========\\
    [SerializeField]
    public InputActionAsset inputActionsMapping;

    InputAction horizontal_ia, jump_ia;
    //==========END MOVEMENT==========//



    //============MAP LAYER============\\
    [SerializeField]
    LayerMask MapLayer;
    //==========END MAP LAYER==========//



    //============RAYCAST============\\
    [SerializeField]
    private Transform LeftRaycastOrigin;

    [SerializeField]
    private Transform RightRaycastOrigin;
    //==========END RAYCAST==========//



    //============STATES============\\
    enum STATES { ONFLOOR, ONAIR };
    STATES CurrentState;
    //==========END STATES==========//



    private void Awake()
    {
        inputActionsMapping.Enable();
        horizontal_ia = inputActionsMapping.FindActionMap("Movement").FindAction("Horizontal");
        jump_ia = inputActionsMapping.FindActionMap("Movement").FindAction("Jump");
        anim = GetComponent<Animator>();

        rb2D = GetComponent<Rigidbody2D>();
    }



    private void Start()
    {
        CurrentState = STATES.ONFLOOR;
    }



    private void Update()
    {
        Debug.DrawLine(LeftRaycastOrigin.position, LeftRaycastOrigin.position - LeftRaycastOrigin.up * 0.05f, Color.red);
        Debug.DrawLine(RightRaycastOrigin.position, RightRaycastOrigin.position - RightRaycastOrigin.up * 0.05f, Color.red);
        switch (CurrentState)
        {
            case STATES.ONFLOOR:
                OnFloor();
                break;
            case STATES.ONAIR:
                OnAir();
                break;

        }
    }




    //======================================= STATES FUNCTIONS =======================================\\
    void OnFloor()
    {
        float horizontalDirection = Mathf.RoundToInt(horizontal_ia.ReadValue<float>());

        rb2D.velocity = new Vector2(speed * horizontalDirection, rb2D.velocity.y);
        anim.SetFloat("Run", Math.Abs(rb2D.velocity.magnitude));

        if (horizontalDirection > 0)
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }

        if (horizontalDirection < 0)
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }


        if (jump_ia.triggered)
        {
            rb2D.AddForce(new Vector2(0, 1) * jumpImpulse, ForceMode2D.Impulse);
        }

        if (ToOnAir())
            return;

    }

    void OnAir()
    {
        float horizontalDirection = Mathf.RoundToInt(horizontal_ia.ReadValue<float>());

        anim.SetBool("Salta", true);

        rb2D.velocity = new Vector2((speed * (horizontalDirection/100 * 50)), rb2D.velocity.y);
        if (ToOnFloor())
            return;
    }
    //======================================= STATES FUNCTIONS =======================================//



    //========================================== TRANSITIONS =========================================\\
    bool ToOnFloor()
    {
        if (DetectFloor())
        {
            anim.SetBool("Salta", false);
            return true;
        }
        return false;
    }
    bool ToOnAir()
    {
        if (DetectFloor())
        {
            return false;
        }
        return true;
    }
    //======================================== END TRANSITIONS =======================================//



    //========================================== DIRECTIONS =========================================\\
    bool DetectFloor()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(LeftRaycastOrigin.position, -LeftRaycastOrigin.up, 0.05f, MapLayer);

        RaycastHit2D rightHit = Physics2D.Raycast(RightRaycastOrigin.position, -RightRaycastOrigin.up, 0.05f, MapLayer);

        if (leftHit || rightHit)
        {
            CurrentState = STATES.ONFLOOR;
            return true;
        }
        else
        {
            CurrentState = STATES.ONAIR;
        }
        return false;
    }
    //======================================== END DIRECTIONS =======================================//

}
