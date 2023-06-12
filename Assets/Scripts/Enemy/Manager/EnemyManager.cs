using Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : Service
{
    [Other]
    private Player player;

    [Other]
    private GameManager gameManager;

    [Other]
    private ObjectManager objectManager;

    [Other]
    private EventSystem eventSystem;

    [HideInInspector]
    public int sortingOrderIncrease;

    private List<Enemy> enemies;
    private List<EnemyGroupTracker> enemyGroupTrackers;
    private Dictionary<EnemyType, EObject> enemyTypeToObjectType;
    private int maxEnemyCount;
    private float playerSafeRadius;
    private Vector2 zone1,
        zone2,
        zone3;
    private float cornerAngle;
    // 用两个Vector2变量来描述一个可空心的矩形表示的范围
    private Dictionary<EnemySpawnZone, Vector2> offsetMins,
        offsetMaxs;

    public static LayerMask enemyLayerMask;
    public static LayerMask enemyColiderLayerMask;
    public EnemyInfoData EnemyInfoData { get; private set; }

    // 测试生成单个敌人用，后续要删
    [Header("测试用敌人信息")]
    public List<EnemyInfo> defaultEnemyInfos;
    private Dictionary<EnemyType, EnemyInfo> defaultEnemyInfoDict;
    private OrbitEnemyGroupInfo testOrbitEnemyGroupInfo;
    private EllipseEnemyGroupInfo testEllipseEnemyGroupInfo;
    private CorkscrewEnemyGroupInfo testCorkscrewEnemyGroupInfo;

    private bool paused;
    public bool Paused
    {
        get => paused;
        set
        {
            if (paused != value)
            {
                if (value)
                {
                    enemyGroupTrackers.ForEach(x => x.PauseAutoSpawn());
                    paused = true;
                }
                else if (gameManager.enemySpawnOn)
                {
                    enemyGroupTrackers.ForEach(x => x.ResumeAutoSpawn());
                    paused = false;
                }
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();

        defaultEnemyInfoDict = new Dictionary<EnemyType, EnemyInfo>();
        foreach (var enemyInfo in defaultEnemyInfos)
        {
            defaultEnemyInfoDict.Add(enemyInfo.enemyType, enemyInfo);
        }
        enemyTypeToObjectType = new Dictionary<EnemyType, EObject>
        {
            { EnemyType.Hand, EObject.Hand },
            { EnemyType.BigEye, EObject.BigEye },
            { EnemyType.SelfExplosionEnemy, EObject.SelfExplosionEnemy },
        };
        enemyLayerMask = LayerMask.GetMask("Enemy");
        enemyColiderLayerMask = LayerMask.GetMask("EnemyCollider", "BossCollider");
        enemies = new List<Enemy>();
        EnemyInfoData = Resources.Load<EnemyInfoData>("EnemyInfoData");
    }

    protected override void Start()
    {
        base.Start();

        sortingOrderIncrease = new SortingOrderTool(
            objectManager.GetPrefabs(enemyTypeToObjectType.Values.ToArray())
        ).orderIncrease;
        LoadAndSetEnemyGroupData();

        eventSystem.AddListener<int>(EEvent.BeforeLoadScene, BeforeLoadScene);
        eventSystem.AddListener<int>(EEvent.AfterLoadScene, AfterLoadScene);
    }

    private void Initialize()
    {
        paused = false;
        enemyGroupTrackers.ForEach(x => x.Initialize());
    }

    private void Update()
    {
        for (int i = 0; i < enemies.Count; )
        {
            if (IsOutOfBufferZone(enemies[i].transform.position))
            {
                enemies[i].DestroySelf();
            }
            else
                i++;
        }
    }

    public void DestroyAllEnemies()
    {
        while (enemies.Count > 0)
        {
            enemies[0].DestroySelf();
        }
    }

    public bool IsOutOfBufferZone(Vector3 position)
    {
        Vector3 relaPos = position - player.transform.position;
        return Mathf.Abs(relaPos.x) > zone3.x || Mathf.Abs(relaPos.y) > zone3.y;
    }

    public void SpawnEnemyGroup(EnemyGroupTracker enemyGroupTracker)
    {
        switch (enemyGroupTracker.enemyGroupType)
        {
            case EnemyGroupType.Normal:
                SpawnNormalEnemyGroup(enemyGroupTracker);
                break;
            case EnemyGroupType.Orbit:
                SpawnOrbitEnemyGroup(enemyGroupTracker.orbitEnemyGroupInfo, enemyGroupTracker);
                break;
            case EnemyGroupType.Ellipse:
                SpawnEllipseEnemyGroup(enemyGroupTracker.ellipseEnemyGroupInfo, enemyGroupTracker);
                break;
            case EnemyGroupType.Corkscrew:
                SpawnCorkscrewEnemyGroup(
                    enemyGroupTracker.corkscrewEnemyGroupInfo,
                    enemyGroupTracker
                );
                break;
        }
        Paused = enemies.Count >= maxEnemyCount;
    }

    private void SpawnNormalEnemyGroup(EnemyGroupTracker enemyGroupTracker)
    {
        EnemyGroupInfo enemyGroupInfo = enemyGroupTracker.enemyGroupInfo;
        for (int j = 0; j < enemyGroupInfo.spawnCount; j++)
        {
            Vector2 groupSpawnRelaPos = GetEnemyGroupSpawnRelaPos(enemyGroupInfo);
            for (int i = 0; i < enemyGroupInfo.enemyIndexList.Count; i++)
            {
                Vector2 spawnRelaPos = groupSpawnRelaPos;
                if (
                    enemyGroupInfo.enemyOffsets != null
                    && i <= enemyGroupInfo.enemyOffsets.Count - 1
                )
                {
                    spawnRelaPos += enemyGroupInfo.enemyOffsets[i];
                }
                if (IsSpawnInZone(spawnRelaPos, enemyGroupInfo.spawnZone))
                    SpawnEnemy(
                        EnemyInfoData.enemyInfos[enemyGroupInfo.enemyIndexList[i]],
                        spawnRelaPos,
                        enemyGroupTracker
                    );
            }
        }
    }

    private bool IsSpawnInZone(Vector2 spawnRelaPos, EnemySpawnZone enemySpawnZone)
    {
        Vector2 offsetMax = offsetMaxs[enemySpawnZone];
        if (Mathf.Abs(spawnRelaPos.x) > offsetMax.x || Mathf.Abs(spawnRelaPos.y) > offsetMax.y)
            return false;
        Vector2 offsetMin = offsetMins[enemySpawnZone];
        if (Mathf.Abs(spawnRelaPos.x) < offsetMin.x && Mathf.Abs(spawnRelaPos.y) < offsetMin.y)
            return false;
        if (
            (enemySpawnZone == EnemySpawnZone.Zone1 || enemySpawnZone == EnemySpawnZone.Both)
            && spawnRelaPos.magnitude < playerSafeRadius
        )
            return false;
        return true;
    }

    public void SpawnTestEnemy(EnemyType enemyType)
    {
        SpawnEnemy(defaultEnemyInfoDict[enemyType], GetEnemyGroupSpawnRelaPos(), null);
    }

    private void SpawnEnemy(
        EnemyInfo enemyInfo,
        Vector3 spawnRelaPos,
        EnemyGroupTracker enemyGroupTracker
    )
    {
        Vector3 spawnPos = player.transform.position + spawnRelaPos;
        CheckAndChangeEnemyCollider(spawnPos);
        Enemy enemy;
        if (!enemyInfo.isBoss)
            enemy = objectManager
                .Activate(enemyTypeToObjectType[enemyInfo.enemyType], spawnPos)
                .Transform.GetComponent<Enemy>();
        else
        {
            EObject Eboss = default;
            if (enemyTypeToObjectType[enemyInfo.enemyType] == EObject.Hand)
                Eboss = EObject.HandBoss;
            else if (enemyTypeToObjectType[enemyInfo.enemyType] == EObject.BigEye)
                Eboss = EObject.BigEyeBoss;
            else
            {
                Debug.LogWarning($"无{enemyTypeToObjectType[enemyInfo.enemyType]}对应的Boss预制体");
                return;
            }
            //Debug.Log(Eboss);
            enemy = objectManager.Activate(Eboss, spawnPos).Transform.GetComponent<Enemy>();
        }
        enemy.Initialize(enemyInfo, enemyGroupTracker);
        enemies.Add(enemy);
        enemyGroupTracker?.AddEnemy(enemy);
    }

    private Vector2 GetEnemyGroupSpawnRelaPos(EnemyGroupInfo enemyGroup = null)
    {
        // 先考虑第一象限，计算生成的角度
        float angle = Random.Range(0f, Mathf.PI / 2);

        EnemySpawnZone spawnZone = enemyGroup == null ? EnemySpawnZone.Zone2 : enemyGroup.spawnZone;
        Vector2 relaPos;
        if (angle < cornerAngle)
        {
            float posX = Random.Range(offsetMins[spawnZone].x, offsetMaxs[spawnZone].x);
            relaPos = new Vector2(posX, posX * Mathf.Tan(angle));
        }
        else
        {
            float posY = Random.Range(offsetMins[spawnZone].y, offsetMaxs[spawnZone].y);
            relaPos = new Vector2(posY / Mathf.Tan(angle), posY);
        }

        // 随机象限
        int quadrant = Random.Range(0, 4);
        if (quadrant >= 2)
            relaPos.y = -relaPos.y;
        if (quadrant == 1 || quadrant == 3)
            relaPos.x = -relaPos.x;

        return relaPos;
    }

    private void SpawnOrbitEnemyGroup(
        OrbitEnemyGroupInfo orbitEnemyGroupInfo,
        EnemyGroupTracker enemyGroupTracker
    )
    {
        float angle = Random.Range(0f, 2 * Mathf.PI);
        Vector3 angleDir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector3 rowSpacingDir = angleDir * orbitEnemyGroupInfo.rowSpacing;
        Vector3 columnSpacingDir =
            new Vector3(-Mathf.Sin(angle), Mathf.Cos(angle)) * orbitEnemyGroupInfo.columnSpacing;
        Vector3 baseSpawnRelaPos =
            angleDir * zone1.x - columnSpacingDir * orbitEnemyGroupInfo.columnCount / 2;
        for (int i = 0; i < orbitEnemyGroupInfo.rowCount; i++)
        {
            Vector3 spawnRelaPos = baseSpawnRelaPos + i * rowSpacingDir;
            for (int j = 0; j < orbitEnemyGroupInfo.columnCount; j++)
            {
                SpawnOrbitEnemy(
                    orbitEnemyGroupInfo.index,
                    spawnRelaPos,
                    enemyGroupTracker,
                    orbitEnemyGroupInfo.moveSpeed,
                    -angleDir
                );
                spawnRelaPos += columnSpacingDir;
            }
        }
    }

    public void SpawnTestOrbitEnemyGroup() => SpawnOrbitEnemyGroup(testOrbitEnemyGroupInfo, null);

    private void SpawnEllipseEnemyGroup(
        EllipseEnemyGroupInfo ellipseEnemyGroupInfo,
        EnemyGroupTracker enemyGroupTracker
    )
    {
        for (int k = 0; k < ellipseEnemyGroupInfo.circleCount; k++)
        {
            // 计算椭圆长短轴与周长以计算生成个数
            float a =
                ellipseEnemyGroupInfo.semiLength
                + ellipseEnemyGroupInfo.circleSpacing * 9f / 16f * k;
            float b = a * 9f / 16f;
            float girth = 2f * Mathf.PI * b + 4 * (a - b);
            int count = Mathf.FloorToInt(girth) / 4;
            for (int i = 1; i < count; i++)
            {
                // 建立了个数与角度的简单模型，可以基本实现需求
                float theta = Mathf.PI / 2 * Mathf.Pow((float)i / count, 1.25f);
                float r =
                    a
                    * b
                    / Mathf.Sqrt(
                        b * b * Mathf.Cos(theta) * Mathf.Cos(theta)
                            + a * a * Mathf.Sin(theta) * Mathf.Sin(theta)
                    );
                float x = r * Mathf.Cos(theta);
                float y = r * Mathf.Sin(theta);
                SpawnEllipseEnemy(
                    ellipseEnemyGroupInfo.index,
                    new Vector3(x, y),
                    enemyGroupTracker,
                    ellipseEnemyGroupInfo.moveSpeed
                );
                SpawnEllipseEnemy(
                    ellipseEnemyGroupInfo.index,
                    new Vector3(x, -y),
                    enemyGroupTracker,
                    ellipseEnemyGroupInfo.moveSpeed
                );
                SpawnEllipseEnemy(
                    ellipseEnemyGroupInfo.index,
                    new Vector3(-x, y),
                    enemyGroupTracker,
                    ellipseEnemyGroupInfo.moveSpeed
                );
                SpawnEllipseEnemy(
                    ellipseEnemyGroupInfo.index,
                    new Vector3(-x, -y),
                    enemyGroupTracker,
                    ellipseEnemyGroupInfo.moveSpeed
                );
            }
            SpawnEllipseEnemy(
                ellipseEnemyGroupInfo.index,
                new Vector3(a, 0),
                enemyGroupTracker,
                ellipseEnemyGroupInfo.moveSpeed
            );
            SpawnEllipseEnemy(
                ellipseEnemyGroupInfo.index,
                new Vector3(-a, 0),
                enemyGroupTracker,
                ellipseEnemyGroupInfo.moveSpeed
            );
            SpawnEllipseEnemy(
                ellipseEnemyGroupInfo.index,
                new Vector3(0, b),
                enemyGroupTracker,
                ellipseEnemyGroupInfo.moveSpeed
            );
            SpawnEllipseEnemy(
                ellipseEnemyGroupInfo.index,
                new Vector3(0, -b),
                enemyGroupTracker,
                ellipseEnemyGroupInfo.moveSpeed
            );
        }
    }

    public void SpawnTestEllipseEnemyGroup() =>
        SpawnEllipseEnemyGroup(testEllipseEnemyGroupInfo, null);

    private void SpawnCorkscrewEnemyGroup(
        CorkscrewEnemyGroupInfo corkscrewEnemyGroupInfo,
        EnemyGroupTracker enemyGroupTracker
    )
    {
        // 阿基米德螺旋线方程参数
        float a = corkscrewEnemyGroupInfo.originalDist;
        float b = corkscrewEnemyGroupInfo.circleSpacing / (2 * Mathf.PI);
        float totalAngleRad = corkscrewEnemyGroupInfo.totalAngle * Mathf.Deg2Rad;
        float theta = 0f;
        while (theta < totalAngleRad)
        {
            float rho = a + b * theta;
            SpawnCorkscrewEnemy(
                corkscrewEnemyGroupInfo.index,
                new Vector3(rho * Mathf.Cos(theta), rho * Mathf.Sin(theta)),
                enemyGroupTracker,
                corkscrewEnemyGroupInfo
            );
            // 近似模型，可以基本实现需求
            float deltaTheta = 1 / (a + 3 * b * theta);
            theta += deltaTheta;
        }
    }

    public void SpawnTestCorkscrewEnemyGroup() =>
        SpawnCorkscrewEnemyGroup(testCorkscrewEnemyGroupInfo, null);

    private void SpawnOrbitEnemy(
        int enemyIndex,
        Vector3 spawnRelaPos,
        EnemyGroupTracker enemyGroupTracker,
        float moveSpeed,
        Vector2 moveDir
    )
    {
        OrbitEnemy enemy = objectManager
            .Activate(EObject.OrbitEnemy, player.transform.position + spawnRelaPos)
            .Transform.GetComponent<OrbitEnemy>();
        enemy.Initialize(
            EnemyInfoData.specialEnemyInfos[enemyIndex],
            enemyGroupTracker,
            moveSpeed,
            moveDir
        );
        enemies.Add(enemy);
        enemyGroupTracker?.AddEnemy(enemy);
    }

    private void SpawnEllipseEnemy(
        int enemyIndex,
        Vector3 spawnRelaPos,
        EnemyGroupTracker enemyGroupTracker,
        float moveSpeed
    ) => SpawnOrbitEnemy(enemyIndex, spawnRelaPos, enemyGroupTracker, moveSpeed, -spawnRelaPos);

    private void SpawnCorkscrewEnemy(
        int enemyIndex,
        Vector3 spawnRelaPos,
        EnemyGroupTracker enemyGroupTracker,
        CorkscrewEnemyGroupInfo corkscrewEnemyGroupInfo
    )
    {
        CorkscrewEnemy enemy = objectManager
            .Activate(EObject.CorkscrewEnemy, player.transform.position + spawnRelaPos)
            .Transform.GetComponent<CorkscrewEnemy>();
        enemy.Initialize(
            EnemyInfoData.specialEnemyInfos[enemyIndex],
            enemyGroupTracker,
            corkscrewEnemyGroupInfo,
            player.transform.position
        );
        enemies.Add(enemy);
        enemyGroupTracker?.AddEnemy(enemy);
    }

    public static void CheckAndChangeEnemyCollider(Vector3 position, float radius = 0.01f)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            position,
            radius,
            enemyColiderLayerMask
        );
        if (colliders.Length > 0)
        {
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<EnemyCollider>(out var enemyCollider))
                {
                    enemyCollider.EnableCollider(true);
                }
            }
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        Paused = enemies.Count >= maxEnemyCount;
    }

    public Enemy FindNearestEnemyToPlayer(float distance)
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(
            player.transform.position,
            distance,
            enemyLayerMask
        );
        if (enemyColliders.Length == 0)
            return null;

        Enemy nearestEnemy = enemyColliders[0].GetComponentInParent<Enemy>();
        float minSqrDist = (
            nearestEnemy.transform.position - player.transform.position
        ).sqrMagnitude;
        for (int i = 1; i < enemyColliders.Length; i++)
        {
            Enemy enemy = enemyColliders[i].GetComponentInParent<Enemy>();
            float sqrDist = (enemy.transform.position - player.transform.position).sqrMagnitude;
            if (sqrDist < minSqrDist)
            {
                minSqrDist = sqrDist;
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }

    public List<Enemy> FindRandomEnemies(Vector3 centerPos, float distance, int count)
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(
            centerPos,
            distance,
            enemyLayerMask
        );
        return enemyColliders.RandomPick(count).ConvertAll(x => x.GetComponentInParent<Enemy>());
    }

    private void LoadAndSetEnemyGroupData()
    {
        EnemyGroupData enemyGroupData = Resources.Load<EnemyGroupData>("EnemyGroupData");
        maxEnemyCount = enemyGroupData.maxEnemyCount;
        playerSafeRadius = enemyGroupData.playerSafeRadius;
        enemyGroupTrackers = new List<EnemyGroupTracker>();

        // 正常敌群
        for (int i = 0; i < enemyGroupData.enemyGroupConfigInfos.Count; i++)
        {
            EnemyGroupConfigInfo enemyGroupConfigInfo = enemyGroupData.enemyGroupConfigInfos[i];
            if (
                enemyGroupConfigInfo.groupInfoIndex < 0
                || enemyGroupConfigInfo.groupInfoIndex >= enemyGroupData.enemyGroupInfos.Count
            )
            {
                Debug.LogError($"EnemyGroupData中的敌群配置合集中索引为{i}的敌群配置的敌群索引超出范围");
            }
            if (enemyGroupConfigInfo.configOn)
                enemyGroupTrackers.Add(
                    new EnemyGroupTracker(
                        enemyGroupData.enemyGroupInfos[enemyGroupConfigInfo.groupInfoIndex],
                        enemyGroupConfigInfo,
                        gameManager.gameTimer,
                        this
                    )
                );
        }
        // 判断EnemyGroupInfos中数据的正确性
        for (int i = 0; i < enemyGroupData.enemyGroupInfos.Count; i++)
        {
            EnemyGroupInfo enemyGroup = enemyGroupData.enemyGroupInfos[i];

            foreach (var enemyIndex in enemyGroup.enemyIndexList)
            {
                if (enemyIndex < 0 || enemyIndex >= EnemyInfoData.enemyInfos.Count)
                {
                    Debug.LogError($"EnemyGroupData中的敌群描述信息表中索引为{i}的敌群信息中有敌人索引超出范围");
                }
            }
        }

        // 轨道敌群
        for (int i = 0; i < enemyGroupData.orbitEnemyGroupConfigInfos.Count; i++)
        {
            EnemyGroupConfigInfo enemyGroupConfigInfo = enemyGroupData.orbitEnemyGroupConfigInfos[
                i
            ];
            if (
                enemyGroupConfigInfo.groupInfoIndex < 0
                || enemyGroupConfigInfo.groupInfoIndex >= enemyGroupData.orbitEnemyGroupInfos.Count
            )
            {
                Debug.LogError($"EnemyGroupData中的轨道敌群配置合集中索引为{i}的轨道敌群配置的敌群索引超出范围");
            }
            if (enemyGroupConfigInfo.configOn)
                enemyGroupTrackers.Add(
                    new EnemyGroupTracker(
                        enemyGroupData.orbitEnemyGroupInfos[enemyGroupConfigInfo.groupInfoIndex],
                        enemyGroupConfigInfo,
                        gameManager.gameTimer,
                        this
                    )
                );
        }
        // 判断EnemyGroupInfos中数据的正确性
        if (enemyGroupData.orbitEnemyGroupInfos.Count == 0)
            Debug.LogError($"EnemyGroupData中的轨道敌群描述信息表为空，因此将无法生成测试用的轨道敌群");
        testOrbitEnemyGroupInfo = enemyGroupData.orbitEnemyGroupInfos[0];
        for (int i = 0; i < enemyGroupData.orbitEnemyGroupInfos.Count; i++)
        {
            int enemyIndex = enemyGroupData.orbitEnemyGroupInfos[i].index;
            if (enemyIndex < 0 || enemyIndex >= EnemyInfoData.specialEnemyInfos.Count)
            {
                Debug.LogError($"EnemyGroupData中的轨道敌群描述信息表中索引为{i}的轨道敌群信息的特殊敌人索引超出范围");
            }
            if (
                Mathf.Abs(
                    Mathf.Min(
                        enemyGroupData.orbitEnemyGroupInfos[i].rowSpacing,
                        enemyGroupData.orbitEnemyGroupInfos[i].columnSpacing
                    )
                ) < 0.709f
            )
            {
                Debug.LogWarning($"EnemyGroupData中的轨道敌群描述信息表中索引为{i}的轨道敌群信息的敌群间隔过小");
            }
        }

        // 椭圆敌群
        for (int i = 0; i < enemyGroupData.ellipseEnemyGroupConfigInfos.Count; i++)
        {
            EnemyGroupConfigInfo enemyGroupConfigInfo = enemyGroupData.ellipseEnemyGroupConfigInfos[
                i
            ];
            if (
                enemyGroupConfigInfo.groupInfoIndex < 0
                || enemyGroupConfigInfo.groupInfoIndex
                    >= enemyGroupData.ellipseEnemyGroupInfos.Count
            )
            {
                Debug.LogError($"EnemyGroupData中的椭圆敌群配置合集中索引为{i}的椭圆敌群配置的敌群索引超出范围");
            }
            if (enemyGroupConfigInfo.configOn)
                enemyGroupTrackers.Add(
                    new EnemyGroupTracker(
                        enemyGroupData.ellipseEnemyGroupInfos[enemyGroupConfigInfo.groupInfoIndex],
                        enemyGroupConfigInfo,
                        gameManager.gameTimer,
                        this
                    )
                );
        }
        // 判断EnemyGroupInfos中数据的正确性
        if (enemyGroupData.ellipseEnemyGroupInfos.Count == 0)
            Debug.LogError($"EnemyGroupData中的椭圆敌群描述信息表为空，因此将无法生成测试用的椭圆敌群");
        testEllipseEnemyGroupInfo = enemyGroupData.ellipseEnemyGroupInfos[0];
        for (int i = 0; i < enemyGroupData.ellipseEnemyGroupInfos.Count; i++)
        {
            int enemyIndex = enemyGroupData.ellipseEnemyGroupInfos[i].index;
            if (enemyIndex < 0 || enemyIndex >= EnemyInfoData.specialEnemyInfos.Count)
            {
                Debug.LogError($"EnemyGroupData中的椭圆敌群描述信息表中索引为{i}的椭圆敌群信息的特殊敌人索引超出范围");
            }
            if (Mathf.Abs(enemyGroupData.ellipseEnemyGroupInfos[i].circleSpacing) < 0.71)
            {
                Debug.LogWarning($"EnemyGroupData中的椭圆敌群描述信息表中索引为{i}的椭圆敌群信息的敌群间隔过小");
            }
        }

        // 螺旋敌群
        for (int i = 0; i < enemyGroupData.corkscrewEnemyGroupConfigInfos.Count; i++)
        {
            EnemyGroupConfigInfo enemyGroupConfigInfo =
                enemyGroupData.corkscrewEnemyGroupConfigInfos[i];
            if (
                enemyGroupConfigInfo.groupInfoIndex < 0
                || enemyGroupConfigInfo.groupInfoIndex
                    >= enemyGroupData.corkscrewEnemyGroupInfos.Count
            )
            {
                Debug.LogError($"EnemyGroupData中的螺旋敌群配置合集中索引为{i}的螺旋敌群配置的敌群索引超出范围");
            }
            if (enemyGroupConfigInfo.configOn)
                enemyGroupTrackers.Add(
                    new EnemyGroupTracker(
                        enemyGroupData.corkscrewEnemyGroupInfos[
                            enemyGroupConfigInfo.groupInfoIndex
                        ],
                        enemyGroupConfigInfo,
                        gameManager.gameTimer,
                        this
                    )
                );
        }
        // 判断EnemyGroupInfos中数据的正确性
        if (enemyGroupData.corkscrewEnemyGroupInfos.Count == 0)
            Debug.LogError($"EnemyGroupData中的螺旋敌群描述信息表为空，因此将无法生成测试用的螺旋敌群");
        testCorkscrewEnemyGroupInfo = enemyGroupData.corkscrewEnemyGroupInfos[0];
        for (int i = 0; i < enemyGroupData.corkscrewEnemyGroupInfos.Count; i++)
        {
            int enemyIndex = enemyGroupData.corkscrewEnemyGroupInfos[i].index;
            if (enemyIndex < 0 || enemyIndex >= EnemyInfoData.specialEnemyInfos.Count)
            {
                Debug.LogError($"EnemyGroupData中的螺旋敌群描述信息表中索引为{i}的螺旋敌群信息的特殊敌人索引超出范围");
            }
        }

        zone1 = Camera.main.ViewportToWorldPoint(Vector2.one) + new Vector3(0.5f, 0.5f);
        zone2 =
            zone1
            + new Vector2(
                enemyGroupData.generateZoneIncreasement,
                enemyGroupData.generateZoneIncreasement * zone1.y / zone1.x
            );
        zone3 =
            zone2
            + new Vector2(
                enemyGroupData.bufferZoneIncreasement,
                enemyGroupData.bufferZoneIncreasement * zone1.y / zone1.x
            );
        //print($"screen size = {Camera.main.ViewportToWorldPoint(Vector2.one)}");
        //print($"zone1 size = {zone1}");
        //print($"zone2 size = {zone2}");
        //print($"zone3 size = {zone3}");

        cornerAngle = Mathf.Atan2(zone1.y, zone1.x);

        offsetMins = new Dictionary<EnemySpawnZone, Vector2>
        {
            { EnemySpawnZone.Zone1, Vector2.zero },
            { EnemySpawnZone.Zone2, zone1 },
            { EnemySpawnZone.Both, Vector2.zero },
        };
        offsetMaxs = new Dictionary<EnemySpawnZone, Vector2>
        {
            { EnemySpawnZone.Zone1, zone1 },
            { EnemySpawnZone.Zone2, zone2 },
            { EnemySpawnZone.Both, zone2 },
        };
    }

    private void BeforeLoadScene(int _)
    {
        enemyGroupTrackers.ForEach(x => x.Reset());
        enemies.Clear();
    }

    private void AfterLoadScene(int _)
    {
        if (SceneController.InGame)
        {
            Initialize();
        }
    }
}
