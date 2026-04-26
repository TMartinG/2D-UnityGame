using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy_Base : MonoBehaviour
{
    public  short enemy_HP;
    public  short enemy_SP;
    public string uniqueID;

    public EnemyType enemy_Type;
    public EnemyState enemy_State;
    [SerializeField] protected DebuffType[] enemy_Debuffs;
    public float keepDistance;
    public bool useAttackState = true;

    [Header("Fire Debuff")]
    [SerializeField] float fireBuildUp = 0f;
    [SerializeField] float fireBuildUpThreshold = 3f;

    [SerializeField] float fireDuration = 0f;
    [SerializeField] float maxFireDuration = 5f;

    [SerializeField] float fireTickInterval = 1f;
    [SerializeField] short fireDamage = 2;
    [SerializeField] float coolSpeed = 0.5f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private Color originalColor = Color.white;
    [SerializeField] private Color burnColor = Color.red;

    private float visualHeat;

    bool isBurning = false;
    float fireTickTimer = 0f;


    [Header("Freeze Debuff")]
    [SerializeField] float freezeBuildUp = 0f;
    [SerializeField] float freezeThreshold = 8f;
    [SerializeField] private Color freezeColor = Color.cyan;

    public float slowMultiplier = 1f; // 1 = nincs lassítás, 0 = full stop
    [SerializeField] float freezeDuration = 3f;
    float freezeTimer = 0f;
    private float visualFreeze;

    bool isFrozen = false;
    bool isSlowed = false;

    [SerializeField] float slowLerpSpeed = 2f;
    [SerializeField] float freezeCoolSpeed = 1f;
    [SerializeField] float freezeResetDelay = 3f;

    float lastFreezeTime;
    public System.Action OnDeath;
    public bool isDead = false;




    [Header("Patrol State")]
    public EnemyVision vision;
    //Patrol State
    public float patrolRadius; //4
    public float patrolChangeInterval; //3


    [Header("Attack State")]
    //Attack State
    public float attackLostSightTimer; //0
    public float attackLostSightDelay; //2
    public float attackAttackRange;


    [Header("Chase State")]
    //Attack State
    public float chaseLostSightTimer; //0
    public float chaseLostSightDelay; //2
    public float chaseAttackRange;

    public IMovement movement;
    public IAttack attack;
    public Transform player;
    public EnemyStateMachine stateMachine;

    public Vector2 lastSeenPosition;
    public bool hasLastSeenPosition = false;

    // STATES
    public IdleState idleState;
    public ChaseState chaseState;
    public AttackState attackState;
    public PatrolState patrolState;
    public SearchState searchState;

    [Header("Drops")]
    public bool dropsYellow;
    public float dropChance = 0.5f;

    [SerializeField] GameObject healthPrefab;
    public float healthDropChance = 0.3f; // 30%

    [Header("Animation")]
    public Animator animator;

    void Awake()
    {
        uniqueID = gameObject.name + "_" + transform.position.ToString();
    }

    public virtual void Die()
    {
        if (isDead) return;

        isDead = true;

        WorldStateManager.Instance.RegisterDead(uniqueID);

        OnDeath?.Invoke();
        Destroy(gameObject);
        DropSkill();
        DropHealthPickup();
    }

    public void ApplyState(short maxHP)
    {
        if (WorldStateManager.Instance.IsDead(uniqueID))
        {
            Destroy(gameObject);
            return;
        }

        enemy_HP = WorldStateManager.Instance.GetHealth(uniqueID, maxHP);
    }

    public virtual void GetDamaged(short amount, Vector2 hitPosition)
    {
         if (isFrozen)
        {
            amount *= 2;
        }

        enemy_HP -= amount;

        WorldStateManager.Instance.SetHealth(uniqueID, enemy_HP);

        OnHit(hitPosition);
        //Debug.Log(enemy_HP);
        if (enemy_HP <= 0)
        {
            Die();
        }
    }
    public virtual void GetDamaged(short amount)
    {
         if (isFrozen)
        {
            
            amount *= 2;
        }

        WorldStateManager.Instance.SetHealth(uniqueID, enemy_HP);

        enemy_HP -= amount;
        //Debug.Log(enemy_HP);
        if (enemy_HP <= 0)
        {
            Die();
        }
    }
    public void OnHit(Vector2 hitPosition)
    {
        lastSeenPosition = hitPosition;
        hasLastSeenPosition = true;

        stateMachine.ChangeState(searchState);
    }
    void DropSkill()
    {
        if (Random.value > dropChance) return;

        if (Random.value > 0.5f)        {
            dropsYellow = true;
        }
        else
        {
            dropsYellow = false;
        }

        SkillBase skill = SkillManager.Instance.GetRandomDrop(dropsYellow);

        if (skill.pickupPrefab == null)
        {
            //Debug.LogWarning("Nincs prefab hozzárendelve: " + skill.skillName);
            return;
        }

        GameObject obj = Instantiate(
            skill.pickupPrefab,
            transform.position,
            Quaternion.identity,
            transform.parent
        );
        SkillPickUp pickup = obj.GetComponent<SkillPickUp>();
        pickup.skill = skill;
    }

    public void DropHealthPickup()
    {
        if (healthPrefab == null) return;

        float roll = Random.value;

        if (roll <= healthDropChance)
        {
            Instantiate(healthPrefab, transform.position, Quaternion.identity, transform.parent);
        }
    }

    protected virtual void Start()
    {
        if (WorldStateManager.Instance.IsDead(uniqueID))
        {
            Destroy(gameObject);
            return;
        }

        enemy_HP = WorldStateManager.Instance.GetHealth(uniqueID, enemy_HP);


        player = GameObject.FindGameObjectWithTag("Player").transform;

        movement = GetComponent<IMovement>();
        attack = GetComponent<IAttack>();
        animator = GetComponent<Animator>();

        stateMachine = new EnemyStateMachine();

        idleState = new IdleState(this);
        chaseState = new ChaseState(this);
        attackState = new AttackState(this);
        patrolState = new PatrolState(this);
        searchState = new SearchState(this);
        
        stateMachine.Initialize(patrolState);
    }


    protected virtual void Update()
    {
        stateMachine.Update();

        HandleFire();
        HandleFreeze();
        UpdateBurnVisual();
    }

    void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public float DistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.position);
    }

    public virtual void ApplyDebuff(DebuffType type, float duration, short fireDamage, float maxFireDuration, float slowLerpSpeed, float freezeThreshold, float freezeDuration)
    {

        if (type == DebuffType.fire)
        {
            this.fireDamage = fireDamage;
            this.maxFireDuration = maxFireDuration;
            AddFireBuildUp(duration);
        }

        if (type == DebuffType.freeze)
        {
            this.slowLerpSpeed = slowLerpSpeed;
            this.freezeThreshold = freezeThreshold;
            this.freezeDuration = freezeDuration;
            AddFreezeBuildUp(duration);
        }
    }

    public void AddFireBuildUp(float amount)
    {
        fireBuildUp += amount;

        if (fireBuildUp >= fireBuildUpThreshold)
        {
            isBurning = true;
            fireDuration += amount;
            fireDuration = Mathf.Clamp(fireDuration, 0f, maxFireDuration);
        }
    }

    public void HandleFire()
    {
        if (!isBurning) 
        {
            fireBuildUp = Mathf.MoveTowards(fireBuildUp, 0f, coolSpeed * Time.deltaTime);
            return;
        }

        fireDuration -= Time.deltaTime;

        fireTickTimer += Time.deltaTime;

        if (fireTickTimer >= fireTickInterval)
        {
            GetDamaged(fireDamage);
            fireTickTimer = 0f;
        }

        if (fireDuration <= 0f)
        {
            isBurning = false;
            fireBuildUp = 0f;
        }
    }
    

    public void UpdateBurnVisual()
    {
        float fireHeat = fireBuildUp / fireBuildUpThreshold;
        fireHeat = Mathf.Clamp01(fireHeat);
        visualHeat = Mathf.Lerp(visualHeat, fireHeat, 3f * Time.deltaTime);

        float freezeHeat = freezeBuildUp / freezeThreshold;
        freezeHeat = Mathf.Clamp01(freezeHeat);
        visualFreeze = Mathf.Lerp(visualFreeze, freezeHeat, 3f * Time.deltaTime);

        Color fireCol = Color.Lerp(originalColor, burnColor, visualHeat);
        Color freezeCol = Color.Lerp(originalColor, freezeColor, visualFreeze);

        Color finalColor = Color.Lerp(fireCol, freezeCol, visualFreeze);

        if (isFrozen)
        {
            finalColor = freezeColor;
        }

        spriteRenderer.color = finalColor;
    }

    public void AddFreezeBuildUp(float amount)
    {
        freezeBuildUp += amount;

        isSlowed = true;
        lastFreezeTime = Time.time;

        if (freezeBuildUp >= freezeThreshold)
        {
            Freeze();
        }
    }

    void Freeze()
    {
        isFrozen = true;
        freezeTimer = freezeDuration;
        slowMultiplier = 0f;
    }

    public void HandleFreeze()
    {
        if (isFrozen)
        {
            freezeTimer -= Time.deltaTime;

            if (freezeTimer <= 0f)
            {
                isFrozen = false;
                freezeBuildUp = 0f;
                slowMultiplier = 1f;
            }

            return;
        }

        if (Time.time > lastFreezeTime + freezeResetDelay)
        {
            freezeBuildUp = Mathf.MoveTowards(freezeBuildUp, 0f, freezeCoolSpeed * Time.deltaTime);

            if (freezeBuildUp <= 0.01f)
            {
                isSlowed = false;
            }
        }

        //  Lassítás számolása
        if (isSlowed)
        {
            float targetSlow = 1f - (freezeBuildUp / freezeThreshold);
            targetSlow = Mathf.Clamp(targetSlow, 0.2f, 1f); // optional: ne álljon meg teljesen

            slowMultiplier = Mathf.Lerp(slowMultiplier, targetSlow, slowLerpSpeed * Time.deltaTime);
        }
    }

}

public enum EnemyType
{
    basic,
    flying,
    boss1,
    boss2
}
public enum EnemyState
{
    idle,
    attacking,
    chasing,
    patrolling,
    searching
}
public enum DebuffType
{
    fire,
    freeze,
    frozen
}
public interface IMovement
{
    void MoveTo(Vector2 targetPosition);
    void MoveTo_Search(Vector2 targetPosition);
}
public interface IAttack
{
    void Attack(Transform target);
}
public interface IEnemyState
{
    void Enter();
    void Update();
    void FixedUpdate();
    void Exit();
}

