using System.Reflection;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TMP_InputFieldCustom : TMP_InputField
{
    public override void OnSubmit(BaseEventData eventData)
    {
        FieldInfo eventField = typeof(TMP_InputField).GetField("m_ProcessingEvent", BindingFlags.Instance | BindingFlags.NonPublic);
        Event m_ProcessingEvent = (Event)eventField.GetValue(this);
        var shouldContinue = KeyPressed(m_ProcessingEvent);
        if (shouldContinue != EditState.Finish) return;
        
        base.OnSubmit(eventData);
    }
}
