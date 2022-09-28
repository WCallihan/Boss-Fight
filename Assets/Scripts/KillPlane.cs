using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    //when a damageable object enter the plan, kill it
    private void OnCollisionEnter(Collision collision) {
        IDamageable otherDamageable = collision.gameObject.GetComponent<IDamageable>();
        otherDamageable?.TakeDamage(int.MaxValue);
    }
}