using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileUpgrade : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.enableMissiles = true;
        Destroy(gameObject);
    }
}
