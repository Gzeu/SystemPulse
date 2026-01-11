# SystemPulse

[![Build and Test](https://github.com/Gzeu/SystemPulse/actions/workflows/build.yml/badge.svg)](https://github.com/Gzeu/SystemPulse/actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![Version](https://img.shields.io/badge/Version-0.2.0--Phase3-blue)
![Platform](https://img.shields.io/badge/Platform-Windows%2010%2B-0078d4)
![Framework](https://img.shields.io/badge/Framework-.NET%208.0-512bd4)

Advanced system monitoring application for Windows 10/11/12 built with **WinUI 3** and **.NET 8**.

## 🎯 Project Status

### Phase 2: ✅ COMPLETE (Foundation & Services)
- Core service architecture implemented
- MVVM pattern with CommunityToolkit
- 5 core ViewModels
- 9 UI pages scaffolded
- Dependency injection configured

### Phase 3: 🔄 IN PROGRESS (UI Implementation)
- **Foundation Complete** ✅
  - Theme helper system
  - Chart data helpers
  - 5 XAML converters
  - 3 new ViewModels (Services, Startup, Users)
  - OxyPlot charting library
  - CommunityToolkit DataGrid

- **Next Steps**:
  - Dashboard with real-time gauges
  - Process management DataGrid
  - Performance charts
  - Settings implementation
  - Service/Startup/Users pages
  - Theme switching & UI polish

---

## ✨ Features

### Real-time Monitoring
- **CPU Usage**: Live percentage with historical tracking
- **Memory (RAM)**: Usage percentage and peak analysis  
- **GPU Monitoring**: Graphics card performance (DirectX 12+)
- **Network**: Bandwidth utilization (Mbps)
- **Disk I/O**: Read/write performance tracking
- **System Health**: Process & thread count

### Process Management
- ✅ List all running processes with real-time stats
- 🔄 Sort and filter by CPU, memory, name
- ✅ Kill or suspend processes
- 🔄 View detailed process information
- 🔄 Process tree visualization
- 🔄 Priority adjustment

### System Services
- ✅ View Windows services status
- ✅ Start/stop/restart services
- 🔄 Configure startup mode
- 🔄 Service dependency tracking

### Performance Analysis
- 🔄 5-minute real-time charts
- 🔄 Peak/average statistics
- 🔄 Exportable reports (CSV)
- 🔄 System snapshots

### Advanced Features
- 🔄 Auto-start management
- 🔄 Active user sessions
- 🔄 Theme support (Light/Dark)
- 🔄 Customizable refresh intervals
- 🔄 System alerts & thresholds

**Legend**: ✅ Implemented | 🔄 In Progress | 📋 Planned

---

## 📋 Requirements

- **OS**: Windows 10 (19041+) or Windows 11/12
- **Framework**: .NET 8.0 Runtime
- **RAM**: 256 MB minimum
- **Privileges**: Administrator (for process/service management)

---

## 🚀 Quick Start

### From Release (Coming Soon)
1. Download latest release
2. Extract to desired location
3. Run `SystemPulse.exe`

### Build from Source

**Prerequisites**:
- Visual Studio 2022 or JetBrains Rider
- .NET 8 SDK
- Windows App SDK 1.6+

**Build**:
```bash
git clone https://github.com/Gzeu/SystemPulse.git
cd SystemPulse
dotnet build
dotnet run --project src/SystemPulse.App
```

---

## 🏗️ Architecture

```
┌─────────────────────────────────────┐
│   UI Layer (WinUI 3)                │
│   - 9 XAML Pages + MainWindow       │
│   - Real-time Data Binding          │
│   - Charts & Gauges                 │
└────────────┬────────────────────────┘
             │ Binding
┌────────────▼────────────────────────┐
│   ViewModel Layer (MVVM)            │
│   - 8 ViewModels                    │
│   - Observable Properties           │
│   - Relay Commands                  │
└────────────┬────────────────────────┘
             │ Service Calls
┌────────────▼────────────────────────┐
│   Service Layer (Dependency-Injected)│
│   - ISystemMonitorService           │
│   - IProcessService                 │
│   - IWMIService                     │
│   - ILoggingService                 │
│   - ISettingsService                │
└────────────┬────────────────────────┘
             │ WMI/Performance Counters
┌────────────▼────────────────────────┐
│   Windows APIs                      │
│   - WMI (services, processes, GPU)  │
│   - Performance Counters (CPU, RAM) │
│   - Registry (settings, startup)    │
└─────────────────────────────────────┘
```

---

## 📁 Project Structure

```
SystemPulse/
├── .github/
│   └── workflows/                 # CI/CD pipelines
│       ├── build.yml             # Build & test
│       └── release.yml           # Release automation
├── docs/
│   ├── ARCHITECTURE.md           # Design documentation
│   ├── PHASE_3_IMPLEMENTATION_PLAN.md
│   └── UI_COMPONENT_SPECIFICATIONS.md
├── src/
│   ├── SystemPulse.App/          # WinUI Application (55+ files)
│   │   ├── Services/             # 10 service files
│   │   ├── ViewModels/           # 8 ViewModel files  
│   │   ├── Views/                # 9 XAML page pairs
│   │   ├── Converters/           # 5 XAML value converters
│   │   ├── Helpers/              # 4 helper utilities
│   │   ├── Models/               # 3 data models
│   │   ├── App.xaml & App.xaml.cs
│   │   ├── MainWindow.xaml & .xaml.cs
│   │   └── SystemPulse.App.csproj
│   ├── SystemPulse.Core/         # Core business logic
│   └── SystemPulse.Tests/        # Unit tests
├── COMPLETION_SUMMARY.md         # Phase 2 summary
├── PHASE_3_GETTING_STARTED.md    # Phase 3 guide
├── README.md                     # This file
└── .gitignore
```

---

## 🛠️ Technology Stack

### Framework & Language
- **Framework**: .NET 8.0 (LTS)
- **Language**: C# 12
- **UI Framework**: WinUI 3

### Key Libraries
- **MVVM**: CommunityToolkit.MVVM (8.2.2)
- **Logging**: Serilog (3.1.1)
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection (8.0.0)
- **System Monitoring**: System.Management (4.7.0)
- **Performance Counters**: System.Diagnostics.PerformanceCounter (8.0.0)
- **Charting**: OxyPlot.Wpf (2.1.2)
- **DataGrid**: CommunityToolkit.WinUI.Controls.DataGrid (7.1.2)

---

## 🎮 Usage

### Dashboard View
1. Launch the application
2. View real-time system metrics in gauge format
3. Monitor CPU, RAM, GPU, and network usage
4. Check system uptime and process count

### Process Management
1. Navigate to "Processes" tab
2. View list of running processes
3. Sort by CPU, Memory, or Name
4. Search for specific processes
5. Right-click for options: Kill, Suspend, Restart
6. Set process priority level

### Performance Monitoring
1. Go to "Performance" tab
2. View 5-minute historical graphs
3. Select time range (1m, 5m, 15m, 30m, 1h)
4. View statistics (Current, Min, Max, Average)
5. Export data to CSV

### Configuration
1. Open Settings
2. Choose theme (Light/Dark/System)
3. Set refresh interval (1-60 seconds)
4. Adjust window opacity
5. Enable/disable notifications

---

## 📊 Performance Targets

- **UI Response Time**: <100ms
- **Memory Usage**: <200MB
- **CPU Usage (Idle)**: <2%
- **CPU Usage (Monitoring)**: <5%
- **Chart Rendering**: <500ms for 300 points
- **Process List (1000+ items)**: <1 second refresh

---

## 🗺️ Roadmap

### Phase 2 ✅ COMPLETE
- Core service architecture
- MVVM pattern setup
- UI page scaffolding
- Dependency injection

### Phase 3 🔄 IN PROGRESS
- Dashboard implementation
- Process management UI
- Performance charts
- Settings & theme support
- All 9 pages fully functional

### Phase 4 📋 PLANNED
- Custom alert thresholds
- Performance export (PDF)
- System tray integration
- Plugin architecture

### Phase 5 📋 PLANNED
- Performance optimization
- UI refinements
- Accessibility improvements
- Localization

### Phase 6 📋 PLANNED
- Release build & packaging
- Installation wizard
- Final documentation
- Public release

---

## 🐛 Known Issues

### Phase 3 (In Development)
- GPU monitoring requires DirectX 12 API (not yet implemented)
- Some protected processes may refuse icon access
- WMI queries can be slow on systems with 1000+ processes
- Theme switching requires application restart (WinUI limitation)

---

## 📝 Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Development Guidelines
- Follow C# naming conventions
- Use MVVM pattern for new features
- Add unit tests for new logic
- Update documentation
- Test on Windows 10/11

---

## 📄 License

This project is licensed under the MIT License - see [LICENSE](LICENSE) file for details.

---

## 👤 Author

**Gzeu** - Full-stack developer specializing in system tools and productivity applications.

- GitHub: [@Gzeu](https://github.com/Gzeu)
- Location: București, Romania

---

## 🙏 Acknowledgments

- Microsoft for WinUI 3 framework
- Community Toolkit team for MVVM support
- OxyPlot project for charting library
- All contributors and testers

---

## 📞 Support

### Documentation
- [Architecture Guide](docs/ARCHITECTURE.md)
- [Phase 3 Plan](docs/PHASE_3_IMPLEMENTATION_PLAN.md)
- [UI Specifications](docs/UI_COMPONENT_SPECIFICATIONS.md)
- [Getting Started](PHASE_3_GETTING_STARTED.md)

### Issues & Feedback
- **Bug Reports**: [GitHub Issues](https://github.com/Gzeu/SystemPulse/issues)
- **Feature Requests**: [GitHub Discussions](https://github.com/Gzeu/SystemPulse/discussions)
- **Security Issues**: security@example.com (responsible disclosure)

---

## 🔗 Links

- **Repository**: [github.com/Gzeu/SystemPulse](https://github.com/Gzeu/SystemPulse)
- **Issues**: [github.com/Gzeu/SystemPulse/issues](https://github.com/Gzeu/SystemPulse/issues)
- **Discussions**: [github.com/Gzeu/SystemPulse/discussions](https://github.com/Gzeu/SystemPulse/discussions)

---

**Current Version**: 0.2.0 (Phase 3 Foundation)  
**Last Updated**: January 11, 2026 - 20:42 UTC  
**Status**: 🔄 UI Implementation in Progress
