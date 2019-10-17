using Controllers;
using UnityEngine;

namespace Core
{
    public class Core : MonoBehaviour
    {
        [SerializeField] private GameObject paddle;
        [SerializeField] private GameObject ball;
        [SerializeField] private GameObject brick;
        [SerializeField] private GameObject bonus;
        [SerializeField] private Camera camera;
        [SerializeField] private Sprite[] bonusSprites;

        public static Core GetCore { get; private set; }

        private PaddleController paddleController;
        private GameController gameController;

        private void Awake()
        {
            GetCore = this;
            gameController = new GameController(transform.position, paddle, brick, ball);
            paddleController = new PaddleController(gameController.paddle, camera);
        }

        private void Update()
        {
            
        }

        private void LateUpdate()
        {
            paddleController.ControllerLateUpdate(Time.deltaTime);
        }
    }
}

