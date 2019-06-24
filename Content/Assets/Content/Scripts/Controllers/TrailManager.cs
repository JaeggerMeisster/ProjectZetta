﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ara;

public class TrailManager : MonoBehaviour
{
    [SerializeField]
    private AraTrail _trailRenderer;
    [SerializeField]
    private float maxThickness;
    [SerializeField]
    private float slopeMultiplier;
    [SerializeField]
    private float slopeStep;

    private float currentThickness;
    public bool isFiring;

    void Update()
    {
        _trailRenderer.initialThickness = currentThickness;
        currentThickness.MixedInterpolate(isFiring ? maxThickness : 0, slopeMultiplier, slopeStep);
        isFiring = false;
    }
}