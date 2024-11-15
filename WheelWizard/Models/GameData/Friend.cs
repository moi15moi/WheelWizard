﻿using WheelWizard.Helpers;

namespace WheelWizard.Models.GameData;

public class Friend : BasePlayer
{
    public required uint Wins { get; set; }
    public required uint Losses { get; set; }
    
    public required byte CountryCode { get; set; }
    public string CountryName => Humanizer.GetCountryEmoji(CountryCode);
}
