using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsController : MonoBehaviour
{

    [SerializeField] CameraController.STATES toRightState;
    [SerializeField] CameraController.STATES toLeftState;

    [SerializeField]    float nextCameraYCoordinate;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("choca");
            if (CameraController.Instance.transform.position.x < transform.position.x) //if the camera is on the left side of the door, move it to the right side the camera and the player
            {
                Debug.Log("to right");
                //asign Y axis to the correct camera Y axis
                CameraController.Instance.transform.position = new Vector3(CameraController.Instance.transform.position.x, nextCameraYCoordinate, CameraController.Instance.transform.position.z);
                
                PlayerController.Instance.nextTransitionPosition = transform.position + new Vector3(2.5f, 0, 0);
                PlayerController.Instance.CurrentState = PlayerController.STATES.ON_TRANSITION_RIGHT;

                if (toRightState == CameraController.STATES.HORIZONTAL) //if the camera is going to be on HORIZONTAL state, the next camera position is the position of the player + 2 units of margin
                {
                    CameraController.Instance.nextPosition = new Vector3(CameraController.Instance.target.transform.position.x + 4, CameraController.Instance.transform.position.y, CameraController.Instance.transform.position.z);
                }
                
                else{ //else, the next camera position is going to be 8 units far of the door
                    CameraController.Instance.nextPosition = new Vector3(transform.position.x + 8, CameraController.Instance.transform.position.y, CameraController.Instance.transform.position.z);
                }

                
                CameraController.Instance.currentState = CameraController.STATES.TRANSITIONING_RIGHT; //change the state of the camera
                
                CameraController.Instance.nextState = toRightState; //change the next state of the camera. Assigned to know which state is going to be when the transition finish
            }

            //else, if the camera is on the right side of the door, move it to the left side
            else if (CameraController.Instance.transform.position.x > transform.position.x)
            {
                //it does the same of the previous if, but to the left
                Debug.Log("to left");
                CameraController.Instance.transform.position = new Vector3(CameraController.Instance.transform.position.x, nextCameraYCoordinate, CameraController.Instance.transform.position.z);

                PlayerController.Instance.nextTransitionPosition = transform.position + new Vector3(-2.5f, 0, 0);
                PlayerController.Instance.CurrentState = PlayerController.STATES.ON_TRANSITION_LEFT;

                if (toLeftState == CameraController.STATES.HORIZONTAL)
                {
                    CameraController.Instance.nextPosition = new Vector3(CameraController.Instance.target.transform.position.x - 4, CameraController.Instance.transform.position.y, CameraController.Instance.transform.position.z);
                }

                else
                {
                    CameraController.Instance.nextPosition = new Vector3(transform.position.x - 8, CameraController.Instance.transform.position.y, CameraController.Instance.transform.position.z);
                }
                CameraController.Instance.currentState = CameraController.STATES.TRANSITIONING_LEFT;
                CameraController.Instance.nextState = toLeftState;
            }
        }
    }
}
