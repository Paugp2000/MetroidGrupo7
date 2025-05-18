using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickable : MonoBehaviour
{
    //============ITEM TYPES===========\\
    enum ITEM { ENERGI, MISSILE, MISSILE_UPGRADE };
    //==========END ITEM TYPES==========//

    public GameObject GrupoMissile; 
    public bool Missiles = false;       //BOOL TO ENABLE MISSILES

    [SerializeField] ITEM item;         //SELECTED ITEM TYPE
    [SerializeField] private AudioClip itemSound;       //SOUND
    private AudioSource audioSource;        // AUDIO SOURCE COMPONENT

    private void Start()
    {
        // GET AUDIOSOURCE COMPONENT AND ENABLE IT
        audioSource = GetComponent<AudioSource>();
        audioSource.enabled = true;
    }

    //============PLAY PICKUP SOUND===========\\
    private void SoundActive()
    {
        if (itemSound != null)
        {
            AudioSource.PlayClipAtPoint(itemSound, transform.position);
        }
    }

    //============ITEM PICKUP===========\\
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            switch (item)
            {
                case ITEM.ENERGI:
                    GameManager.Instance.AddEnergy(); // ADD ENERGY TO PLAYER
                    Destroy(gameObject); // DESTROY PICKED ITEM
                    break;

                case ITEM.MISSILE:
                    GameManager.Instance.AddMissile(); // ADD A MISSILE
                    Destroy(gameObject); // DESTROY PICKED ITEM
                    break;

                case ITEM.MISSILE_UPGRADE:
                    GameManager.Instance.enableMissiles = true; // ENABLE MISSILE FUNCTIONALITY
                    GameManager.Instance.AddMissile(30); // GIVE PLAYER 30 MISSILES
                    GrupoMissile.SetActive(true);
                    Missiles = true;

                    SoundActive(); // PLAY PICKUP SOUND

                    Destroy(gameObject); // DESTROY PICKED ITEM
                    break;
            }
        }
    }
}
