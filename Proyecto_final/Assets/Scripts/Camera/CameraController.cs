using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //============TARGET & MOVEMENT SETTINGS===========\\
    public Transform target; // PLAYER OR FOLLOW TARGET
    [SerializeField] float targetMargin; // DISTANCE BEFORE CAMERA MOVES

    public Vector3 nextPosition; // NEXT POSITION FOR TRANSITIONS
    [SerializeField] float transitionSpeed; // SPEED OF TRANSITION MOVEMENT
    //==========END TARGET & MOVEMENT SETTINGS==========//

    //============CAMERA STATES===========\\
    public enum STATES { HORIZONTAL, VERTICAL, FIXED, TRANSITIONING_RIGHT, TRANSITIONING_LEFT }
    public STATES currentState;
    public STATES nextState;
    //==========END CAMERA STATES==========//

    //============SINGLETON===========\\
    public static CameraController Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //==========END SINGLETON==========//

    //============SET CURRENT AND NEXT STATES===========\\
    private void Start()
    {
        currentState = STATES.HORIZONTAL;
        nextState = STATES.HORIZONTAL;
    }

    private void Update()
    {
        //============STATE MACHINE============\\
        switch (currentState)
        {
            case STATES.HORIZONTAL:
                Horizontal();
                break;
            case STATES.VERTICAL:
                Vertical();
                break;
            case STATES.FIXED:
                Fixed();
                break;
            case STATES.TRANSITIONING_RIGHT:
                TransitioningRight();
                break;
            case STATES.TRANSITIONING_LEFT:
                TransitioningLeft();
                break;
        }
    }

    //============HORIZONTAL CAMERA FOLLOW===========\\
    private void Horizontal()
    {
        if (target.transform.position.x <= transform.position.x - targetMargin)
        {
            transform.position = new Vector3(target.transform.position.x + targetMargin, transform.position.y, transform.position.z);
        }
        else if (target.transform.position.x >= transform.position.x + targetMargin)
        {
            transform.position = new Vector3(target.transform.position.x - targetMargin, transform.position.y, transform.position.z);
        }
    }

    //============VERTICAL CAMERA FOLLOW===========\\
    private void Vertical()
    {
        if (target.transform.position.y <= transform.position.y - targetMargin)
        {
            transform.position = new Vector3(transform.position.x, target.transform.position.y + targetMargin, transform.position.z);
        }
        else if (target.transform.position.y >= transform.position.y + targetMargin)
        {
            transform.position = new Vector3(transform.position.x, target.transform.position.y - targetMargin, transform.position.z);
        }
    }

    //============CAMERA TRANSITION TO RIGHT===========\\
    private void TransitioningRight()
    {
        if (transform.position.x < nextPosition.x)
        {
            transform.position = new Vector3(transform.position.x + (transitionSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else
        {
            Debug.Log(nextState); // DEBUG NEXT STATE
            currentState = nextState;
        }
    }

    //============CAMERA TRANSITION TO LEFT===========\\
    private void TransitioningLeft()
    {
        if (transform.position.x > nextPosition.x)
        {
            transform.position = new Vector3(transform.position.x - (transitionSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else
        {
            Debug.Log(nextState); // DEBUG NEXT STATE
            currentState = nextState;
        }
    }

    //============FIXED CAMERA (NO MOVEMENT)===========\\
    private void Fixed()
    {
        //THE CAMERA DOESN'T MOVE WHEN IS FIXED
    }
}
