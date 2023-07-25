using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyPoolableObject : PoolableObject
{
    public float AutoDestoryTime = 1f;
    private const string DisableMethodName = "Disable";

    public virtual void OnEnable()
    {
        CancelInvoke(DisableMethodName);
        Invoke(DisableMethodName, AutoDestoryTime);
    }

    public virtual void Disable()
    {
        base.OnDisable();
        gameObject.SetActive(false);
    }
}
