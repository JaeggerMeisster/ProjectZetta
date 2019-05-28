﻿#pragma warning disable 649
using UnityEngine;
public class Thruster : MonoBehaviour, IBlock, IThruster
{
    [SerializeField]
    private float _health;
    [SerializeField]
    private int _mass;
    [SerializeField]
    private int _armor;
    [SerializeField]
    private float _thrust;
    [SerializeField]
    private GameObject thruster;
    [SerializeField]
    private Animator animator;
    public float health {
        get => _health;
        set
        {
            _health = value;
            if (value <= .0f) SubtractFromGridAndDestroy();
        }
    }
    public float thrust {
        get => _thrust;
        set => _thrust = value;
    }
    public int mass => _mass;
    public int armor => _armor;
    public void SubtractFromGridAndDestroy()
    {
        Destroy(gameObject);
    }
    public void SetThrusterFlame(bool value, float strength = 0f)
    {
        animator.SetBool("IsFiring", value);
    }
    public void DebugThis()
    {
        Debug.Log("Rood id of " + transform.root.name + " is : " + transform.GetRootGridID());
    }
}
