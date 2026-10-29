/*
 * file:        UnitAnimator.cs
 * project:     Chiikawa Defense
 * description: simple fake animations so each character only needs one png.
 *              walking = waddle side to side, attacking = quick stretch,
 *              getting hit = red flash. Unit.cs adds this automatically.
 */

using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    public float waddleSpeed = 10f;      // how fast it rocks side to side
    public float waddleAngle = 8f;       // how far it tilts (degrees)
    public float attackTime = 0.2f;      // how long the attack stretch lasts
    public float attackStretch = 0.25f;  // how much it stretches when attacking
    public float flashTime = 0.1f;       // how long the red flash lasts

    private SpriteRenderer sprite;       // the picture
    private Vector3 startScale;          // normal size
    private Color startColor;            // normal color
    private bool walking;                // true while moving
    private float attackTimer;           // counts down during the stretch
    private float flashTimer;            // counts down during the flash

    // runs once. remembers the normal size and color.
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        startScale = transform.localScale;

        if (sprite != null) {
            startColor = sprite.color;
        }
    }

    // called by Unit.cs. true = walking, false = standing still.
    public void SetWalking(bool isWalking)
    {
        walking = isWalking;
    }

    // called by Unit.cs when it attacks.
    public void PlayAttack()
    {
        attackTimer = attackTime;
    }

    // called by Unit.cs when it gets hit.
    public void PlayHit()
    {
        flashTimer = flashTime;
    }

    // runs every frame. updates the waddle, stretch, and flash.
    void Update()
    {
        // waddle: tilt back and forth while walking
        float angle = 0f;
        if (walking == true) {
            angle = Mathf.Sin(Time.time * waddleSpeed) * waddleAngle;
        }
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // attack: get wider and shorter for a moment
        float stretch = 0f;
        if (attackTimer > 0f) {
            attackTimer = attackTimer - Time.deltaTime;
            stretch = attackStretch;
        }
        transform.localScale = new Vector3(startScale.x * (1f + stretch), startScale.y * (1f - stretch / 2f), startScale.z);

        // hit: flash red for a moment
        if (sprite == null) {
            return;
        }
        if (flashTimer > 0f) {
            flashTimer = flashTimer - Time.deltaTime;
            sprite.color = Color.red;
        } else {
            sprite.color = startColor;
        }
    }
}
