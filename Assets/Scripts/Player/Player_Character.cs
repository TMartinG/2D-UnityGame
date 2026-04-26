using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player_Character : MonoBehaviour
{
    private Player_Movement playerMovement;
    public Light_Manager lightManager;
    [SerializeField] float energyRegenRate = 10f; // per másodperc
    [SerializeField] float regenDelayYellow = 2f;
    [SerializeField] float regenDelayRed = 2f;
    private float lastRedEnergyUseTime;
    private float lastYellowEnergyUseTime;


    [SerializeField] int playerMaxHP;
    public int playerCurrentHP;

    public float playerYellowMaxEnergy = 100f;
    public float playerYellowCurrentEnergy = 100f;
    public float playerRedMaxEnergy = 100f;
    public float playerRedCurrentEnergy = 100f;

    [SerializeField] float playerLightMaxEnergy = 100f;
    [SerializeField] float playerLightCurrentEnergy = 100f;

    [SerializeField] GameObject[] heartsSymbol;
    [SerializeField] Transform yellowEnergyOrb;
    [SerializeField] Transform redEnergyOrb;
    [SerializeField] float maxScale = 3f;


    [SerializeField] float damageCooldown = 1f; // hány másodpercig ne kapjon sebzést
    private bool isInvincible = false;

    public void SetInvincible(bool value)
    {
        isInvincible = value;
    }

    [SerializeField] SpriteRenderer headSpriteRenderer;
    [SerializeField] SpriteRenderer bodySpriteRenderer;

    [SerializeField] GameObject gKey;
    [SerializeField] GameObject oKey;

    public int currentBiomeIndex = 1;

    void Awake()
    {
        playerMovement = GetComponent<Player_Movement>();
        playerCurrentHP = 5;
        playerMaxHP = 5;

        UpdateHearts();
        UpdateYellowEnergyUI();
        UpdateRedEnergyUI();

    }

    public void ApplyKeyState()
    {
        gKey.SetActive(WorldStateManager.Instance.gKeyObtained);
        oKey.SetActive(WorldStateManager.Instance.oKeyObtained);
    }

    void Update()
    {
        #region Player Movement

        // Vízszintes mozgás input
        float moveX = Input.GetAxisRaw("Horizontal");
        playerMovement.SetMoveDirection(new Vector2(moveX, 0));

        // Ugrás input
        if (Input.GetKey(KeyCode.Space))
        {
            playerMovement.RequestJump();
        }

        /*if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(2);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Heal(2);
            AddRedEnergy(100f);
            AddYellowEnergy(100f);
        }*/

        if (Input.GetKeyDown(KeyCode.B))
        {
            System.Diagnostics.Process.Start(Application.persistentDataPath);
        }
        
        HandleEnergyRegen();

        #endregion

        


    }

    void HandleEnergyRegen()
    {
        float regen = energyRegenRate * Time.deltaTime;

        // RED aktív → YELLOW töltődik
        if (lightManager.activeType == LightType.Red)
        {
            if (Time.time >= lastYellowEnergyUseTime + regenDelayYellow)
            {
                playerYellowCurrentEnergy += regen;
                playerYellowCurrentEnergy = Mathf.Clamp(playerYellowCurrentEnergy, 0, playerYellowMaxEnergy);
            }
        }

        //  YELLOW aktív → RED töltődik
        else if (lightManager.activeType == LightType.Yellow)
        {
            if (Time.time >= lastRedEnergyUseTime + regenDelayRed)
            {
                playerRedCurrentEnergy += regen;
                playerRedCurrentEnergy = Mathf.Clamp(playerRedCurrentEnergy, 0, playerRedMaxEnergy);
            }
        }

        //  NINCS aktív FÉNY → MINDKETTŐ töltődik
        else if (lightManager.activeLight == null)
        {
            playerYellowCurrentEnergy += regen;
            playerRedCurrentEnergy += regen;

            playerYellowCurrentEnergy = Mathf.Clamp(playerYellowCurrentEnergy, 0, playerYellowMaxEnergy);
            playerRedCurrentEnergy = Mathf.Clamp(playerRedCurrentEnergy, 0, playerRedMaxEnergy);
        }

        UpdateYellowEnergyUI();
        UpdateRedEnergyUI();
    }

    public void UpdateHearts()
    {
        for (int i = 0; i < heartsSymbol.Length; i++)
        {
            if (i < playerCurrentHP)
            {
                heartsSymbol[i].GetComponent<Image>().enabled = true;
            }
            else
            {
                heartsSymbol[i].GetComponent<Image>().enabled = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
        return; // ha épp sebezhetetlen, nem történik semmi

        playerCurrentHP -= damage;
        playerCurrentHP = Mathf.Clamp(playerCurrentHP, 0, playerMaxHP);

        UpdateHearts();
        if (playerCurrentHP <= 0)
        {
            Die();
        }
        StartCoroutine(DamageCooldown());

    }

    public void Die()
    {
        lightManager.lightDict[LightType.Yellow].Activate();
        lightManager.lightDict[LightType.Red].Activate();
        ResetCamera();
        SaveSystem.LoadGame(gameObject);
    }

    public void LightsOFF()
    {
        if (lightManager.activeType == LightType.Yellow)
        {
            lightManager.lightDict[LightType.Red].Deactivate();
        }
        else if(lightManager.activeType == LightType.Red)
        {
            lightManager.lightDict[LightType.Yellow].Deactivate();
        }
        else
        {
            lightManager.lightDict[LightType.Yellow].Deactivate();
            lightManager.lightDict[LightType.Red].Deactivate();
        }
    }

    void ResetCamera()
    {
        playerMovement.GrowToTarget(15, 2f);
        playerMovement.maxCameraSize = 15;
    }

    IEnumerator DamageCooldown()
    {
        isInvincible = true;

        // villogás effekt
        StartCoroutine(FlashEffect());

        yield return new WaitForSeconds(damageCooldown);

        isInvincible = false;
    }

    public IEnumerator FlashEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            headSpriteRenderer.enabled = false;
            bodySpriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            headSpriteRenderer.enabled = true;
            bodySpriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Heal(int amount)
    {
        playerCurrentHP += amount;
        playerCurrentHP = Mathf.Clamp(playerCurrentHP, 0, playerMaxHP);

        UpdateHearts();
    }

    void UpdateYellowEnergyUI()
    {
        float t = Mathf.Clamp01(playerYellowCurrentEnergy / playerYellowMaxEnergy);

        float scale = Mathf.Lerp(0f, maxScale, t);

        yellowEnergyOrb.localScale = Vector3.one * scale;
    }

    void UpdateRedEnergyUI()
    {
        float t = Mathf.Clamp01(playerRedCurrentEnergy / playerRedMaxEnergy);

        float scale = Mathf.Lerp(0f, maxScale, t);

        redEnergyOrb.localScale = Vector3.one * scale;
    }

    public bool UseYellowEnergy(float amount)
    {
        if (playerYellowCurrentEnergy < amount)
            return false;

        playerYellowCurrentEnergy -= amount;
        lastYellowEnergyUseTime = Time.time;

        UpdateYellowEnergyUI();

        return true;
    }

    public bool UseRedEnergy(float amount)
    {
        if (playerRedCurrentEnergy < amount)
            return false;

        playerRedCurrentEnergy -= amount;
        lastRedEnergyUseTime = Time.time;

        UpdateRedEnergyUI();

        return true;
    }

    public void AddYellowEnergy(float amount)
    {
        playerYellowCurrentEnergy += amount;
        playerYellowCurrentEnergy = Mathf.Clamp(
            playerYellowCurrentEnergy,
            0,
            playerYellowMaxEnergy
        );

        UpdateYellowEnergyUI();
    }

    public void AddRedEnergy(float amount)
    {
        playerRedCurrentEnergy += amount;
        playerRedCurrentEnergy = Mathf.Clamp(
            playerRedCurrentEnergy,
            0,
            playerRedMaxEnergy
        );

        UpdateRedEnergyUI();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Health") && playerCurrentHP != playerMaxHP)
        {
            Heal(1);
            AddRedEnergy(30f);
            AddYellowEnergy(30f);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Gkey"))
        {
            Destroy(collision.gameObject);
            gKey.SetActive(true);
            WorldStateManager.Instance.gKeyObtained = true;
        }

        if (collision.CompareTag("Okey"))
        {
            Destroy(collision.gameObject);
            oKey.SetActive(true);
            WorldStateManager.Instance.oKeyObtained = true;
        }
    }

    public bool CanEnd()
    {
        return WorldStateManager.Instance.gKeyObtained && WorldStateManager.Instance.oKeyObtained;
    }


}
