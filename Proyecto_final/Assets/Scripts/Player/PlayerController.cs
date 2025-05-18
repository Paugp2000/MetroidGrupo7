using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //============SPEEDS & FORCES===========\\
    [SerializeField]
    float speed, jumpImpulse;
    //==========END SPEEDS & FORCES==========//

    //============SOUND===========\\

    [SerializeField] private AudioClip jumpSound;  // Aquí agregamos una variable para el sonido
    [SerializeField] private AudioClip deadSound;  // Aquí agregamos una variable para el sonido
    [SerializeField] private AudioClip musicaJuego;  // Aquí agregamos una variable para el sonido
    private AudioSource audioSource;  // Aquí almacenamos el AudioSource


    [SerializeField] private string sceneName = "NombreDeLaEscena"; // Cambia esto por el nombre de la escena
    [SerializeField] private float delay = 5f; // Tiempo de espera en segundos

    //==========END SOUND==========//


    //============RIGIDBODY===========\\
    public Rigidbody2D rb2D;
    //==========END RIGIDBODY==========//

    Animator anim;

    //============MOVEMENT===========\\
    [SerializeField]
    float jumpTime = 0.25f;
    const float MIN_JUMP_TIME = 0.15f;

    const int JUMP_MOBILITY_PERCENT = 75;

    public bool canMove = true;

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
    public enum STATES { ONFLOOR, ONAIR, HURTED, DEAD, ON_TRANSITION_LEFT, ON_TRANSITION_RIGHT, APPEARING};
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

        //============INPUT ACTIONS============\\
        inputActionsMapping.Enable();
        horizontal_ia = inputActionsMapping.FindActionMap("Movement").FindAction("Horizontal");
        jump_ia = inputActionsMapping.FindActionMap("Movement").FindAction("Jump");


        //============ANIMATOR============\\
        anim = GetComponent<Animator>();


        //============RIGIDBODY============\\
        rb2D = GetComponent<Rigidbody2D>();




        //============SINGELTON CHECK============\\
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

        CurrentState = STATES.APPEARING; //SET THE FIRST STATE

        StartCoroutine(Appear()); //CALL TO THE COROUINE "Appear"

        untouchableTime = maxUntouchableTime; //SET THE UNTOUCHABLE TIME TO THE MAX TIME

        rb2D.isKinematic = false; //SET KINEMATIC

        audioSource = GetComponent<AudioSource>(); //GET AUDIO SOURCE

        
    }

    private void Update()
    {

        //============STATE MACHINE============\\
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
            case STATES.APPEARING:
                Appearing();
                break;
        }
    }

    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\\
    //======================================================================= STATES FUNCTIONS =======================================================================\\
    void OnFloor()
    {
        jumpTime = MIN_JUMP_TIME;      //SET THE JUMP TIME TO THE MINIMUM

        float horizontalDirection = Mathf.RoundToInt(horizontal_ia.ReadValue<float>());        //GET THE HORIZONTAL VALUE

        rb2D.velocity = new Vector2(speed * horizontalDirection, rb2D.velocity.y);      //SET THE VELOCITY
        anim.SetFloat("Run", Math.Abs(rb2D.velocity.magnitude));


        //============DIRECTION CHECK TO ROTATE THE CHARACTER============\\
        if (horizontalDirection > 0)
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }

        if (horizontalDirection < 0)
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }



        //============JUMP TRIGGER============\\
        if (jump_ia.triggered)
        {
            if (audioSource != null && jumpSound != null) //Reproduce el sonido si el AudioSource está asignado
            {
                audioSource.PlayOneShot(jumpSound);
            }

            rb2D.AddForce(new Vector2(0, 1) * jumpImpulse, ForceMode2D.Impulse);        //SET JUMP FORCE
            AnaliticsManager.Instance.AddJump();        //ADD ONE JUMP TO THE ANALITICS
        }


        //============POSSIBLE TRANSITIONS============\\
        if (ToOnAir())
            return;
    }


    void OnAir()
    {
        jumpTime -= Time.deltaTime;     //WHILE ONAIR "jumpTime" DECREASES TO HAVE A MINIMUM TIME OF JUMP

        float horizontalDirection = Mathf.RoundToInt(horizontal_ia.ReadValue<float>());     //GET THE HORIZONTAL VALUE


        //============DIRECTION CHECK TO ROTATE THE CHARACTER============\\
        if (horizontalDirection > 0)
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }

        if (horizontalDirection < 0)
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }



        //============SMOOTH JUMP============\\
        if (rb2D.velocity.y < 0)
        { 
        }
        //this condition secures to have a minimum time on the air AND can stop going up if the player don't have the jump triggered when jumpTime is over
        else if ((jump_ia.WasReleasedThisFrame() && rb2D.velocity.y > 0 && jumpTime <= 0) || (jumpTime <= 0 && !jump_ia.IsPressed())) {   
            
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
        }

        anim.SetBool("Salta", true);



        rb2D.velocity = new Vector2(speed * (horizontalDirection / 100 * JUMP_MOBILITY_PERCENT), rb2D.velocity.y);      //ADJUST HORIZONTAL MOVEMENT MOBILITY ON AIR


        //============POSSIBLE TRANSITIONS============\\
        if (ToOnFloor())
            return;
    }

    void Hurted()
    {
        Untouchable = true;     //IF IT IS HURT BECOMES UNTOUCHABLE


        //============STAY UNTOUCHABLE WHILE UNTOUCHABLE TIME IS NOT OVER============\\
        if (untouchableTime > 0)
        {
            untouchableTime -= Time.deltaTime;
        }
        else
        {
            Untouchable = false;
            untouchableTime = maxUntouchableTime;       //SET UNTOUCHABLE TIME TO MAXIMUM FOR THE NEXT TIME

            //============POSSIBLE TRANSITIONS============\\
            if (ToOnAir())
                return;

            if (ToOnFloor())
                return;
        }
    }


    void Dead()
    {
        rb2D.velocity = Vector2.zero;       //SET VELOCITY TO STOP THE CHARACTER

        rb2D.isKinematic = true;        //SET KINEMATIC TRUE TO DON'T MOVE THE CHARACTER

        anim.SetTrigger("Dead");        //Animacion Muerte

        if (deadSound != null)      //Sonido Muerte
        {
            AudioSource.PlayClipAtPoint(deadSound, transform.position);
        }
        StartCoroutine(ChangeSceneAfterDelay());
    }

    void OnTransitionL()
    {
        if (transform.position.x > nextTransitionPosition.x)
        {
            transform.position = new Vector3(transform.position.x + (transitionSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else
        {

            //============POSSIBLE TRANSITIONS============\\
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

            //============POSSIBLE TRANSITIONS============\\
            if (ToOnAir())
                return;

            if (ToOnFloor())
                return;
        }

    }


    void Appearing()
    {
        //THE PLAYER CAN'T DO ANYTHING WHILE APPEARING
    }

    //==================================================================== END STATES FUNCTIONS ======================================================================//
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX//

    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\\
    //========================================================================== TRANSITIONS =========================================================================\\
    bool ToOnFloor()
    {

        //============CHECK IF DETECTING FLOOR============\\
        if (DetectFloor())
        {
            anim.SetBool("Salta", false);
            return true;        //SET TRUE BECAUSE IF IT IS TOUCHING FLOOR IT IS ON FLOOR
        }
        return false;
    }
    bool ToOnAir()
    {
        if (DetectFloor())
        {
            return false;       //SET FALSE BECAUSE IF IT IS TOUCHING FLOOR IT IS NOT ON AIR
        }
        return true;
    }

    //======================================================================== END TRANSITIONS =======================================================================//
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX//

    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\\
    //========================================================================== DETECTIONS ==========================================================================\\
    bool DetectFloor()
    {

        //============FLOOR RAYCASTS============\\
        RaycastHit2D leftHit = Physics2D.Raycast(LeftRaycastOrigin.position, -LeftRaycastOrigin.up, 0.05f, MapLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(RightRaycastOrigin.position, -RightRaycastOrigin.up, 0.05f, MapLayer);


        //============RETURN VALUES CONDITIONS============\\
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

        //============GET DAMAGE IF ENEMY TOUCH AND THE PLAYER IS NOT UNTOUCHABLE============\\
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
    //======================================================================== END DETECTIONS ========================================================================//
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX//


    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\\
    //======================================================================== EXTRA FUNCTIONS =======================================================================\\


    //============PUSH THE PLAYER WHEN RECIVE DAMAGE============\\
    void Push(Vector3 pushOrigin)
    {
        StartCoroutine(pushColorCoroutine());       
        rb2D.velocity = new Vector2(0, 0);      //STOP THE CHARACTER

        //============GET AWAY FROM THE PUSH ORIGIN============\\
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

    //============COROUTINE TO CHANGE COLOR WHEN TAKE DAMAGE============\\
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


    IEnumerator Appear()
    {
        yield return new WaitForSeconds(7);     //WHAIT 7 SECONDS
        Debug.Log("Puedes empezar a jugar");
        CurrentState = STATES.ONFLOOR;      //SET STATE TO ONFLOOR
        audioSource.enabled = true;     //ENABLE AUDIO SOURCE TO START THE BACKGROUD MUSIC
    }


    bool Transitioning()
    {
        return true;        //RETURN TRUE TO KNOW WHEN PLAYER IS TRANSITIONING
    }


    private System.Collections.IEnumerator ChangeSceneAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);      //CHANGE SCENE
    }

    public void FinIntro()
    {
        if (audioSource != null && musicaJuego != null)       //PLAYS MUSIC IF NOT NULL
        {
            audioSource.PlayOneShot(musicaJuego);
        }
    }
    //====================================================================== END EXTRA FUNCTIONS =====================================================================//
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX//
}
