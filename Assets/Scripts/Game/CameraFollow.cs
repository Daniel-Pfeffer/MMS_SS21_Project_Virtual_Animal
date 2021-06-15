using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;


    private void FixedUpdate()
    {
        Vector3 destination = target.position + offset;
        Vector3 smoothDestination = Vector3.Lerp(transform.position, destination, smoothSpeed);
        transform.position = smoothDestination;
    }
}