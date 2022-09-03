using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankController))]
public class Player : MonoBehaviour {

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private GameObject tankBodyPart;
    [SerializeField] private Material tankBodyMaterial;
    [SerializeField] private Material invincibilityMaterial;

    private int currentHealth;
    private bool invincible;

    void Start() {
        currentHealth = maxHealth;
        invincible = false;
    }

    public void IncreaseHealth(int amount) {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Player's health: " + currentHealth);
    }

    public void DecreaseHealth(int amount) {
        if(invincible) {
            return;
        }
        currentHealth -= amount;
        Debug.Log("Player's health: " + currentHealth);
        if(currentHealth <= 0) {
            Kill();
        }
    }

    public void MakeInvincible() {
        invincible = true;
        tankBodyPart.GetComponent<MeshRenderer>().material = invincibilityMaterial;
    }

    public void MakeNotInvincible() {
        invincible = false;
        tankBodyPart.GetComponent<MeshRenderer>().material = tankBodyMaterial;
    }

    public void Kill() {
        if(invincible) {
            return;
        }
        gameObject.SetActive(false);
    }
}