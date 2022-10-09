using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChargeAttack : MonoBehaviour {

    [SerializeField] private float chargeAttackColliderRadius;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private AudioClip chargingPrepSound;
    [SerializeField] private AudioClip chargingSound;

    public event Action StartedChargeAttack;
    public event Action EndedChargeAttack;

    public IEnumerator ChargeAttack() {
        //invoke event for anything listening
        StartedChargeAttack?.Invoke();

        //start playing the charging sound effect
        AudioHelper.PlayClip2D(chargingPrepSound, 1);

        //change the collider radius to be bigger over time
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        float originalColliderRadius = collider.radius;
        float totalTime = 2f;
        float timer = 0;
        while(timer < totalTime) {
            collider.radius = Mathf.Lerp(originalColliderRadius, chargeAttackColliderRadius, timer / totalTime);
            timer += Time.deltaTime;
            yield return null;
        }

        //play the dashing sound effect
        AudioHelper.PlayClip2D(chargingSound, 1);

        //move the boss rapidly down on the map
        Vector3 endOfCharge = transform.position + new Vector3(0, 0, -22);
        while(Vector3.Distance(transform.position, endOfCharge) > 0.5) {
            transform.position = Vector3.MoveTowards(transform.position, endOfCharge, chargeSpeed * Time.deltaTime);
            yield return null;
        }

        //change the collider radius back to normal over time
        totalTime = 1f;
        timer = 0;
        while(timer < totalTime) {
            collider.radius = Mathf.Lerp(chargeAttackColliderRadius, originalColliderRadius, timer / totalTime);
            timer += Time.deltaTime;
            yield return null;
        }

        //invoke event for anything listening
        EndedChargeAttack?.Invoke();
    }
}