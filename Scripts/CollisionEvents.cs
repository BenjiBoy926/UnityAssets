using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvents : MonoBehaviour
{
    #region Public Properties
    [field: SerializeField]
    public UnityEvent<Collision> CollisionEnterEvent { get; private set; }
    [field: SerializeField]
    public UnityEvent<Collision> CollisionStayEvent { get; private set; }
    [field: SerializeField]
    public UnityEvent<Collision> CollisionExitEvent { get; private set; }

    [field: Space]

    [field: SerializeField]
    public UnityEvent<Collider> TriggerEnterEvent { get; private set; }
    [field: SerializeField]
    public UnityEvent<Collider> TriggerStayEvent { get; private set; }
    [field: SerializeField]
    public UnityEvent<Collider> TriggerExitEvent { get; private set; }
    #endregion

    #region Monobehaviour Messages
    private void OnCollisionEnter(Collision collision)
    {
        CollisionEnterEvent.Invoke(collision);   
    }
    private void OnCollisionStay(Collision collision)
    {
        CollisionStayEvent.Invoke(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        CollisionExitEvent.Invoke(collision);
    }
    private void OnTriggerEnter(Collider other)
    {
        TriggerEnterEvent.Invoke(other);
    }
    private void OnTriggerStay(Collider other)
    {
        TriggerStayEvent.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        TriggerExitEvent.Invoke(other);
    }
    #endregion
}
