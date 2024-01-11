using UnityEngine;
using System;

public static class EventHandler
{
    public static event Action DifferenceClicked;
    public static void CallDifferenceClickedEvent()
    {
        if (DifferenceClicked != null)
            DifferenceClicked();
    }

    public static event Action DecreaseHealth;
    public static void CallDecreaseHealthEvent()
    {
        if (DecreaseHealth != null)
            DecreaseHealth();
    }

    public static event Action ChangeImageTransform;
    public static void CallChangeImageTransformEvent()
    {
        if (ChangeImageTransform != null)
            ChangeImageTransform();
    }
    
    public static event Action ResetImageTransform;
    public static void CallResetImageTransformEvent()
    {
        if (ResetImageTransform != null)
            ResetImageTransform();
    }
}
