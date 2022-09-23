using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable {

    public int MaxHealth;
    [SerializeField] private Collider objectCollider;
    [SerializeField] private List<MeshRenderer> artMeshRenderers;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private ParticleSystem killParticles;
    [SerializeField] private AudioClip killSound;
    private int currentHealth;

    public event Action<int> TookDamage;

    private void Awake() {
        currentHealth = MaxHealth;
    }

    public void TakeDamage(int damage) {

        //apply damage
        currentHealth -= damage;

        //invoke the event for anything listening
        TookDamage?.Invoke(currentHealth);

        //play feedback
        StartCoroutine(HurtFlash());
        AudioHelper.PlayClip2D(hurtSound, 1);

        //kill the object if it reaches 0
        if(currentHealth <= 0) {
            Kill();
        }
    }

    private IEnumerator HurtFlash() {
        //set all materials to red
        foreach(MeshRenderer r in artMeshRenderers) {
            r.material.SetColor("_EmissionColor", Color.red);
        }

        yield return new WaitForSeconds(0.1f);

        //set all materials back to normal
        foreach(MeshRenderer r in artMeshRenderers) {
            r.material.SetColor("_EmissionColor", Color.black);
        }
    }

    private void Kill() {
        //spawn kill particles
        Instantiate(killParticles, gameObject.transform);

        //turn off art and collider
        objectCollider.enabled = false;
        foreach(MeshRenderer r in artMeshRenderers) {
            r.enabled = false;
        }
        //play kill sound
        AudioHelper.PlayClip2D(killSound, 1);
    }
}