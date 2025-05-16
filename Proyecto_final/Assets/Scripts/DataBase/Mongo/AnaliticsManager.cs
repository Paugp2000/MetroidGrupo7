using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnaliticsManager : MonoBehaviour
{



    private string playerName;
    private double finalTime = 0;
    private int jumpsNumber = 0;
    private int PowerBeamShootsNumber = 0;
    private int missileShootsNumber = 0;
    private int KillsNumber = 0;

    public static AnaliticsManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);


        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void SaveAnalitics()
    {
        MongoConnection.Instance.SaveGameAnalitics();
    }


    // PLAYER NAME
    public void setPlayerName(string playerName)
    {
        this.playerName = playerName;
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    // FINAL TIME
    public void SetTimeOnGame(float time)
    {
        finalTime = time;
    }

    public double GetFinalTime()
    {
        return finalTime;
    }

    // JUMPS NUMBER
    public void AddJump()
    {
        jumpsNumber++;
    }

    public int GetJumps()
    {
        return jumpsNumber;
    }

    // POWER BEAMS SHOOTS NUMBER
    public void AddPowerBeamsShoot()
    {
        PowerBeamShootsNumber++;
    }

    public int GetPowerBeamsShootsNumber()
    {
        return PowerBeamShootsNumber;
    }

    // MISSILES SHOOTS NUMBER
    public void AddMissileShoot()
    {
        missileShootsNumber++;
    }

    public int GetMissileShootsNumbers()
    {
        return missileShootsNumber;
    }

    // KillsNumber
    public void AddKill()
    {
        missileShootsNumber++;
    }

    public int GetKillsNumber()
    {
        return KillsNumber;
    }

}
