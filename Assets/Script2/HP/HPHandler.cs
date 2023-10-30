using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class HPHandler : NetworkBehaviour
{

    int MaxHp { get; set; }

    [Networked(OnChanged = nameof(OnHPChanged))]
    int HP { get; set; }

    [Networked(OnChanged = nameof(OnStateChanged))]
    public bool isDead { get; set; }

    bool isInitialized = false;

    public Color uiOnHitColor;

    public Image uiONHitImage;

    public GameObject playerModel;
    public GameObject deathGameObjectPrefab;



    //Other components
    HitboxRoot hitboxRoot;
    CharacterMovementHandler characterMovementHandler;
    
    public Slider hpBar;

    //[SerializeField] Image hpBarImage;
    //[SerializeField] Image hpHealFillImage;
    //[SerializeField] ParticleSystem playerParticle;

    void Awake()
    {
        characterMovementHandler = GetComponent<CharacterMovementHandler>();
        hitboxRoot = GetComponentInChildren<HitboxRoot>();
        MaxHp = 5;
    }
    IEnumerator OnHitCo()
    {
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
        yield return new WaitForSeconds(2.0f);
        characterMovementHandler.RequestRespawn();
    }


    void Start()
    {
        Debug.Log("내 이름은 " + transform.name);
        hpBar = transform.GetComponentInChildren<Slider>();
        HpReset();

        isDead = false;


        isInitialized = true;
    }
    void Update()
    {
        
    }
    public void OnTakeDamage(int _attackDamage = 1)
    {
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
    //변하면 호출인데 아직 잘 모름 
    static void OnHPChanged(Changed<HPHandler> changed)
    {
        Debug.Log($" OnHPChanged()");


        int newHP = changed.Behaviour.HP;
        changed.Behaviour.HPBarValue();
        //load the old value
        changed.LoadOld();

        int oldHP = changed.Behaviour.HP;
        
        //check if the HP has been decreased
        if (newHP != oldHP)
        {
            //changed.Behaviour.HPBarValue(); 이런 호스트 체력을 다른 플레이어들 UI로 공유해주네 
            //여기서 쓰는 함수는 static 이기 떄문에 ? changed.Behaviour를 써야하네 공부 해봐야할듯
            changed.Behaviour.OnHPReduced();

        }
    }
    void HPBarValue()
    {

        transform.GetComponent<HPHandler>().hpBar.value = (float)HP / (float)MaxHp;
        Debug.Log($"{transform.name}  MaxHp{MaxHp} , _currentHp{HP}  hpBar.value = {hpBar.value}");
    }
    

    void OnHPReduced()
    {

        if (!isInitialized)
        {
            return;
        }
        StartCoroutine(OnHitCo());
    }

    static void OnStateChanged(Changed<HPHandler> changed)
    {
        //Debug.Log($"OnHPReduced()");

        bool isDeathCurrent = changed.Behaviour.isDead;

        changed.LoadOld();

        bool isDeadOld = changed.Behaviour.isDead;

        //Handle on death for the player. Also check if the player was dead but is now alive in that case reive the player.
        if (isDeathCurrent)
            changed.Behaviour.OnDeath();
        else if (!isDeathCurrent && isDeadOld)
        {
            changed.Behaviour.OnReive();

        }


    }
    void OnDeath()
    {

        playerModel.gameObject.SetActive(false);
        hitboxRoot.HitboxRootActive = false;
        characterMovementHandler.SetCharacterControllerEnabled(false);
        

        Instantiate(deathGameObjectPrefab,transform.position,Quaternion.identity);

    }
    void OnReive()
    {

        if (Object.HasInputAuthority)
            uiONHitImage.color = new Color(0,0,0,0);

        playerModel.gameObject.SetActive(true);
        hitboxRoot.HitboxRootActive = true;
        characterMovementHandler.SetCharacterControllerEnabled(true);
        

    }

    public void OnRespawned()
    {
        isDead = false;
        HpReset();
    }

    void HpReset()
    {
        HP = MaxHp;
        

    }
    
}



    
