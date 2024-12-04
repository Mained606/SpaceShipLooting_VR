using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 상태 간 전환을 관리하는 상태 머신 클래스
/// </summary>
public class StateMachine<T>
{
    private T context;                                      // StateMachine을 가지고 있는 주체

    private State<T> currentState;                          // 현재 상태
    public State<T> CurrentState => currentState;           // 읽기 전용 프로퍼티

    private State<T> previousState;                         // 이전 상태
    public State<T> PreviousState => previousState;         // 읽기 전용 프로퍼티

    private float elapsedTimeInState = 0.0f;                // 현재 상태 지속 시간       
    public float ElapsedTimeInState => elapsedTimeInState;  // 읽기 전용 프로퍼티

    // 등록된 상태를 상태의 타입을 키 값으로 저장
    private Dictionary<System.Type, State<T>> states = new Dictionary<System.Type, State<T>>();

    // 생성자
    public StateMachine(T context, State<T> initialState)
    {
        this.context = context;

        AddState(initialState);

        currentState = initialState;
        currentState.OnEnter();
    }

    // StateMachine에 State 등록
    public void AddState(State<T> state)
    {
        state.SetMachineAndContext(this, context);
        states[state.GetType()] = state;
    }

    // StateMachine에서 State의 업데이트 실행
    public void Update(float deltaTime)
    {
        elapsedTimeInState += deltaTime;
        currentState?.Update(deltaTime);
    }

    // currentState의 상태 바꾸기
    public R ChangeState<R>() where R : State<T>
    {
        // 현 상태와 새로운 상태 비교
        var newType = typeof(R);
        if (currentState.GetType() == newType)
        {
            return currentState as R;
        }

        // 상태 변경이전
        if (currentState != null)
        {
            currentState.OnExit();
        }
        previousState = currentState;

        // 상태 변경 
        currentState = states[newType];
        currentState.OnEnter();
        elapsedTimeInState = 0.0f;

        return currentState as R;
    }
}