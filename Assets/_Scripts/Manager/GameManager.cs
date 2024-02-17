﻿using System;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Bardent.CoreSystem;
using System.Linq;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using Bardent;

[Serializable]
class PlayerDataSaveSystem
{
    public float health;
    public float playerPosX, playerPosY;
    public bool canGrab;
    public bool canDash;
    public bool canJump;
    public bool canCrouch;

    /*
    public int currentPrimaryWeaponId;
    public int currentSecondaryWeaponId;
    */
}

public class GameManager : MonoBehaviour
{
    public event Action<GameState> OnGameStateChanged;

    private GameState currentGameState = GameState.Gameplay;

    // ADDED
    public static GameManager gm;

    private string filePath;

    public GameObject gameOverUI;
    private GameObject playerObj;
    private bool firstTime = true;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
        else if (gm != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        filePath = Application.persistentDataPath + "/playerInfo.dat";

        Load();

        Debug.Log("GAME LOADED");
    }

    public void Save()
    {
        Player player = FindObjectOfType<Player>();
        PlayerInputHandler playerInputHandler = FindObjectOfType<PlayerInputHandler>();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        PlayerDataSaveSystem data = new PlayerDataSaveSystem();

        data.health = player.GetComponentInChildren<Stats>().Health.CurrentValue;
        data.playerPosX = player.transform.position.x;
        data.playerPosY = player.transform.position.y;
        data.canGrab = playerInputHandler.grabUnlocked;
        data.canDash = playerInputHandler.dashUnlocked;
        data.canJump = playerInputHandler.jumpUnlocked;
        data.canCrouch = player.crouchUnlocked;

        /* TO DO
        if (FindObjectOfType<WeaponInventory>().weaponData != null)
        {
            data.currentPrimaryWeaponId = player.primaryWeaponEquipped.itemID;
            data.currentSecondaryWeaponId = player.secondaryWeaponEquipped.itemID;
        }
        */

        bf.Serialize(file, data);

        file.Close();

        Debug.Log("GAME SAVED");
    }

    public void Load()
    {
        gameOverUI.SetActive(false); // ADDED
        playerObj.SetActive(true); // ADDED
        playerObj.GetComponent<PlayerHealth>().StartPlayer(); // ADDED

        if (File.Exists(filePath))
        {
            Player player = FindObjectOfType<Player>();
            PlayerInputHandler playerInputHandler = FindObjectOfType<PlayerInputHandler>();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);

            PlayerDataSaveSystem data = (PlayerDataSaveSystem)bf.Deserialize(file);
            file.Close();

            player.GetComponentInChildren<Stats>().Health.CurrentValue = data.health;
            player.transform.position = new Vector3(data.playerPosX, data.playerPosY, 0);
            playerInputHandler.grabUnlocked = data.canGrab;
            playerInputHandler.dashUnlocked = data.canDash;
            playerInputHandler.jumpUnlocked = data.canJump;
            player.crouchUnlocked = data.canCrouch;
        }
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
        Debug.Log("Game Over");
    }

    public void Restart()
    {
        firstTime = false;
        gameOverUI.SetActive(false);
        SceneManager.LoadScene("Game");
        playerObj.SetActive(true);
        playerObj.GetComponent<PlayerHealth>().StartPlayer();
    }

    public void MainMenu()
    {
        firstTime = false;
        gameOverUI.SetActive(false);
        SceneManager.LoadScene("Menu");
        playerObj.GetComponent<PlayerHealth>().StartPlayer();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OnEnable()
    {
        Player.OnStart += GetPlayer;
        SceneManager.sceneLoaded += SetPlayer;
    }

    public void OnDestroy() => Player.OnStart -= GetPlayer;

    private void GetPlayer(GameObject player) => playerObj = player;
    private void SetPlayer(Scene scene, LoadSceneMode mode)
    {
        if (!firstTime && scene.buildIndex == 1)
        {
            playerObj.SetActive(true);
        }
    }

    public void ChangeState(GameState state)
    {
        if (state == currentGameState)
            return;

        switch (state)
        {
            case GameState.UI:
                EnterUIState();
                break;
            case GameState.Gameplay:
                EnterGameplayState();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        currentGameState = state;
        OnGameStateChanged?.Invoke(currentGameState);
    }

    private void EnterUIState()
    {
        Time.timeScale = 0f;
    }

    private void EnterGameplayState()
    {
        Time.timeScale = 1f;
    }

    public enum GameState
    {
        UI,
        Gameplay
    }
}