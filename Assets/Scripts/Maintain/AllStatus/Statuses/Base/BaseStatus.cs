using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace LazySoccer.Status
{
    public class BaseStatus<T> : MonoBehaviour
    {
        #region Base Action

        [SerializeField] protected T status;
        public Action<T> OnStateStatusAction;
        public virtual T StatusAction
        {
            get => status;
            set
            {
                status = value;
                OnStateStatusAction?.Invoke(status);
            }
        }
        public virtual void SetAction(T action)
        {
            StatusAction = action;
        }
        public virtual void SetAction(string action)
        {
            StatusAction = (T)Enum.Parse(typeof(T), action);
        }
        #endregion

#if UNITY_EDITOR
        [Title("Editor Mode")]
        [Button]
        private void AutoCheckStatus(T status) => StatusAction = status;
#endif
    }
}
