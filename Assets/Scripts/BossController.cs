using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float leftPatrolBound;
    [SerializeField] private float rightPatrolBound;
    [SerializeField] private float lowerPatrolBound;
    [SerializeField] private float upperPatrolBound;
    [SerializeField] private float patrolLength;

    [Header("Attacks")]
    [SerializeField] private int touchingBossDamage;
    [SerializeField] private GameObject airDropPrefab;
    [SerializeField] private float airDropAttackCooldown;
    [SerializeField] private List<Transform> bossChargePositions;
    [SerializeField] private float chargeAttackColliderRadius;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private AudioClip chargingSound;
    [SerializeField] private AudioClip dashingSound;

    //movement
    private Vector3 targetPatrolPosition;
    private Vector3 targetChargePosition;
    private float patrolTimer;
    //use these as mini states
    private bool patroling;
    private bool reachedPatrolPosition;
    private bool reachedChargePosition;

    //attacks
    private float airDropTimer;
    public event Action StartedChargeAttack;
    public event Action EndedChargeAttack;

    private void Awake() {
        patroling = true;
        patrolTimer = patrolLength;
        reachedPatrolPosition = true; //start true to generate an initial position

        airDropTimer = 0;
    }

    private void Update() {
        if(patroling) {
            //walk around to a random spot on the map

            //if the spot has been reached, pick a new spot
            if(reachedPatrolPosition) {

                reachedPatrolPosition = false;

                float randX = UnityEngine.Random.Range(leftPatrolBound, rightPatrolBound);
                float randZ = UnityEngine.Random.Range(lowerPatrolBound, upperPatrolBound);
                targetPatrolPosition = new Vector3(randX, transform.position.y, randZ);
            }

            //move towards target position
            transform.position = Vector3.MoveTowards(transform.position, targetPatrolPosition, speed * Time.deltaTime);
            //update patroling timer
            patrolTimer -= Time.deltaTime;

            //passive attack
            airDropTimer -= Time.deltaTime;
            AirDropAttack();

            //if the spot has now been reached, set the flag
            if(Vector3.Distance(transform.position, targetPatrolPosition) <= 0.5f) {

                reachedPatrolPosition = true;

                //check if the patrol timer is up
                if(patrolTimer <= 0) {

                    patroling = false;

                    //pick one of the charge positions
                    int randIndex = UnityEngine.Random.Range(0, bossChargePositions.Count - 1);
                    targetChargePosition = bossChargePositions[randIndex].position;

                    reachedChargePosition = false;
                }
            }

        } else {
            //walk to the randomly selected charge position

            if(!reachedChargePosition) {
                //move towards target charging position
                transform.position = Vector3.MoveTowards(transform.position, targetChargePosition, speed * Time.deltaTime);

                //if the spot has now been reached, initiate charge attack
                if(Vector3.Distance(transform.position, targetChargePosition) <= 0.5f) {
                    //set flag
                    reachedChargePosition = true;
                    //charge attack
                    StartCoroutine(ChargeAttack());
                }
            }
        }
    }

    private void AirDropAttack() {
        //check if the cooldown is done
        if(airDropTimer <= 0) {
            //choose random spot within the map
            float randX = UnityEngine.Random.Range(leftPatrolBound, rightPatrolBound);
            float randZ = UnityEngine.Random.Range(lowerPatrolBound, upperPatrolBound);
            Vector3 airDropPosition = new Vector3(randX, 0, randZ);
            //spawn an air drop attack prefab
            Instantiate(airDropPrefab, airDropPosition, airDropPrefab.transform.rotation);
            //reset the timer
            airDropTimer = airDropAttackCooldown;
        }
    }

    private IEnumerator ChargeAttack() {
        //invoke event for anything listening
        StartedChargeAttack?.Invoke();

        //start playing the charging sound effect
        AudioHelper.PlayClip2D(chargingSound, 1);

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
        AudioHelper.PlayClip2D(dashingSound, 1);

        //move the boss rapidly down on the map
        Vector3 endOfCharge = transform.position + new Vector3(0, 0, -22);
        while (Vector3.Distance(transform.position, endOfCharge) > 0.5) {
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

        //go back to patroling
        patroling = true;
        patrolTimer = patrolLength;
        reachedPatrolPosition = false;
    }

    private void OnCollisionEnter(Collision collision) {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        damageable?.TakeDamage(touchingBossDamage);
    }
}