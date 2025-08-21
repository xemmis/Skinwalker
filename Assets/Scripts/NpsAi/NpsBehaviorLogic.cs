using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]

public class NpsBehaviorLogic : PausableObject
{

    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public BoxCollider BoxCollider { get; private set; }
    public AudioSource AudioSource { get; private set; }
    [field: SerializeField] public string IntegerName { get; private set; }

    public Transform CurrentDestination;

    [SerializeField] private INPSState _currentState;

    public bool IsGoTalk = false;


    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        BoxCollider = GetComponent<BoxCollider>();
        AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        BoxCollider.isTrigger = true;
        AudioSource.playOnAwake = false;
        AudioSource.priority = 0;
        ChangeState(new WalkState());
    }
    public void ChangeState(INPSState newState)
    {
        _currentState?.Exit(this);
        _currentState = newState;
        _currentState?.Enter(this);
    }

    protected override void Update()
    {
        base.Update();

        _currentState?.Update(this);
    }

}