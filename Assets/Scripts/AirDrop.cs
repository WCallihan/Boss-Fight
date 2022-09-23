using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDrop : MonoBehaviour {

    [SerializeField] private int damage;
    [SerializeField] private float delayTime;
    [SerializeField] private GameObject baseRadiusArt;
    [SerializeField] private GameObject timeIndicatorArt;
    [SerializeField] private GameObject explosionParticles;

    private float delayTimer;
    private Collider player = null;

    private void Awake() {
        delayTimer = 0;
    }

    private void Update() {
        //grow the indicator art over time linearly
        timeIndicatorArt.transform.localScale = Vector3.Lerp(Vector3.zero, baseRadiusArt.transform.localScale, delayTimer / delayTime);

        //count down until the missile strikes
        delayTimer += Time.deltaTime;

        //when the timer is done...
        if(delayTimer >= delayTime) {
            //damage the player if they are inside the radius
            player?.GetComponent<IDamageable>().TakeDamage(damage);
            //spawn the explosion particles
            Instantiate(explosionParticles, transform.position, explosionParticles.transform.rotation);
            //destroy the air drop object
            Destroy(gameObject);
        }
    }

    //player is touching the impact area
    private void OnTriggerEnter(Collider other) {
        TankController tankCheck = other.GetComponent<TankController>();
        if(tankCheck != null && player == null) {
            player = other;
        }
    }

    //player is no longer touching the impact area
    private void OnTriggerExit(Collider other) {
        if(other == player) {
            player = null;
        }
    }
}