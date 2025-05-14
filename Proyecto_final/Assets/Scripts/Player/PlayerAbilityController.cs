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

    Rigidbody2D rb;

    private void Awake()
    {
        lightShoot = inputActionsMapping.FindActionMap("Abilities").FindAction("LightShoot");
        missileShoot = inputActionsMapping.FindActionMap("Abilities").FindAction("MissileShoot");
        pointingUp = inputActionsMapping.FindActionMap("Pointing").FindAction("Up");

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        bool didShoot = false;
        bool isPointingUp = pointingUp.ReadValue<float>() > 0;
        bool isMoving = Mathf.Abs(rb.velocity.x) > 0.1f;

        if (PlayerController.Instance.CurrentState == PlayerController.STATES.ONFLOOR || PlayerController.Instance.CurrentState == PlayerController.STATES.ONAIR)
        {
            if (lightShoot.triggered)
            {
                AnaliticsManager.Instance.AddPowerBeamsShoot();
                didShoot = true;


                if (isPointingUp)
                    Instantiate(powerBeam, topShootPoint.position, topShootPoint.rotation);
                else
                    Instantiate(powerBeam, shootPoint.position, shootPoint.rotation);
            }
            else if (missileShoot.triggered && GameManager.Instance.missiles > 0)
            {
                didShoot = true;
                AnaliticsManager.Instance.AddMissileShoot();
                GameManager.Instance.missiles--;

                if (isPointingUp)
                    Instantiate(missile, topShootPoint.position, topShootPoint.rotation);
                else
                    Instantiate(missile, shootPoint.position, shootPoint.rotation);
            }
        }

        if (didShoot)
        {
            StartShootingAnimation(isPointingUp, isMoving);
        }

        if (isShooting)
        {
            UpdateShootingDirection(isPointingUp, isMoving);
        }
    }

    private void StartShootingAnimation(bool isPointingUp, bool isMoving)
    {
        UpdateShootingDirection(isPointingUp, isMoving);
        isShooting = true;

        CancelInvoke(nameof(StopShootAnimation));
        Invoke(nameof(StopShootAnimation), 0.2f);
    }

    private void UpdateShootingDirection(bool isPointingUp, bool isMoving)
    {
        // Apagar todas las animaciones primero
        anim.SetBool("Shoot", false);
        anim.SetBool("ShootUpRun", false);
        anim.SetBool("ShootUpIdle", false);
        anim.SetBool("ShootAir", false);
        anim.SetBool("ShootUpAir", false);

        var playerState = PlayerController.Instance.CurrentState;

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
        anim.SetBool("Shoot", false);
        anim.SetBool("ShootUpRun", false);
        anim.SetBool("ShootUpIdle", false);
        anim.SetBool("ShootAir", false);
        anim.SetBool("ShootUpAir", false);
        isShooting = false;
    }
}
