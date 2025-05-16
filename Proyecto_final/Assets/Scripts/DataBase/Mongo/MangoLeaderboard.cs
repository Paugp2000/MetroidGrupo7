using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
public class MangoLeaderboard : MonoBehaviour
{

    [SerializeField] public List<TMP_Text> NamesList = new List<TMP_Text>();
    [SerializeField] public List<TMP_Text> TimesList = new List<TMP_Text>();

    private int positionCounter;

    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<BsonDocument> usersCollection;


    private void Start()
    {
        positionCounter = 0;

        //============CONECTION TO DB============//
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
        //============CONECTION TO DB============//



        var filter = Builders<BsonDocument>.Filter.Empty;

        var projection = Builders<BsonDocument>.Projection.Include("PlayerName").Include("Game time");

        var top10 = usersCollection.Find(filter).Sort(Builders<BsonDocument>.Sort.Ascending("Game time")).Project(projection).Limit(10).ToList();

        foreach (var document in top10)
        {
            
            NamesList[positionCounter].text = document["PlayerName"].AsString;

            double scoreTime = document["Game time"].ToDouble();

            int hours = (int)(scoreTime / 3600);
            int minutes = (int)((scoreTime % 3600) / 60);
            int seconds = (int)(scoreTime % 60);
            int milliseconds = (int)((scoreTime * 10) % 10);
            string formattedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", hours, minutes, seconds, milliseconds);

            TimesList[positionCounter].text = formattedTime;

            positionCounter++;
            //string name = document["PlayerName"].AsString;
            //double gameTime = document["Game time"].ToDouble();

            //Debug.Log($"PlayerName: {name}, Game Time: {gameTime}");
        }







    }

}
