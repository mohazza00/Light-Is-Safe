using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D Rb;
    public Animator Animator;
    public SpriteRenderer Sprite;
    public GameObject Character;
    public GameObject Ghost;

    [Header("Dependencies")]
    public FearMeter FearMeter;
    public PlayerInteraction PlayerInteraction;
    public GameManager GameManager;

    [Header("Movement")]
    public Vector2 PlayerForwardDir;
    [SerializeField] private float _movementSpeed;
    private Vector2 _movement;
    public bool CanMove = true;

    [Header("Lights")]
    public bool IsTouchingSwitch;
    public LightSwitch CurrentSwitch;
    public GameObject Torch;

    private Vector2 _ghostPosition;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponentInChildren<Animator>();
        Sprite = Character.GetComponent<SpriteRenderer>();
        PlayerInteraction = GetComponent<PlayerInteraction>();
        FearMeter = GetComponentInChildren<FearMeter>();
    }

    private void Start()
    {
        GameManager = GameManager.Instance;
        _ghostPosition = Ghost.transform.localPosition;
    }

    private void Update()
    {
        switch (GameManager.CurrentGameState)
        {
            case GameState.READY:
                CancelMovement();
                break;
            case GameState.PLAYING:
                HandleMovement();
                InteractWithLightSwitches();
                HandleControlInput();
                break;
            case GameState.PAUSED:
                HandleControlInput();
                break;
            case GameState.GAMEOVER:
                _ghostPosition += Vector2.up * Time.deltaTime * 2f;
                Ghost.transform.localPosition = _ghostPosition + new Vector2(1f, 0f) * Mathf.Sin(Time.time * 5) * 0.5f;
                break;
            case GameState.LEVEL_CLEARED:
                CancelMovement();
                break;
        }
    }

    private void HandleControlInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.CurrentGameState != GameState.PAUSED)
                GameManager.PauseGame();
            else
                GameManager.ResumeGame();
        }
    }

    private void InteractWithLightSwitches()
    {
        if (IsTouchingSwitch)
        {
            if (GameManager.LightsManager.CurrentLightsState == LightsState.ON)
            {
                IsTouchingSwitch = false;
                CurrentSwitch.TouchingPlayer = null;
                CurrentSwitch = null;
                return;
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                GameManager.LightsManager.CurrentSwitch = CurrentSwitch;
                GameManager.LightsManager.TurnOnLights();
            }
        }
    }

    private void HandleMovement()
    {
        if (!CanMove)
            return;
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        Animator.SetFloat("Speed", _movement.sqrMagnitude);

        if (PlayerInteraction.HeldObject == null)
        {
            Animator.SetFloat("Horizontal", _movement.x);
            Animator.SetFloat("Vertical", _movement.y);

            if (_movement.sqrMagnitude != 0)
            {
                Animator.SetFloat("LastHorizontal", _movement.x);
                Animator.SetFloat("LastVertical", _movement.y);
                PlayerForwardDir = _movement;
            }
        }
        else
        {
            Animator.SetFloat("Horizontal", Animator.GetFloat("LastHorizontal"));
            Animator.SetFloat("Vertical", Animator.GetFloat("LastVertical"));
        }
        
        Rb.velocity = _movement.normalized * _movementSpeed;
    }

    public void CancelMovement()
    {
        Rb.velocity = Vector2.zero;
        _movement = Vector2.zero;
        Animator.SetFloat("Speed", 0f);
    }

    public void ReleaseSoul()
    {
        StartCoroutine(ShowGameOverPanel());
    }

    IEnumerator ShowGameOverPanel()
    {
        if(PlayerInteraction.InteractingWithLockBox)
        {
            PlayerInteraction.CloseLockBoxUI();
        }
        CancelMovement();
        Animator.SetBool("Death", true);
        Rb.velocity = Vector2.zero;
        Ghost.SetActive(true);
        GameManager.CurrentGameState = GameState.GAMEOVER;
        yield return new WaitForSeconds(0.8f);
        GameManager.GameOver();     
    }
}
