﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpValues", menuName = "Power Ups", order = 1)]
public class PowerUpEditor : ScriptableObject
{
    [HideInInspector]
    public bool BZ_hasDuration = false;
    [HideInInspector]
    public float BZ_duration = 1f;
    [Header("Blast Zone - 1")]
    public float BZ_radius;
    public float BZ_power;
    public float BZ_upwardForce;
    public float BZ_playerPower;
    public float BZ_playerUpwardForce;


    [HideInInspector]
    public bool CO_hasDuration = true;
    [Header("Chillout - 2")]
    public float CO_duration;
    public float CO_radius;

    [HideInInspector]
    public bool GF_hasDuration = true;
    [HideInInspector]
    public float GF_radius = 0f;
    [Header("Gotta Go Fast - 3")]
    public float GF_duration;
    public float GF_aiSpeedMultiplier;
    public float GF_playerSpeedMultiplier;

    [HideInInspector]
    public bool CTT_hasDuration = true;
    [HideInInspector]
    public float CTT_radius = 0f;
    [Header("Cant Touch This - 4")]
    public float CTT_duration;

    [HideInInspector]
    public bool SS_hasDuration = true;
    [HideInInspector]
    public float SS_radius = 0f;
    [Header("Sneaky Snake - 5")]
    public float SS_duration;

    [HideInInspector]
    public bool BS_hasDuration = true;
    [HideInInspector]
    public float BS_radius;
    [Header("Balls of Steel - 6")]
    public float BS_duration;
    public Material BS_material;

    [HideInInspector]
    public bool SB_hasDuration = true;
    [HideInInspector]
    public float SB_radius = 0f;
    [Header("Super Bounce - 7")]
    public float SB_duration;
    public float SB_bounceMultiplier;

    [HideInInspector]
    public bool CD_hasDuration = true;
    [Header("Calm Down - 8")]
    public float CD_duration;
    public float CD_radius;
    public float CD_aiSpeedMultiplier;
    public float CD_playerSpeedMultiplier;

    [HideInInspector]
    public bool DM_hasDuration = false;
    [Header("Dis Mine - 9")]
    public float DM_duration;
    public float DM_radius = 0f;
    public float DM_power;
    public float DM_upwardForce;
    public float DM_playerPower;
    public float DM_playerUpwardForce;
    public float DM_positionOffSet;
    public GameObject DM_disMine;

}
