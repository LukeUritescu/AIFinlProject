﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    void ObserverUpdate(System.Object sender, Transform message);
}
