using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.Netcode.Samples;
using UnityEngine;

[RequireComponent(typeof(ClientNetworkTransform))]
public class Player_Move : NetworkBehaviour
{
    public float acceleration = 0;
    public SpriteRenderer spriteRenderer;
    private float speed = 1;
    public float speedMax = 1;
    public float friction = 1;
    private float dirX = 0;
    private float orientX = 1;
    private float orientY = 1;
    private float dirY = 0;

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("Sprite Render du GameObject " + gameObject.name + " est Introuvable !");
            }
        }
    }

    void ApplySpeed()
    {
        Vector2 playerPosition = transform.position;
        playerPosition.x += orientX * speed * Time.fixedDeltaTime;
        playerPosition.y += orientY * speed * Time.fixedDeltaTime;
        transform.position = playerPosition;
    }
    void FixedUpdate()
    {
        ApplySpeed();
        UpdateMove();
        MoveInput();
    }
    void UpdateMove()
    {
        if (dirX != 0 || dirY != 0)
        {
            speed += acceleration * Time.fixedDeltaTime;
            if (speed > speedMax)
            {
                speed = speedMax;
            }
            orientY = dirY;
            orientX = dirX;
        }
        else if (speed > 0)
        {
            float applyFriction = friction * Time.fixedDeltaTime;
            if (speed >= applyFriction)
            {
                speed -= applyFriction;
            }
            else
            {
                speed = 0;
            }
        }
    }

    void MoveInput()
    {
        if (IsLocalPlayer)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                spriteRenderer.flipX = false;
                dirX = -1;
                if (Input.GetKey(KeyCode.Z))
                {
                    dirY = 1;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    dirY = -1;

                }
                else
                {
                    dirY = 0;
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                spriteRenderer.flipX = true;
                dirX = 1;
                if (Input.GetKey(KeyCode.Z))
                {
                    dirY = 1;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    dirY = -1;
                }
                else
                {
                    dirY = 0;
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                dirY = -1;
                if (Input.GetKey(KeyCode.D))
                {
                    dirX = 1;
                }
                else if (Input.GetKey(KeyCode.Q))
                {
                    dirX = -1;
                }
                else
                {
                    dirX = 0;
                }
            }
            else if (Input.GetKey(KeyCode.Z))
            {
                dirY = 1;
                if (Input.GetKey(KeyCode.D))
                {
                    dirX = 1;
                }
                else if (Input.GetKey(KeyCode.Q))
                {
                    dirX = -1;
                }
                else
                {
                    dirX = 0;
                }
            }
            else
            {
                dirY = 0;
                dirX = 0;
            }
        }
    }
}
