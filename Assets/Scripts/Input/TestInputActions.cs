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
            ""name"": ""CameraInput"",
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
                },
                {
                    ""name"": ""LeftClick"",
                    ""type"": ""Button"",
                    ""id"": ""31c71d76-ea96-436a-83a9-08e459d1528b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""aeabcf32-4654-471f-965d-e22e4920bbed"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""d8fcf1f4-dbab-4dfb-a924-8ed6ee6c8194"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""70c2e2ff-11e0-45d7-8c05-7d8560e9eba4"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
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
                    ""name"": ""Stop"",
                    ""type"": ""Button"",
                    ""id"": ""ffd37f1f-9669-4d46-978d-706e3cea1147"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2eb25cf1-22c1-4f12-a791-377807256442"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Stop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // CameraInput
        m_CameraInput = asset.FindActionMap("CameraInput", throwIfNotFound: true);
        m_CameraInput_MiddleClickDown = m_CameraInput.FindAction("MiddleClickDown", throwIfNotFound: true);
        m_CameraInput_MousePosition = m_CameraInput.FindAction("MousePosition", throwIfNotFound: true);
        m_CameraInput_Scroll = m_CameraInput.FindAction("Scroll", throwIfNotFound: true);
        m_CameraInput_WASDMove = m_CameraInput.FindAction("WASDMove", throwIfNotFound: true);
        m_CameraInput_WASDPress = m_CameraInput.FindAction("WASDPress", throwIfNotFound: true);
        m_CameraInput_LeftClick = m_CameraInput.FindAction("LeftClick", throwIfNotFound: true);
        m_CameraInput_Pause = m_CameraInput.FindAction("Pause", throwIfNotFound: true);
        // ActionKey
        m_ActionKey = asset.FindActionMap("ActionKey", throwIfNotFound: true);
        m_ActionKey_Stop = m_ActionKey.FindAction("Stop", throwIfNotFound: true);
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

    // CameraInput
    private readonly InputActionMap m_CameraInput;
    private ICameraInputActions m_CameraInputActionsCallbackInterface;
    private readonly InputAction m_CameraInput_MiddleClickDown;
    private readonly InputAction m_CameraInput_MousePosition;
    private readonly InputAction m_CameraInput_Scroll;
    private readonly InputAction m_CameraInput_WASDMove;
    private readonly InputAction m_CameraInput_WASDPress;
    private readonly InputAction m_CameraInput_LeftClick;
    private readonly InputAction m_CameraInput_Pause;
    public struct CameraInputActions
    {
        private @TestInputActions m_Wrapper;
        public CameraInputActions(@TestInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MiddleClickDown => m_Wrapper.m_CameraInput_MiddleClickDown;
        public InputAction @MousePosition => m_Wrapper.m_CameraInput_MousePosition;
        public InputAction @Scroll => m_Wrapper.m_CameraInput_Scroll;
        public InputAction @WASDMove => m_Wrapper.m_CameraInput_WASDMove;
        public InputAction @WASDPress => m_Wrapper.m_CameraInput_WASDPress;
        public InputAction @LeftClick => m_Wrapper.m_CameraInput_LeftClick;
        public InputAction @Pause => m_Wrapper.m_CameraInput_Pause;
        public InputActionMap Get() { return m_Wrapper.m_CameraInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraInputActions set) { return set.Get(); }
        public void SetCallbacks(ICameraInputActions instance)
        {
            if (m_Wrapper.m_CameraInputActionsCallbackInterface != null)
            {
                @MiddleClickDown.started -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnMiddleClickDown;
                @MiddleClickDown.performed -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnMiddleClickDown;
                @MiddleClickDown.canceled -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnMiddleClickDown;
                @MousePosition.started -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnMousePosition;
                @Scroll.started -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnScroll;
                @Scroll.performed -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnScroll;
                @Scroll.canceled -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnScroll;
                @WASDMove.started -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnWASDMove;
                @WASDMove.performed -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnWASDMove;
                @WASDMove.canceled -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnWASDMove;
                @WASDPress.started -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnWASDPress;
                @WASDPress.performed -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnWASDPress;
                @WASDPress.canceled -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnWASDPress;
                @LeftClick.started -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnLeftClick;
                @LeftClick.performed -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnLeftClick;
                @LeftClick.canceled -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnLeftClick;
                @Pause.started -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_CameraInputActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_CameraInputActionsCallbackInterface = instance;
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
                @LeftClick.started += instance.OnLeftClick;
                @LeftClick.performed += instance.OnLeftClick;
                @LeftClick.canceled += instance.OnLeftClick;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public CameraInputActions @CameraInput => new CameraInputActions(this);

    // ActionKey
    private readonly InputActionMap m_ActionKey;
    private IActionKeyActions m_ActionKeyActionsCallbackInterface;
    private readonly InputAction m_ActionKey_Stop;
    public struct ActionKeyActions
    {
        private @TestInputActions m_Wrapper;
        public ActionKeyActions(@TestInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Stop => m_Wrapper.m_ActionKey_Stop;
        public InputActionMap Get() { return m_Wrapper.m_ActionKey; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ActionKeyActions set) { return set.Get(); }
        public void SetCallbacks(IActionKeyActions instance)
        {
            if (m_Wrapper.m_ActionKeyActionsCallbackInterface != null)
            {
                @Stop.started -= m_Wrapper.m_ActionKeyActionsCallbackInterface.OnStop;
                @Stop.performed -= m_Wrapper.m_ActionKeyActionsCallbackInterface.OnStop;
                @Stop.canceled -= m_Wrapper.m_ActionKeyActionsCallbackInterface.OnStop;
            }
            m_Wrapper.m_ActionKeyActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Stop.started += instance.OnStop;
                @Stop.performed += instance.OnStop;
                @Stop.canceled += instance.OnStop;
            }
        }
    }
    public ActionKeyActions @ActionKey => new ActionKeyActions(this);
    public interface ICameraInputActions
    {
        void OnMiddleClickDown(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnScroll(InputAction.CallbackContext context);
        void OnWASDMove(InputAction.CallbackContext context);
        void OnWASDPress(InputAction.CallbackContext context);
        void OnLeftClick(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
    public interface IActionKeyActions
    {
        void OnStop(InputAction.CallbackContext context);
    }
}
