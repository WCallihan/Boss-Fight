using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

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
        if(other.gameObject.CompareTag("Player")) {
            Debug.Log("rocket hit player");
            return;
        }
        Debug.Log("rocket hit something");
        //spawn explosion particles
        Instantiate(explosionParticles, transform.position, explosionParticles.transform.rotation);
        //play sound effect
        AudioHelper.PlayClip2D(explosionSound, 1);
        //TODO: add functionality for when the rocket hits the boss
        Destroy(gameObject);
    }
}