using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour, IDamageable {

    [SerializeField] private Collider wallCollider;
    [SerializeField] private MeshRenderer artMeshRenderer;

    public void TakeDamage(int damage) {
        //turn off art and collider
        wallCollider.enabled = false;
        artMeshRenderer.enabled = false;
    }

    public void ShowWall() {
        //turn on art and collider
        wallCollider.enabled = true;
        wallCollider.enabled = true;
    }
}