using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] private float shotRange = 100f;
    [SerializeField] private float damage = 3f;
    [SerializeField] private ParticleSystem muzzleFlash;
    PlayerInput playerInput;
    Vector3 rayStartPosition;
    Vector3 rayHitPosition;

    private void Start()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Gun.performed += Shoot;
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        muzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit, shotRange))
        {
            rayStartPosition = transform.position;
            rayHitPosition = hit.point;

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(rayStartPosition, rayHitPosition);
        Gizmos.DrawSphere(rayHitPosition, .5f);
    }
}