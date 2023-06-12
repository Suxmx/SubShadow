using MyTimer;
using System.Collections.Generic;

public enum EnemyGroupType
{
    Normal,
    Orbit,
    Ellipse,
    Corkscrew,
}

public class EnemyGroupTracker
{
    private readonly EnemyGroupConfigInfo enemyGroupConfigInfo;
    private readonly TimerOnly gameTimer;
    private readonly EnemyManager enemyManager;

    public readonly EnemyGroupType enemyGroupType;
    public readonly int expMultiplier;
    public readonly EnemyGroupInfo enemyGroupInfo;
    public readonly OrbitEnemyGroupInfo orbitEnemyGroupInfo;
    public readonly EllipseEnemyGroupInfo ellipseEnemyGroupInfo;
    public readonly CorkscrewEnemyGroupInfo corkscrewEnemyGroupInfo;

    private bool running;
    private readonly List<Enemy> enemies;
    private readonly Repeation<float, CurrentTime> spawnTimer;

    public EnemyGroupTracker(EnemyGroupConfigInfo enemyGroupConfigInfo, TimerOnly gameTimer, 
        EnemyManager enemyManager)
    {
        this.enemyGroupConfigInfo = enemyGroupConfigInfo;
        this.gameTimer = gameTimer;
        this.enemyManager = enemyManager;
        running = false;
        enemies = new List<Enemy>();
        spawnTimer = new Repeation<float, CurrentTime>();
        spawnTimer.OnComplete += SpawnEnemyGroup;
    }

    public EnemyGroupTracker(EnemyGroupInfo enemyGroupInfo, EnemyGroupConfigInfo enemyGroupConfigInfo, 
        TimerOnly gameTimer, EnemyManager enemyManager) 
        : this(enemyGroupConfigInfo, gameTimer, enemyManager)
    {
        enemyGroupType = EnemyGroupType.Normal;
        this.enemyGroupInfo = enemyGroupInfo;
        expMultiplier = enemyGroupInfo.expMultiplier;
        spawnTimer.Initialize(0f, enemyGroupInfo.spawnInterval, enemyGroupInfo.spawnInterval, false);
    }

    public EnemyGroupTracker(OrbitEnemyGroupInfo orbitEnemyGroupInfo, EnemyGroupConfigInfo enemyGroupConfigInfo, 
        TimerOnly gameTimer, EnemyManager enemyManager) 
        : this(enemyGroupConfigInfo, gameTimer, enemyManager)
    {
        enemyGroupType = EnemyGroupType.Orbit;
        this.orbitEnemyGroupInfo = orbitEnemyGroupInfo;
        expMultiplier = orbitEnemyGroupInfo.expMultiplier;
        spawnTimer.Initialize(0f, orbitEnemyGroupInfo.spawnInterval, orbitEnemyGroupInfo.spawnInterval, false);
    }

    public EnemyGroupTracker(EllipseEnemyGroupInfo ellipseEnemyGroupInfo, EnemyGroupConfigInfo enemyGroupConfigInfo, 
        TimerOnly gameTimer, EnemyManager enemyManager) 
        : this(enemyGroupConfigInfo, gameTimer, enemyManager)
    {
        enemyGroupType = EnemyGroupType.Ellipse;
        this.ellipseEnemyGroupInfo = ellipseEnemyGroupInfo;
        expMultiplier = ellipseEnemyGroupInfo.expMultiplier;
        spawnTimer.Initialize(0f, ellipseEnemyGroupInfo.spawnInterval, ellipseEnemyGroupInfo.spawnInterval, false);
    }

    public EnemyGroupTracker(CorkscrewEnemyGroupInfo corkscrewEnemyGroupInfo, EnemyGroupConfigInfo enemyGroupConfigInfo, 
        TimerOnly gameTimer, EnemyManager enemyManager) 
        : this(enemyGroupConfigInfo, gameTimer, enemyManager)
    {
        enemyGroupType = EnemyGroupType.Corkscrew;
        this.corkscrewEnemyGroupInfo = corkscrewEnemyGroupInfo;
        expMultiplier = corkscrewEnemyGroupInfo.expMultiplier;
        spawnTimer.Initialize(0f, corkscrewEnemyGroupInfo.spawnInterval, corkscrewEnemyGroupInfo.spawnInterval, false);
    }

    public void Initialize()
    {
        gameTimer.OnTick += CheckStart;
    }

    private void CheckStart(float current)
    {
        if (current >= enemyGroupConfigInfo.startMoment)
        {
            running = true;
            gameTimer.OnTick -= CheckStart;
            gameTimer.OnTick += CheckEnd;
            if (!enemyManager.Paused)
            {
                SpawnEnemyGroup();
                spawnTimer.Restart();
            }
        }
    }

    private void CheckEnd(float current)
    {
        if (current >= enemyGroupConfigInfo.endMoment)
        {
            spawnTimer.Paused = true;
            running = false;
            DestroyAllEnemies();
            gameTimer.OnTick -= CheckEnd;
        }
    }

    private void SpawnEnemyGroup()
    {
        enemyManager.SpawnEnemyGroup(this);
    }

    private void DestroyAllEnemies()
    {
        while (enemies.Count > 0)
        {
            enemies[0].DestroySelf();
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    public void PauseAutoSpawn()
    {
        spawnTimer.Paused = true;
    }

    public void ResumeAutoSpawn()
    {
        if (running) spawnTimer.Paused = false;
    }

    public void Reset()
    {
        enemies.Clear();
        spawnTimer.ReturnToZero();
        running = false;
    }
}
