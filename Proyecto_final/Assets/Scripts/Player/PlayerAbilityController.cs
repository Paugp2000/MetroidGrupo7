using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityController : MonoBehaviour
{
    //============SHOOT POINTS & PROJECTILES===========\\
    [SerializeField] Transform shootPoint, topShootPoint; // SPAWN POINTS FOR SHOOTING
    [SerializeField] GameObject powerBeam, missile; // PROJECTILE PREFABS
    //==========END SHOOT POINTS & PROJECTILES==========//

    //============INPUT ACTIONS===========\\
    [SerializeField] public InputActionAsset inputActionsMapping;
    InputAction lightShoot, missileShoot, pointingUp;
    //==========END INPUT ACTIONS==========//

    Animator anim;
    bool isShooting = false;

    Rigidbody2D rb;

    private void Awake()
    {
        //============SETUP INPUT ACTIONS===========\\
        lightShoot = inputActionsMapping.FindActionMap("Abilities").FindAction("LightShoot");
        missileShoot = inputActionsMapping.FindActionMap("Abilities").FindAction("MissileShoot");
        pointingUp = inputActionsMapping.FindActionMap("Pointing").FindAction("Up");

        //============ANIMATOR AND RIGIDBODY COMPONENTS===========\\
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        bool didShoot = false;
        bool isPointingUp = pointingUp.ReadValue<float>() > 0;
        bool isMoving = Mathf.Abs(rb.velocity.x) > 0.1f;

        //============SHOOT INPUT===========\\
        if (PlayerController.Instance.CurrentState == PlayerController.STATES.ONFLOOR || PlayerController.Instance.CurrentState == PlayerController.STATES.ONAIR)
        {
            if (lightShoot.triggered)
            {
                AnaliticsManager.Instance.AddPowerBeamsShoot(); // LOG POWER BEAM SHOOT
                didShoot = true;

                //============INSTANTIATE POWER BEAM BASED ON DIRECTION===========\\
                if (isPointingUp)
                    Instantiate(powerBeam, topShootPoint.position, topShootPoint.rotation);
                else
                    Instantiate(powerBeam, shootPoint.position, shootPoint.rotation);
            }
            else if (missileShoot.triggered && GameManager.Instance.missiles > 0)
            {
                didShoot = true;
                AnaliticsManager.Instance.AddMissileShoot(); // LOG MISSILE SHOOT
                GameManager.Instance.missiles--;

                // INSTANTIATE MISSILE BASED ON DIRECTION
                if (isPointingUp)
                    Instantiate(missile, topShootPoint.position, topShootPoint.rotation);
                else
                    Instantiate(missile, shootPoint.position, shootPoint.rotation);
            }
        }

        //============START SHOOT ANIMATION===========\\
        if (didShoot)
        {
            StartShootingAnimation(isPointingUp, isMoving);
        }

        //============UPDATE SHOOTING DIRECTION===========\\
        if (isShooting)
        {
            UpdateShootingDirection(isPointingUp, isMoving);
        }
    }

    private void StartShootingAnimation(bool isPointingUp, bool isMoving)
    {
        UpdateShootingDirection(isPointingUp, isMoving);
        isShooting = true;

        CancelInvoke(nameof(StopShootAnimation)); // CANCEL PREVIOUS STOP IF ACTIVE
        Invoke(nameof(StopShootAnimation), 0.2f); // STOP SHOOT ANIMATION AFTER DELAY
    }

    private void UpdateShootingDirection(bool isPointingUp, bool isMoving)
    {
        // RESET ALL SHOOT ANIMATION STATES
        anim.SetBool("Shoot", false);
        anim.SetBool("ShootUpRun", false);
        anim.SetBool("ShootUpIdle", false);
        anim.SetBool("ShootAir", false);
        anim.SetBool("ShootUpAir", false);

        var playerState = PlayerController.Instance.CurrentState;

        // SET CORRECT SHOOTING ANIMATION BASED ON STATE AND INPUT
        if (playerState == PlayerController.STATES.ONAIR)
        {
            if (isPointingUp)
                anim.SetBool("ShootUpAir", true);
            else
                anim.SetBool("ShootAir", true);
        }
        else
        {
            if (isPointingUp)
            {
                if (isMoving)
                    anim.SetBool("ShootUpRun", true);
                else
                    anim.SetBool("ShootUpIdle", true);
            }
            else
            {
                anim.SetBool("Shoot", true);
            }
        }
    }

    private void StopShootAnimation()
    {
        // STOP ALL SHOOT ANIMATION STATES
        anim.SetBool("Shoot", false);
        anim.SetBool("ShootUpRun", false);
        anim.SetBool("ShootUpIdle", false);
        anim.SetBool("ShootAir", false);
        anim.SetBool("ShootUpAir", false);
        isShooting = false;
    }
}
