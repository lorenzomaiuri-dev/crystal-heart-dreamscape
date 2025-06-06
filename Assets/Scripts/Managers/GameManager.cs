using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance = null;

    private PlayerData currentPlayerData;

    public GameState currentGameState = GameState.Playing;
    public PauseMenuUIController pauseMenuUIController;
    [SerializeField] private GameObject playerObject;

    public struct WorldViewCoordinates
    {
        public float Top;
        public float Right;
        public float Bottom;
        public float Left;
    }

    public WorldViewCoordinates worldViewCoords;

    // Initialize the singleton instance
    private void Awake()
    {
        // If there is not already an instance, set it to this
        if (Instance == null)
        {
            Instance = this;
        }
        // If an instance already exists, destroy whatever this object is to enforce the singleton
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Set to DontDestroyOnLoad so that it won't be destroyed when reloading our scene
        DontDestroyOnLoad(gameObject);
    }

    // called third
    private void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    private void Update()
    {
        if (InputManager.Instance != null && InputManager.Instance.pauseInput && currentGameState != GameState.GameOver)
        {
            SetGameState(currentGameState == GameState.Playing ? GameState.Paused : GameState.Playing);
            InputManager.Instance.pauseInput = false;
        }
        
        if (currentGameState == GameState.Playing)
        {
            // here is where we can do things while the game is running
            GetWorldViewCoordinates();
            // SpawnEnemies();
            // RepositionEnemies();
            DestroyStrayBullets();
        }
        else if (currentGameState == GameState.GameOver)
        {
            TextDisplayManager.Instance.ShowGameOverText("GAME OVER\n\nPress [R] to Restart");

            if (InputManager.Instance != null && InputManager.Instance.reloadInput)
            {
                InputManager.Instance.pauseInput = false;
                SetGameState(GameState.Playing);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    // called first
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called when the game is terminated
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // called second
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject playerInstance = GameObject.FindGameObjectWithTag("Player");

        if (playerInstance != null)
        {
            Debug.Log("ApplyPlayerData");
            Debug.Log(currentPlayerData.currentHp);
            // Do Startup Functions - Scene has to be fully loaded
            if (currentPlayerData != null)
            {
                ApplyPlayerData(playerInstance, currentPlayerData);
                //Debug.Log("Dati del giocatore caricati e applicati alla nuova istanza.");
                currentPlayerData = null;
            }
            else
            {
                //Debug.Log("Nessun dato del giocatore salvato, inizializzazione con valori di default.");
            }
        }
        else
        {
            //Debug.Log("Non trovato il giocatore nella scena");
        }
    }

    // initializes and starts the game
    private void StartGame()
    {
        currentGameState = GameState.Playing;
        //SoundManager.Instance.MusicSource.Play();
        LoadGameDataOnStart();
        LoadPlayerDataOnStart();
    }
    
    private void LoadGameDataOnStart()
    {
        GameData loadedData = SaveManager.Instance.LoadGame();

        if (loadedData != null)
        {
            // Applica i dati caricati
            if (playerObject != null)
            {
                playerObject.transform.position = loadedData.playerPosition;
                //Debug.Log("Dati di gioco caricati e applicati.");
            }
            else
            {
                //Debug.LogError("Oggetto giocatore non assegnato in GameInitializer!");
            }
        }
        else
        {
            //Debug.Log("Nessun salvataggio trovato all'avvio della scena di gioco.");
        }
    }
    
    private void LoadPlayerDataOnStart()
    {
        GameData loadedData = SaveManager.Instance.LoadGame();

        if (loadedData != null)
        {
            // Applica i dati caricati
            if (playerObject != null)
            {
                playerObject.transform.position = loadedData.playerPosition;
                //Debug.Log("Dati di gioco caricati e applicati.");
            }
            else
            {
                //Debug.LogError("Oggetto giocatore non assegnato in GameInitializer!");
            }
        }
        else
        {
            //Debug.Log("Nessun salvataggio trovato all'avvio della scena di gioco.");
        }
    }
    
    public void SavePlayerData()
    {
        Debug.Log("SavePlayerData");
        GameObject playerInstance = GameObject.FindGameObjectWithTag("Player");
        
        if (playerInstance != null)
        {
            PlayerData data = new PlayerData();

            PlayerController playerController = playerInstance.GetComponent<PlayerController>();
            if (playerController != null)
            {
                data.currentHp = playerController.GetCurrentHealth();
            }

            currentPlayerData = data;
            //Debug.Log("Dati del giocatore salvati temporaneamente.");
        }
        else
        {
            //Debug.Log("Nessuna istanza del giocatore attiva per salvare i dati.");
        }
    }

    private void ApplyPlayerData(GameObject playerInstance, PlayerData data)
    {
        PlayerController playerController = playerInstance.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.UpdateCurrentHealth(data.currentHp);
        }
    }

    private void FreezePlayer(bool freeze)
    {
        // freeze player and input
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerController>().FreezePlayer(freeze);
        }
    }

    private void FreezeEnemies(bool freeze)
    {
        // freeze all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyController>().FreezeEnemy(freeze);
        }
    }

    private void FreezeBullets(bool freeze)
    {
        // freeze all bullets
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            bullet.GetComponent<BulletController>().FreezeBullet(freeze);
        }
    }

    public void PlayerDefeated()
    {
        SetGameState(GameState.GameOver);
        // stop all sounds
        // SoundManager.Instance.Stop();
        // SoundManager.Instance.StopMusic();
        // freeze player and input
        //FreezePlayer(true);
        // freeze all enemies
        //FreezeEnemies(true);
        // remove all bullets
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
        // // remove all explosions
        // GameObject[] explosions = GameObject.FindGameObjectsWithTag("Explosion");
        // foreach (GameObject explosion in explosions)
        // {
        //     Destroy(explosion);
        // }
    }

    public void SetGameState(GameState newState)
    {
        currentGameState = newState;

        switch (currentGameState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                if (pauseMenuUIController != null)
                {
                    pauseMenuUIController.HidePauseMenu();
                }

                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                if (pauseMenuUIController != null)
                {
                    pauseMenuUIController.ShowPauseMenu();
                }

                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                break;
            default:
                break;
        }
    }

    private void DestroyStrayBullets()
    {
        // destroy all bullets that are outside the camera world view
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            if (bullet.transform.position.x < worldViewCoords.Left ||
                bullet.transform.position.x > worldViewCoords.Right ||
                bullet.transform.position.y > worldViewCoords.Top ||
                bullet.transform.position.y < worldViewCoords.Bottom)
            {
                Destroy(bullet);
            }
        }
    }
    
    public void SaveCurrentGame()
    {
        GameObject playerInstance = GameObject.FindGameObjectWithTag("Player");
        
        // freeze player and input
        if (playerInstance != null)
        {
            GameData saveData = new GameData();
            saveData.playerPosition = playerInstance.transform.position;
            
            SaveManager.Instance.SaveGame(saveData);
        }
    }

    public void LoadScene(string sceneName)
    {
        SavePlayerData();
        SceneManager.LoadScene(sceneName);
    }
    
    private void GetWorldViewCoordinates()
    {
        // get camera world coordinates just outside the left-bottom and top-right views
        Vector3 wv0 = Camera.main.ViewportToWorldPoint(new Vector3(-0.1f, -0.1f, 0));
        Vector3 wv1 = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 1.1f, 0));
        worldViewCoords.Left = wv0.x;
        worldViewCoords.Bottom = wv0.y;
        worldViewCoords.Right = wv1.x;
        worldViewCoords.Top = wv1.y;
    }
}