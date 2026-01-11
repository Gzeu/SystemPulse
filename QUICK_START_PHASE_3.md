# Quick Start - Phase 3

## TL;DR - Get Running in 5 Minutes

### 1. Clone & Build
```bash
git clone https://github.com/Gzeu/SystemPulse.git
cd SystemPulse
dotnet build
```

### 2. Run
```bash
dotnet run --project src/SystemPulse.App
```

### 3. What You'll See
- Application launches with navigation menu
- Status bar shows live CPU/RAM/GPU metrics
- Empty pages ready for implementation
- No errors (all foundation in place)

---

## What's Ready

✅ **Services**: CPU, RAM, GPU, Process, Logging, Settings  
✅ **ViewModels**: 8 ViewModels with data binding setup  
✅ **Helpers**: Theme, Charts, Dialogs, Export utilities  
✅ **Converters**: 5 XAML value converters  
✅ **Charts**: OxyPlot library ready  
✅ **DataGrid**: CommunityToolkit DataGrid ready  
✅ **DI**: Dependency injection configured  
✅ **Pages**: 9 pages scaffolded, ready for UI  

---

## Key Files to Understand

### Understand the Architecture
```
📖 docs/ARCHITECTURE.md              - Overall design
📖 docs/PHASE_3_IMPLEMENTATION_PLAN.md - What to build
📖 docs/UI_COMPONENT_SPECIFICATIONS.md - How it should look
📖 PHASE_3_GETTING_STARTED.md        - Implementation guide
```

### View the Code
```
📂 src/SystemPulse.App/
  ├── ViewModels/                    - 8 ViewModels ready
  ├── Views/                         - 9 pages to implement
  ├── Helpers/                       - 4 helper utilities
  ├── Converters/                    - 5 XAML converters
  └── Services/                      - Core service layer
```

---

## Next Steps

### Option 1: Start Simple (Recommended)
1. Open `OverviewPage.xaml`
2. Add gauges using WinUI ProgressRing
3. Bind to `ShellViewModel.SystemMetrics`
4. See real-time data flow

### Option 2: Start Complex
1. Open `ProcessesPage.xaml`
2. Add DataGrid from CommunityToolkit
3. Bind to `ProcessesViewModel.Processes`
4. Implement search/filter

### Option 3: Understand First
1. Read PHASE_3_GETTING_STARTED.md
2. Review UI_COMPONENT_SPECIFICATIONS.md
3. Study existing ViewModel code
4. Then start implementation

---

## Common Commands

```bash
# Build
dotnet build

# Run debug
dotnet run --project src/SystemPulse.App

# Run release
dotnet run --project src/SystemPulse.App -c Release

# Clean
dotnet clean

# Build only
dotnet build --no-restore
```

---

## Architecture at a Glance

```
UI (XAML Pages)
    ↓ Binding
ViewModels (MVVM)
    ↓ Service Calls
Services (WMI, Performance Counters)
    ↓ Windows APIs
Operating System
```

---

## Example: Real-time CPU Display

### The Data Flow
1. Windows OS → Performance Counter API
2. SystemMonitorService reads CPU%
3. ShellViewModel exposes SystemMetrics
4. XAML binding updates TextBlock every second
5. User sees live CPU% on screen

### The Code
```csharp
// ViewModel
[ObservableProperty]
private PerformanceMetrics systemMetrics;

// XAML
<TextBlock Text="{Binding SystemMetrics.CPUUsage, 
           Converter={StaticResource PercentageConverter}}" />
```

---

## Quick Troubleshooting

**App won't build?**
```bash
dotnet restore
dotnet build
```

**Missing packages?**
```bash
dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget
```

**Pages are empty?**
That's normal - UI implementation is Phase 3 work.

**Want to see charts?**
Implement PerformancePage with OxyPlot.

---

## Success Indicators

✅ Application launches without errors  
✅ Navigation menu visible  
✅ Status bar shows CPU/RAM/GPU  
✅ Pages are blank (ready for implementation)  
✅ Logs appear in %APPDATA%/SystemPulse/logs/  

---

## Resources

- [WinUI 3 Docs](https://learn.microsoft.com/en-us/windows/apps/winui/)
- [MVVM Toolkit](https://learn.microsoft.com/en-us/windows/communitytoolkit/mvvm/)
- [OxyPlot Docs](https://oxyplot.readthedocs.io/)
- [DataGrid Guide](https://learn.microsoft.com/en-us/windows/communitytoolkit/controls/datagrid/)

---

## Ready to Code?

✅ Yes! Start with `PHASE_3_GETTING_STARTED.md`

---

*Last Updated: January 11, 2026*
