using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespausarMusica : MonoBehaviour
{
    [SerializeField] private AudioClip gameMusic;
    private AudioSource audioSource;
    private void Start()
    {
        // Obtén el componente AudioSource
        audioSource = GetComponent<AudioSource>();
    }
    public void FinIntro()
    {
        if (gameMusic != null)
        {
            AudioSource.PlayClipAtPoint(gameMusic, transform.position);
        }
        
    }
}
