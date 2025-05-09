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

   

    Animator anim;
    bool isShooting = false;

    
    private void Awake()
    {
        lightShoot = inputActionsMapping.FindActionMap("Abilities").FindAction("LightShoot");
        missileShoot = inputActionsMapping.FindActionMap("Abilities").FindAction("MissileShoot");
        pointingUp = inputActionsMapping.FindActionMap("Pointing").FindAction("Up");
        anim = GetComponent<Animator>();

    }

    private void Update()
    {
        bool didShoot = false;

        if (PlayerController.Instance.CurrentState == PlayerController.STATES.ONFLOOR ||
            PlayerController.Instance.CurrentState == PlayerController.STATES.ONAIR)
        {
            
            if (lightShoot.triggered)
            {
                
                didShoot = true;

                if (pointingUp.ReadValue<float>() > 0)
                    Instantiate(powerBeam, topShootPoint.position, topShootPoint.rotation);
                else
                    Instantiate(powerBeam, shootPoint.position, shootPoint.rotation);
            }
            else if (missileShoot.triggered && GameManager.Instance.missiles > 0)
            {
                didShoot = true;
                GameManager.Instance.missiles--;

                if (pointingUp.ReadValue<float>() > 0)
                    Instantiate(missile, topShootPoint.position, topShootPoint.rotation);
                else
                    Instantiate(missile, shootPoint.position, shootPoint.rotation);
            }
        }

        if (didShoot)
        {
            StartShootingAnimation();
        }
    }

    private void StartShootingAnimation()
    {
        if (!isShooting)
        {
            if (pointingUp.triggered)
            {

            }
            anim.SetBool("Shoot", true);
            isShooting = true;
        }

        // Reset the timer to stop animation in 0.2s if no more shooting happens
        CancelInvoke(nameof(StopShootAnimation));
        Invoke(nameof(StopShootAnimation), 0.2f);
    }

    private void StopShootAnimation()
    {
        anim.SetBool("Shoot", false);
        isShooting = false;
    }
}
