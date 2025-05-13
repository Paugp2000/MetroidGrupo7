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


    async void Start()
    {
    }

    public void SaveFinalTime()
    {
        Debug.Log("Mongo Connection recivido");

        string connectionString = "mongodb+srv://a22alarodcos:<Alan.11111111>@metroid.6f7gtwb.mongodb.net/";

        try
        {
            client = new MongoClient(connectionString);
            database = client.GetDatabase("Metroid");
            usersCollection = database.GetCollection<BsonDocument>("Metroid.GameStats");
        }
        catch (System.Exception e)
        {
            Debug.Log("MongoDB Connection Error: " + e.Message);
        }

        var document = new BsonDocument
        {
            { "Final Time: ", GameManager.Instance.GetFinalTime()}
        };
        usersCollection.InsertOne(document);
        Debug.Log("Mongo Connection Enviado");
    }
}