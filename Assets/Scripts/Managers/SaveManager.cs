using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Json;
using System;

[Serializable]
public class GameData
{
    public Vector3 playerPosition;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private string saveFilePath;
    public bool shouldLoadGame { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Application.persistentDataPath + "/savegame.json";
            shouldLoadGame = false; // Di default, non caricare
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameData LoadGame()
    {
        if (shouldLoadGame && File.Exists(saveFilePath))
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GameData));
            using (FileStream stream = new FileStream(saveFilePath, FileMode.Open))
            {
                try
                {
                    GameData loadedData = (GameData)serializer.ReadObject(stream);
                    Debug.Log("Dati di gioco caricati da: " + saveFilePath);
                    shouldLoadGame = false;
                    return loadedData;
                }
                catch (System.Runtime.Serialization.SerializationException e)
                {
                    Debug.LogError("Errore durante la deserializzazione del file di salvataggio: " + e.Message);
                    return null;
                }
            }
        }
        else
        {
            if (!shouldLoadGame)
            {
                Debug.Log("Richiesta di nuova partita, nessun caricamento effettuato.");
            }
            else
            {
                Debug.Log("Nessun file di salvataggio trovato in: " + saveFilePath);
            }
            return null;
        }
    }
    
    public void SaveGame(GameData data)
    {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GameData));
        using (FileStream stream = new FileStream(saveFilePath, FileMode.Create))
        {
            serializer.WriteObject(stream, data);
        }
        Debug.Log("Dati di gioco salvati in: " + saveFilePath);
    }

    public GameData GetNewGameData()
    {
        return new GameData
        {
            playerPosition = Vector3.zero
        };
    }
}