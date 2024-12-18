using System.Collections.Generic;
using UnityEngine;

namespace MyPet.AI
{
    /// <summary>
    /// <T>의 상태를 관리하는 클레스
    /// </summary>
    [System.Serializable]
    public abstract class State<T>
    {
        protected StateMachine<T> stateMachine; //현 state가 등록 되어있는 Machine
        protected T context;                    //stateMachine을 가지고 있는 주체
        
        public State() { }                      //생성자

        public void SetMachineAndContext(StateMachine<T> stateMachine, T context)
        {
            this.stateMachine = stateMachine;
            this.context = context;

            OnInitialized();
        }

        public virtual void OnInitialized() { } //생성후 1회 실행, 초기값 설정

        public virtual void OnEnter() { }       //상태 전환시 상태로 들어올때 1회 실행

        public abstract void Update(float deltaTime);   //상태 실행중

        public virtual void OnExit() { }        //상태 전환시 상태를 나갈때 1회 실행

    }

    /// <summary>
    /// <T>의 State들을 관리하는 클래스
    /// </summary>
    public class StateMachine<T>
    {
        private T context;                      //StateMachine을 가지고 있는 주체

        private State<T> currenState;           //현재 상태
        public State<T> CurrentState => currenState;

        private State<T> previousState;         //이전 상태
        public State<T> PreviousState => previousState;

        private float elapsedTimeInState = 0.0f;    //현재 상태 지속 시간
        public float ElapsedTimeInState => elapsedTimeInState;

        //등록된 상태를 상태의 타입을 키값으로 저장한다
        private Dictionary<System.Type, State<T>> states = new Dictionary<System.Type, State<T>>();

        //생성자 : 
        public StateMachine(T context, State<T> initialState)
        {
            this.context = context;

            AddState(initialState);
            currenState = initialState;
            currenState.OnEnter();
        }

        //StateMachine에 State 등록
        public void AddState(State<T> state)
        {
            state.SetMachineAndContext(this, context);
            states[state.GetType()] = state;
        }

        //StateMachine에서 State의 업데이트 실행
        public void Update(float deltaTime)
        {
            elapsedTimeInState += deltaTime;

            currenState.Update(deltaTime);
        }

        //currenState의 상태 바꾸기
        public R ChangeState<R>() where R : State<T>
        {
            //현상태와 새로운 상태 비교
            var newType = typeof(R);
            if (currenState.GetType() == newType)
            {
                return currenState as R;
            }

            //상태 변경이전
            if (currenState != null)
            {
                currenState.OnExit();
            }
            previousState = currenState;

            //상태 변경
            currenState = states[newType];
            currenState.OnEnter();
            elapsedTimeInState = 0.0f;

            return currenState as R;
        }

    }
}
