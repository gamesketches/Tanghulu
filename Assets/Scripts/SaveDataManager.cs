using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager instance;
    PlayerData playerData;

    void Awake()
    {
        instance = this;
        //MakeCleanPlayerData();
        LoadSaveData();
    }

    /// <summary>
    /// Changes number of coins player has by value
    /// </summary>
    /// <param name="coins">Amount to be added to coins (can be negative)</param>
    public void UpdatePlayerCoins(int coins) {
        playerData.numCoins += coins;
        if (playerData.numCoins < 0) playerData.numCoins = 0;
        SavePlayerData();
    }

    public int GetPlayerCoins() {
        return playerData.numCoins;
    }

    public void UpdateOwnedColorSchemes(int newColorScheme) {
        List<int> previouslyOwnedPalettes = new List<int>(playerData.unlockedPalettes);
        previouslyOwnedPalettes.Add(newColorScheme);
        playerData.unlockedPalettes = previouslyOwnedPalettes.ToArray();
        SavePlayerData();
    }

    public bool PlayerOwnsScheme(int colorScheme) {
        for (int i = 0; i < playerData.unlockedPalettes.Length; i++)
        {
            if (playerData.unlockedPalettes[i] == colorScheme) return true;
        }
        return false;
    }

    public int GetPlayerPreferredColorScheme() {
        return playerData.preferredColorScheme;
    }

    public void SetPlayerPreferredColorScheme(int newColorScheme) {
        playerData.preferredColorScheme = newColorScheme;
        SavePlayerData();
    }

    void SavePlayerData() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerData.gd");
        bf.Serialize(file, playerData);
        file.Close();
    }

    void LoadSaveData() {
        if (File.Exists(Application.persistentDataPath + "/playerData.gd")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerData.gd", FileMode.Open);
            try {
                playerData = (PlayerData)bf.Deserialize(file);
            } catch {
                playerData = new PlayerData();
            }
            file.Close();
        }
        else {
            playerData = new PlayerData();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SavePlayerData();
    }

    private void OnApplicationQuit() {
        SavePlayerData();
    }

    private void MakeCleanPlayerData() {
        playerData = new PlayerData();
        SavePlayerData();
    }
}

[System.Serializable]
public class PlayerData {
    public int numCoins;
    public int preferredColorScheme;
    public int[] unlockedPalettes;

    public PlayerData() {
        numCoins = 0;
        preferredColorScheme = 0;
        unlockedPalettes = new int[] { 0 };
    }
}