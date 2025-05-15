using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class MongoReader : MonoBehaviour
{
    List<double> times = new();
    List<int> jumps = new();
    List<int> powerBeamShoots = new();
    List<int> missileShoots = new();
    List<int> kills = new();


    double averageTime;
    double averageJumps;
    double averagePowerBeamShoots;
    double averageMissileShoots;
    double averageKills;


    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<BsonDocument> usersCollection;

    private void Start()
    {
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


        //============READ DATA============//
        var allData = usersCollection.Find(new BsonDocument()).ToList();

        foreach (var document in allData)
        {
            times.Add(document.GetValue("Game Time").ToDouble());
            jumps.Add(document.GetValue("Number of jumps").ToInt32());
            powerBeamShoots.Add(document.GetValue("Power beam shoots").ToInt32());
            missileShoots.Add(document.GetValue("Missile shoots").ToInt32());
            kills.Add(document.GetValue("Number of kills").ToInt32());
        }
        //============READ DATA============//


        //============AVERAGE CALCULATIONS============//
        averageTime = times.Average();
        averageJumps = jumps.Average();
        averagePowerBeamShoots = powerBeamShoots.Average();
        averageMissileShoots = missileShoots.Average();
        averageKills = kills.Average();
        //============AVERAGE CALCULATIONS============//


        Debug.Log("AVG Times " + averageTime);
        Debug.Log("AVG Jumps " + averageJumps);
        Debug.Log("AVG Power beams shoots " + averagePowerBeamShoots);
        Debug.Log("AVG Missiles shoots" + averageMissileShoots);
        Debug.Log("AVG Kills" + averageKills);

    }
    
}
