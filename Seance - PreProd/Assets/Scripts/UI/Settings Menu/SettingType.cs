using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.UI.SettingsMenu
{
    /// <summary>
    /// Edouard
    /// </summary>
    public enum SettingType
    {
        // Graphics
        //   Display
        DISPLAY_SCREEN,
        WINDOW_MODE,
        SCREEN_RESOLUTION,
        //   Performance
        REFRESH_RATE,
        VSYNC,
        //   Quality
        QUALITY_PRESET,
        SHADOW_QUALITY,
        ANTIALIASING,
        TEXTURE_QUALITY,
        EFFECT_QUALITY,
        POSTPROCESS_QUALITY,
        
        // Audio
        //   Volume
        GENERAL_VOLUME,
        MUSIC_VOLUME,
        EFFECT_VOLUME,
        AMBIANCE_VOLUME,
        DIALOGUE_VOLUME,
        INTERFACE_VOLUME,
        //   Voice chat
        VOICE_METHOD,
        VOICE_GLOBAL_VOLUME,
        VOICE_LEFT_VOLUME,
        VOICE_RIGHT_VOLUME,
        //   Devices
        OUTPUT_DEVICE,
        INPUT_DEVICE,
        
        // Controls

        // Accessibility
        //   Image
        IMAGE_BRIGHTNESS,
        IMAGE_CONTRAST,
        //   Subtitle
        SUBTITLE_ENABLE,
        SUBTITLE_SIZE,
        SUBTITLE_COLOR,
        SUBTITLE_BACKGROUND_OPACITY,
        SUBTITLE_BACKGROUND_COLOR,
        //   Colorblindness
        COLORBLIND_FILTER,
        COLORBLIND_INTENSITY,
        //   Readability
        FONT_SIZE,

        // Interface

        // System
        //   Language
        SYSTEM_LANGUAGE,
        //   Debug
        ENABLE_DEBUG
    }
}
