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
    string playersName;
    int gamesPlayed;
    int gameJumps;
    int gamePowerBeamsShoots;
    int gameMissilesShoots;
    int gameKills;


    [SerializeField] TMP_Text PlayerNameText;
    [SerializeField] TMP_Text GamesPlayedText;  
    [SerializeField] TMP_Text JumpsText;
    [SerializeField] TMP_Text PowerBeamShootsText;
    [SerializeField] TMP_Text MissileShootsText;
    [SerializeField] TMP_Text KillsText;


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

            playersName = playerData.GetValue("playerName").ToString();
            gamesPlayed = playerData.GetValue("gamesPlayed").ToInt32();
            gameKills = playerData.GetValue("totalKills").ToInt32();
            gameMissilesShoots = playerData.GetValue("missilesShooted").ToInt32();
            gamePowerBeamsShoots = playerData.GetValue("powerBeamsShooted").ToInt32();
            gameJumps = playerData.GetValue("jumps").ToInt32();

            PlayerNameText.text = playersName;
            GamesPlayedText.text = ((int)gamesPlayed).ToString();
            JumpsText.text = ((int)gameJumps).ToString();
            PowerBeamShootsText.text = ((int)gamePowerBeamsShoots).ToString();
            MissileShootsText.text = ((int)gameMissilesShoots).ToString();
            KillsText.text = ((int)gameKills).ToString();

            Debug.Log("Games played: " + gamesPlayed);
            Debug.Log("Number of kills: " + gameKills);
            Debug.Log("Number of Missiles: " + gameMissilesShoots);
            Debug.Log("Number of Beams: " + gamePowerBeamsShoots);
            Debug.Log("Number of Jumps: " + gameJumps);
        }
        else
        {
            PlayerNameText.text = "Null";
            GamesPlayedText.text = "Null";
            JumpsText.text = "Null";
            PowerBeamShootsText.text = "Null";
            MissileShootsText.text = "Null";
            KillsText.text = "Null";

            Debug.Log("Player not found");
        }



    }

}
