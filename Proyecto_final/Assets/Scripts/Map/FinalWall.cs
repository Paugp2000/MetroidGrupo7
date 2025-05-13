using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalWall : MonoBehaviour
{

    [SerializeField]GameObject Rydley;

    private void Update()
    {
        if (Rydley == null)
            Destroy(gameObject);
    }



}
