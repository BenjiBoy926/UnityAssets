using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBlocker : MonoBehaviour, IPointerDownHandler
{
    #region Public Events
    public event System.Action OnClick;
    #endregion

    #region Public Methods
    public static UIBlocker Create(Transform requester)
    {
        // Make sure the requester is not null
        if (!requester) throw new System.ArgumentNullException(nameof(requester));

        Canvas canvas = requester.GetComponentInParent<Canvas>();

        // If there is no canvas in the parent then throw exception
        if (!canvas) throw new System.ArgumentException(
            $"Cannot request a UIBlocker for object {requester} " +
            $"because it is not a canvas or a child of a canvas!");

        // Make sure we have the root canvas
        canvas = canvas.rootCanvas;

        // Create a UI blocker from resources
        UIBlocker instance = ResourcesExtensions.InstantiateFromResources<UIBlocker>(nameof(UIBlocker), canvas.transform);
        instance.transform.SetAsLastSibling();

        return instance;
    }
    #endregion

    #region Interface Implementations
    public void OnPointerDown(PointerEventData data)
    {
        if (!data.used)
        {
            OnClick.Invoke();
            Destroy(gameObject);
            data.Use();
        }
    }
    #endregion
}
