using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderData;

public class ItemPickable : MonoBehaviour
{
    enum ITEM {ENERGI, MISSILE, MISSILE_UPGRADE};

    public GameObject GrupoMissile;
    public bool Missiles = false;

    [SerializeField] ITEM item;
    [SerializeField] private AudioClip itemSound;  // Aqu� agregamos una variable para el sonido
    private AudioSource audioSource;  // Aqu� almacenamos el AudioSource

    private void Start()
    {
        // Obt�n el componente AudioSource
        audioSource = GetComponent<AudioSource>();
        audioSource.enabled = true;



    }
    private void SoundActive() 
    {
        if (itemSound != null)
        {
            AudioSource.PlayClipAtPoint(itemSound, transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            switch (item)
            {
                case ITEM.ENERGI:
                    GameManager.Instance.AddEnergy();
                    Destroy(gameObject);
                    break;
                case ITEM.MISSILE:
                    GameManager.Instance.AddMissile();
                    Destroy(gameObject);
                    break;
                case ITEM.MISSILE_UPGRADE:
                   
                    GameManager.Instance.enableMissiles = true;
                    GameManager.Instance.AddMissile(10);
                    GrupoMissile.SetActive(true);
                    Missiles = true;

                    // Reproduce el sonido si el AudioSource est� asignado a la funcion
                    SoundActive();

                    Destroy(gameObject);
                    break;
            }
        }
    }

}
