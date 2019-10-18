using BaseScripts;
using Models;
using UnityEngine;

namespace Controllers
{
    public class PaddleController : BaseController
    {
        private Paddle paddleModel;
        private GameObject paddleView;

        public PaddleController(GameObject paddleView, Camera camera)
        {
            this.paddleView = paddleView;
            paddleModel = new Paddle(paddleView.transform, camera);
        }

        public override void ControllerLateUpdate(float Time)
        {
            paddleView.transform.position = paddleModel.OnModelUpdate(Time);
        }
    }
}


