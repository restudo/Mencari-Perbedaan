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

    public static event Action<ImageTransform> ChangeImageTransform;
    public static void CallChangeImageTransformEvent(ImageTransform imgTransform)
    {
        if (ChangeImageTransform != null)
            ChangeImageTransform(imgTransform);
    }
    
    public static event Action ResetImageTransform;
    public static void CallResetImageTransformEvent()
    {
        if (ResetImageTransform != null)
            ResetImageTransform();
    }
    
    public static event Action PlaySFX;
    public static void CallPlaySFXEvent()
    {
        if (PlaySFX != null)
            PlaySFX();
    }
    
    public static event Action ImagesEntry;
    public static void CallImagesEntryEvent()
    {
        if (ImagesEntry != null)
            ImagesEntry();
    }
    
    public static event Action DestroyHint;
    public static void CallDestroyHintEvent()
    {
        if (DestroyHint != null)
            DestroyHint();
    }
}
