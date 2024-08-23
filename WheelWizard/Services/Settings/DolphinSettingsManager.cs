﻿using System.Linq;

namespace CT_MKWII_WPF.Services.Settings;

public class DolphinSettingsManager
{
    private static void ChangeSettings(params (string Key, string Value)[] settings)
    {
        var gfxFile = DolphinSettingHelper.FindGfxFile();
        if (string.IsNullOrEmpty(gfxFile)) 
            return;

        foreach (var (key, value) in settings)
        {
            DolphinSettingHelper.ChangeIniSettings(gfxFile, "Settings", key, value);
        }
    }

    public static void EnableRecommendedSettings()
    {
        ChangeSettings(
            ("ShaderCompilationMode", "2"),
            ("WaitForShadersBeforeStarting", "True"),
            ("MSAA", "0x00000002"),
            ("SSAA", "False")
        );
    }

    public static void DisableRecommendedSettings()
    {
        ChangeSettings(
            ("ShaderCompilationMode", "0"),
            ("WaitForShadersBeforeStarting", "False"),
            ("MSAA", "0x00000001"),
            ("SSAA", "False")
        );
    }

    public static bool IsRecommendedSettingsEnabled()
    {
        var gfxFile = DolphinSettingHelper.FindGfxFile();
        if (string.IsNullOrEmpty(gfxFile)) 
            return false;

        var settings = new[] { "ShaderCompilationMode", "WaitForShadersBeforeStarting", "MSAA", "SSAA" };
        var expectedValues = new[] { "2", "True", "0x00000002", "False" };

        return settings.Select((setting, index) =>
                DolphinSettingHelper.ReadIniSetting(gfxFile, "Settings", setting) == expectedValues[index])
            .All(result => result);
    }

    public static bool GetCurrentVSyncStatus()
    {
        var gfxFile = DolphinSettingHelper.FindGfxFile();
        if (gfxFile != "")
            return DolphinSettingHelper.ReadIniSetting(gfxFile, "VSync") == "True";
        return false;
    }

    // public static int GetCurrentResolution()
    // {
    //     var gfxFile = DolphinSettingHelper.FindGfxFile();
    //     if (gfxFile == "")
    //         return -1;
    //
    //     var resolution = DolphinSettingHelper.ReadIniSetting(gfxFile, "Settings", "InternalResolution");
    //     if (!int.TryParse(resolution, out _)) return -1;
    //     return int.Parse(resolution);
    // }
}
