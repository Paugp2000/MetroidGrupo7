using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDoorsController : MonoBehaviour
{
    [SerializeField] float maxOpenTime = 2f;
     float openedTime = 2f;
    [SerializeField] RedDoorsController partnerDoor;

    enum DOORSTATES {OPENED, CLOSED};
    DOORSTATES currentState = DOORSTATES.CLOSED;

    private void Start()
    {
        CloseDoor();
    }

    private void Update()
    {
        switch (currentState)
        {
            case DOORSTATES.OPENED:
                Opened();
                break;
            case DOORSTATES.CLOSED:
                Closed();
                break;
        }
    }

    void Opened()
    {
        openedTime += Time.deltaTime;
        if (openedTime >= maxOpenTime) {
            CloseDoor();
        }
    }
    void Closed()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Missile"))
        {
            OpenDoor();
            partnerDoor.OpenDoor();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StayOpen();
            partnerDoor.StayOpen();
        }
    }

    public void OpenDoor()
    {
        currentState = DOORSTATES.OPENED;
        GetComponent<BoxCollider2D>().enabled = false;
        openedTime = 0;
    }
    private void CloseDoor()
    {
        currentState = DOORSTATES.CLOSED;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void StayOpen()
    {
        openedTime = 0;
    }
}
