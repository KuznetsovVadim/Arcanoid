using Controllers;
using Helper;
using UnityEngine;

namespace BaseScripts
{
    public class Core : MonoBehaviour
    {
        [SerializeField] private GameObject paddle;
        [SerializeField] private GameObject ball;
        [SerializeField] private GameObject brick;
        [SerializeField] private GameObject bonus;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CoreUI coreUI;
        [SerializeField] private Sprite[] bonusSprites;


        public static Core GetCore { get; private set; }
        public static CoreUI GetCoreUI { get; private set; }

        private PaddleController paddleController;
        private GameController gameController;
        private BallController ballController;
        private BonusController bonusController;

        private void Awake()
        {
            GetCore = this;
            GetCoreUI = coreUI;

            gameController = new GameController(transform.position, paddle, brick, ball);
            bonusController = new BonusController();
            coreUI.Init(gameController, bonusController);
            paddleController = new PaddleController(gameController.paddle, mainCamera);
            ballController = new BallController(gameController);
        }

        private void Update()
        {
            
        }

        private void LateUpdate()
        {
            if (!Locker.Lock)
            {
                paddleController.ControllerLateUpdate(Time.fixedDeltaTime);
                ballController.ControllerLateUpdate(Time.deltaTime);
            }
        }
    }
}

