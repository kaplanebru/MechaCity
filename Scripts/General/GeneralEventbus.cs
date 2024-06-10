using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public static class GeneralEventbus
{

    public static Action<int> OnTurnTowerSelection;
    public static Action<int> OnTurnTowerDeselect;

    public static Action<int, TeamType> OnTeamChange;
}
