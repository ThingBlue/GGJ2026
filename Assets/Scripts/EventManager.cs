using UnityEngine;
using UnityEngine.Events;

/*
 * STEPS FOR USING UNITY EVENTS:
 *     1. ADD NEW EVENT
 *         public UnityEvent eventName;
 *     2. INITIALIZE EVENT
 *         if (eventName == null) eventName = new UnityEvent();
 *     3. ADD LISTENER TO EVENT
 *         EventManager.instance.eventName.AddListener(eventCallbackName);
 *     4. INVOKE EVENT CALLBACKS
 *         EventManager.instance.eventName.Invoke();
 */

public class EventManager : MonoBehaviour
{
    #region Dialogue events

    public UnityEvent destroyDialogueOptionButtonsEvent;

    #endregion

    public static EventManager instance;
    private void Awake()
    {
        // Singleton
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        // Initialize events
        if (destroyDialogueOptionButtonsEvent == null) destroyDialogueOptionButtonsEvent = new UnityEvent();
    }
}
