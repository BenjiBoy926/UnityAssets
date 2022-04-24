using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NaughtyAttributes;

/// <summary>
/// General component raises events from the event system and 
/// holds state on whether the pointer is present, whether the
/// graphic is selected, and other event system states for UI objects
/// </summary>
public class EventHandler : Selectable, 
    IPointerEnterHandler, IPointerExitHandler, 
    IPointerDownHandler, IPointerUpHandler,
    IPointerClickHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    ISelectHandler, IDeselectHandler
{
    #region Public Properties
    public bool IsPointerPresent => isPointerPresent;
    public bool IsPointerDown => isPointerDown;
    public bool IsPointerDragging => isPointerDragging;
    public bool IsSelected => isSelected;
    #endregion

    #region Events
    public event Action<PointerEventData> PointerEnterEvent = delegate { };
    public event Action<PointerEventData> PointerExitEvent = delegate { };

    public event Action<PointerEventData> PointerDownEvent = delegate { };
    public event Action<PointerEventData> PointerUpEvent = delegate { };

    public event Action<PointerEventData> PointerClickEvent = delegate { };

    public event Action<PointerEventData> BeginDragEvent = delegate { };
    public event Action<PointerEventData> DragEvent = delegate { };
    public event Action<PointerEventData> EndDragEvent = delegate { };

    public event Action<BaseEventData> SelectEvent = delegate { };
    public event Action<BaseEventData> DeselectEvent = delegate { };
    #endregion

    #region Private Fields
    [Header("Info")]

    [SerializeField, ReadOnly, AllowNesting]
    private bool isPointerPresent;
    [SerializeField, ReadOnly, AllowNesting]
    private bool isPointerDown;
    [SerializeField, ReadOnly, AllowNesting]
    private bool isPointerDragging;
    [SerializeField, ReadOnly, AllowNesting]
    private bool isSelected;
    #endregion

    #region Pointer Interface Implementations
    public override void OnPointerEnter(PointerEventData data)
    {
        base.OnPointerEnter(data);
        isPointerPresent = true;
        PointerEnterEvent.Invoke(data);
    }
    public override void OnPointerExit(PointerEventData data)
    {
        base.OnPointerExit(data);
        isPointerPresent = false;
        PointerExitEvent.Invoke(data);
    }

    public override void OnPointerDown(PointerEventData data)
    {
        base.OnPointerDown(data);
        isPointerDown = true;
        PointerDownEvent.Invoke(data);
    }
    public override void OnPointerUp(PointerEventData data)
    {
        base.OnPointerUp(data);
        isPointerDown = false;
        PointerUpEvent.Invoke(data);
    }

    public void OnPointerClick(PointerEventData data)
    {
        PointerClickEvent.Invoke(data);
    }

    public void OnBeginDrag(PointerEventData data)
    {
        isPointerDragging = true;
        BeginDragEvent.Invoke(data);
    }
    public void OnDrag(PointerEventData data)
    {
        DragEvent.Invoke(data);
    }
    public void OnEndDrag(PointerEventData data)
    {
        isPointerDragging = false;
        EndDragEvent.Invoke(data);
    }

    public override void OnSelect(BaseEventData data)
    {
        base.OnSelect(data);
        isSelected = true;
        SelectEvent.Invoke(data);
    }
    public override void OnDeselect(BaseEventData data)
    {
        base.OnDeselect(data);
        isSelected = false;
        DeselectEvent.Invoke(data);
    }
    #endregion
}
