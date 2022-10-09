using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    [SerializeField] private Health playerHealth;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float shakeDuration;

    private void OnEnable() {
        playerHealth.TookDamage += StartShake;
    }

    private void OnDisable() {
        playerHealth.TookDamage -= StartShake;
    }

    private void StartShake(int currentHealth) {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake() {
        //save original position
        Vector3 startPosition = transform.position;

        //shake camera for duration
        float timer = 0;
        while(timer < shakeDuration) {
            //get the strength of the shake from the curve
            float strength = curve.Evaluate(timer / shakeDuration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            timer += Time.deltaTime;
            yield return null;
        }

        //set back to original position
        transform.position = startPosition;
    }
}