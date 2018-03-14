﻿using HUX.Buttons;
using HUX.Interaction;
using HUX.Receivers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CompoundButtonClick : InteractionReceiver
{
    [SerializeField]
    private UnityEvent onClick;

    protected override void OnTapped(GameObject obj, InteractionManager.InteractionEventArgs eventArgs)
    {
        base.OnTapped(obj, eventArgs);
    }
}