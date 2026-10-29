/*
 * file:        Tower.cs
 * project:     Chiikawa Defense
 * description: a base that units attack. goes on both your tower and the
 *              enemy tower. when one hits zero health, the game is over.
 */

using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    // every tower in the scene. units use this to find the enemy tower
    public static List<Tower> allTowers = new List<Tower>();

    public Team team = Team.Player;      // whose tower this is
    public int maxHealth = 1000;         // starting health
    public float halfWidth = 0.5f;       // half the tower's width, so units stop at its edge

    private int currentHealth;           // health right now

    // runs when the tower appears. adds it to the shared list.
    void OnEnable()
    {
        allTowers.Add(this);
    }

    // runs when the tower is removed. takes it out of the shared list.
    void OnDisable()
    {
        allTowers.Remove(this);
    }

    // runs before anything else. sets starting health.
    void Awake()
    {
        currentHealth = maxHealth;
    }

    // lowers health. ends the game when health hits zero.
    public void TakeDamage(int amount)
    {
        // already destroyed, ignore extra hits
        if (IsDead() == true) {
            return;
        }

        currentHealth = currentHealth - amount;

        if (currentHealth <= 0) {
            currentHealth = 0;

            // no game manager in the scene, nothing to show
            if (GameManager.instance == null) {
                return;
            }

            // our tower died, we lose. enemy tower died, we win
            if (team == Team.Player) {
                GameManager.instance.GameOver(false);
            }
            if (team == Team.Enemy) {
                GameManager.instance.GameOver(true);
            }
        }
    }

    // returns true if this tower has no health left.
    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    // returns health as a number from 0 to 1. used later for the health bar and for spawning the chimera.
    public float GetHealthPercent()
    {
        return (float)currentHealth / maxHealth;
    }
}
