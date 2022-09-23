using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitboxIndicator : MonoBehaviour {

    [SerializeField] private CapsuleCollider bossCollider;
    private bool visible;

    private void Awake() {
        HideIndicator();
    }

    private void OnEnable() {
        bossCollider.gameObject.GetComponent<BossController>().StartedChargeAttack += ShowIndicator;
        bossCollider.gameObject.GetComponent<BossController>().EndedChargeAttack += HideIndicator;
    }

    private void OnDisable() {
        bossCollider.gameObject.GetComponent<BossController>().StartedChargeAttack -= ShowIndicator;
        bossCollider.gameObject.GetComponent<BossController>().EndedChargeAttack -= HideIndicator;
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