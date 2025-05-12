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
    [SerializeField] private AudioClip itemSound;  // Aquí agregamos una variable para el sonido
    private AudioSource audioSource;  // Aquí almacenamos el AudioSource

    private void Start()
    {
        // Obtén el componente AudioSource
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
                    if (GameManager.Instance.enableMissiles)
                    {
                        GameManager.Instance.missiles++;
                        Destroy(gameObject);
                    }
                    break;
                case ITEM.MISSILE_UPGRADE:
                   
                    GameManager.Instance.enableMissiles = true;
                    GrupoMissile.SetActive(true);
                    Missiles = true;

                    // Reproduce el sonido si el AudioSource está asignado a la funcion
                    SoundActive();

                    Destroy(gameObject);
                    break;
            }
        }
    }

}
