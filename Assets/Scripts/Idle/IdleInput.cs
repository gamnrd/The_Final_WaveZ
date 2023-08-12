using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class IdleInput : MonoBehaviour
{
    public float touchSpeed = 100f;
    public Vector3 newPos;
    private float minScroll = 2.0f;

    public Vector3 startPos;
    public Vector3 endPos;

    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchTapAction;
    private InputAction touchAndMoveAction;
    [SerializeField] Vector2 touchDistanceToMove;

    [Header("Events")]
    [SerializeField] private GameEvent shopTapped;

    private Camera cam;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cam = Camera.main;
        touchPositionAction = playerInput.actions.FindAction("TapPosition");
        touchTapAction = playerInput.actions.FindAction("Tap");
        touchAndMoveAction = playerInput.actions.FindAction("TouchAndMove");
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameMode != GameMode.Idle || IdleManager.instance.menuActive)
            return;

        if (Touch.activeFingers.Count == 1)
        {
            MoveCamera(Touch.activeTouches[0]);
        }
    }

    private void MoveCamera(Touch touch)
    {
        if (touch.phase != TouchPhase.Moved || touch.delta.sqrMagnitude < minScroll)
            return;

        newPos = new Vector3(touch.delta.normalized.x, 0, touch.delta.normalized.y) * Time.deltaTime * touchSpeed;
        TouchCameraController.Instance?.Move(newPos);
    }


    public void OnTap(InputAction.CallbackContext context)
    {
        if (IdleManager.instance.menuActive || !context.performed)
            return;
        
        if (context.performed)
        {
            Ray ray = cam.ScreenPointToRay(touchPositionAction.ReadValue<Vector2>());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Store"))
                {
                
                    if (hit.collider.gameObject.TryGetComponent<Shop>(out Shop _shop))
                    {
                        //Debug.Log(hit.collider.gameObject.name);
                        shopTapped.Raise(this, _shop);
                        //ShopUI.instance.SetupShop(_shop);
                    }
                }

            }
        }
    }
}
