using Controllers;
using Helper;
using Models;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BaseScripts
{
    public class CoreUI: MonoBehaviour
    {
        [SerializeField] Text score;
        [SerializeField] Text life;
        [SerializeField] Text level;

        [SerializeField] Button start;
        [SerializeField] Button exit;

        [SerializeField] GameObject notification;
        [SerializeField] Text notificationMessage;

        GameController gameController;

        GameModel gameModel;

        public void Init(GameController gameController)
        {
            this.gameController = gameController;
            gameModel = gameController.gameModel;
            gameModel.modelHasChanged = RefreshUI;
            Locker.Lock = true;
            exit?.onClick.AddListener(ExitGame);
            start?.onClick.AddListener(StartNewGame);
            notification?.SetActive(false);
        }

        public void StartBonusTimer(int seconds, Action onBonusEnd)
        {
            StartCoroutine(CountDownBonusTimer(seconds, onBonusEnd));
        }

        private IEnumerator CountDownBonusTimer(int seconds, Action methods)
        {
            int counter = seconds;
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
            }
            methods?.Invoke();
        }

        private void RefreshUI()
        {
            score.text = string.Format($"SCORE: {gameModel.PlayerScore}");
            life.text = string.Format($"LIFE: {gameModel.PlayerLife}");
            level.text = string.Format($"LEVEL: {gameModel.PlayerLevel}");
        }

        public void CotinueLevel()
        {
            Locker.Lock = true;
            notification.SetActive(true);
            StartCoroutine(CountDownNotification(Constants.COUNT_DOWN_DURATION, Constants.CONTINUE_GAME_MESSAGE, ()=>
            {
                notification.SetActive(false);
                gameController.ContinueLevel();
                Locker.Lock = false;
            }));
            
        }

        public void StartNextLevel()
        {
            Locker.Lock = true;
            notification.SetActive(true);
            StartCoroutine(CountDownNotification(Constants.COUNT_DOWN_DURATION, Constants.NEW_LEVEL_MESSAGE, () =>
            {
                notification.SetActive(false);
                gameController.StartNextLevel();
                Locker.Lock = false;
            }
            ));
        }

        public void RestartGame()
        {
            Locker.Lock = true;
            start.gameObject.SetActive(true);
            notification.SetActive(true);
            exit.gameObject.SetActive(true);
            notificationMessage.text = Constants.GAME_OVER_MESSAGE;
        }

        private void StartNewGame()
        {
            Locker.Lock = true;
            start.gameObject.SetActive(false);
            exit.gameObject.SetActive(false);
            notification.SetActive(true);
            if (gameController.isNeedControllers)
            {
                gameController.GetAllControllers();
            }
            StartCoroutine(CountDownNotification(Constants.COUNT_DOWN_DURATION, Constants.NEW_GAME_MESSAGE, () =>
            {
                notification.SetActive(false);
                gameController.StartNewGame();
                Locker.Lock = false;
            }));
        }

        private IEnumerator CountDownNotification(int seconds, string notificationText, Action methods)
        {
            int counter = seconds;
            notificationMessage.text = string.Format($"{notificationText} {counter} sec");
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
                notificationMessage.text = string.Format($"{notificationText} {counter} sec");
            }
            methods?.Invoke();
        }

        private void ExitGame()
        {
            Application.Quit();
        }
    }
}

