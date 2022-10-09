using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitboxIndicator : MonoBehaviour {

    [SerializeField] private CapsuleCollider bossCollider;
    BossChargeAttack chargeAttack;
    private bool visible;

    private void Awake() {
        HideIndicator();
        chargeAttack = bossCollider.gameObject.GetComponent<BossChargeAttack>();
    }

    private void OnEnable() {
        chargeAttack.StartedChargeAttack += ShowIndicator;
        chargeAttack.EndedChargeAttack += HideIndicator;
    }

    private void OnDisable() {
        chargeAttack.StartedChargeAttack -= ShowIndicator;
        chargeAttack.EndedChargeAttack -= HideIndicator;
    }

    private void Update() {
        if(visible) {
            float scale = 2 * bossCollider.radius;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    private void ShowIndicator() {
        visible = true;
    }

    private void HideIndicator() {
        visible = false;
        transform.localScale = Vector3.zero;
    }
}