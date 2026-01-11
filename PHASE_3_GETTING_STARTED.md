# Phase 3 Implementation Guide - Getting Started

**Phase**: 3 (UI Implementation)  
**Status**: Ready to Start  
**Completed**: Foundation & Dependencies  
**Next**: UI Page Implementation  

---

## What's Ready in Phase 3

### ✅ Foundation Complete
- [x] Phase 3 Implementation Plan (docs/PHASE_3_IMPLEMENTATION_PLAN.md)
- [x] UI Component Specifications (docs/UI_COMPONENT_SPECIFICATIONS.md)
- [x] Theme Helper System
- [x] Chart Data Helper
- [x] Dialog Helper
- [x] Export Helper
- [x] 5 Converters for XAML binding
- [x] Service Management ViewModel
- [x] Startup Apps ViewModel
- [x] Users Session ViewModel
- [x] Updated App.xaml.cs with Phase 3 DI
- [x] Updated .csproj with charting library (OxyPlot)

### 📦 Dependencies Added
```xml
<!-- Charting Library -->
<PackageReference Include="OxyPlot.Wpf" Version="2.1.2" />
<PackageReference Include="OxyPlot.Core" Version="2.1.2" />

<!-- DataGrid Control -->
<PackageReference Include="CommunityToolkit.WinUI.Controls.DataGrid" Version="7.1.2" />
```

### 🏗️ Architecture Summary

```
Services (Data Layer)
    ↓
ViewModels (Business Logic) - 8 ViewModels
    ↓
Converters (Formatting) - 5 Converters
    ↓
Helpers (Utilities) - 4 Helpers
    ↓
Views/Pages (UI) - 9 Pages + MainWindow
    ↓
UI (Binding & Display)
```

---

## Phase 3 Implementation Steps

### Step 1: Build & Verify Foundation

```bash
# Build the project
cd SystemPulse
dotnet build

# Run the application (will show MainWindow with empty pages)
dotnet run --project src/SystemPulse.App
```

**Expected Result**: Application launches with navigation working but pages are empty.

---

### Step 2: Implement Dashboard Page (OverviewPage)

**Files to Update**:
- `src/SystemPulse.App/Views/OverviewPage.xaml` - Add gauge UI
- `src/SystemPulse.App/Views/OverviewPage.xaml.cs` - Bind ViewModel

**Tasks**:
1. Create gauge components for CPU, RAM, GPU
2. Add real-time binding to `ShellViewModel.SystemMetrics`
3. Format values using converters
4. Style with color scheme from UI_COMPONENT_SPECIFICATIONS.md

**Example Gauge Control**:
```xaml
<Grid Background="#F9FAFB" CornerRadius="16" Padding="20" Margin="10">
    <StackPanel Spacing="12">
        <TextBlock Text="CPU" FontSize="14" FontWeight="SemiBold" Foreground="#6B7280"/>
        
        <!-- Circular Progress (use WinUI ProgressRing) -->
        <ProgressRing 
            IsActive="True" 
            Value="{Binding SystemMetrics.CPUUsage}"
            Foreground="#00D4FF" />
        
        <!-- Value Display -->
        <TextBlock 
            Text="{Binding SystemMetrics.CPUUsage, Converter={StaticResource PercentageConverter}}" 
            FontSize="24" 
            FontWeight="Bold" 
            Foreground="#00D4FF"
            TextAlignment="Center" />
    </StackPanel>
</Grid>
```

**Estimated Time**: 2 hours  
**Complexity**: Medium

---

### Step 3: Implement Process Management Page (ProcessesPage)

**Files to Update**:
- `src/SystemPulse.App/Views/ProcessesPage.xaml` - Add DataGrid
- `src/SystemPulse.App/Views/ProcessesPage.xaml.cs` - Setup bindings

**Tasks**:
1. Add CommunityToolkit DataGrid with columns (Name, PID, CPU%, Memory, Threads, User, Status)
2. Implement search TextBox binding
3. Add sort by column functionality
4. Create context menu (right-click)
5. Add action buttons (Kill, Suspend, Properties)
6. Handle async process operations

**Example DataGrid**:
```xaml
<controls:DataGrid
    ItemsSource="{Binding Processes}"
    SelectedItem="{Binding SelectedProcess, Mode=TwoWay}"
    CanUserSortColumns="True"
    CanUserReorderColumns="True"
    ColumnHeaderHeight="40"
    RowHeight="32">
    
    <controls:DataGrid.Columns>
        <controls:DataGridTextColumn Header="Process Name" Binding="{Binding Name}" Width="200"/>
        <controls:DataGridTextColumn Header="PID" Binding="{Binding PID}" Width="80"/>
        <controls:DataGridTextColumn Header="CPU %" Binding="{Binding CPUUsage}" Width="80"/>
        <controls:DataGridTextColumn Header="Memory" Binding="{Binding MemoryUsage, Converter={StaticResource BytesConverter}}" Width="100"/>
    </controls:DataGrid.Columns>
</controls:DataGrid>
```

**Estimated Time**: 3 hours  
**Complexity**: High

---

### Step 4: Implement Performance Charts (PerformancePage)

**Files to Update**:
- `src/SystemPulse.App/Views/PerformancePage.xaml` - Add OxyPlot charts
- `src/SystemPulse.App/Views/PerformancePage.xaml.cs` - Setup chart data
- `src/SystemPulse.App/ViewModels/PerformanceViewModel.cs` - Enhance with chart series

**Tasks**:
1. Add OxyPlot LineChart for CPU, RAM, GPU, Disk, Network
2. Bind `PerformanceViewModel.CpuHistory` to chart
3. Implement time range selector (1m, 5m, 15m, 30m, 1h)
4. Add statistics panel (Current, Min, Max, Average)
5. Export to CSV button
6. Update real-time (1 second intervals)

**Example OxyPlot Chart**:
```csharp
var model = new PlotModel { Title = "CPU Usage" };
var series = new LineSeries { Title = "CPU %" };

foreach (var value in CpuHistory)
{
    series.Points.Add(new DataPoint(series.Points.Count, value));
}

model.Series.Add(series);
```

**Estimated Time**: 3 hours  
**Complexity**: High

---

### Step 5: Implement Settings Page (SettingsPage)

**Files to Update**:
- `src/SystemPulse.App/Views/SettingsPage.xaml` - Add controls
- `src/SystemPulse.App/Views/SettingsPage.xaml.cs` - Wire up ViewModel

**Tasks**:
1. Theme selector (Light/Dark/System)
2. Refresh interval slider (1-60 seconds)
3. Window opacity slider (50-100%)
4. Toggle switches (Always on top, Auto-start, Minimize to tray)
5. Save/Reset buttons
6. Wire `ThemeHelper.SetTheme()` when theme changes

**Example Settings Controls**:
```xaml
<StackPanel Padding="20" Spacing="20">
    <TextBlock Text="Appearance" Style="{ThemeResource SubtitleTextBlockStyle}"/>
    
    <Grid ColumnDefinitions="Auto,*" Padding="0,10">
        <TextBlock Grid.Column="0" Text="Theme" VerticalAlignment="Center" Width="150"/>
        <ComboBox 
            Grid.Column="1" 
            ItemsSource="{Binding ThemeOptions}"
            SelectedItem="{Binding SelectedTheme, Mode=TwoWay}" />
    </Grid>
    
    <Grid ColumnDefinitions="Auto,*" Padding="0,10">
        <TextBlock Grid.Column="0" Text="Refresh Interval" VerticalAlignment="Center" Width="150"/>
        <Slider 
            Grid.Column="1" 
            Minimum="1" Maximum="60" 
            Value="{Binding RefreshInterval, Mode=TwoWay}" />
    </Grid>
    
    <StackPanel Orientation="Horizontal" Spacing="10" Margin="0,20,0,0">
        <Button Content="Save" Command="{Binding SaveSettingsCommand}"/>
        <Button Content="Reset" Command="{Binding ResetCommand}"/>
    </StackPanel>
</StackPanel>
```

**Estimated Time**: 1.5 hours  
**Complexity**: Medium

---

### Step 6: Implement Services & Startup Pages

**Services Page**:
- Display list of Windows services
- Start/Stop/Restart buttons
- Status filter
- Search functionality

**Startup Apps Page**:
- List startup applications
- Enable/Disable toggles
- Filter by source (Windows, User, Manufacturer)

**Estimated Time**: 2 hours each  
**Complexity**: Medium

---

### Step 7: Implement Users & Details Pages

**Users Page**:
- List active user sessions
- Logoff button
- Session details (logon time, idle time)

**Details Page**:
- Detailed process information
- CPU/Memory graphs
- Environment variables
- Open files list

**Estimated Time**: 2 hours each  
**Complexity**: Medium-High

---

### Step 8: Polish & Testing

**Tasks**:
1. Theme switching implementation
2. Error handling for all operations
3. Loading states & spinners
4. Performance optimization
5. UI consistency review
6. End-to-end testing

**Estimated Time**: 2 hours  
**Complexity**: Medium

---

## Development Commands

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run --project src/SystemPulse.App
```

### Run in Release Mode
```bash
dotnet run --project src/SystemPulse.App -c Release
```

### Build & Package
```bash
dotnet publish src/SystemPulse.App -c Release -o publish
```

### Run Tests
```bash
dotnet test
```

### Clean
```bash
dotnet clean
```

---

## Key Implementation Patterns

### 1. ViewModel to View Binding

```csharp
// In ViewModel
[ObservableProperty]
private PerformanceMetrics systemMetrics;

// In XAML
<TextBlock Text="{Binding SystemMetrics.CPUUsage, Converter={StaticResource PercentageConverter}}" />
```

### 2. Async Command Execution

```csharp
// In ViewModel
[RelayCommand]
public async Task RefreshProcessesAsync()
{
    try
    {
        IsLoading = true;
        var processes = await _processService.GetProcessesAsync();
        // Update collection
    }
    finally
    {
        IsLoading = false;
    }
}

// In XAML
<Button Command="{Binding RefreshProcessesCommand}" IsEnabled="{Binding !IsLoading}" />
```

### 3. Error Handling

```csharp
try
{
    await _service.PerformOperationAsync();
    StatusText = "Operation completed";
}
catch (UnauthorizedAccessException)
{
    await DialogHelper.ShowErrorDialogAsync(
        "Permission Denied",
        "Admin privileges required for this operation.");
}
catch (Exception ex)
{
    _logger.LogError("Operation failed", ex);
    StatusText = "Error: Check logs for details";
}
```

### 4. Theme Application

```csharp
// When theme setting changes
partial void OnSelectedThemeChanged(string value)
{
    ThemeHelper.SetTheme(value);
    _ = _settingsService.SetThemeAsync(value);
}
```

---

## Testing Checklist

### Functionality
- [ ] Dashboard displays real-time metrics
- [ ] Process list updates every second
- [ ] Charts render correctly with 5 minutes of data
- [ ] Settings persist after restart
- [ ] Theme changes applied immediately
- [ ] Process actions (kill, suspend) work
- [ ] Service actions (start, stop) work
- [ ] Search/filter works on all pages
- [ ] Export to CSV works

### Performance
- [ ] <100ms response to user input
- [ ] <5% CPU usage at idle
- [ ] <200MB memory usage
- [ ] 1000+ processes handled smoothly
- [ ] No memory leaks over time

### UI/UX
- [ ] All pages accessible from navigation
- [ ] Responsive to window resize
- [ ] Consistent color scheme
- [ ] Keyboard navigation works
- [ ] Status messages clear and helpful
- [ ] Loading states visible
- [ ] Error messages informative

---

## Troubleshooting

### Chart Not Rendering
```
Solution: Ensure OxyPlot is properly installed
dotnet add package OxyPlot.Wpf
```

### DataGrid Shows No Data
```
Solution: Verify ItemsSource binding and converter definitions in ResourceDictionary
```

### Theme Not Changing
```
Solution: Ensure ThemeHelper.SetWindowReference() called in OnLaunched()
```

### Process List Freezing
```
Solution: Implement virtualization in DataGrid and use background thread for WMI queries
```

---

## Next Phase Preview (Phase 4)

**Advanced Features**:
- [ ] Custom alert thresholds
- [ ] Performance export (PDF)
- [ ] System tray integration
- [ ] Portable installation
- [ ] Update checker
- [ ] Plugin system

---

## Resources

### WinUI 3 Documentation
- https://learn.microsoft.com/en-us/windows/apps/winui/winui3/

### MVVM Toolkit
- https://learn.microsoft.com/en-us/windows/communitytoolkit/mvvm/

### OxyPlot Documentation
- https://oxyplot.readthedocs.io/

### DataGrid Control
- https://learn.microsoft.com/en-us/windows/communitytoolkit/controls/datagrid/

---

## Support & Issues

If you encounter issues:

1. Check the logs in `%APPDATA%/SystemPulse/logs/`
2. Review error messages in status bar
3. Create GitHub issue with:
   - Error message
   - Steps to reproduce
   - System information
   - Log excerpt

---

**Version**: 1.0  
**Last Updated**: January 11, 2026 - 20:40 UTC  
**Phase 3 Status**: Foundation Complete - Ready for UI Implementation
