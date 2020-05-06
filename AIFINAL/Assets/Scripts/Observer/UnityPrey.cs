using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityHive : HiveMind
{
    private GameObject _gameObject;

    public UnityHive(GameObject g) : base()
    {
        _gameObject = g;
    }
}
