using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //LayerMask camera;

    [SerializeField] Transform target;
    [SerializeField] float targetMargin;


    enum STATES {HORIZONTAL, VERTICAL}
    STATES currentState;
    private void Start()
    {
        currentState = STATES.HORIZONTAL;
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
        transform.position = new Vector3(transform.position.x, target.transform.position.y, transform.position.z);

    }



}
