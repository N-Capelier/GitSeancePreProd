using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.UI.Home
{
    [Serializable]
    public enum HomeMenuState
    {
        MAIN,
        HOST_GAME,
        JOIN_GAME,
        GRAPH_SETTINGS,
        AUDIO_SETTINGS,
        CONTROLS_SETTINGS,
        ACCESSIBILITY_SETTINGS,
        INTERFACE_SETTINGS,
        SYSTEM_SETTINGS,
        REPORT_BUG,
        REPORT_THANKS,
        CREDITS,
        QUIT_ALERT
    }
}
