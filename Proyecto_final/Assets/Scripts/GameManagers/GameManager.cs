using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //============ PLAYER STATS ============\\
    private int maxEnergy = 99;       // MAXIMUM ENERGY LIMIT
    private int energy;              // CURRENT ENERGY
    public int missiles;            // CURRENT MISSILES COUNT
    public bool enableMissiles;     // BOOL TO CHECK IF MISSILES ARE ENABLED

    //============ AUDIO ============\\
    [SerializeField] private AudioClip introMusic; // INTRO MUSIC CLIP
    private AudioSource audioSource;               // AUDIO SOURCE COMPONENT

    //============ GAME TIME TRACKING ============\\
    private float timeOnGame; // IN-GAME TIME PLAYER

    //============ SINGLETON ============\\
    public static GameManager Instance;

    //============ SCENE EVENT REGISTRATION ============\\
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // REGISTER TO SCENE LOAD EVENT
    }

    //============ INITIALIZE VALUES ON SCENE LOAD ============\\
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        timeOnGame = 0;                  // RESET GAME TIME
        audioSource = GetComponent<AudioSource>();      // GET AUDIOSOURCE
        energy = 30;                     // INITIAL ENERGY
        missiles = 0;                    // INITIAL MISSILES
        enableMissiles = false;         // DISABLE MISSILES INITIALLY
    }

    //============SINGELTON CHECK============\\
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); // PERSIST BETWEEN SCENES

            if (introMusic != null)
            {
                AudioSource.PlayClipAtPoint(introMusic, transform.position); // PLAY INTRO MUSIC
            }
        }
        else
        {
            Destroy(gameObject); // DESTROY DUPLICATE
        }
    }


    //============ UPDATE GAME TIME ============\\
    private void Update()
    {
        timeOnGame += Time.deltaTime;
    }

    //============ ENERGY METHODS ============\\
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

    public void TakeEnergy(int damage)
    {
        Debug.Log(energy);
        energy -= damage;
        Debug.Log(energy);

        if (energy <= 0)
        {
            PlayerController.Instance.CurrentState = PlayerController.STATES.DEAD; // TRIGGER PLAYER DEATH
        }
    }

    //============ MISSILE METHODS ============\\
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

    public void EnableMissiles()
    {
        enableMissiles = true;
    }

    //============ ANALYTICS ============\\
    public void StopTimeOnGame()
    {
        AnaliticsManager.Instance.SetTimeOnGame(timeOnGame); // SEND TIME TO ANALYTICS
    }
}
