using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace LazySoccer.Status
{
    public class AuthenticationStatus : BaseStatus<StatusAuthentication>
    {
        [SerializeField] private StatusAuthentication back;
        public override StatusAuthentication StatusAction 
        { 
            set {

                if (value == StatusAuthentication.Back)
                {
                    status = back;
                }
                else
                    status = value;
                OnStateStatusAction?.Invoke(status);
            }
        }
    }
}
