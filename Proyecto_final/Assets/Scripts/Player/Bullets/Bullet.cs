using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    [SerializeField] protected float lifeTime;
    [SerializeField] private AudioClip shootSound;  // Aquí agregamos una variable para el sonido
    private AudioSource audioSource;  // Aquí almacenamos el AudioSource

    private void Start()
    {
        // Obtén el componente AudioSource
        audioSource = GetComponent<AudioSource>();

        // Reproduce el sonido si el AudioSource está asignado
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Configura la velocidad de la bala
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;

        // Destruye la bala después de un tiempo
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Destruye la bala al colisionar
        Destroy(gameObject);
    }
}
