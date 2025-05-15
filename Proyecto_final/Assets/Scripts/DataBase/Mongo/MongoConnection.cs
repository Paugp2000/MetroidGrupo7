using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MongoConnection : MonoBehaviour
{

    public static MongoConnection Instance;

    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<BsonDocument> usersCollection;


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


    

    public void SaveGameAnalitics()
    {
        Debug.Log("Mongo Connection recivido");

        string connectionString = "mongodb+srv://Player:Player@metroid.6f7gtwb.mongodb.net/?retryWrites=true&w=majority&appName=Metroid";

        try
        {
            client = new MongoClient(connectionString);
            database = client.GetDatabase("Metroid");
            usersCollection = database.GetCollection<BsonDocument>("GameStats");
        }
        catch (System.Exception e)
        {
            Debug.Log("MongoDB Connection Error: " + e.Message);
        }

        var document = new BsonDocument
        {
            { "PlayerName", AnaliticsManager.Instance.GetPlayerName()},
            { "Game time", AnaliticsManager.Instance.GetFinalTime()},
            { "Number of jumps", AnaliticsManager.Instance.GetJumps()},
            { "Power beam shoots", AnaliticsManager.Instance.GetPowerBeamsShootsNumber()},
            { "Missile shoots", AnaliticsManager.Instance.GetMissileShootsNumbers()},
            { "Number of kills", AnaliticsManager.Instance.GetKillsNumber()}
        };
        usersCollection.InsertOne(document);
        Debug.Log("Mongo Connection Enviado");
    }
}