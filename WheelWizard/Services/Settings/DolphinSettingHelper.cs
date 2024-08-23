﻿using CT_MKWII_WPF.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace CT_MKWII_WPF.Services.Settings;

public static class DolphinSettingHelper
{
    private static string ConfigFolderPath => PathManager.ConfigFolderPath;
    
    public static string ReadIniSetting(string fileLocation, string section, string settingToRead)
    {
        if (!File.Exists(fileLocation))
        {
            MessageBox.Show(
                "Something went wrong, INI file could not be read, Plzz report this as a bug: " +
                fileLocation, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        //read through every line to find the section
        var lines = File.ReadAllLines(fileLocation);
        var sectionFound = lines.Any(t => t == $"[{section}]");

        if (!sectionFound) 
            return "";

        //now we know the section exists, we need to find the setting
        foreach (var line in lines)
        {
            if (!line.Contains(settingToRead)) 
                continue;
            //we found the setting, now we need to return the value
            var setting = line.Split("=");
            return setting[1].Trim();
        }

        return "";
    }
    
    public static string ReadIniSetting(string fileLocation, string settingToRead)
    {
        if (!File.Exists(fileLocation))
        {
            MessageBox.Show(
                "Something went wrong, INI file could not be read, Plzz report this as a bug: " +
                fileLocation, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        var lines = File.ReadAllLines(fileLocation);
        foreach (var line in lines)
        {
            if (!line.StartsWith(settingToRead)) 
                continue;
            
            var setting = line.Split("=");
            return setting[1].Trim();
        }

        return "";
    }

    public static void ChangeIniSettings(string fileLocation, string section, string settingToChange, string value)
    {
        if (!File.Exists(fileLocation))
        {
            MessageBox.Show(
                $"Something went wrong, INI file could not be found, Plzz report this as a bug: {fileLocation}",
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var lines = File.ReadAllLines(fileLocation).ToList();
        var sectionFound = false;
        var settingFound = false;
        var sectionIndex = -1;

        for (var i = 0; i < lines.Count; i++)
        {
            if (lines[i].Trim() == $"[{section}]")
            {
                sectionFound = true;
                sectionIndex = i;
            }
            else if (sectionFound && (lines[i].StartsWith($"{settingToChange}=") ||
                                      lines[i].StartsWith($"{settingToChange} =")))
            {
                lines[i] = $"{settingToChange} = {value}";
                settingFound = true;
                break;
            }
            else if (lines[i].Trim().StartsWith("[") && lines[i].Trim().EndsWith("]") && sectionFound)
                break;
        }

        if (!sectionFound)
        {
            lines.Add($"[{section}]");
            lines.Add($"{settingToChange}={value}");
        }
        else if (!settingFound)
            lines.Insert(sectionIndex + 1, $"{settingToChange}={value}");
        
        File.WriteAllLines(fileLocation, lines);
    }
    
    public static string AutomaticallyFindDolphinPath()
    {
        var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                       "Dolphin Emulator");
        if (FileHelper.DirectoryExists(appDataPath))
            return appDataPath;

        var documentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                         "Dolphin Emulator");
        return FileHelper.DirectoryExists(documentsPath) ? documentsPath : string.Empty;
    }
    
    public static string FindWiiMoteNew()
    {
        var wiimoteFile = Path.Combine(ConfigFolderPath, "WiimoteNew.ini");
        if (FileHelper.FileExists(wiimoteFile))
            return wiimoteFile;
        
        MessageBox.Show($"Could not find WiimoteNew file, tried looking in {wiimoteFile}", 
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return string.Empty;
    }

    public static string FindGfxFile()
    {
        var gfxFile = Path.Combine(ConfigFolderPath, "GFX.ini");
        if (FileHelper.FileExists(gfxFile))
            return gfxFile;
        
        MessageBox.Show($"Could not find GFX file, tried looking in {gfxFile}", 
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return string.Empty;
    }
}
