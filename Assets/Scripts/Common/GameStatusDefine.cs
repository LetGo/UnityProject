using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum GameStateId
{
    None = -1,
    Default = 0,
    Login = Default,
    ServerList,
    CreatePlayer,
    SelectPlayer,
    World,
    Reservered,
    Count
}

enum GameStageItem
{
    None,
    Idle,
    LaunchState,
    TransitionToNew,
    RunningState,
}