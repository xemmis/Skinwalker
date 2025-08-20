using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(DialogueSystem))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]

public class NpsBehaviorLogic : PausableObject
{

    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public BoxCollider BoxCollider { get; private set; }
    public AudioSource AudioSource { get; private set; }
    public string IntegerName { get; private set; }    

    public Transform CurrentDestination;
    public DialogueSystem DialogueSystem;

    private INPSState _currentState;


    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        BoxCollider = GetComponent<BoxCollider>();
        DialogueSystem = GetComponent<DialogueSystem>();
        AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        BoxCollider.isTrigger = true;
        AudioSource.playOnAwake = false;
        AudioSource.priority = 0;
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