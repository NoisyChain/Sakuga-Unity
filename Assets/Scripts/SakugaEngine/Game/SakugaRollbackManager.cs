using UnityEngine;
using System;
using PleaseResync;

namespace SakugaEngine.Game
{
    public class SakugaRollbackManager : PleaseResyncManager
    {
        private const uint MAX_PLAYERS = 2;
        private const uint MAX_SPECTETORS = 10;

        [SerializeField] private GameManager GameManager;

        public void Start()
        {
            GameManager.InputSize = InputSize;
        }

        public override void OnlineGame(uint maxPlayers, uint ID)
        {
            StartOnlineGame(GameManager, maxPlayers, ID);
        }

        public override void LocalGame(uint maxPlayers)
        {
            StartOfflineGame(GameManager, maxPlayers);
        }

        public override void ReplayMode(uint maxPlayers)
        {
            StartReplay(GameManager, maxPlayers);
        }
    }
}
