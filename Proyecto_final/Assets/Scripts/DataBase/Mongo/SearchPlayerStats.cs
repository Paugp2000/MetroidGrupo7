using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchPlayerStats : MonoBehaviour
{

    [SerializeField] private TMP_InputField playerNameInputField;

    string playerName;

    int gameKills;
    int gameMissilesShoots;
    int gamePowerBeamsShoots;
    int gameJumps;
    int gamesPlayed;

    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<BsonDocument> usersCollection;

    public void UpdateName()
    {
        playerName = playerNameInputField.text;
        Debug.Log("player Name: " + playerName);
        ActualizeStats(playerName);
    }

    private void ActualizeStats(string playerName)
    {
        string connectionString = "mongodb+srv://Player:Player@metroid.6f7gtwb.mongodb.net/?retryWrites=true&w=majority&appName=Metroid";

        try
        {
            client = new MongoClient(connectionString);
            database = client.GetDatabase("Metroid");
            usersCollection = database.GetCollection<BsonDocument>("Players");
        }
        catch (System.Exception e)
        {
            Debug.Log("MongoDB Connection Error: " + e.Message);
        }

        var nameFilter = Builders<BsonDocument>.Filter.Eq("playerName", playerName);
        var playerData = usersCollection.Find(nameFilter).FirstOrDefault();

        if (playerData != null)
        {

            gamesPlayed = playerData.GetValue("gamesPlayed").ToInt32();
            gameKills = playerData.GetValue("totalKills").ToInt32();
            gameMissilesShoots = playerData.GetValue("missilesShooted").ToInt32();
            gamePowerBeamsShoots = playerData.GetValue("powerBeamsShooted").ToInt32();
            gameJumps = playerData.GetValue("jumps").ToInt32();

            Debug.Log("Games played: " + gamesPlayed);
            Debug.Log("Number of kills: " + gameKills);
            Debug.Log("Number of Missiles: " + gameMissilesShoots);
            Debug.Log("Number of Beams: " + gamePowerBeamsShoots);
            Debug.Log("Number of Jumps: " + gameJumps);
        }
        else
        {
            Debug.Log("Player not found");
        }



    }

}
