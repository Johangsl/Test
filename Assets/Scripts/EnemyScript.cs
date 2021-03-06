﻿using UnityEngine;

/// <summary>
/// Enemy generic behavior
/// </summary>
public class EnemyScript : MonoBehaviour
{
    private bool hasSpawn;
    private MoveScript moveScript;
    private WeaponScript[] weapons;
    private HealthScript healthScript;

    private Animator animator;
		

	public Vector2 zigZagMove;

    void Awake()
    {
        // Retrieve the weapon only once
        weapons = GetComponentsInChildren<WeaponScript>();

        // Retrieve scripts to disable when not spawn
        moveScript = GetComponent<MoveScript>();
        healthScript = GetComponent<HealthScript>();
        animator = GetComponent<Animator>();
    }

    // 1 - Disable everything
    void Start()
    {
        hasSpawn = false;

        // Disable everything
        // -- collider
        DisableEnemy();

    }

    private void DisableEnemy()
    {
        collider2D.enabled = false;
        // -- Moving
        moveScript.enabled = false;
        // -- Shooting
        foreach (WeaponScript weapon in weapons)
        {
            weapon.enabled = false;
        }

    }

    void Update()
    {
        // 2 - Check if the enemy has spawned.
        if (hasSpawn == false)
        {
            if (renderer.IsVisibleFrom(Camera.main))
            {
				Spawn();
            }
        }
        else
        {
            // Auto-fire
            foreach (WeaponScript weapon in weapons)
            {
                if (weapon != null && weapon.enabled && weapon.CanAttack)
                {
                    weapon.Attack(true);
                    SoundEffectsHelper.Instance.MakeEnemyShotSound();
                }
            }

            // 4 - Out of the camera ? Destroy the game object.
            if (renderer.IsVisibleFrom(Camera.main) == false)
            {
                Destroy(gameObject);
            }
        }

		//zig Zag
		float currPos = gameObject.transform.position.y;
		gameObject.transform.Translate (0.0f,  0.02f, 0.0f, Space.World) ;

        if (healthScript.hp <= 0)
        {
            DisableEnemy();
            animator.SetBool("Death", true);
            Destroy(gameObject, 0.2f);
        }
    }

    // 3 - Activate itself.
    private void Spawn()
    {
        hasSpawn = true;

        // Enable everything
        // -- Collider
        collider2D.enabled = true;
        // -- Moving
        moveScript.enabled = true;
        // -- Shooting
        foreach (WeaponScript weapon in weapons)
        {
            weapon.enabled = true;
        }
    }

}