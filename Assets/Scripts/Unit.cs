/*
 * file:        Unit.cs
 * project:     Chiikawa Defense
 * description: controls one character on the battlefield. every character
 *              (chiikawa, usagi, enemies, the chimera) uses this same script,
 *              just with different stats typed into the inspector.
 *
 *              what a unit does each frame:
 *                1. look for an enemy unit in front of it
 *                2. if none, look for the enemy tower
 *                3. if nothing is in range, keep walking
 *                4. if something is in range, stop and attack it
 */

using System.Collections.Generic;
using UnityEngine;

// which side something is on. used by both units and towers.
public enum Team
{
    Player,
    Enemy
}

public class Unit : MonoBehaviour
{
    // every unit alive right now. shared by all units so they can find each other
    public static List<Unit> allUnits = new List<Unit>();

    [Header("Team")]
    public Team team = Team.Player;      // player walks right, enemy walks left

    [Header("Stats")]
    public int maxHealth = 100;          // starting health
    public int damage = 10;              // damage per hit
    public float moveSpeed = 1.5f;       // walking speed
    public float attackRange = 0.8f;     // how close a target has to be to hit it
    public float attackCooldown = 1f;    // seconds between hits
    public int cost = 50;                // coins to spawn (used later)

    private int currentHealth;           // health right now
    private float attackTimer;           // counts down until the next hit
    private float direction;             // 1 = walk right, -1 = walk left
    private UnitAnimator unitAnimator;   // fake animations (waddle, stretch, flash)

    // runs when the unit appears. adds it to the shared list.
    void OnEnable()
    {
        allUnits.Add(this);
    }

    // runs when the unit is destroyed. removes it from the shared list.
    void OnDisable()
    {
        allUnits.Remove(this);
    }

    // runs once at the start. sets health, walking direction, and sprite facing.
    void Start()
    {
        currentHealth = maxHealth;

        // player units walk right, enemies walk left
        if (team == Team.Player) {
            direction = 1f;
        } else {
            direction = -1f;
        }

        // all sprites are drawn facing right, so flip the enemies
        if (team == Team.Enemy) {
            SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
            if (sprite != null) {
                sprite.flipX = true;
            }
        }

        // grab the fake animations, or add them if the prefab doesn't have them yet
        unitAnimator = GetComponent<UnitAnimator>();
        if (unitAnimator == null) {
            unitAnimator = gameObject.AddComponent<UnitAnimator>();
        }
    }

    // runs every frame. decides whether to walk or attack.
    void Update()
    {
        // dead units do nothing
        if (IsDead() == true) {
            return;
        }

        // count down the attack cooldown
        attackTimer = attackTimer - Time.deltaTime;

        // first look for an enemy unit, they get priority over the tower
        Unit enemyUnit = FindEnemyUnit();
        Tower enemyTower = null;

        // only check the tower if no enemy unit is in the way
        if (enemyUnit == null) {
            enemyTower = FindEnemyTower();
        }

        // nothing in range, keep walking
        if (enemyUnit == null && enemyTower == null) {
            Walk();
            return;
        }

        // something is in range, so stop
        SetWalking(false);

        // not ready to hit yet
        if (attackTimer > 0f) {
            return;
        }

        // hit whichever target we found
        if (enemyUnit != null) {
            enemyUnit.TakeDamage(damage);
        }
        if (enemyTower != null) {
            enemyTower.TakeDamage(damage);
        }

        // play attack animation and reset cooldown
        if (unitAnimator != null) {
            unitAnimator.PlayAttack();
        }
        attackTimer = attackCooldown;
    }

    // moves the unit forward a little bit.
    void Walk()
    {
        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime, Space.World);
        SetWalking(true);
    }

    // finds the closest enemy unit in front of us and within attack range. returns null if there isn't one.
    Unit FindEnemyUnit()
    {
        Unit closest = null;
        float closestDist = attackRange;          // anything farther than this is ignored

        foreach (Unit other in allUnits)
        {
            // skip ourselves
            if (other == this) {
                continue;
            }

            // skip teammates
            if (other.team == team) {
                continue;
            }

            // skip units that are already dead
            if (other.IsDead() == true) {
                continue;
            }

            // distance in the direction we're walking. negative means behind us
            float distance = (other.transform.position.x - transform.position.x) * direction;

            // skip units behind us
            if (distance < -0.1f) {
                continue;
            }

            // keep this one if it's the closest so far
            if (distance <= closestDist) {
                closest = other;
                closestDist = distance;
            }
        }

        return closest;
    }

    // finds the enemy tower if it's within attack range. returns null if it's too far away.
    Tower FindEnemyTower()
    {
        foreach (Tower tower in Tower.allTowers)
        {
            // skip our own tower
            if (tower.team == team) {
                continue;
            }

            // skip a tower that's already destroyed
            if (tower.IsDead() == true) {
                continue;
            }

            // measure to the front edge of the tower, not its center
            float distance = (tower.transform.position.x - transform.position.x) * direction;
            distance = distance - tower.halfWidth;

            if (distance <= attackRange) {
                return tower;
            }
        }

        return null;
    }

    // lowers health. destroys the unit when health hits zero.
    public void TakeDamage(int amount)
    {
        // already dead, ignore extra hits
        if (IsDead() == true) {
            return;
        }

        currentHealth = currentHealth - amount;

        // red flash and hit sound
        if (unitAnimator != null) {
            unitAnimator.PlayHit();
        }
        if (AudioManager.instance != null) {
            AudioManager.instance.PlayHit();
        }

        if (currentHealth <= 0) {
            currentHealth = 0;
            if (AudioManager.instance != null) {
                AudioManager.instance.PlayDeath();
            }
            Destroy(gameObject);                  // later: give coins, play a death sound
        }
    }

    // returns true if this unit has no health left.
    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    // tells the fake animations whether we're walking or standing.
    void SetWalking(bool walking)
    {
        if (unitAnimator != null) {
            unitAnimator.SetWalking(walking);
        }
    }
}
