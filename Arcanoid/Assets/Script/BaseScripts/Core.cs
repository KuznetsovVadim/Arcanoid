using Controllers;
using Helper;
using System.Collections.Generic;
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

        private List<BaseController> controllers;

        private void Awake()
        {
            GetCore = this;
            GetCoreUI = coreUI;

            gameController = new GameController(transform.position, paddle, brick, ball);
            paddleController = new PaddleController(gameController.paddle, mainCamera);
            ballController = new BallController(gameController);

            controllers = new List<BaseController>()
            {
                gameController,
                paddleController,
                ballController
            };

            bonusController = new BonusController(bonusSprites, bonus);
            coreUI.Init(gameController);

            controllers.Add(bonusController);
        }

        public T GetController<T>() where T: BaseController
        {
            if (controllers == null || controllers.Count == 0)
            {
                return null;
            }

            for (int i = 0; i < controllers.Count; i++)
            {
                if(controllers[i] is T)
                {
                    return controllers[i] as T;
                }
            }
            return null;
        }

        private void LateUpdate()
        {
            if (!Locker.Lock)
            {
                ballController.ControllerUpdate(Time.deltaTime);
                paddleController.ControllerLateUpdate(Time.fixedDeltaTime);
                bonusController.ControllerLateUpdate(Time.deltaTime);
            }
        }
    }
}

