using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAirDropAttack : MonoBehaviour {

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject airDropPrefab;
    [SerializeField] private float dropAreaRadius;
    [SerializeField] private float airDropAttackCooldown;

    private bool active;
    private float airDropTimer;

    private void Awake() {
        active = false;
        airDropTimer = 0;
    }

    private void Update() {
        //decrement timer and check if it should attack
        airDropTimer -= Time.deltaTime;
        if(active && airDropTimer <= 0) {
            AirDropAttack();
        }
    }

    public void SetActive(bool a) {
        active = a;
    }

    private void AirDropAttack() {
        //get radial bounds around the player where the air drop could spawn
        float lowerXBound = player.transform.position.x - dropAreaRadius;
        float upperXBound = player.transform.position.x + dropAreaRadius;
        float lowerZBound = player.transform.position.z - dropAreaRadius;
        float upperZBound = player.transform.position.z + dropAreaRadius;

        //pick a point within that area at random
        float randX = UnityEngine.Random.Range(lowerXBound, upperXBound);
        float randZ = UnityEngine.Random.Range(lowerZBound, upperZBound);
        Vector3 airDropPoint = new Vector3(randX, 0, randZ);

        //spawwn an air drop at the chosen point
        Instantiate(airDropPrefab, airDropPoint, airDropPrefab.transform.rotation);

        //reset the timer
        airDropTimer = airDropAttackCooldown;
    }
}