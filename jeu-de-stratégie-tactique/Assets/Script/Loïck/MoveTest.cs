using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Rendering;
using UnityEngine;

public class MoveTest : NetworkBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotSpeed = 10f;

    void Update()
    {
        if (IsLocalPlayer)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Translate(new Vector3(Time.deltaTime*speed*-1,0,0));
            }   
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector3(Time.deltaTime*speed,0,0));
            }           
            if (Input.GetKey(KeyCode.Space))
            {
                transform.Rotate(new Vector3(0,Time.deltaTime*rotSpeed,0));
            }
        }
    }
}
