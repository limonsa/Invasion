using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    [Header("Game Entities")]
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject[] pickpuPrefabs;
    [SerializeField] private Transform[] spawnPositions;
    private int activeSpawnPoints = 5; // Set the initial active spawn points


    [Header("Game Variables")] //things you can change
    [SerializeField] private float enemySpawnRate;
    [SerializeField] private float nukeSpawnRate = 0.2f;
    [SerializeField] private float powerUpSpawnRate = 0.2f;

    public ScoreManager scoreManager;
    public PickupSpawner pickupSpawner;

    private Player player;
    private GameObject tempEnemy;
    private GameObject tempNuke;
    private bool isEnemySpawning;       //Also supports the spawning of Nukes and PowerUps
    private bool isGameRunning;
    private Stack<GameObject> stkEnemysHolder;    //Supports the destruction caused by the Nuke explosion
    private Stack<GameObject> stkNukesHolder;   //Supports the destruction caused by the Nuke explosion
    private Stack<GameObject> stkPowerUpsHolder;   //Supports the destruction caused by the Nuke explosion
    private Stack<GameObject> stkHealthsHolder;
    private Weapon meleeWeapon = new Weapon("Melee", 1, 0);

    private static GameManager _instance;
    private UIManager uiManager;

    public Action OnGameStart;
    public Action OnGameEnd;

    private bool turn = false;
    private void Awake()
    {
        SetSingleton();
    }

    void SetSingleton()
    {
        //If there is a pre-existing instance that isn't this one, destroy it
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);

        _instance = this;
    }

    public bool IsGameRunning()
    {
        return isGameRunning;
    }

    public static GameManager GetInstance()
    {
        //Creates a public instance, since our GameManager is private
        //So other scripts can call elements of this instance
        return _instance;
    }

    void Start()
    {
        FindPlayer();

        isEnemySpawning = true;
        StartCoroutine(EnemySpawner());
        //Spawns Nukes
        //TODO: StartCoroutine(NukeSpawner());
        //Spawns Gun Power-ups
        //TODO: StartCoroutine(PowerUpSpawner());
        //Spawn Health PickUp
        StartCoroutine(HealthSpawner());

        //Subcribes to the action that receives the order to explote a nuke
        PlayerInput.ExploteNuke += UseNuke;

        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        stkEnemysHolder = new Stack<GameObject>();
        stkNukesHolder = new Stack<GameObject>();
        stkPowerUpsHolder = new Stack<GameObject>();
        stkHealthsHolder = new Stack<GameObject>();
    }

    void Update()
    {
        /*if (scoreManager.score >= 100 && activeSpawnPoints < 7)
        {
            activeSpawnPoints++;
        }
        else if (scoreManager.score >= 50 && activeSpawnPoints < 6)
        {
            activeSpawnPoints++;
        }

        //adding score to player to make this simple
        player.Score = scoreManager.score;*/
    }

    public void CreateEnemy()
    {
        //Create reference to GameObject itself
        //Create a holder? => yes, created to support the Nuke explosion
        tempEnemy = Instantiate(enemyPrefab[UnityEngine.Random.Range(0, enemyPrefab.Length)]);
        tempEnemy.transform.position = spawnPositions[UnityEngine.Random.Range(0, Math.Min(activeSpawnPoints, spawnPositions.Length))].position;
        tempEnemy.GetComponent<Enemy>().weapon = meleeWeapon;
        //tempEnemy.GetComponent<MeleeEnemy>().SetMeleeEnemy(2, 0.25f);

        if (tempEnemy.GetComponent<MeleeEnemy>() != null)
        {
            tempEnemy.GetComponent<MeleeEnemy>().SetMeleeEnemy(1, 0.5f);
        }

        if (tempEnemy.GetComponent<MachineGunEnemy>() != null)
        {
            tempEnemy.GetComponent<MachineGunEnemy>().SetMachineGunEnemy(0.5f, 1, 30f);
        }
        stkEnemysHolder.Push(tempEnemy);
    }

    IEnumerator EnemySpawner()
    {
        while (isEnemySpawning)
        {
            /*if (scoreManager.score >= 100) // Phase 3
            {
                enemySpawnRate = 1.0f; // 100% faster enemy spawning when score exceeds 100
                //add implementation for changing visuals and music
            }

            else if (scoreManager.score >= 50) // Phase 2
            {
                enemySpawnRate = 0.75f; // 50% Faster enemy spawning when score exceeds 50
                                        //add implementation for changing visuals and music
            }*/

            yield return new WaitForSeconds(1.0f / enemySpawnRate);
            CreateEnemy();
        }
    }

    public void NotifyDeath(Enemy enemy)
    {
        //pickupSpawner.SpawnPickups(enemy.transform.position);
        if (turn) {
            CreateNuke(enemy.transform.position);
        }
        else {
            CreatePowerUp(enemy.transform.position);
        }
        turn = !turn;
    }

    public void FindPlayer()
    {
        try
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }
        catch (NullReferenceException e)
        {
            Debug.Log("Player not found " + e.Message);
        }
    }

    public Player GetPlayer() { return player; }

    IEnumerator NukeSpawner()
    {
        while (isEnemySpawning)
        {
            yield return new WaitForSeconds(1.0f / nukeSpawnRate);
            CreateNuke();
        }
    }

    /// <summary>
    /// Creates new Nuke pickup
    /// </summary>
    public void CreateNuke()
    {
        tempNuke = Instantiate(pickpuPrefabs[(int)Pickup.Types.Nuke]);
        tempNuke.transform.position = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Length)].position;
        //tempNuke.GetComponent<Nuke>().DisplayInConsole("Hello, I am Nuke");
        stkNukesHolder.Push(tempNuke);
    }
    public void CreateNuke(Vector3 pos)
    {
        tempNuke = Instantiate(pickpuPrefabs[(int)Pickup.Types.Nuke]);
        tempNuke.transform.position = pos;
        //tempNuke.GetComponent<Nuke>().DisplayInConsole("Hello, I am Nuke");
        stkNukesHolder.Push(tempNuke);
    }

    IEnumerator PowerUpSpawner()
    {
        while (isEnemySpawning)
        {
            yield return new WaitForSeconds(1.0f / powerUpSpawnRate);
            CreatePowerUp();
        }
    }

    IEnumerator HealthSpawner()
    {
        while (isEnemySpawning)
        {
            yield return new WaitForSeconds(1.0f / powerUpSpawnRate);
            CreateHealth();
        }
    }


    /// <summary>
    /// Creates new Gun power-up pickup
    /// </summary>
    public void CreatePowerUp()
    {
        tempNuke = Instantiate(pickpuPrefabs[(int)Pickup.Types.PowerUp]);
        tempNuke.transform.position = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Length)].position;
        //tempNuke.GetComponent<PowerUp>().DisplayInConsole("Hello, I am PowerUp");
        stkPowerUpsHolder.Push(tempNuke);
    }
    public void CreatePowerUp(Vector3 pos)
    {
        tempNuke = Instantiate(pickpuPrefabs[(int)Pickup.Types.PowerUp]);
        tempNuke.transform.position = pos;
        //tempNuke.GetComponent<PowerUp>().DisplayInConsole("Hello, I am PowerUp");
        stkPowerUpsHolder.Push(tempNuke);
    }

    public void CreateHealth()
    {
        tempNuke = Instantiate(pickpuPrefabs[(int)Pickup.Types.Health]);
        tempNuke.transform.position = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Length)].position;
        //tempNuke.GetComponent<PowerUp>().DisplayInConsole("Hello, I am PowerUp");
        stkHealthsHolder.Push(tempNuke);
    }
    public void CreateHealth(Vector3 pos)
    {
        tempNuke = Instantiate(pickpuPrefabs[(int)Pickup.Types.Health]);
        tempNuke.transform.position = pos;
        //tempNuke.GetComponent<PowerUp>().DisplayInConsole("Hello, I am PowerUp");
        stkHealthsHolder.Push(tempNuke);
    }

    /// <summary>
    /// When colliding with the player a nuke is picked up
    /// and the UI is updated showing another nuke ready to be used
    /// </summary>
    public void LoadNuke()
    {
        //Debug.Log($"GAME MANAGER SAYS >>> LoadNuke will add nukePrefab to {uiManager.ToString()}");
        uiManager.LoadNuke(pickpuPrefabs[(int)Pickup.Types.Nuke].GetComponentInChildren<SpriteRenderer>(true));
    }

    /// <summary>
    /// If available, uses a nuke and destroys all the entities in the scene (including any pickups)
    /// the UI is updated showing one less nuke ready to be used
    /// </summary>
    private void UseNuke()
    {
        if (uiManager.CountAvailableNukes() > 0)
        {
            //Debug.Log($"GAME MANAGER SAYS >>> A nuke will be used destroying {stkEnemysHolder.Count} enemies, {stkNukesHolder.Count} nukes and {stkPowerUpsHolder.Count} power-ups");
            for (int i = 0; i < stkEnemysHolder.Count; i++)
            {
                Destroy(stkEnemysHolder.Pop());
            }
            for (int i = 0; i < stkNukesHolder.Count; i++)
            {
                Destroy(stkNukesHolder.Pop());
            }
            for (int i = 0; i < stkPowerUpsHolder.Count; i++)
            {
                Destroy(stkPowerUpsHolder.Pop());
            }
            uiManager.UseNuke();
        }
    }
}