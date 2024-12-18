using UnityEngine;

namespace MyPet.AI
{
    public class SitState : State<AnimalController>
    {
        private Animator animator;

        //animator parameter
        protected int isSitHash = Animator.StringToHash("IsSit");

        public override void OnInitialized()
        {
            //ÂüÁ¶
            animator = context.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            animator.SetBool(isSitHash, true);
        }

        public override void Update(float deltaTime)
        {

        }

        public override void OnExit()
        {
            animator.SetBool(isSitHash, false);
        }
    }
}