﻿#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BlockBaseClass
{
    [SerializeField]
    private uint _blockID;
    [SerializeField]
    private Vector2Int _orientation = Vector2Int.zero;
    [SerializeField]
    private float _health;
    [SerializeField]
    private int _mass;
    [SerializeField]
    private int _armor;
    [SerializeField]
    private float _blastResistance;
    [SerializeField]
    private Vector2Int _size = new Vector2Int(1,1);
    private IBlock _block;
    private MonoBehaviour _parentClass;
    private ShipGrid _shipGrid;
    private int _gridID;
    [SerializeField]
    private Transform _transform;
    public BlockBaseClass(float health, int mass, int armor, IBlock block, MonoBehaviour parentClass, ShipGrid shipGrid)
    {
        _health = health;
        _mass = mass;
        _armor = armor;
        _block = block;
        _parentClass = parentClass;
        _shipGrid = shipGrid;
    }
    public Vector2Int orientation
    {
        get
        {
            if (_orientation == Vector2Int.zero)
            {
                _orientation = Extensions.GetOrientation(_parentClass.transform.localRotation.eulerAngles.z);
            }
            return _orientation;
        }
        set
        {
            
        }
    }
    public float health
    {
        get => _health;
        set
        {
            _health = value;
            if (value <= .0f)
            {
                shipGrid.RemoveFromGrid(transform);
            }
        }
    }
    public IBlock block
    {
        get => _block;
        set => _block = value;
    }
    public MonoBehaviour parentClass
    {
        get => _parentClass;
        set => _parentClass = value;
    }
    public int mass => _mass;
    public int armor => _armor;
    public int gridID
    {
        get => _gridID;
        set => _gridID = value;
    }
    public ShipGrid shipGrid
    {
        get => _shipGrid;
        set => _shipGrid = value;
    }
    public float blastResistance
    {
        get => _blastResistance;
        set => _blastResistance = value;
    }
    public uint blockID => _blockID;
    public Transform transform
    {
        get
        {
            _transform = _transform != null ? _transform : parentClass.transform;
            return _transform;
        }
    }
    public Vector2Int size
    {
        get => _size;
        set => _size = value;
    }
    public Vector2Int effectiveSize => _parentClass.transform.EffectiveSize(size);
}
