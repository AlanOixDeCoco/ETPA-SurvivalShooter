using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _movementLookTarget;
    [SerializeField] private Transform _aimLookTarget;
    [SerializeField] private Transform _mesh;

    [Header("Inputs")]
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _shootAction;
    [SerializeField] private InputActionReference _reloadAction;
    [SerializeField] private LayerMask _mouseColliderLayer;

    [Header("Specs")]
    [SerializeField] private float _walkSpeed = 4f;
    [SerializeField] private float _runSpeed = 6f;

    [Header("Weapon")]
    [SerializeField] private WeaponController _weaponController;

    // Components references
    private CharacterController _characterController;
    private Animator _meshAnimator;
    private Camera _playerCamera;

    // Properties
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public Vector2 TargetInput { get; private set; } = Vector2.up;
    public bool ShootInput { get; private set; } = false;
    public WeaponController WeaponController { get => _weaponController; private set => _weaponController = value; }
    public Transform MovementLookTarget { get => _movementLookTarget; private set => _movementLookTarget = value; }
    public Transform AimLookTarget { get => _aimLookTarget; private set => _aimLookTarget = value; }
    public Transform Mesh { get => _mesh; private set => _mesh = value; }
    public Animator MeshAnimator { get => _meshAnimator; private set => _meshAnimator = value; }

    // Variables
    private StateMachine _stateMachine;

    private void OnEnable()
    {
        // Move action
        _moveAction.action.performed += (callback) => MoveInput = callback.ReadValue<Vector2>();
        _moveAction.action.canceled += (callback) => MoveInput = Vector2.zero;

        // Shoot action
        _shootAction.action.started += (callback) => ShootInput = true;
        _shootAction.action.canceled += (callback) => ShootInput = false;

        // Reload action
        _reloadAction.action.started += (callback) => _weaponController.Reload();

        // Enable asset
        _moveAction.asset.Enable();
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        MeshAnimator = Mesh.GetComponent<Animator>();

        // Create statemachine
        _stateMachine = new StateMachine();

        // Create states
        var idleState = new PlayerIdleState(this);
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

    private void Start()
    {
        _playerCamera = Camera.main;
    }

    private void Update()
    {
        _stateMachine.Tick();

        // Edit aim target
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, _mouseColliderLayer))
        {
            Vector3 hitPoint = hit.point;
            //hitPoint.y = _movementLookTarget.position.y;
            _aimLookTarget.position = hitPoint;
        }
    }

    private void Move(float speed)
    {
        Vector3 movement = new Vector3(MoveInput.x, 0, MoveInput.y);
        movement *= speed;

        movement = Quaternion.Euler(0, -45, 0) * movement;

        // Edit movement target
        _movementLookTarget.position = transform.position + movement;

        // Apply movement
        _characterController.SimpleMove(movement);
    }

    public void Walk()
    {
        Move(_walkSpeed);
    }

    public void Run()
    {
        Move(_runSpeed);
    }

    public void Die()
    {
        _moveAction.asset.Disable();
        _characterController.enabled = false;
    }
}
