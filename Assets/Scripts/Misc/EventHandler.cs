using UnityEngine;
using System;

public static class EventHandler
{
    public static event Action<float> DifferenceClicked;
    public static void CallDifferenceClickedEvent(float animDuration)
    {
        if (DifferenceClicked != null)
            DifferenceClicked(animDuration);
    }
    
    public static event Action<string> DifferenceClickedFeedback;
    public static void CallDifferenceClickedFeedbackEvent(string side)
    {
        if (DifferenceClickedFeedback != null)
            DifferenceClickedFeedback(side);
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
    
    public static event Action HintAnim;
    public static void CallHintAnimEvent()
    {
        if (HintAnim != null)
            HintAnim();
    }
    
    public static event Action Transparent;
    public static void CallTransparentEvent()
    {
        if (Transparent != null)
            Transparent();
    }
    
    public static event Action<Vector3> SetToPointPosition;
    public static void CallSetToPointPositionEvent(Vector3 pointPos)
    {
        if (SetToPointPosition != null)
            SetToPointPosition(pointPos);
    }
}
