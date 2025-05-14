using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    public Transform target;
    [SerializeField] float targetMargin;

    public Vector3 nextPosition;
    [SerializeField] float transitionSpeed;
    
    public enum STATES {HORIZONTAL, VERTICAL, FIXED, TRANSITIONING_RIGHT, TRANSITIONING_LEFT }
    public STATES currentState;
    public STATES nextState;


    //========================================== SINGELTON =========================================\\
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
    //========================================== END SINGELTON =========================================//


    private void Start()
    {
        currentState = STATES.VERTICAL;
        nextState = STATES.HORIZONTAL;
    }
    private void Update()
    {
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

    
    private void Horizontal() {

        if (target.transform.position.x <= transform.position.x - targetMargin) {

            transform.position = new Vector3(target.transform.position.x + targetMargin, transform.position.y, transform.position.z);
        }

        else if (target.transform.position.x >= transform.position.x + targetMargin)
        {

            transform.position = new Vector3(target.transform.position.x - targetMargin, transform.position.y, transform.position.z);
        }

    }
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

    private void TransitioningRight()
    {
        if(transform.position.x < nextPosition.x)
        {
            transform.position = new Vector3(transform.position.x + (transitionSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else
        {
            Debug.Log(nextState);
            currentState = nextState;
        }
    }
    private void TransitioningLeft()
    {
        if (transform.position.x > nextPosition.x)
        {
            transform.position = new Vector3(transform.position.x - (transitionSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else
        {
            Debug.Log(nextState);
            currentState = nextState;
        }
    }
    private void Fixed()
    {

    }
}

