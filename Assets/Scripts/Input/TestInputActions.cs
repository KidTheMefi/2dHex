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
                    ""name"": ""MiddleClickDown"",
                    ""type"": ""Button"",
                    ""id"": ""9295f834-5a43-49b4-ac65-ff1c037faf15"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a91838ca-efae-44e9-a37d-75820b2c3b94"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MiddleClickUp"",
                    ""type"": ""Button"",
                    ""id"": ""5539f89b-cb68-40dd-bab5-a100fe16a660"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Scroll"",
                    ""type"": ""Value"",
                    ""id"": ""9b316619-da1d-4e84-94a1-fc28d6b51e62"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""146b8565-2bcc-4be0-ac4b-ff53cc316864"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MiddleClickDown"",
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
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6928ac4a-0db1-4c46-bbb2-39f558dc3a88"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MiddleClickUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5e000dab-5e75-470a-84ff-36574ed8c4c1"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll"",
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
        m_MouseInput_MiddleClickDown = m_MouseInput.FindAction("MiddleClickDown", throwIfNotFound: true);
        m_MouseInput_MousePosition = m_MouseInput.FindAction("MousePosition", throwIfNotFound: true);
        m_MouseInput_MiddleClickUp = m_MouseInput.FindAction("MiddleClickUp", throwIfNotFound: true);
        m_MouseInput_Scroll = m_MouseInput.FindAction("Scroll", throwIfNotFound: true);
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
    private readonly InputAction m_MouseInput_MiddleClickDown;
    private readonly InputAction m_MouseInput_MousePosition;
    private readonly InputAction m_MouseInput_MiddleClickUp;
    private readonly InputAction m_MouseInput_Scroll;
    public struct MouseInputActions
    {
        private @TestInputActions m_Wrapper;
        public MouseInputActions(@TestInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MiddleClickDown => m_Wrapper.m_MouseInput_MiddleClickDown;
        public InputAction @MousePosition => m_Wrapper.m_MouseInput_MousePosition;
        public InputAction @MiddleClickUp => m_Wrapper.m_MouseInput_MiddleClickUp;
        public InputAction @Scroll => m_Wrapper.m_MouseInput_Scroll;
        public InputActionMap Get() { return m_Wrapper.m_MouseInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseInputActions set) { return set.Get(); }
        public void SetCallbacks(IMouseInputActions instance)
        {
            if (m_Wrapper.m_MouseInputActionsCallbackInterface != null)
            {
                @MiddleClickDown.started -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnMiddleClickDown;
                @MiddleClickDown.performed -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnMiddleClickDown;
                @MiddleClickDown.canceled -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnMiddleClickDown;
                @MousePosition.started -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnMousePosition;
                @MiddleClickUp.started -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnMiddleClickUp;
                @MiddleClickUp.performed -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnMiddleClickUp;
                @MiddleClickUp.canceled -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnMiddleClickUp;
                @Scroll.started -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnScroll;
                @Scroll.performed -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnScroll;
                @Scroll.canceled -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnScroll;
            }
            m_Wrapper.m_MouseInputActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MiddleClickDown.started += instance.OnMiddleClickDown;
                @MiddleClickDown.performed += instance.OnMiddleClickDown;
                @MiddleClickDown.canceled += instance.OnMiddleClickDown;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @MiddleClickUp.started += instance.OnMiddleClickUp;
                @MiddleClickUp.performed += instance.OnMiddleClickUp;
                @MiddleClickUp.canceled += instance.OnMiddleClickUp;
                @Scroll.started += instance.OnScroll;
                @Scroll.performed += instance.OnScroll;
                @Scroll.canceled += instance.OnScroll;
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
        void OnMiddleClickDown(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnMiddleClickUp(InputAction.CallbackContext context);
        void OnScroll(InputAction.CallbackContext context);
    }
    public interface IActionKeyActions
    {
        void OnJump(InputAction.CallbackContext context);
    }
}
