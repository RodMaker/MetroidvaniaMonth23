using System;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Bardent.CoreSystem;
using System.Linq;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using Bardent;
using System.Collections;
using Cinemachine;

[Serializable]
class PlayerDataSaveSystem
{
    public int currentHealth;
    public int maxHealth;
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

    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private float respawnTime;

    private float respawnTimeStart;

    private bool respawn;

    private CinemachineVirtualCamera CVC;

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

    private void Start()
    {
        CVC = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
    }

    public void Save()
    {
        Player player = FindObjectOfType<Player>();
        PlayerInputHandler playerInputHandler = FindObjectOfType<PlayerInputHandler>();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        PlayerDataSaveSystem data = new PlayerDataSaveSystem();

        data.currentHealth = player.GetComponent<PlayerHealth>().currentHealth;
        data.maxHealth = player.GetComponent<PlayerHealth>().maxHealth;
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
        //gameOverUI.SetActive(false); // ADDED
        //playerObj.SetActive(true); // ADDED
        //playerObj.GetComponent<PlayerHealth>().StartPlayer(); // ADDED

        if (File.Exists(filePath))
        {
            Player player = FindObjectOfType<Player>();
            PlayerInputHandler playerInputHandler = FindObjectOfType<PlayerInputHandler>();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);

            PlayerDataSaveSystem data = (PlayerDataSaveSystem)bf.Deserialize(file);
            file.Close();

            player.GetComponent<PlayerHealth>().currentHealth = data.currentHealth;
            player.GetComponent<PlayerHealth>().maxHealth = data.maxHealth;
            player.transform.position = new Vector3(data.playerPosX, data.playerPosY, 0);
            playerInputHandler.grabUnlocked = data.canGrab;
            playerInputHandler.dashUnlocked = data.canDash;
            playerInputHandler.jumpUnlocked = data.canJump;
            player.crouchUnlocked = data.canCrouch;
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over");

        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(1);
        gameOverUI.SetActive(true);
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

    private void Update()
    {
        CheckRespawn();
    }

    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
        gameOverUI.SetActive(false);
    }

    private void CheckRespawn()
    {
        if (Time.time >= respawnTimeStart - respawnTime && respawn)
        {
            var playerTemp = Instantiate(player, respawnPoint);
            player.GetComponentInParent<PlayerHealth>().StartPlayer();
            player.SetActive(true);
            Load();
            CVC.m_Follow = playerTemp.transform;
            respawn = false;
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