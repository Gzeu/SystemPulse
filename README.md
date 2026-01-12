# SystemPulse - Advanced System Monitoring & Management

**A professional, high-performance system monitoring suite built with WinUI 3 and .NET 8.0**

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![WinUI 3](https://img.shields.io/badge/WinUI-3.0-0078D4?logo=windows)](https://docs.microsoft.com/windows/apps/winui/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Status](https://img.shields.io/badge/Status-1.0.0--Stable-brightgreen)](FINAL_COMPLETION_REPORT.md)

---

## 🎯 Project Status: ✅ **100% COMPLETE**

SystemPulse has successfully reached its 1.0.0 stable milestone. All architectural phases, UI implementations, and platform integrations are finalized.

- **Phase 1 & 2 (Foundation)**: ✅ 100%
- **Phase 3 (Core UI - 9 Pages)**: ✅ 100%
- **Phase 4 (Platform Integration)**: ✅ 100%

---

## ✨ Feature Highlights

### 🖥️ Real-Time Dashboard
- **Comprehensive Metrics**: Live tracking of CPU (all cores), RAM, GPU, Network, and Disk.
- **Modern Gauges**: High-fidelity visual feedback for primary system resources.
- **Historical Charting**: 5 categories of performance data with up to 1 hour of history using OxyPlot.

### ⚙️ Advanced Management
- **Process Control**: Search, Kill, Suspend, Resume, and Priority management.
- **Service Manager**: Full Windows Service lifecycle control (Start/Stop/Restart).
- **Startup Optimizer**: Identify and disable high-impact startup applications.
- **Session Monitor**: Track and manage active local and remote user sessions.

### 🛠️ Power User Utilities
- **Global Shortcuts**: 31 keyboard shortcuts (Ctrl+1-9, F5, etc.) for zero-mouse navigation.
- **Smart Notifications**: Windows 10/11 Toast alerts for critical resource spikes (>90%).
- **CSV Export**: Export any performance or process data for external analysis.

### 🎨 Platform Customization
- **Theme Support**: Seamless switching between Light, Dark, and System modes.
- **Window Opacity**: Adjustable transparency (50-100%) for non-intrusive monitoring.
- **Always-on-Top**: Keep the monitor visible over games or other applications.
- **Minimize to Tray**: Background operation with a native system tray context menu.

---

## 🚀 Quick Start

### Prerequisites
- Windows 10 (1809+) or Windows 11
- .NET 8.0 Runtime

### Installation (Developer)
```bash
git clone https://github.com/Gzeu/SystemPulse.git
cd SystemPulse
dotnet restore
dotnet build
dotnet run --project src/SystemPulse.App
```

---

## 📖 Documentation

### Technical Guides
- [**Win32 Integration**](docs/WIN32_INTEGRATION.md): Deep dive into P/Invoke and native features.
- [**Keyboard Shortcuts**](docs/KEYBOARD_SHORTCUTS.md): Full cheat sheet for power users.
- [**Architecture Plan**](docs/PHASE_3_IMPLEMENTATION_PLAN.md): Detailed MVVM and Service-Layer design.

### User Guides
- [**Overview & Performance**](docs/OVERVIEW_PAGE_USAGE.md): How to read the dashboard and charts.
- [**System Utilities**](docs/UTILITY_PAGES_USAGE.md): Managing Services, Startup apps, and Users.
- [**Settings & Customization**](docs/SETTINGS_PAGE_USAGE.md): Personalizing your experience.

---

## 🛠️ Technology Stack

- **Framework**: WinUI 3 (Windows App SDK)
- **Language**: C# / .NET 8.0
- **Architecture**: MVVM (CommunityToolkit.Mvvm)
- **Charting**: OxyPlot.Wpf
- **Platform APIs**: Win32 (User32/Kernel32), DXGI (DirectX), WMI
- **Logging**: Serilog

---

## 📜 License

Licensed under the **MIT License**. See [LICENSE](LICENSE) for more information.

---

**Built for performance and clarity. 🚀**  
*Last Updated: January 12, 2026*
