using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private int timeout;

    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private AudioClip explosionSound;

    private float timer;

    private void Awake() {
        timer = 0;
    }

    private void Update() {
        //move forward at a constant speed
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        //increase the timer
        timer += Time.deltaTime;
        //destroy rocket if it has been flying for too long
        if(timer >= timeout) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        //do nothing if it hits the player
        if(other.gameObject.CompareTag("Player")) {
            return;
        }

        //spawn explosion particles
        Instantiate(explosionParticles, transform.position, explosionParticles.transform.rotation);
        //play sound effect
        AudioHelper.PlayClip2D(explosionSound, 1);

        //attempt to damage whatever it hits
        IDamageable damageableHit = other.GetComponent<IDamageable>();
        damageableHit?.TakeDamage(damage);

        //destroy the rocket
        Destroy(gameObject);
    }
}