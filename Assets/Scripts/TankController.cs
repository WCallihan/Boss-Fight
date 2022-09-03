using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = .25f;
    [SerializeField] private float turnSpeed = 2f;

    [SerializeField] private GameObject turretPivot;
    [SerializeField] private Transform rocketSpawnPoint;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private float rocketCooldown;

    [SerializeField] private ParticleSystem rocketFiringParticles;
    [SerializeField] private AudioClip rocketFiringSound;

    private float rocketTimer;

    private Rigidbody rb = null;

    public float MaxSpeed {
        get => moveSpeed;
        set => moveSpeed = value > 0 ? value : 0.1f;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rocketTimer = 0;
    }

    private void FixedUpdate()
    {
        MoveTank();
        TurnTank();
        TurnTurret();
    }

    private void Update() {
        rocketTimer -= Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Space) && rocketTimer <= 0) {
            SpawnRocket();
        }
    }

    private void MoveTank()
    {
        // calculate the move amount
        float moveAmountThisFrame = Input.GetAxis("Vertical") * moveSpeed;
        // create a vector from amount and direction
        Vector3 moveOffset = transform.forward * moveAmountThisFrame;
        // apply vector to the rigidbody
        rb.MovePosition(rb.position + moveOffset);
        // technically adjusting vector is more accurate! (but more complex)
    }

    private void TurnTank()
    {
        // calculate the turn amount
        float turnAmountThisFrame = Input.GetAxis("Horizontal") * turnSpeed;
        // create a Quaternion from amount and direction (x,y,z)
        Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
        // apply quaternion to the rigidbody
        rb.MoveRotation(rb.rotation * turnOffset);
    }

    private void TurnTurret() {
        //uses raycasts to get the world position the mouse is pointing to
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 100)) {
            //points the turret in the direction of the mouse, only rotating on the x axis
            turretPivot.transform.LookAt(new Vector3(hit.point.x, turretPivot.transform.position.y, hit.point.z));
        }
    }

    private void SpawnRocket() {
        //reset countdown timer
        rocketTimer = rocketCooldown;
        //spawn particle effects
        Instantiate(rocketFiringParticles, rocketSpawnPoint);
        //play sound effect
        AudioHelper.PlayClip2D(rocketFiringSound, 1);
        //spawn rocket
        Instantiate(rocketPrefab, rocketSpawnPoint.position, rocketSpawnPoint.rotation);
    }
}
