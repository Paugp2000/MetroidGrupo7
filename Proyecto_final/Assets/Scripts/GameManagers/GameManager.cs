using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int maxEnergy = 99;
    private int energy;
    public int missiles;

    [SerializeField] private AudioClip introMusic;
    
    private AudioSource audioSource;

    public bool enableMissiles;

    private float timeOnGame;

    public static GameManager Instance;
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        timeOnGame = 0; // Reinicia timeOnGame cuando se carga una escena
        audioSource = GetComponent<AudioSource>();
        energy = 30;
        missiles = 0;
        enableMissiles = false;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
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

    private void Start()
    {
    }

    private void Update()
    {
        timeOnGame += Time.deltaTime;
    }

    public int GetEnergy()
    {
        return energy;
    }

    public void AddEnergy()
    {
        energy += 3;
        if (energy > maxEnergy)
        {
            energy = maxEnergy;
        }
    }

    public void AddMissile()
    {
        missiles++;
    }

    public void AddMissile(int missilesNumber)
    {
        missiles += missilesNumber;
    }

    public int GetMissiles()
    {
        return missiles;
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

    public void StopTimeOnGame()
    {
        AnaliticsManager.Instance.SetTimeOnGame(timeOnGame);
    }


}

