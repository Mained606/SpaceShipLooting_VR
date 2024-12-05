using UnityEngine;

namespace MyPet.AI
{
    [System.Serializable]
    public class DogIdleState : State<AnimalController>
    {
        private Animator animator;
        

        public override void OnInitialized()
        {
            //ÂüÁ¶
            animator = context.GetComponent<Animator>();
            
        }

        public override void OnEnter()
        {

        }

        public override void Update(float deltaTime)
        {

        }

        public override void OnExit()
        {

        }
    }
}