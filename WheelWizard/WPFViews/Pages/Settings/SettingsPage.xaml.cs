﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WheelWizard.Services.Installation;
using WheelWizard.WPFViews.Popups;

namespace WheelWizard.WPFViews.Pages.Settings;

public partial class SettingsPage : Page
{
    public SettingsPage() : this(new WhWzSettings()) { }
    public SettingsPage(UserControl initialSettingsPage)
    {
        InitializeComponent();
        
        WhWzVersionText.Text = "WhWz: v" + AutoUpdater.CurrentVersion;
        RrVersionText.Text = "RR: " + RetroRewindInstaller.CurrentRRVersion();
        
    #if RELEASE_BUILD
        ReleaseText.Visibility = Visibility.Collapsed;
        DevButton.Visibility = Visibility.Collapsed;
    #endif
        
        SettingsContent.Content = initialSettingsPage;
        
        // we also make sure that the correct radio button is selected
        var initialSettingsPageType = initialSettingsPage.GetType();
        var initialSettingsPageName = initialSettingsPageType.Name;
        var initialSettingsPageRadioButton = SettingPages
                                             .Children.OfType<RadioButton>()
                                             .FirstOrDefault(radioButton => radioButton.Tag.ToString() == initialSettingsPageName);
        if (initialSettingsPageRadioButton != null)
            initialSettingsPageRadioButton.IsChecked = true;
    }

    private void TopBarRadio_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is not RadioButton radioButton) 
            return;
        
        // As long as the Ks... files are next to this file, it works. 
        var namespaceName = GetType().Namespace;
        var typeName = $"{namespaceName}.{radioButton.Tag}";
        var type = Type.GetType(typeName);
        if (type == null || !typeof(UserControl).IsAssignableFrom(type)) 
            return;

        if (Activator.CreateInstance(type) is not UserControl instance) 
            return;
        
        SettingsContent.Content = instance;
    }

    private void DevButton_OnClick(object sender, RoutedEventArgs e)
    {
        DevToolWindow devToolWindow = new();
        devToolWindow.Show();
    }
}
