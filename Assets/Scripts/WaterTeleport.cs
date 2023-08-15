using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTeleport : MonoBehaviour
{
    public Transform teleportDestination; // Ссылка на точку телепортации на берегу

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called.");
        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.transform);
        }
    }


    private void TeleportPlayer(Transform playerTransform)
    {
        playerTransform.position = teleportDestination.position;
    }
}