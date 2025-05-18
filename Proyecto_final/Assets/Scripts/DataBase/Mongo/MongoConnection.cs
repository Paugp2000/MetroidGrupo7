using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MongoConnection : MonoBehaviour
{
    //===================== SINGLETON INSTANCE =====================\\
    public static MongoConnection Instance;

    //===================== MONGODB VARIABLES =====================\\
    private MongoClient client;                                 // MONGODB CLIENT
    private IMongoDatabase database;                            // DATABASE REFERENCE
    private IMongoCollection<BsonDocument> usersCollection;     // COLLECTION REFERENCE

    //===================== CHECK SINGLETON =====================\\
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); // PERSIST BETWEEN SCENES
        }
        else
        {
            Destroy(gameObject); // DESTROY DUPLICATE
        }
    }

    //===================== START ANALYTICS SAVE =====================\\
    public void SaveGameAnalitics()
    {
        StartCoroutine(_SaveGameAnalitics());
    }

    //===================== SAVE ANALYTICS TO MONGODB =====================\\
    public IEnumerator _SaveGameAnalitics()
    {
        Debug.Log("MONGO CONNECTION RECEIVED");

        string connectionString = "mongodb+srv://Player:Player@metroid.6f7gtwb.mongodb.net/?retryWrites=true&w=majority&appName=Metroid";

        try
        {
            // INITIALIZE MONGODB CONNECTION
            client = new MongoClient(connectionString);
            database = client.GetDatabase("Metroid");
            usersCollection = database.GetCollection<BsonDocument>("GameStats");
        }
        catch (System.Exception e)
        {
            Debug.Log("MONGODB CONNECTION ERROR: " + e.Message);
        }

        // CREATE DOCUMENT WITH ANALYTICS DATA
        var document = new BsonDocument
        {
            { "PlayerName", AnaliticsManager.Instance.GetPlayerName() },
            { "Game time", AnaliticsManager.Instance.GetFinalTime() },
            { "Number of jumps", AnaliticsManager.Instance.GetJumps() },
            { "Power beam shoots", AnaliticsManager.Instance.GetPowerBeamsShootsNumber() },
            { "Missile shoots", AnaliticsManager.Instance.GetMissileShootsNumbers() },
            { "Number of kills", AnaliticsManager.Instance.GetKillsNumber() }
        };

        // INSERT DOCUMENT INTO DATABASE
        usersCollection.InsertOne(document);
        Debug.Log("MONGO CONNECTION SENT");
        yield return null;
    }
}
