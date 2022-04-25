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
                    ""name"": ""Scroll"",
                    ""type"": ""Value"",
                    ""id"": ""9b316619-da1d-4e84-94a1-fc28d6b51e62"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WASDMove"",
                    ""type"": ""PassThrough"",
                    ""id"": ""caa0823e-d8d7-469d-9518-d88884b0e489"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WASDPress"",
                    ""type"": ""Button"",
                    ""id"": ""5966a75a-c454-4d7a-b0b1-7814c1d12fd0"",
                    ""expectedControlType"": ""Button"",
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
                    ""id"": ""5e000dab-5e75-470a-84ff-36574ed8c4c1"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""e908ca87-06dd-4bc5-b3c0-4a608dc87a94"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASDMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9acdc553-da49-4cfa-9d6a-cb52615c0835"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASDMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""87361596-0221-4d3e-a9ca-08572a41a6f1"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASDMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""445d46bc-3ad5-46ac-82d9-5c5be94751e9"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASDMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ff5e6f5a-c30c-4c98-b792-611e0be3704a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASDMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5d05006d-8f62-4936-839b-b53a985ec9c9"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASDPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8bbdfb0b-c2c2-4e2f-9536-77087fbbd46a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASDPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3d83536e-d309-42c4-b14a-d1152ae4240a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASDPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e733671-f41f-491c-8266-1226e8cbb1ed"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASDPress"",
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
        m_MouseInput_Scroll = m_MouseInput.FindAction("Scroll", throwIfNotFound: true);
        m_MouseInput_WASDMove = m_MouseInput.FindAction("WASDMove", throwIfNotFound: true);
        m_MouseInput_WASDPress = m_MouseInput.FindAction("WASDPress", throwIfNotFound: true);
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
    private readonly InputAction m_MouseInput_Scroll;
    private readonly InputAction m_MouseInput_WASDMove;
    private readonly InputAction m_MouseInput_WASDPress;
    public struct MouseInputActions
    {
        private @TestInputActions m_Wrapper;
        public MouseInputActions(@TestInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MiddleClickDown => m_Wrapper.m_MouseInput_MiddleClickDown;
        public InputAction @MousePosition => m_Wrapper.m_MouseInput_MousePosition;
        public InputAction @Scroll => m_Wrapper.m_MouseInput_Scroll;
        public InputAction @WASDMove => m_Wrapper.m_MouseInput_WASDMove;
        public InputAction @WASDPress => m_Wrapper.m_MouseInput_WASDPress;
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
                @Scroll.started -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnScroll;
                @Scroll.performed -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnScroll;
                @Scroll.canceled -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnScroll;
                @WASDMove.started -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnWASDMove;
                @WASDMove.performed -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnWASDMove;
                @WASDMove.canceled -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnWASDMove;
                @WASDPress.started -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnWASDPress;
                @WASDPress.performed -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnWASDPress;
                @WASDPress.canceled -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnWASDPress;
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
                @Scroll.started += instance.OnScroll;
                @Scroll.performed += instance.OnScroll;
                @Scroll.canceled += instance.OnScroll;
                @WASDMove.started += instance.OnWASDMove;
                @WASDMove.performed += instance.OnWASDMove;
                @WASDMove.canceled += instance.OnWASDMove;
                @WASDPress.started += instance.OnWASDPress;
                @WASDPress.performed += instance.OnWASDPress;
                @WASDPress.canceled += instance.OnWASDPress;
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
        void OnScroll(InputAction.CallbackContext context);
        void OnWASDMove(InputAction.CallbackContext context);
        void OnWASDPress(InputAction.CallbackContext context);
    }
    public interface IActionKeyActions
    {
        void OnJump(InputAction.CallbackContext context);
    }
}
