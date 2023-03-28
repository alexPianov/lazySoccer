using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
public class BaseAction<T>
{
    [SerializeField] private T _value;
    public Action<T> onActionUser;
    public virtual T Value
    {
        get { return _value; }
        set
        {
            _value = value;
            onActionUser?.Invoke(_value);
        }
    }
#if UNITY_EDITOR
    [Button, LabelText("Update value")] private void UpdateName(T value) => Value = value;

#endif
}
