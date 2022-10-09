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
    [SerializeField] private BossAirDropAttack airDropAttack;
    [SerializeField] private BossChargeAttack chargeAttack;
    [SerializeField] private List<Transform> bossChargePositions;

    //movement
    private Vector3 targetPatrolPosition;
    private Vector3 targetChargePosition;
    private float patrolTimer;
    //use these as mini states
    private bool patroling;
    private bool reachedPatrolPosition;
    private bool reachedChargePosition;
    private bool dead;

    private void OnEnable() {
        GetComponent<Health>().Died += SetDead;
        chargeAttack.EndedChargeAttack += StartPatrolling;
    }

    private void OnDisable() {
        GetComponent<Health>().Died -= SetDead;
        chargeAttack.EndedChargeAttack -= StartPatrolling;
    }

    private void Awake() {
        dead = false;
    }

    private void Start() {
        StartPatrolling();
    }

    private void Update() {
        if(dead) return;

        if(patroling) {
            //walk around to a random spot on the map

            //if the spot has been reached, pick a new spot
            if(reachedPatrolPosition) {

                reachedPatrolPosition = false;
                SetPatrolPoint();
            }

            //move towards target position
            transform.position = Vector3.MoveTowards(transform.position, targetPatrolPosition, speed * Time.deltaTime);
            //update patroling timer
            patrolTimer -= Time.deltaTime;

            //if the spot has now been reached, set the flag
            if(Vector3.Distance(transform.position, targetPatrolPosition) <= 0.5f) {

                reachedPatrolPosition = true;

                //check if the patrol timer is up
                if(patrolTimer <= 0) {
                    //stop patrolling
                    patroling = false;
                    airDropAttack.SetActive(false);

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
                    StartCoroutine(chargeAttack.ChargeAttack());
                }
            }
        }
    }

    private void StartPatrolling() {
        patroling = true;
        patrolTimer = patrolLength;
        reachedPatrolPosition = false;
        SetPatrolPoint();
        airDropAttack.SetActive(true);
    }

    private void SetPatrolPoint() {
        float randX = UnityEngine.Random.Range(leftPatrolBound, rightPatrolBound);
        float randZ = UnityEngine.Random.Range(lowerPatrolBound, upperPatrolBound);
        targetPatrolPosition = new Vector3(randX, transform.position.y, randZ);
    }

    private void OnCollisionEnter(Collision collision) {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        damageable?.TakeDamage(touchingBossDamage);
    }

    private void SetDead() {
        dead = true;
        airDropAttack.SetActive(false);
    }
}