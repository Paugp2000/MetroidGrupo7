using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickable : MonoBehaviour
{
    enum ITEM {ENERGI, MISSILE, MISSILE_UPGRADE};

    [SerializeField] ITEM item;

    private void Start()
    {
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
                    Destroy(gameObject);
                    break;
            }
        }
    }

}
