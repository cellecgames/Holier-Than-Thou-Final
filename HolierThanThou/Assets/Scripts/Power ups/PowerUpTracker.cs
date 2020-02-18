﻿//2019-10-30 Shijun Guo: Loading Icons for powerups
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpTracker : MonoBehaviour
{
    public PowerUp slot1;
    public PowerUp slot2;

    private bool activated1;
    private bool activated2;
    private bool canActivate1;
    private bool canActivate2;

    private float powerTimer1;
    private float powerTimer2;

    private GameObject _player;

    //An array to contain all the power-ups' images. The sequence same as PowerUpBox
    public Sprite[] powerupImages;
    //Images for the power-ups
    public Image buttonImage1;
    public Image buttonImage2;
    public Text itemButton1;
    public Text itemButton2;
    private Competitor competitor;

    private PlayerAchievementTracker PAT;

    public bool usedPowerup = false;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        canActivate1 = true;
        canActivate2 = true;
        UpdateUI();
        competitor = GetComponent<Competitor>();

        //Testing for Loading Powerups
        //To fix the preload powerup forcibly
        slot1 = null;
        slot2 = null;

        PAT = gameObject.GetComponent<PlayerAchievementTracker>();
    }


    private void Update()
    {
        if (activated1)
        {
            canActivate1 = false;
            powerTimer1 -= Time.deltaTime;
            UpdateUI();
        }

        if(powerTimer1 <= 0 && activated1)
        {
            activated1 = false;
            slot1.ResetEffects(competitor.Name);
            slot1 = null;
            canActivate1 = true;
            UpdateUI();
        }        

        if(activated2)
        {
            canActivate2 = false;
            powerTimer2 -= Time.deltaTime;
            UpdateUI();
        }

        if(powerTimer2 <= 0 && activated2)
        {
            activated2 = false;
            slot2.ResetEffects(competitor.Name);
            slot2 = null;
            canActivate2 = true;
            UpdateUI();
        }
    }

    public void UseItem1()
    {
        if (canActivate1)
        {
            if (slot1 != null)
            {

                if (slot1.hasDuration)
                {
                    powerTimer1 = slot1.duration;
                    activated1 = true;
                    slot1.ActivatePowerUp(competitor.Name, competitor.origin);
                    if(!usedPowerup)
                    {
                        PAT.TooglePowerupUsed();
                        usedPowerup = true;
                    }

                }
                else
                {
                    slot1.ActivatePowerUp(competitor.Name, competitor.origin);
                    slot1 = null;
                    if (!usedPowerup)
                    {
                        PAT.TooglePowerupUsed();
                        usedPowerup = true;
                    }

                }
                UpdateUI();
            }

        }
    }

    public void UseItem2()
    {
        if (canActivate2)
        {
            if (slot2 != null)
            {

                if (slot2.hasDuration)
                {
                    powerTimer2 = slot2.duration;
                    activated2 = true;
                    slot2.ActivatePowerUp(competitor.Name, competitor.origin);
                    if (!usedPowerup)
                    {
                        PAT.TooglePowerupUsed();
                        usedPowerup = true;
                    }
                }
                else
                {
                    slot2.ActivatePowerUp(competitor.Name, competitor.origin);
                    slot2 = null;
                    if (!usedPowerup)
                    {
                        PAT.TooglePowerupUsed();
                        usedPowerup = true;
                    }
                }
                UpdateUI();
            }

        }
    }

    public void UpdateUI()
    {
        if (slot1 != null)
        {
            itemButton1.text = "";
        }
        else
        {
            itemButton1.text = "No Item";
            ResetPowerUpIcon(1);
        }

        if (slot2 != null)
        {

            itemButton2.text = "";
        }
        else
        {
            itemButton2.text = "No Item";
            ResetPowerUpIcon(2);
        }
    }

    //The index of this function based on PowerUpBox
    //The index: 9 is the default image for null or background.
    public void LoadPowerUpIcon(int buttonNumber, int index)
    {
        switch (buttonNumber)
        {
            case 1:
                buttonImage1.sprite = powerupImages[index];
                break;
            case 2:
                buttonImage2.sprite = powerupImages[index];
                break;
            default:
                break;
        }
    }
    //Empty shoul be the backgrond image.
    public void ResetPowerUpIcon(int buttonNumber)
    {
        switch (buttonNumber)
        {
            case 1:
                buttonImage1.sprite = powerupImages[9];
                break;
            case 2:
                buttonImage2.sprite = powerupImages[9];
                break;
            default:
                break;
        }
    }
}
