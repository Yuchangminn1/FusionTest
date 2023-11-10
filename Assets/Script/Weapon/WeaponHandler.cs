using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEditor;

public class WeaponHandler : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnFireChanged))]
    //"OnFireChanged"�� ���ο��� ���� �Ƹ� 
    public bool isFiring { get; set; }

    public ParticleSystem fireParticleSystem;
    public Transform aimPoint;
    public LayerMask collisionLayer;


    float lastTimeFire = 0;

    HPHandler hpHandler;

    void Awake()
    {
        hpHandler = GetComponent<HPHandler>();
    }


    public override void FixedUpdateNetwork()
    {
        if (hpHandler.isDead)
        {
            return;
        }
        if (GetInput(out NetworkInputData networkInputData))
        {
            if (networkInputData.isFireButtonPressed)
            {
                Fire(networkInputData.aimFowardVector);
            }
        }
        //Debug.Log($"{transform.name} FixedUpdateNetwork()");
        


    }

    //�÷��̾ �߻��ư�� ����
    void Fire(Vector3 aimForwardVector)
    {
        Debug.Log("Weapon  = " + Object.name);

        //Debug.Log($"{transform.name} Fire()");

        if (Time.time - lastTimeFire < 0.15f)
        {
            return;
        }

        StartCoroutine(FireEffectCO());

        Runner.LagCompensation.Raycast(aimPoint.position, aimForwardVector, 100, Object.InputAuthority, out var hitnfo, collisionLayer, HitOptions.IncludePhysX);

        float hitDistance = 100;
        bool ishitOtherPlayer = false;

        if (hitnfo.Distance > 0)
        {
            hitDistance = hitnfo.Distance;
        }
        if (hitnfo.Hitbox != null)
        {

            if (Object.HasStateAuthority)
            {

                //��Ʈ�ڽ��� �浹���� ��� �� �浹ü�� ��ũ��Ʈ�� �����ͼ� �Լ� ����
                hitnfo.Hitbox.transform.root.GetComponent<HPHandler>().OnTakeDamage();
            }


            ishitOtherPlayer = true;
        }
        else if (hitnfo.Collider != null)
        {
            //Debug.Log("@");

            // Debug.Log($"{Time.time} {transform.name} hit physX collider {hitnfo.Collider.transform.name}");
        }

        //Debug
        if (ishitOtherPlayer)
        {
            Debug.DrawRay(aimPoint.position, aimForwardVector * hitDistance, Color.red, 1);
        }
        else
        {
            Debug.DrawRay(aimPoint.position, aimForwardVector * hitDistance, Color.green, 1);
        }

        //�߻�ð� ����
        lastTimeFire = Time.time;
    }
    //������ ���� �����ϱ���� ��ٸ��� 
    IEnumerator FireEffectCO()
    {

        if (fireParticleSystem.isPlaying != true)
        {
            isFiring = true;
            fireParticleSystem.Play();
            Debug.Log(fireParticleSystem.transform.position);
            yield return new WaitForSeconds(0.09f);
        }
        isFiring = false;
    }

    static void OnFireChanged(Changed<WeaponHandler> changed)
    {

        //Debug.Log($"{Time.time} OnFireChange value {changed.Behaviour.isFiring}");
        bool isFiringCurrent = changed.Behaviour.isFiring;

        //Load the old value
        changed.LoadOld();

        bool isFiringOld = changed.Behaviour.isFiring;

        if (isFiringCurrent && !isFiringOld)
            changed.Behaviour.OnFireRemote();

    }

    void OnFireRemote()
    {

        if (!Object.HasInputAuthority)
        {
            fireParticleSystem.Play();
        }
    }


}
