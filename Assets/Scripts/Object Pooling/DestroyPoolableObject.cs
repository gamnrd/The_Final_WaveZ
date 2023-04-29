using UnityEngine;

public class DestroyPoolableObject : PoolableObject
{
    private const string DisableMethodName = "Disable";

    public virtual void OnEnable()
    {

    }

    public virtual void Disable()
    {
        //base.OnDisable();
        gameObject.SetActive(false);
    }
}
