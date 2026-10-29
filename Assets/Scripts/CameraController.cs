/*
 * file:        CameraController.cs
 * project:     Chiikawa Defense
 * description: moves the camera left with A and right with D. stops at the
 *              edges so the player can't scroll past the towers.
 *              uses unity's new input system (the default in unity 6).
 */

using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 8f;         // how fast the camera scrolls
    public float minX = -4f;             // farthest left the camera can go
    public float maxX = 4f;              // farthest right the camera can go

    // runs every frame. moves the camera based on A and D.
    void Update()
    {
        // no keyboard plugged in
        if (Keyboard.current == null) {
            return;
        }

        float move = 0f;

        if (Keyboard.current.aKey.isPressed == true) {
            move = move - 1f;
        }
        if (Keyboard.current.dKey.isPressed == true) {
            move = move + 1f;
        }

        // move the camera
        Vector3 position = transform.position;
        position.x = position.x + move * moveSpeed * Time.deltaTime;

        // keep it inside the edges
        if (position.x < minX) {
            position.x = minX;
        }
        if (position.x > maxX) {
            position.x = maxX;
        }

        transform.position = position;
    }
}
