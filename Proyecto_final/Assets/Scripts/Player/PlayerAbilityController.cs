using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerAbilityController : MonoBehaviour
{

    [SerializeField] Transform shootPoint, topShootPoint;
    [SerializeField] GameObject powerBeam, missile;


    [SerializeField] public InputActionAsset inputActionsMapping;
    InputAction lightShoot, missileShoot, pointingUp;

    private void Awake()
    {
        lightShoot = inputActionsMapping.FindActionMap("Abilities").FindAction("LightShoot");
        missileShoot = inputActionsMapping.FindActionMap("Abilities").FindAction("MissileShoot");
        pointingUp = inputActionsMapping.FindActionMap("Pointing").FindAction("Up");
    }

    private void Update()
    {
        if (PlayerController.Instance.CurrentState == PlayerController.STATES.ONFLOOR || PlayerController.Instance.CurrentState == PlayerController.STATES.ONAIR) {
            if (lightShoot.triggered)
            {
                if (pointingUp.ReadValue<float>() > 0)
                {
                    Instantiate(powerBeam, topShootPoint.position, topShootPoint.rotation); 
                }
                else
                {
                    Instantiate(powerBeam, shootPoint.position, shootPoint.rotation);
                }

            }
            else if(missileShoot.triggered && GameManager.Instance.missiles > 0)
            {
                GameManager.Instance.missiles--;
                if (pointingUp.ReadValue<float>() > 0)
                {
                    Quaternion rotation = Quaternion.Euler(0, -180, 0);
                    Instantiate(missile, topShootPoint.position, topShootPoint.rotation);
                }
                else
                {
                    Quaternion rotation = Quaternion.Euler(0, -180, 0);
                    Instantiate(missile, shootPoint.position, shootPoint.rotation);
                }

            }
        }
    }
}
