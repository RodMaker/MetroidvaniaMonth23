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
using UnityEngine.XR;

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
    public GameObject playerObj;
    public bool firstTime = true;

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

            Debug.Log("GAME LOADED");
        }
    }

    /*
    private void Start()
    {
        //gameOverUI = GameOverMenu.Instance.gameObject;
    }
    public void GameOver()
    {
        Debug.Log("Game Over");

        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(1);
        //SceneManager.LoadScene("GameOver");
        gameOverUI.SetActive(true);
    }

    public void Restart()
    {
        firstTime = false;
        //gameOverUI.SetActive(false);
        //gameOverUI = GameOverMenu.Instance.gameObject;
        StartCoroutine(RestartRoutine());
        //gameOverUI = GameObject.Find("Canvas").GetComponentInChildren<GameOverMenu>().gameObject;
        //gameOverUI = FindObjectOfType<GameOverMenu>().gameObject;
    }

    private IEnumerator RestartRoutine()
    {
        var asyncLevelLoad = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
        yield return new WaitUntil(() => asyncLevelLoad.isDone);
        playerObj.SetActive(true);
        playerObj.GetComponent<PlayerHealth>().StartPlayer();
        Bardent.Camera.Instance.GetComponent<CinemachineVirtualCamera>().m_Follow = playerObj.transform;
        //gameOverUI = GameOverMenu.Instance.gameObject;
    }

    public void MainMenu()
    {
        firstTime = false;
        //gameOverUI.SetActive(false);
        SceneManager.LoadScene("Menu");
        playerObj.GetComponent<PlayerHealth>().StartPlayer();
    }

    public void Quit()
    {
        Application.Quit();
    }
    */

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