using UnityEngine;

namespace MyPet.AI
{
    public class DrinkState : State<AnimalController>
    {
        private Animator animator;

        //animator parameter
        protected int isDrinkHash = Animator.StringToHash("IsDrink");

        public override void OnInitialized()
        {
            //ÂüÁ¶
            animator = context.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            animator.SetBool(isDrinkHash, true);
        }

        public override void Update(float deltaTime)
        {

        }

        public override void OnExit()
        {
            animator.SetBool(isDrinkHash, false);
        }
    }
}
