// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input/TestInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @TestInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @TestInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TestInputActions"",
    ""maps"": [
        {
            ""name"": ""MouseInput"",
            ""id"": ""f9cac46f-95df-43ee-98af-cf797007e189"",
            ""actions"": [
                {
                    ""name"": ""MapClick"",
                    ""type"": ""Button"",
                    ""id"": ""9295f834-5a43-49b4-ac65-ff1c037faf15"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MapClickVector"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a91838ca-efae-44e9-a37d-75820b2c3b94"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MapClickUp"",
                    ""type"": ""Button"",
                    ""id"": ""5539f89b-cb68-40dd-bab5-a100fe16a660"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""146b8565-2bcc-4be0-ac4b-ff53cc316864"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MapClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0a8c4c4f-fb4c-4b01-9348-fa3c58b4423f"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MapClickVector"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6928ac4a-0db1-4c46-bbb2-39f558dc3a88"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MapClickUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""ActionKey"",
            ""id"": ""57fda782-503b-466b-840b-e3f5b26e7334"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""4256988a-ab86-43ba-b410-547e86ccef6e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1a39d664-b04d-41bd-83c9-a9d8f8399999"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // MouseInput
        m_MouseInput = asset.FindActionMap("MouseInput", throwIfNotFound: true);
        m_MouseInput_MapClick = m_MouseInput.FindAction("MapClick", throwIfNotFound: true);
        m_MouseInput_MapClickVector = m_MouseInput.FindAction("MapClickVector", throwIfNotFound: true);
        m_MouseInput_MapClickUp = m_MouseInput.FindAction("MapClickUp", throwIfNotFound: true);
        // ActionKey
        m_ActionKey = asset.FindActionMap("ActionKey", throwIfNotFound: true);
        m_ActionKey_Jump = m_ActionKey.FindAction("Jump", throwIfNotFound: true);
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

    // MouseInput
    private readonly InputActionMap m_MouseInput;
    private IMouseInputActions m_MouseInputActionsCallbackInterface;
    private readonly InputAction m_MouseInput_MapClick;
    private readonly InputAction m_MouseInput_MapClickVector;
    private readonly InputAction m_MouseInput_MapClickUp;
    public struct MouseInputActions
    {
        private @TestInputActions m_Wrapper;
        public MouseInputActions(@TestInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MapClick => m_Wrapper.m_MouseInput_MapClick;
        public InputAction @MapClickVector => m_Wrapper.m_MouseInput_MapClickVector;
        public InputAction @MapClickUp => m_Wrapper.m_MouseInput_MapClickUp;
        public InputActionMap Get() { return m_Wrapper.m_MouseInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseInputActions set) { return set.Get(); }
        public void SetCallbacks(IMouseInputActions instance)
        {
            if (m_Wrapper.m_MouseInputActionsCallbackInterface != null)
            {
                @MapClick.started -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnMapClick;
                @MapClick.performed -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnMapClick;
                @MapClick.canceled -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnMapClick;
                @MapClickVector.started -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnMapClickVector;
                @MapClickVector.performed -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnMapClickVector;
                @MapClickVector.canceled -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnMapClickVector;
                @MapClickUp.started -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnMapClickUp;
                @MapClickUp.performed -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnMapClickUp;
                @MapClickUp.canceled -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnMapClickUp;
            }
            m_Wrapper.m_MouseInputActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MapClick.started += instance.OnMapClick;
                @MapClick.performed += instance.OnMapClick;
                @MapClick.canceled += instance.OnMapClick;
                @MapClickVector.started += instance.OnMapClickVector;
                @MapClickVector.performed += instance.OnMapClickVector;
                @MapClickVector.canceled += instance.OnMapClickVector;
                @MapClickUp.started += instance.OnMapClickUp;
                @MapClickUp.performed += instance.OnMapClickUp;
                @MapClickUp.canceled += instance.OnMapClickUp;
            }
        }
    }
    public MouseInputActions @MouseInput => new MouseInputActions(this);

    // ActionKey
    private readonly InputActionMap m_ActionKey;
    private IActionKeyActions m_ActionKeyActionsCallbackInterface;
    private readonly InputAction m_ActionKey_Jump;
    public struct ActionKeyActions
    {
        private @TestInputActions m_Wrapper;
        public ActionKeyActions(@TestInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_ActionKey_Jump;
        public InputActionMap Get() { return m_Wrapper.m_ActionKey; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ActionKeyActions set) { return set.Get(); }
        public void SetCallbacks(IActionKeyActions instance)
        {
            if (m_Wrapper.m_ActionKeyActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_ActionKeyActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_ActionKeyActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_ActionKeyActionsCallbackInterface.OnJump;
            }
            m_Wrapper.m_ActionKeyActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
            }
        }
    }
    public ActionKeyActions @ActionKey => new ActionKeyActions(this);
    public interface IMouseInputActions
    {
        void OnMapClick(InputAction.CallbackContext context);
        void OnMapClickVector(InputAction.CallbackContext context);
        void OnMapClickUp(InputAction.CallbackContext context);
    }
    public interface IActionKeyActions
    {
        void OnJump(InputAction.CallbackContext context);
    }
}
