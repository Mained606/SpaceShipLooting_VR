using UnityEngine;

/// <summary>
/// 다양한 행동을 관리하는 기본 State 클래스
/// </summary>
[System.Serializable]
public abstract class State<T>
{
    protected StateMachine<T> stateMachine;         // 현 State가 등록되어있는 머신
    protected T context;                            // stateMachine을 가지고 있는 주체
    public State() {}                               // 생성자

    public void SetMachineAndContext(StateMachine<T> stateMachine, T context)
    {
        this.stateMachine = stateMachine;
        this.context = context;

        OnInitialized();
    }

    public virtual void OnInitialized() {}          // 생성후 1회 실행, 초기값 설정
    public virtual void OnEnter() {}                // 상태 전환 시 상태로 들어올 때 1회 실행
    public abstract void Update(float deltaTime);   // 상태 실행중 
    public virtual void OnExit() {}                 // 상태 전환시 나갈 때 1회 실행
}

