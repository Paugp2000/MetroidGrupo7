using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    [SerializeField] protected float lifeTime;
    [SerializeField] private AudioClip shootSound;  // Aqu� agregamos una variable para el sonido
    private AudioSource audioSource;  // Aqu� almacenamos el AudioSource

    private void Start()
    {
        // Obt�n el componente AudioSource
        audioSource = GetComponent<AudioSource>();

        // Reproduce el sonido si el AudioSource est� asignado
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Configura la velocidad de la bala
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;

        // Destruye la bala despu�s de un tiempo
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Destruye la bala al colisionar
        Destroy(gameObject);
    }
}
