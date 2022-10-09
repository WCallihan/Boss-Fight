using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour, IDamageable {

    [SerializeField] private BossChargeAttack bossChargeAttack;
    [SerializeField] private Collider wallCollider;
    [SerializeField] private MeshRenderer artMeshRenderer;

    private void Awake() {
        HideWall();
    }

    private void OnEnable() {
        bossChargeAttack.StartedChargeAttack += ShowWall;
        bossChargeAttack.EndedChargeAttack += HideWall;
    }

    private void OnDisable() {
        bossChargeAttack.StartedChargeAttack -= ShowWall;
        bossChargeAttack.EndedChargeAttack -= HideWall;
    }

    public void TakeDamage(int damage) {
        HideWall();
    }

    private void ShowWall() {
        //turn on art and collider
        wallCollider.enabled = true;
        artMeshRenderer.enabled = true;
    }
    private void HideWall() {
        //turn off art and collider
        wallCollider.enabled = false;
        artMeshRenderer.enabled = false;
    }
}