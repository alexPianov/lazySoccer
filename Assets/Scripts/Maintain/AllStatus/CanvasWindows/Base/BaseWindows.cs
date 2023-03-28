using LazySoccer.Status;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace LazySoccer.Windows
{
    public class BaseWindows<T> : MonoBehaviour
    {
        [SerializeField] protected BaseStatus<T> baseStatus;
        [Title("")]
        [SerializeField] private GameObject container;

        [SerializeField] private List<T> StatusGames;

        [Title("Action")]
        [SerializeField] private bool Activity;
        [SerializeField, ShowIf(nameof(Activity))] private UnityEvent unityEvent;

        public virtual void Awake()
        {
            
        }
        
        public virtual void Start()
        {
            baseStatus.OnStateStatusAction += UpdatesStatusAction;
            UpdatesStatusAction(baseStatus.StatusAction);
        }

        protected void UpdatesStatusAction(T status)
        {
            ActiveContainer(StatusGames.Contains(status));
        }

        private void ActiveContainer(bool active)
        {
            if(!container) return;
            
            container.SetActive(active);
            
            if (active)
            {
                unityEvent?.Invoke();
            }
        }
    }
}
