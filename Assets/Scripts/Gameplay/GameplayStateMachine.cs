using System.Collections.Generic;
using Framework.StateMachine;
using Gameplay.State;

namespace Gameplay
{
    public class GameplayStateMachine : BaseStateMachine
    {
        private GameplayManager gameplayManager;

        public GameplayStateMachine(GameplayManager gameplayManager)
        {
            List<IBaseState> stateList = new List<IBaseState>()
            {
                new OverviewState(gameplayManager),
                new AimState(gameplayManager),
                new TravelState(gameplayManager),
                new ArrivalState(gameplayManager),
                new ArrivalTowerState(gameplayManager)
            };
            InitializeStateMachine<AimState>(stateList);
        }
    }
}