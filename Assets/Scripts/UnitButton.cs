/*
 * file:        UnitButton.cs
 * project:     Chiikawa Defense
 * description: goes on each spawn button. spawns a unit if the player can
 *              afford it, and greys the button out when they can't.
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    public Unit unitPrefab;              // which unit this button spawns
    public Transform spawnPoint;         // where the unit appears
    public TMP_Text label;               // the text on the button

    private Button button;               // the button this script is on

    // runs once. grabs the button and sets its label.
    void Start()
    {
        button = GetComponent<Button>();

        if (label != null) {
            label.text = unitPrefab.name + " (" + unitPrefab.cost + ")";
        }
    }

    // runs every frame. greys out the button if the player is too broke.
    void Update()
    {
        if (CoinManager.instance.coins >= unitPrefab.cost) {
            button.interactable = true;
        } else {
            button.interactable = false;
        }
    }

    // called when the button is clicked. pays and spawns the unit.
    public void SpawnUnit()
    {
        if (CoinManager.instance.SpendCoins(unitPrefab.cost) == false) {
            return;
        }

        Instantiate(unitPrefab, spawnPoint.position, Quaternion.identity);

        if (AudioManager.instance != null) {
            AudioManager.instance.PlayClick();
        }
    }
}
