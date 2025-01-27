using System.Collections.Generic;
using Cameras.Overview;
using Cameras.Overview.States;
using Framework.StateMachine;

namespace StateMachines
{
    public class OverviewStateMachine : BaseStateMachine
    {
        public OverviewManager OverviewManager;

        public OverviewStateMachine(OverviewManager overviewManager)
        {
            List<IBaseState> stateList = new List<IBaseState>()
            {
                new AllViewState(overviewManager),
                new TowerViewState(overviewManager),
                new ShooterViewState(overviewManager),
            };
            InitializeStateMachine(stateList);
        }
    }
}