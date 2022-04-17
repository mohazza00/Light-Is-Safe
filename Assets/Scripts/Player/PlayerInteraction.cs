using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Setting")]
    public float InteractionDistance;
    public LayerMask InteractableLayer;
    public LayerMask WallLayer;
    public Transform CarryPoint;

    [Header("Drop Objects")]
    public Transform DropSpot_Down;
    public Transform DropSpot_Left;
    public Transform DropSpot_Right;
    public Transform DropSpot_Up;
    public bool HoldingSomething;
    public bool HoldingCandle;
    private Vector2 _currentDropSpot;

    [Header("Tasks")]
    public Task CurrentTask;

    private PlayerController _playerController;
    private Interactable _selection;

    public Pushable HeldObject;
    private Pickable _pickedObject;
    private LockBox _lockBox = null;


    public bool InteractingWithLockBox;

    private bool canDrop;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameState.READY ||
          GameManager.Instance.CurrentGameState == GameState.LEVEL_CLEARED ||
          GameManager.Instance.CurrentGameState == GameState.GAMEOVER)
            return;

        if (GameManager.Instance.CurrentGameState == GameState.PAUSED)
        {
            if(HoldingSomething)
            {
                if (Input.GetKeyUp(KeyCode.J))
                {
                    HeldObject.GetComponent<FixedJoint2D>().enabled = false;
                    HeldObject.GetComponent<Pushable>().StopPushing();
                    HeldObject = null;
                    HoldingSomething = false;
                }
            }
           
            return;
        }
            

        if(HeldObject == null && _pickedObject == null)
        {
            if (_playerController.GameManager.LightsManager.CurrentLightsState == LightsState.OFF 
                && !InteractingWithLockBox)
                return;

            if (_selection != null) _selection.Deselect();

            _selection = null;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, _playerController.PlayerForwardDir, InteractionDistance, InteractableLayer);
            if (hit.collider != null)
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (interactable != null)
                {
                    if (!interactable.CanInteract)
                        return;
                    _selection = interactable;
                    interactable.Select();
                    HandleInteraction(interactable);
                }
            }
        }

        else
        {
            if(HeldObject != null)
            {
                if (Input.GetKey(KeyCode.J))
                {
                    HeldObject.GetComponent<FixedJoint2D>().enabled = true;
                    HeldObject.GetComponent<FixedJoint2D>().connectedBody = _playerController.Rb;
                }
                else if (Input.GetKeyUp(KeyCode.J))
                {
                    if (CurrentTask != null)
                    {
                        if (CurrentTask.TaskObjectType == HeldObject.ObjectType)
                        {
                            CurrentTask.ClearTask();
                            CurrentTask = null;
                            HeldObject.GetComponent<FixedJoint2D>().enabled = false;
                            HeldObject.GetComponent<Pushable>().StopPushing();
                            HeldObject = null;
                            HoldingSomething = false;
                        }
                    }
                    else
                    {
                        HeldObject.GetComponent<FixedJoint2D>().enabled = false;
                        HeldObject.GetComponent<Pushable>().StopPushing();
                        HeldObject.Deselect();
                        HeldObject = null;
                        HoldingSomething = false;
                    }
                  
                }
            }

            if (_pickedObject != null)
            {
                UpdateDropSpot();
                if (Input.GetKeyDown(KeyCode.J))
                {
                    if (!canDrop)
                        return;

                    if(CurrentTask != null)
                    {
                        if (CurrentTask.TaskObjectType == _pickedObject.ObjectType)
                        {
                            CurrentTask.ClearTask();
                            CurrentTask = null;
                            _playerController.Animator.SetBool("Carry", false);
                            _pickedObject.UseObject();
                            _pickedObject = null;
                        }
                    }
                    
                    else
                    {
                        _pickedObject.DropObject(_currentDropSpot);
                        _playerController.Animator.SetBool("Carry", false);
                        if (_pickedObject.ObjectType == ObjectType.CANDLE)
                        {
                            _pickedObject.GetComponent<Candle>().UnsetPlayer();
                            HoldingCandle = false;
                        }
                        _pickedObject.Deselect();
                        _pickedObject = null;

                    }

                    HoldingSomething = false;


                }

            }

        }

        
    }

    private void HandleInteraction(Interactable interactable)
    {
        switch (interactable.InteractionWay)
        {
            case Interactable.InteractionType.CLICK:
                if(Input.GetKeyDown(KeyCode.J))
                {
                    ConsumeItem(interactable);
                }
                break;

            case Interactable.InteractionType.HOLD:
                if(Input.GetKey(KeyCode.J))
                {
                    HeldObject = interactable.GetComponent<Pushable>();
                    HeldObject.StartPushing();
                    HoldingSomething = true;
                }
              
                break;

            case Interactable.InteractionType.CARRY:
                if (Input.GetKeyDown(KeyCode.J))
                {
                    _playerController.Animator.SetBool("Carry", true);
                    _pickedObject = interactable.GetComponent<Pickable>();
                    _pickedObject.PickUpObject(CarryPoint);
                    HoldingSomething = true;
                    if (_pickedObject.ObjectType == ObjectType.CANDLE)
                    {
                        _pickedObject.GetComponent<Candle>().SetPlayer(_playerController);
                        HoldingCandle = true;
                    }
                }

                break;

            case Interactable.InteractionType.LOCK_BOX:
                if (Input.GetKeyDown(KeyCode.J))
                {
                    if(InteractingWithLockBox)
                    {
                        CloseLockBoxUI();

                    }
                    else
                    {
                        _playerController.CancelMovement();
                        _lockBox = interactable.GetComponent<LockBox>();
                        _playerController.CanMove = false;
                        InteractingWithLockBox = true;
                        _lockBox.OpenLockBox();
                    }
                   
                }
                if (_lockBox == null)
                    return;
                if (Input.GetKeyDown(KeyCode.D)) _lockBox.MoveCursorRight();
                if (Input.GetKeyDown(KeyCode.A)) _lockBox.MoveCursorLeft();
                if (Input.GetKeyDown(KeyCode.W)) _lockBox.MoveCursorUp();
                if (Input.GetKeyDown(KeyCode.S)) _lockBox.MoveCursorDown();

                break;
        }
    }

    public void CloseLockBoxUI()
    {
        _lockBox.CheckPassword();
        _lockBox = null;
        _playerController.CanMove = true;
        InteractingWithLockBox = false;
    }
    private void UpdateDropSpot()
    {
        if(_playerController.PlayerForwardDir.y > 0.1f)
        {
            _currentDropSpot = DropSpot_Up.position;
        }

        if (_playerController.PlayerForwardDir.y < -0.1f)
        {
            _currentDropSpot = DropSpot_Down.position;
        }

        if (_playerController.PlayerForwardDir.y == 0 &&
            _playerController.PlayerForwardDir.x == 1)
        {
            _currentDropSpot = DropSpot_Right.position;
        }

        if (_playerController.PlayerForwardDir.y == 0 &&
           _playerController.PlayerForwardDir.x == -1)
        {
            _currentDropSpot = DropSpot_Left.position;
        }

        CheckIfCanDrop();
    }

    private void ConsumeItem(Interactable interactable)
    {
        switch (interactable.ObjectType)
        {
            case ObjectType.JUICE:
                _playerController.FearMeter.DrinkWater();
                interactable.UseObject();
                break;
        }

    }

    private void CheckIfCanDrop()
    {
        Collider2D wall = Physics2D.OverlapBox(_currentDropSpot, new Vector2(1f, 1f), 0, WallLayer);
        if(wall == null)
        {
            canDrop = true;
        }
        else
        {
            canDrop = false;
        }
    }


    public void CandleDead()
    {
        _pickedObject.DropObject(_currentDropSpot);
        _playerController.Animator.SetBool("Carry", false);
        if (_pickedObject.ObjectType == ObjectType.CANDLE)
            HoldingCandle = false;
        _pickedObject = null;
        HoldingSomething = false;
    }
    
}
