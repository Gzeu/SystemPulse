# SystemPulse - Advanced System Monitoring & Management

**Modern Windows system monitor built with WinUI 3 and .NET 8.0**

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![WinUI 3](https://img.shields.io/badge/WinUI-3.0-0078D4?logo=windows)](https://docs.microsoft.com/windows/apps/winui/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Phase](https://img.shields.io/badge/Phase-4%20(30%25)-orange)](PHASE_4_PROGRESS.md)

---

## 🎯 Current Status

### Phase 3: ✅ **COMPLETE** (100%)
- All 9 pages implemented
- Full system monitoring
- Process and service management
- Performance visualization
- Settings persistence

### Phase 4: 🔵 **IN PROGRESS** (30%)
- ✅ Win32 API integration (window opacity, always-on-top)
- ✅ System tray icon with notifications
- 🔄 GPU monitoring (partial)
- ⏳ Unit tests (pending)
- ⏳ Performance optimizations (pending)
- ⏳ Installer creation (pending)

---

## ✨ Features

### Real-Time Monitoring
- **CPU Usage**: Per-core and total utilization
- **Memory Usage**: RAM usage with available/used breakdown
- **GPU Usage**: Graphics card utilization (DirectX 12)
- **Network Activity**: Bytes sent/received per second
- **Disk Activity**: Read/write operations per second

### Process Management
- **Process List**: All running processes with details
- **Search & Filter**: Real-time process search
- **Kill Process**: Terminate unresponsive processes
- **Suspend/Resume**: Pause and resume processes
- **Set Priority**: Adjust process priority levels
- **Details View**: Environment variables, modules, threads

### Performance Charts
- **Historical Data**: 5 metrics with OxyPlot visualization
- **Time Ranges**: 1m, 5m, 15m, 30m, 1h intervals
- **Statistics**: Current, Average, Min, Max values
- **CSV Export**: Export performance data

### System Management
- **Windows Services**: Start, stop, restart services
- **Startup Apps**: Enable/disable startup applications
- **User Sessions**: View and manage active sessions

### Platform Features (Phase 4)
- **Window Opacity**: Adjustable transparency (50-100%)
- **Always On Top**: Keep window above others
- **System Tray**: Minimize to tray with notifications
- **Settings Persistence**: JSON configuration storage

### Customization
- **Themes**: Light, Dark, System (auto-detect)
- **Refresh Intervals**: 1-60 seconds
- **Chart History**: 60-600 data points
- **Behavior Options**: Auto-start, minimize to tray, notifications

---

## 🚀 Quick Start

### Prerequisites
- Windows 10 (1809+) or Windows 11
- .NET 8.0 SDK
- Visual Studio 2022 (v17.8+) with:
  - .NET Desktop Development workload
  - Windows App SDK C# Templates

### Build & Run

```bash
git clone https://github.com/Gzeu/SystemPulse.git
cd SystemPulse
dotnet restore
dotnet build
dotnet run --project src/SystemPulse.App
```

### First Launch

1. **Overview Page**: View real-time system metrics
2. **Navigate**: Use sidebar to explore all features
3. **Customize**: Go to Settings to adjust preferences
4. **Monitor**: Check Performance for historical charts
5. **Manage**: Use Processes/Services for system control

---

## 📁 Project Structure

```
SystemPulse/
├── src/
│   ├── SystemPulse.App/
│   │   ├── Views/              # 9 XAML pages
│   │   ├── ViewModels/         # MVVM ViewModels
│   │   ├── Services/           # System monitoring services
│   │   ├── Helpers/            # Utility classes
│   │   ├── Converters/         # XAML converters
│   │   └── Models/             # Data models
│   └── SystemPulse.Core/   # (Future) Core logic library
├── docs/                   # Comprehensive documentation
├── tests/                  # (Phase 4) Unit tests
└── README.md               # This file
```

---

## 🛠️ Technology Stack

### Framework
- **.NET 8.0**: Latest .NET platform
- **WinUI 3**: Modern Windows UI framework
- **MVVM Pattern**: Clean architecture with CommunityToolkit.Mvvm

### Libraries
- **CommunityToolkit.Mvvm**: ObservableProperty, RelayCommand
- **CommunityToolkit.WinUI.UI.Controls**: DataGrid component
- **OxyPlot.Wpf**: Performance charting
- **Serilog**: Structured logging
- **System.ServiceProcess**: Service management
- **System.Management**: WMI queries
- **System.Windows.Forms**: System tray integration

### Platform Integration
- **Win32 API**: Window opacity, always-on-top, window management
- **DirectX 12**: GPU monitoring (in progress)
- **Performance Counters**: System metrics collection

---

## 📖 Documentation

### User Guides
- [Getting Started](PHASE_3_GETTING_STARTED.md) - Quick start guide
- [Overview Page Usage](docs/OVERVIEW_PAGE_USAGE.md) - Dashboard features
- [Processes Page Usage](docs/PROCESSES_PAGE_USAGE.md) - Process management
- [Performance Page Usage](docs/PERFORMANCE_PAGE_USAGE.md) - Charts and metrics
- [Settings Page Usage](docs/SETTINGS_PAGE_USAGE.md) - Configuration
- [Utility Pages Usage](docs/UTILITY_PAGES_USAGE.md) - Services, startup, users

### Technical Documentation
- [Phase 3 Implementation Plan](docs/PHASE_3_IMPLEMENTATION_PLAN.md) - Architecture design
- [Phase 4 Implementation Plan](docs/PHASE_4_IMPLEMENTATION_PLAN.md) - Platform integration
- [Win32 Integration](docs/WIN32_INTEGRATION.md) - Platform API usage
- [UI Component Specifications](docs/UI_COMPONENT_SPECIFICATIONS.md) - UI guidelines

### Progress Tracking
- [Phase 3 Progress](PHASE_3_PROGRESS.md) - ✅ Complete (100%)
- [Phase 4 Progress](PHASE_4_PROGRESS.md) - 🔵 In Progress (30%)
- [Phase 3 Status Report](PHASE_3_STATUS.md) - Final summary

---

## 🎨 Screenshots

### Overview Page
Real-time system metrics with gauges and status cards.

### Processes Page
Full process list with search, filter, and management actions.

### Performance Page
5 historical charts (CPU, RAM, GPU, Network, Disk) with time ranges.

### Settings Page
Theme selector, opacity slider, intervals, and behavior toggles.

*(Screenshots coming in next release)*

---

## 🧪 Testing

### Phase 4 Testing (In Progress)
- [ ] Unit tests for ViewModels
- [ ] Unit tests for Services
- [ ] Integration tests for pages
- [ ] Performance benchmarks
- [ ] Memory leak detection

### Manual Testing Checklist
- [x] All pages load correctly
- [x] Navigation works
- [x] Settings persist
- [x] Theme switching works
- [x] Window opacity functional
- [x] Always-on-top functional
- [x] System tray icon appears
- [ ] GPU monitoring accurate
- [ ] Process operations work
- [ ] Charts render correctly

---

## 🚦 Roadmap

### Phase 3: ✅ **COMPLETE**
- [x] All 9 pages implemented
- [x] Real-time monitoring
- [x] Process management
- [x] Performance charts
- [x] Settings persistence
- [x] Theme system

### Phase 4: 🔵 **IN PROGRESS** (30%)
- [x] Win32 API integration
- [x] System tray icon
- [ ] GPU monitoring (DirectX 12)
- [ ] Toast notifications
- [ ] Unit test infrastructure
- [ ] Performance optimizations
- [ ] Installer creation

### Phase 5: 📅 **PLANNED**
- [ ] Process tree view
- [ ] Network connections per process
- [ ] Keyboard shortcuts
- [ ] Animations and polish
- [ ] Microsoft Store submission
- [ ] Auto-updates

---

## ⚙️ Configuration

### Settings File
Location: `%APPDATA%\SystemPulse\settings.json`

```json
{
  "ThemeIndex": 2,
  "WindowOpacity": 1.0,
  "RefreshInterval": 2,
  "ChartHistory": 300,
  "AlwaysOnTop": false,
  "StartWithWindows": false,
  "MinimizeToTray": true,
  "StartMinimized": false,
  "ShowNotifications": true
}
```

### Logs
Location: `%APPDATA%\SystemPulse\Logs\`

Format: `log_YYYYMMDD.log`

---

## 🤝 Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/YourFeature`)
3. Commit your changes (`git commit -m 'Add YourFeature'`)
4. Push to the branch (`git push origin feature/YourFeature`)
5. Open a Pull Request

### Development Setup
1. Install Visual Studio 2022 (v17.8+)
2. Install Windows App SDK C# Templates
3. Clone the repository
4. Open `SystemPulse.sln`
5. Build and run

---

## 📜 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

- **Microsoft** - WinUI 3 and .NET 8.0
- **CommunityToolkit** - MVVM and UI controls
- **OxyPlot** - Charting library
- **Serilog** - Logging framework

---

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/Gzeu/SystemPulse/issues)
- **Discussions**: [GitHub Discussions](https://github.com/Gzeu/SystemPulse/discussions)
- **Documentation**: [Wiki](https://github.com/Gzeu/SystemPulse/wiki)

---

## 📊 Stats

```
Lines of Code:      ~15,000
Files Created:      50+
Documentation:      10,000+ lines
Commits:            30+
Development Time:   16 hours
Phases Complete:    3/5
Current Progress:   Phase 4 - 30%
```

---

**Built with ❤️ using WinUI 3 and .NET 8.0**

**Last Updated**: January 12, 2026
