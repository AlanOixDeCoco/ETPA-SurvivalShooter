using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] InputActionReference _moveAction;
    [SerializeField] InputActionReference _runAction;

    [Header("Specs")]
    [SerializeField] CharacterSpecs _characterSpecs;

    // Components references
    private CharacterController _characterController;
    private Animator _animator;

    // Properties
    public CharacterSpecs CharacterSpecs {  
        get { return _characterSpecs; }  
        private set {
            CharacterSpecs = _characterSpecs;    
        } 
    }
    public Vector2 MoveInput { get; private set; }
    public bool RunInput { get; private set; }
    public Vector2 TargetInput { get; private set; }

    // Variables
    private StateMachine _stateMachine;

    private void OnEnable()
    {
        // Move action
        _moveAction.action.performed += (callback) => MoveInput = callback.ReadValue<Vector2>();
        _moveAction.action.canceled += (callback) => MoveInput = Vector2.zero;

        // Run action
        _runAction.action.started += (callback) => RunInput = true;
        _runAction.action.canceled += (callback) => RunInput = false;

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
        var idleState = new PlayerIdleState(_animator);
        var walkState = new PlayerWalkState(this, _animator);
        var runState = new PlayerRunState(this, _animator);

        // Idle --> Walk
        _stateMachine.AddTransition(idleState, walkState, () =>
        {
            return MoveInput.magnitude != 0;
        });

        // Walk --> Idle
        _stateMachine.AddTransition(walkState, idleState, () =>
        {
            return MoveInput.magnitude == 0;
        });

        // Walk --> Run
        _stateMachine.AddTransition(walkState, runState, () =>
        {
            return RunInput;
        });

        // Run --> Walk
        _stateMachine.AddTransition(runState, walkState, () =>
        {
            return !RunInput;
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
        movement *= CharacterSpecs.walkSpeed;

        movement = Quaternion.Euler(0, -45, 0) * movement;

        // Rotate mesh
        transform.LookAt(transform.position + movement);

        // Apply movement
        _characterController.SimpleMove(movement);
    }

    public void Run() { 
        Vector3 movement = new Vector3(MoveInput.x, 0, MoveInput.y);
        movement.Normalize();
        movement *= CharacterSpecs.runSpeed;

        movement = Quaternion.Euler(0, -45, 0) * movement;

        // Rotate mesh
        transform.LookAt(transform.position + movement);

        // Apply movement
        _characterController.SimpleMove(movement);
    }
}
