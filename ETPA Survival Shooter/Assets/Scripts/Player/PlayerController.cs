using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] InputActionReference _moveAction;
    [SerializeField] InputActionReference _shootAction;

    [Header("Specs")]
    [SerializeField] float _walkSpeed = 4f;
    [SerializeField] float _runSpeed = 6f;

    [Header("Weapon")]
    [SerializeField] WeaponController _weaponController;

    // Components references
    private CharacterController _characterController;
    private Animator _animator;

    // Properties
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public Vector2 TargetInput { get; private set; } = Vector2.up;
    public bool ShootInput { get; private set; } = false;
    public WeaponController WeaponController { get => _weaponController; private set => _weaponController = value; }

    // Variables
    private StateMachine _stateMachine;

    private void OnEnable()
    {
        // Move action
        _moveAction.action.performed += (callback) => MoveInput = callback.ReadValue<Vector2>();
        _moveAction.action.canceled += (callback) => MoveInput = Vector2.zero;

        // Run action
        _shootAction.action.started += (callback) => ShootInput = true;
        _shootAction.action.canceled += (callback) => ShootInput = false;

        // Enable asset
        _moveAction.asset.Enable();
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        // Create statemachine
        _stateMachine = new StateMachine();

        // Create states
        var idleState = new PlayerIdleState();
        var movingState = new PlayerMovingState(this);
        var shootingState = new PlayerShootingState(this);

        // Idle --> Moving
        _stateMachine.AddTransition(idleState, movingState, () =>
        {
            return MoveInput.magnitude != 0;
        });

        // Moving --> Idle
        _stateMachine.AddTransition(movingState, idleState, () =>
        {
            return MoveInput.magnitude == 0;
        });

        // Any --> Shooting
        _stateMachine.AddAnyTransition(shootingState, () =>
        {
            return ShootInput;
        });

        // Shooting --> Moving
        _stateMachine.AddTransition(shootingState, movingState, () =>
        {
            return _weaponController.CanShoot && MoveInput.magnitude != 0;
        });

        // Shooting --> Idle
        _stateMachine.AddTransition(shootingState, idleState, () =>
        {
            return _weaponController.CanShoot && MoveInput.magnitude == 0;
        });

        // Set the entry state
        _stateMachine.SetState(idleState);
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    public void Walk()
    {
        Vector3 movement = new Vector3(MoveInput.x, 0, MoveInput.y);
        movement *= _walkSpeed;

        movement = Quaternion.Euler(0, -45, 0) * movement;

        // Rotate mesh
        transform.LookAt(transform.position + movement);

        // Apply movement
        _characterController.SimpleMove(movement);
    }

    public void Run()
    {
        Vector3 movement = new Vector3(MoveInput.x, 0, MoveInput.y);
        movement *= _runSpeed;

        movement = Quaternion.Euler(0, -45, 0) * movement;

        // Rotate mesh
        transform.LookAt(transform.position + movement);

        // Apply movement
        _characterController.SimpleMove(movement);
    }
}
