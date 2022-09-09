//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Joicy/Controllers/ProjectileActionMap.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @ProjectileActionMap : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @ProjectileActionMap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ProjectileActionMap"",
    ""maps"": [
        {
            ""name"": ""Correctable"",
            ""id"": ""1a915606-dc40-4169-a393-79f41b8f9fc7"",
            ""actions"": [
                {
                    ""name"": ""ProjectileCorrection"",
                    ""type"": ""Value"",
                    ""id"": ""bc6304d1-a4a1-41f9-8f93-1965095a9bb8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""InvertVector2(invertX=false)"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Arrows"",
                    ""id"": ""7714f45b-ddd9-4e57-80b7-512f5daac11b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ProjectileCorrection"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4202ff62-f514-4255-adb8-657ac9dfa743"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ProjectileCorrection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""07338a1b-835a-4e5d-8698-2c80c9b5c467"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ProjectileCorrection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""72a48a6d-c606-4dbc-a602-c6efc903e988"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ProjectileCorrection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""380a4084-7357-43db-8764-8bfdb7ce3334"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ProjectileCorrection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Correctable
        m_Correctable = asset.FindActionMap("Correctable", throwIfNotFound: true);
        m_Correctable_ProjectileCorrection = m_Correctable.FindAction("ProjectileCorrection", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Correctable
    private readonly InputActionMap m_Correctable;
    private ICorrectableActions m_CorrectableActionsCallbackInterface;
    private readonly InputAction m_Correctable_ProjectileCorrection;
    public struct CorrectableActions
    {
        private @ProjectileActionMap m_Wrapper;
        public CorrectableActions(@ProjectileActionMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @ProjectileCorrection => m_Wrapper.m_Correctable_ProjectileCorrection;
        public InputActionMap Get() { return m_Wrapper.m_Correctable; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CorrectableActions set) { return set.Get(); }
        public void SetCallbacks(ICorrectableActions instance)
        {
            if (m_Wrapper.m_CorrectableActionsCallbackInterface != null)
            {
                @ProjectileCorrection.started -= m_Wrapper.m_CorrectableActionsCallbackInterface.OnProjectileCorrection;
                @ProjectileCorrection.performed -= m_Wrapper.m_CorrectableActionsCallbackInterface.OnProjectileCorrection;
                @ProjectileCorrection.canceled -= m_Wrapper.m_CorrectableActionsCallbackInterface.OnProjectileCorrection;
            }
            m_Wrapper.m_CorrectableActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ProjectileCorrection.started += instance.OnProjectileCorrection;
                @ProjectileCorrection.performed += instance.OnProjectileCorrection;
                @ProjectileCorrection.canceled += instance.OnProjectileCorrection;
            }
        }
    }
    public CorrectableActions @Correctable => new CorrectableActions(this);
    public interface ICorrectableActions
    {
        void OnProjectileCorrection(InputAction.CallbackContext context);
    }
}