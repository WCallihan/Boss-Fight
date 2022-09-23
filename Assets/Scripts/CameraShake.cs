using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    [SerializeField] private Health playerHealth;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] float shakeDuration;

    private void OnEnable() {
        playerHealth.TookDamage += StartShake;
    }

    private void OnDisable() {
        playerHealth.TookDamage -= StartShake;
    }

    private void StartShake(int damage) {
        StartCoroutine(Shake(damage));
    }

    private IEnumerator Shake(int damage) {
        //save original position
        Vector3 startPosition = transform.position;

        //shake camera for duration
        float timer = 0;
        while(timer < shakeDuration) {
            //get the strength of the shake from the curve and how much damage was taken
            float strength = curve.Evaluate(timer / shakeDuration) * damage; //IMPORTANT: this assumes that all received damage values are either 1 or 2
            transform.position = startPosition + Random.insideUnitSphere * strength;
            timer += Time.deltaTime;
            yield return null;
        }

        //set back to original position
        transform.position = startPosition;
    }
}