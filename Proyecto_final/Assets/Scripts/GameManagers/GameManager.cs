using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int maxEnergy = 99;
    private int energy = 30;
    public int missiles;

    [SerializeField] private AudioClip introMusic;
    private AudioSource audioSource;


    public bool enableMissiles = false;

    private Vector3 lastCheckpoint;


    public static GameManager Instance;
    private void Start()
    {
        // Obtén el componente AudioSource
        audioSource = GetComponent<AudioSource>();
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (introMusic != null)
            {
                AudioSource.PlayClipAtPoint(introMusic, transform.position);
               
            }
            

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLastCheckpoint(Vector3 newCheckpoint)
    {
        Debug.Log("Seted");
        lastCheckpoint = newCheckpoint;
        Debug.Log(lastCheckpoint);
    }

    public int GetEnergy()
    {
        return energy;
    }


    public void AddEnergy()
    {
        energy += 5;
        if (energy > maxEnergy)
        {
            energy = maxEnergy;
        }
    }

    public void AddMissile()
    {
        missiles++;
    }

    public void TakeEnergy(int damage)
    {
        Debug.Log(energy);
        energy -= damage;
        Debug.Log(energy);
        if (energy <= 0)
        {
            PlayerController.Instance.CurrentState = PlayerController.STATES.DEAD;
            
        }
    }

    public void EnableMissiles()
    {
        enableMissiles = true;
    }
}
