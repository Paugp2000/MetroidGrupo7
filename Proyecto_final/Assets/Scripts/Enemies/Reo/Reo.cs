using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Reo : Enemy
{ 
    [SerializeField] public Transform target;
    [SerializeField] public float height = 3f;         // Altura máxima del salto          // Velocidad de movimiento
    [SerializeField] public float cooldown = 1f;       // Tiempo entre saltos

    private Vector2 startPoint;
    private Vector2 endPoint;
    private float travelTime;
    private float timer;
    private bool moving = false;
    private bool invokePending = false;

    void Update()
    {
        if (moving)
        {
            timer += Time.deltaTime * speed;
            float t = Mathf.Clamp01(timer / travelTime);

            // Interpolación lineal entre puntos
            Vector2 currentPos = Vector2.Lerp(startPoint, endPoint, t);

            // Altura parabólica manual: y = -4h * t * (t - 1)
            float parabola = 4 * height * t * (1 - t);
            currentPos.y += parabola;

            transform.position = currentPos;

            if (t >= 1f)
            {
                moving = false;
                invokePending = false;
            }
        }
        else if (!invokePending && Mathf.Abs(target.position.x - transform.position.x) < 10f)
        {
            invokePending = true;
            Invoke(nameof(StartParabola), cooldown);
        }
    }

    void StartParabola()
    {
        startPoint = transform.position;
        endPoint = target.position;
        timer = 0f;
        travelTime = Vector2.Distance(startPoint, endPoint) / speed;
        moving = true;
    }
}