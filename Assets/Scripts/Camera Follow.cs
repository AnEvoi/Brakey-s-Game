using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f,0f,-10f);
    
    [SerializeField] private Transform player;
    [SerializeField] private float camSpeed;

    void Update()
    {
        Vector3 targetPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * camSpeed);
    }
}
