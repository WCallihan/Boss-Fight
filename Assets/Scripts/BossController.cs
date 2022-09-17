using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

    [SerializeField] private float speed;
    [SerializeField] private float leftPatrolBound;
    [SerializeField] private float rightPatrolBound;
    [SerializeField] private float lowerPatrolBound;
    [SerializeField] private float upperPatrolBound;
    [SerializeField] private float patrolLength;
    [SerializeField] private List<Transform> bossChargePositions;

    private Vector3 targetPatrolPosition;
    private Vector3 targetChargePosition;
    private float patrolTimer;

    //use these as place holders for states that may come later
    private bool patroling;
    private bool reachedPatrolPosition;
    private bool reachedChargePosition;

    private void Awake() {
        patroling = true;
        patrolTimer = patrolLength;
        reachedPatrolPosition = true; //start true to generate an initial position
    }

    private void Update() {
        if(patroling) {
            //walk around to a random spot on the map

            //if the spot has been reached, pick a new spot
            if(reachedPatrolPosition) {

                reachedPatrolPosition = false;

                float randX = Random.Range(leftPatrolBound, rightPatrolBound);
                float randZ = Random.Range(lowerPatrolBound, upperPatrolBound);
                targetPatrolPosition = new Vector3(randX, transform.position.y, randZ);

                Debug.Log($"going to new patrol position: {targetPatrolPosition}");
            }

            //move towards target position
            transform.position = Vector3.MoveTowards(transform.position, targetPatrolPosition, speed * Time.deltaTime);
            //update patroling timer
            patrolTimer -= Time.deltaTime;

            //if the spot has now been reached, set the flag
            if(Vector3.Distance(transform.position, targetPatrolPosition) <= 0.5f) {
                Debug.Log("reached target patrol position");

                reachedPatrolPosition = true;

                //check if the patrol timer is up
                if(patrolTimer <= 0) {
                    Debug.Log("patrol timer over");

                    patroling = false;

                    //pick one of the charge positions
                    int randIndex = Random.Range(0, bossChargePositions.Count - 1);
                    targetChargePosition = bossChargePositions[randIndex].position;
                    Debug.Log($"going to new charge position: {targetChargePosition}");

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
                    Debug.Log("reached target charge position");

                    reachedChargePosition = true;
                    //TODO: add charging attack functionality
                    //FOR NOW, JUST GO BACK TO PATROLING
                    patroling = true;
                    patrolTimer = patrolLength;
                    reachedPatrolPosition = false;
                }
            }
        }
    }
}