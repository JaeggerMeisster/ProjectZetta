﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NewThrust
{
    public class ThrustVector
    {
        public float thrust;
        public List<IThruster> thrusterMembers;
        public Vector2 forceDirection;
        public Vector2 thrustVector => forceDirection * thrust;
        public ThrustVector(Vector2 forceDirection)
        {
            thrusterMembers = new List<IThruster>();
            this.forceDirection = forceDirection;
        }
        public bool isFiring
        {
            set
            {
                thrusterMembers.ForEach(item => item.trailManager.isFiring = value);
            }
        }
        public float strength
        {
            set
            {

            }
        }
    }
    public class ThrusterGroup
    {
        private Dictionary<Common.Orientation, bool> orientationShouldFire = new Dictionary<Common.Orientation, bool>();

        public ThrusterGroup()
        {
            orientationShouldFire = new Dictionary<Common.Orientation, bool>()
            {
                { Common.Orientation.forward, false},
                { Common.Orientation.right,   false},
                { Common.Orientation.backward,false},
                { Common.Orientation.left,    false}
            };
        }
        public bool this [Common.Orientation index]
        {
            get => orientationShouldFire[index];
            set => orientationShouldFire[index] = value;
        }
        
    }
    public struct MinMax
    {
        public float min;
        public float max;

        public MinMax(float centre, float offset)
        {
            min = centre.AddAngle(-90 + offset);
            max = centre.AddAngle(90 - offset);
        }
    }

    private float offset = 2;

    public Dictionary<Common.Orientation, ThrustVector> thrustVectors;

    Dictionary<Common.Orientation, MinMax> thrusterRanges = new Dictionary<Common.Orientation, MinMax>();

    ShipGrid parentClass;

    public NewThrust(ShipGrid parentClass, List<IThruster> thrusters = null)
    {
        thrustVectors = new Dictionary<Common.Orientation, ThrustVector>();
        this.parentClass = parentClass;
        thrustVectors.Add(Common.Orientation.forward,  new ThrustVector(Vector2.right));
        thrustVectors.Add(Common.Orientation.left,     new ThrustVector(Vector2.up));
        thrustVectors.Add(Common.Orientation.backward, new ThrustVector(Vector2.left));
        thrustVectors.Add(Common.Orientation.right,    new ThrustVector(Vector2.down));
        //Initializes thruster ranges
        for (int i = 0; i < 4; i++)
            thrusterRanges[(Common.Orientation)i] = new MinMax(((Common.Orientation)i).GetRotation(), offset);
        
        if (thrusters == null)
            return;
        thrusters.ForEach(thruster => AddToThrusterGroup(thruster));
    }
    public void AddToThrusterGroup(IThruster thruster)
    {
        var orientation = ((IBlock)thruster).GetOrientation();
        var tempThrustVector = thrustVectors[orientation];
        tempThrustVector.thrust += thruster.thrust;
        tempThrustVector.thrusterMembers.Add(thruster);
        thrustVectors[orientation] = tempThrustVector;
    }
    private void RemoveFromThrusterGroup(IThruster thruster)
    {
        var orientation = ((IBlock)thruster).GetOrientation();
        var tempThrustVector = thrustVectors[orientation];
        tempThrustVector.thrust -= thruster.thrust;
        tempThrustVector.thrusterMembers.Remove(thruster);
        thrustVectors[orientation] = tempThrustVector;
    }
    public void FireThrustersInDirection(float zRotDirection = 0)
    {
        Common.Orientation? orientation0 = null;
        Common.Orientation? orientation1 = null;
        //loops through the thruster ranges and finds thruster groups that can thrust forward.
        foreach (var thrusterRange in thrusterRanges)
        {
            if (zRotDirection.AngleIsInRange(thrusterRange.Value))
            {
                if (orientation0 == null)
                    orientation0 = thrusterRange.Key;
                else
                    orientation1 = thrusterRange.Key;
            }
        }

        var multiplier0 = 1f;
        var multiplier1 = 1f;

        for (int i = 0; i < 4; i++)
        {
            var orientation = (Common.Orientation)i;
            thrustVectors[orientation].isFiring = false;
        }

        if (orientation0 != null)
        {
            FireThrusterGroup((Common.Orientation)orientation0);
            thrustVectors[(Common.Orientation)orientation0].isFiring = true;
        }

        if (orientation1 != null)
        {
            FireThrusterGroup((Common.Orientation)orientation1);
            thrustVectors[(Common.Orientation)orientation1].isFiring = true;
        }



    }
    private void GetResultantAndMagnitude()
    {

    }
    private void FireThrusterGroup(Common.Orientation orientation, float multiplier = 1)
    {
        var thrustVector = thrustVectors[orientation];
        multiplier = Mathf.Clamp01(multiplier);
        parentClass._rb2d.AddForce(RotationUtilities.RotateVector2(thrustVector.thrustVector * multiplier, parentClass.grid.rotation.eulerAngles.z));
    }
    





}