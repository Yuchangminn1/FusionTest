using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class HPHandler : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnHPChanged))]
    byte HP { get; set; }

    [Networked(OnChanged = nameof(OnStateChanged))]
    public bool isDead { get; set; }

    bool isInitialized = false;

    const byte startingHP = 5;

    public Color uiOnHitColor;

    public Image uiONHitImage;

    public GameObject playerModel;
    public GameObject deathGameObjectPrefab;

    //Other components
    HitboxRoot hitboxRoot;
    CharacterMovementHandler characterMovementHandler;


    void Awake()
    {
        characterMovementHandler = GetComponent<CharacterMovementHandler>();
        hitboxRoot = GetComponentInChildren<HitboxRoot>();
    }
    IEnumerator OnHitCo()
    {
        Debug.Log($"{transform.name} OnHitCo");
        //bodyMeshRender.material.color = Color.white;

        if (Object.HasInputAuthority)
        {
            uiONHitImage.color = uiOnHitColor;
        }
        yield return new WaitForSeconds(0.2f);

        if (Object.HasInputAuthority && !isDead)
        {
            uiONHitImage.color = new Color(0, 0, 0,0);
        }
        

    }

    IEnumerator ServerReviveCO()
    {
        Debug.Log($"{transform.name} ServerReviveCO()");
        yield return new WaitForSeconds(2.0f);
        characterMovementHandler.RequestRespawn();
    }


    void Start()
    {
        HP = startingHP;
        isDead = false;

        
        isInitialized = true;
    }

    public void OnTakeDamage()
    {
        Debug.Log($"{transform.name} OnTakeDamage()");

        if (isDead)
        {
            return;
        }
        HP -= 1;
        Debug.Log($"{Time.time} {transform.name}took damage get {HP} left");
        if (HP <= 0)
        {
            Debug.Log($"{transform.name} isDead");

            StartCoroutine(ServerReviveCO());

            isDead = true;

        }

    }
    //??
    static void OnHPChanged(Changed<HPHandler> changed)
    {
        Debug.Log($" OnHPChanged()");


        byte newHP = changed.Behaviour.HP;
        //load the old value
        changed.LoadOld();

        byte oldHP = changed.Behaviour.HP;
        //check if the HP has been decreased
        if (newHP < oldHP)
        {
            changed.Behaviour.OnHPReduced();
        }
    }

    

    void OnHPReduced()
    {
        Debug.Log($"{transform.name} OnHPReduced()");

        if (!isInitialized)
        {
            return;
        }
        StartCoroutine(OnHitCo());
    }

    static void OnStateChanged(Changed<HPHandler> changed)
    {
        Debug.Log($"OnHPReduced()");

        bool isDeathCurrent = changed.Behaviour.isDead;

        changed.LoadOld();

        bool isDeadOld = changed.Behaviour.isDead;

        //Handle on death for the player. Also check if the player was dead but is now alive in that case reive the player.
        if(isDeathCurrent)
            changed.Behaviour.OnDeath();
        else if (!isDeathCurrent && isDeadOld)
        {
            changed.Behaviour.OnReive();
        }
        

    }
    void OnDeath()
    {
        Debug.Log($"{transform.name} OnDeath");

        playerModel.gameObject.SetActive(false);
        hitboxRoot.HitboxRootActive = false;
        characterMovementHandler.SetCharacterControllerEnabled(false);
        

        Instantiate(deathGameObjectPrefab,transform.position,Quaternion.identity);
    }
    void OnReive()
    {
        Debug.Log($"{transform.name} OnReive");

        if (Object.HasInputAuthority)
            uiONHitImage.color = new Color(0,0,0,0);

        playerModel.gameObject.SetActive(true);
        hitboxRoot.HitboxRootActive = true;
        characterMovementHandler.SetCharacterControllerEnabled(true);

    }

    public void OnRespawned()
    {
        Debug.Log($"{transform.name} OnRespawned()");

        HP = startingHP;
        isDead = false;
    }
}
