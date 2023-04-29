using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public ObjectPool parent;
    public bool isBeingUsed = false;

    public virtual void OnDisable()
    {
        isBeingUsed = false;
        parent.ReturnObjectToPool(this);
    }
}
