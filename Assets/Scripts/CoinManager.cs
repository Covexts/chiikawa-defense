/*
 * file:        CoinManager.cs
 * project:     Chiikawa Defense
 * description: keeps track of the player's coins. coins go up every second,
 *              and the player can pay to upgrade how fast they come in.
 */

using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    // lets any script reach this with CoinManager.instance
    public static CoinManager instance;

    [Header("Coins")]
    public int coins = 100;              // starting coins
    public int coinRate = 10;            // coins earned per second

    [Header("Upgrade")]
    public int upgradeCost = 100;        // price of the next upgrade
    public int rateIncrease = 5;         // extra coins per second each upgrade
    public int costIncrease = 100;       // each upgrade costs this much more
    public int maxLevel = 5;             // most upgrades allowed

    [Header("UI")]
    public TMP_Text coinText;            // shows current coins
    public TMP_Text upgradeText;         // shows upgrade price

    private float timer;                 // counts up to one second
    private int level;                   // upgrades bought so far

    // runs first. saves this as the shared instance.
    void Awake()
    {
        instance = this;
    }

    // runs every frame. adds coins once per second and updates the text.
    void Update()
    {
        timer = timer + Time.deltaTime;

        if (timer >= 1f) {
            coins = coins + coinRate;
            timer = timer - 1f;
        }

        UpdateText();
    }

    // tries to pay. returns true if the player had enough coins.
    public bool SpendCoins(int amount)
    {
        if (coins < amount) {
            return false;
        }

        coins = coins - amount;
        return true;
    }

    // called by the upgrade button. makes coins come in faster.
    public void UpgradeRate()
    {
        // already maxed out
        if (level >= maxLevel) {
            return;
        }

        // not enough coins
        if (SpendCoins(upgradeCost) == false) {
            return;
        }

        coinRate = coinRate + rateIncrease;
        upgradeCost = upgradeCost + costIncrease;
        level = level + 1;

        if (AudioManager.instance != null) {
            AudioManager.instance.PlayClick();
        }
    }

    // refreshes the coin and upgrade labels.
    void UpdateText()
    {
        if (coinText != null) {
            coinText.text = "Coins: " + coins;
        }

        if (upgradeText != null) {
            if (level >= maxLevel) {
                upgradeText.text = "Coin Rate MAX";
            } else {
                upgradeText.text = "Upgrade (" + upgradeCost + ")";
            }
        }
    }
}
