using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //============SPEEDS & FORCES===========\\
    [SerializeField]
    float speed, jumpImpulse;
    //==========END SPEEDS & FORCES==========//

    [SerializeField] private AudioClip jumpSound;  // Aquí agregamos una variable para el sonido
    private AudioSource audioSource;  // Aquí almacenamos el AudioSource

    //============RIGIDBODY===========\\
    public Rigidbody2D rb2D;
    //==========END RIGIDBODY==========//

    Animator anim;

    //============MOVEMENT===========\\
    [SerializeField]
    float jumpTime = 0.25f;
    const float MIN_JUMP_TIME = 0.15f;

    const int jumpMobilityPercet = 75;

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


    //============STATES============\\ads
    public enum STATES { ONFLOOR, ONAIR, HURTED, DEAD, ON_TRANSITION_LEFT, ON_TRANSITION_RIGHT};
    public STATES CurrentState;
    //==========END STATES==========//


    //============VARIABLES============\\
    [SerializeField] float maxUntouchableTime;
    float untouchableTime;
    public Boolean Untouchable = false;
    public Vector3 nextTransitionPosition;
    [SerializeField] float transitionSpeed;
    //==========END VARIABLES==========//


    //============SINGLETON============\\
    public static PlayerController Instance;
    //==========END SINGELTON==========//

    


    private void Awake()
    {
        inputActionsMapping.Enable();
        horizontal_ia = inputActionsMapping.FindActionMap("Movement").FindAction("Horizontal");
        jump_ia = inputActionsMapping.FindActionMap("Movement").FindAction("Jump");
        anim = GetComponent<Animator>();

        rb2D = GetComponent<Rigidbody2D>();

        


        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CurrentState = STATES.ONFLOOR;
        untouchableTime = maxUntouchableTime;
        rb2D.isKinematic = false;

        // Obtén el componente AudioSource
        audioSource = GetComponent<AudioSource>();

        
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
            case STATES.HURTED:
                Hurted();
                break;
            case STATES.DEAD:
                Dead();
                break;
            case STATES.ON_TRANSITION_LEFT:
                OnTransitionL();
                break;
            case STATES.ON_TRANSITION_RIGHT:
                OnTransitionR();
                break;
        }
    }

    //======================================= STATES FUNCTIONS =======================================\\
    void OnFloor()
    {
        jumpTime = MIN_JUMP_TIME;

        float horizontalDirection = Mathf.RoundToInt(horizontal_ia.ReadValue<float>());

        rb2D.velocity = new Vector2(speed * horizontalDirection, rb2D.velocity.y);
        anim.SetFloat("Run", Math.Abs(rb2D.velocity.magnitude));

        if (horizontalDirection > 0)
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }

        if (horizontalDirection < 0)
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }


        if (jump_ia.triggered)
        {
            // Reproduce el sonido si el AudioSource está asignado
            if (audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
            rb2D.AddForce(new Vector2(0, 1) * jumpImpulse, ForceMode2D.Impulse);
        }

        if (ToOnAir())
            return;
    }

    void OnAir()
    {
        jumpTime -= Time.deltaTime;

        float horizontalDirection = Mathf.RoundToInt(horizontal_ia.ReadValue<float>());
        
        if (horizontalDirection > 0)
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }

        if (horizontalDirection < 0)
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }

        rb2D.velocity = new Vector2((speed * (horizontalDirection/100 * jumpMobilityPercet)), rb2D.velocity.y);

        if (rb2D.velocity.y < 0)
        { 
        }
        else if ((jump_ia.WasReleasedThisFrame() && rb2D.velocity.y > 0 && jumpTime <= 0) || (jumpTime <= 0 && !jump_ia.IsPressed())) {
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
           
        }

        anim.SetBool("Salta", true);
        
        

        rb2D.velocity = new Vector2((speed * (horizontalDirection/100 * 50)), rb2D.velocity.y);
        if (ToOnFloor())
            return;
    }

    void Hurted()
    {
        Untouchable = true;
        if (untouchableTime > 0)
        {
            untouchableTime -= Time.deltaTime;
        }
        else
        {
            Untouchable = false;
            untouchableTime = maxUntouchableTime;
            if (ToOnAir())
                return;

            if (ToOnFloor())
                return;
        }
    }

    void Dead()
    {
        rb2D.velocity = Vector2.zero;
        rb2D.isKinematic = true;
        //animaci�n morir
    }

    void OnTransitionL()
    {
        if (transform.position.x > nextTransitionPosition.x)
        {
            transform.position = new Vector3(transform.position.x + (transitionSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else
        {
            if (ToOnAir())
                return;

            if (ToOnFloor())
                return;
        }

    }


    void OnTransitionR()
    {
        if (transform.position.x < nextTransitionPosition.x)
        {
            transform.position = new Vector3(transform.position.x + (transitionSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else
        {
            if (ToOnAir())
                return;

            if (ToOnFloor())
                return;
        }

    }
        //======================================= END STATES FUNCTIONS =======================================//



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

    //========================================== DETECTIONS =========================================\\
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !Untouchable)
        {
            Debug.Log("Auch");
            if (GameManager.Instance.GetEnergy() > 0)
            {
                CurrentState = STATES.HURTED;
                Push(collision.transform.position);
            }

            
        }
    }
    //======================================== END DETECTIONS =======================================//

    //======================================== EXTRA FUNCTIONS =======================================\\

    void Push(Vector3 pushOrigin)
    {
        StartCoroutine(pushColorCoroutine());
        rb2D.velocity = new Vector2(0, 0);

        if (pushOrigin.x < transform.position.x)
        {
            rb2D.velocity = new Vector2(3, 3);
            Debug.Log("patra");
        }
        else
        {
            rb2D.velocity = new Vector2(-3, 3);
            Debug.Log("patra");

        }
    }

    IEnumerator pushColorCoroutine()
    {
        GetComponent<SpriteRenderer>().color = new Color(255, 0.60f, 0.1f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(255, 0.70f, 0.2f, 0.3f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(255, 0.60f, 0.1f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(255, 0.70f, 0.2f, 0.3f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(255, 0.60f, 0.1f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    bool Transitioning()
    {
        return true;
    }
    //======================================== END EXTRA FUNCTIONS =======================================//
}
