using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float horizontalInput;
    public bool jumpInput;
    public bool dashInput;
    public bool shootInput;
    public bool pauseInput;
    public bool reloadInput;

    public static InputManager Instance = null;

    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Set InputManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        ProcessInput();
    }

    public void ProcessInput()
    {
        pauseInput = Input.GetKeyDown(KeyCode.Escape);
        reloadInput = Input.GetKeyDown(KeyCode.R); 
        
        if (GameManager.Instance.currentGameState != GameState.Playing) return;

        horizontalInput = Input.GetAxisRaw("Horizontal");

        jumpInput = Input.GetKeyDown(KeyCode.Space);
        dashInput = Input.GetKey(KeyCode.LeftShift);
        shootInput = Input.GetKey(KeyCode.Z);

        // Debug Inputs
        // if (Input.GetKeyDown(KeyCode.B)) HandleDebugInput("B");
        // if (Input.GetKeyDown(KeyCode.E)) HandleDebugInput("E");
        // if (Input.GetKeyDown(KeyCode.I)) HandleDebugInput("I");
        // if (Input.GetKeyDown(KeyCode.K)) HandleDebugInput("K");
        // if (Input.GetKeyDown(KeyCode.L)) HandleDebugInput("L");
        // if (Input.GetKeyDown(KeyCode.P)) HandleDebugInput("P");
    }

    private void HandleDebugInput(string key)
    {
        // switch (key)
        // {
        //     case "B":
        //         GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        //         if (bullets.Length > 0)
        //         {
        //             freezeBullets = !freezeBullets;
        //             // Add freeze bullets logic here
        //         }
        //         Debug.Log("Freeze Bullets: " + freezeBullets);
        //         break;
        //     case "E":
        //         Defeat();
        //         break;
        //     case "I":
        //         Invincible(!isInvincible);
        //         break;
        //     case "K":
        //         FreezeInput(!freezeInput);
        //         break;
        //     case "L":
        //         ApplyLifeEnergy(10);
        //         break;
        //     case "P":
        //         FreezePlayer(!freezePlayer);
        //         break;
        // }
    }
}

