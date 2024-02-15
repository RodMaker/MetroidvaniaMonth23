using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bardent
{
    public class RebindMenuManager : MonoBehaviour
    {
        public InputActionReference MoveRef, PrimaryAttackRef, SecondaryAttackRef, JumpRef, DashRef, GrabRef, InteractRef;

        private void OnEnable()
        {
            MoveRef.action.Disable();
            PrimaryAttackRef.action.Disable();
            SecondaryAttackRef.action.Disable();
            JumpRef.action.Disable();
            DashRef.action.Disable();
            GrabRef.action.Disable();
            InteractRef.action.Disable();
        }

        private void OnDisable()
        {
            MoveRef.action.Enable();
            PrimaryAttackRef.action.Enable();
            SecondaryAttackRef.action.Enable();
            JumpRef.action.Enable();
            DashRef.action.Enable();
            GrabRef.action.Enable();
            InteractRef.action.Enable();
        }
    }
}
