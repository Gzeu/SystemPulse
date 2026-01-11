# 💓 SystemPulse

> **Real-time System Heartbeat** – Advanced System Monitor for Windows 10/11/12

A modern, beautiful, and feature-rich Task Manager alternative for Windows built with **WinUI 3**, **.NET 9**, and **Fluent Design**. Featuring real-time performance monitoring, live resource graphs, process management, startup optimization, and much more.

[![GitHub Stars](https://img.shields.io/github/stars/Gzeu/SystemPulse?style=flat-square)](https://github.com/Gzeu/SystemPulse)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](LICENSE)
[![Build Status](https://img.shields.io/github/actions/workflow/status/Gzeu/SystemPulse/build.yml?style=flat-square)](https://github.com/Gzeu/SystemPulse/actions)

---

## ✨ Features

### 📊 Dashboard & Monitoring
- **Real-time Performance Overview** – Live CPU, RAM, GPU, Disk, and Network graphs
- **Responsive Charts** – Smooth, animated performance metrics with 1-2 second refresh rates
- **Advanced Filtering & Sorting** – Search processes, sort by CPU/RAM/Disk/Network usage
- **Performance History** – 60-second and 5-minute historical data visualization

### 🎯 Process Management
- **Advanced Process List** – Name, PID, User, CPU%, RAM, Disk I/O, GPU usage, Command Line, Start Time
- **Process Control** – End Task, End Process Tree, Suspend/Resume processes
- **Process Icons** – Extracted from executables where available
- **Safe Termination** – Confirmation dialogs and critical process protection (System, csrss, winlogon, etc.)

### 🚀 System Optimization
- **Startup Manager** – Enable/disable startup apps with estimated impact (High/Medium/Low)
- **Services Management** – View, start, stop, restart services with descriptions
- **Active Sessions** – Manage connected users and remote sessions

### 🎨 Modern Design
- **Fluent Design System 2026** – Mica/Acrylic backdrops, smooth animations
- **Theme Support** – Light/Dark/System modes with automatic Mica fallback on Windows 10
- **Premium Color Palette** – Cyan (#00D4FF), Violet (#7C3AED), Teal (#10B981) accents
- **Always-on-Top Mode** – Semi-transparent overlay support

### ⚙️ Advanced Features
- **GPU Monitoring** – Real NVIDIA/AMD GPU usage detection (WMI/Performance Counters)
- **System Logging** – Event logging with export snapshot functionality
- **Customizable UI** – Refresh rate, opacity, theme settings
- **Lightweight & Fast** – Minimal resource footprint, optimized for modern Windows

---

## 🖼️ Screenshots

*Coming soon – UI development in progress*

---

## 📋 Requirements

- **OS**: Windows 10 (Build 19041+) or Windows 11/12
- **Framework**: .NET 9 LTS
- **SDK**: Windows App SDK 1.8+
- **Architecture**: x64 / ARM64

---

## 🚀 Quick Start

### Prerequisites
- Visual Studio 2022 (v17.8+) with WinUI 3 workload
- .NET 9 SDK
- Windows App SDK 1.8+

### Installation

1. **Clone Repository**
   ```bash
   git clone https://github.com/Gzeu/SystemPulse.git
   cd SystemPulse
   ```

2. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

3. **Build Project**
   ```bash
   dotnet build -c Release
   ```

4. **Run Application**
   ```bash
   dotnet run -p src/SystemPulse.App/SystemPulse.App.csproj
   ```

---

## 📁 Project Structure

```
SystemPulse/
├── .github/
│   ├── workflows/
│   │   ├── build.yml              # CI/CD build pipeline
│   │   ├── release.yml            # Automated releases
│   │   └── codeql.yml             # Security analysis
│   └── ISSUE_TEMPLATE/
├── src/
│   ├── SystemPulse.App/           # WinUI 3 main application
│   │   ├── Views/                 # XAML pages (Overview, Processes, etc.)
│   │   ├── ViewModels/            # MVVM ViewModel layer
│   │   ├── Models/                # Data models
│   │   ├── Services/              # System monitoring, logging, settings
│   │   ├── Helpers/               # Utilities and converters
│   │   ├── Resources/             # Styles, colors, icons
│   │   └── MainWindow.xaml
│   ├── SystemPulse.Core/          # (Optional) Shared business logic
│   └── SystemPulse.Tests/         # Unit and integration tests
├── docs/
│   ├── ARCHITECTURE.md            # Technical architecture
│   ├── SETUP.md                   # Detailed setup guide
│   └── API_REFERENCE.md           # Internal API documentation
├── .gitignore
├── LICENSE
├── CHANGELOG.md
└── SystemPulse.sln
```

---

## 🛠️ Technology Stack

| Technology | Version | Purpose |
|-----------|---------|---------|
| **WinUI 3** | 1.8.0+ | Modern Windows UI framework |
| **.NET** | 9 LTS | Application runtime |
| **Windows App SDK** | 1.8+ | Windows platform APIs |
| **MVVM Toolkit** | 8.2+ | ViewModel/Model binding |
| **LiveCharts2** | 2.1.0+ | Real-time performance charts |
| **WinUI Community Toolkit** | 8.0+ | UI controls & helpers |
| **Serilog** | 3.1+ | Structured logging |
| **WMI / Diagnostics** | Built-in | System monitoring APIs |

---

## 🎯 Development Roadmap

### Phase 1: Core UI & Architecture ✅
- [x] Project structure and scaffolding
- [x] MVVM setup with toolkit
- [ ] Main window with NavigationView
- [ ] Fluent Design styling
- [ ] Theme support (Light/Dark/System)

### Phase 2: Core Pages
- [ ] Overview/Dashboard with live charts
- [ ] Processes page with advanced list
- [ ] Performance page with history
- [ ] Startup manager
- [ ] Services management

### Phase 3: System Integration
- [ ] Performance counter monitoring
- [ ] WMI GPU detection
- [ ] Process control (kill, suspend, resume)
- [ ] Safe termination with confirmations
- [ ] System logging

### Phase 4: Polish & Optimization
- [ ] Icon extraction from executables
- [ ] Startup impact estimation
- [ ] Settings and preferences
- [ ] Export/snapshot functionality
- [ ] Performance optimization

### Phase 5: Release
- [ ] Automated builds and releases
- [ ] Microsoft Store publishing (optional)
- [ ] Version 1.0 release

---

## 📚 Documentation

- **[SETUP.md](docs/SETUP.md)** – Detailed development environment setup
- **[ARCHITECTURE.md](docs/ARCHITECTURE.md)** – Technical design and patterns
- **[API_REFERENCE.md](docs/API_REFERENCE.md)** – Internal APIs and services
- **[CONTRIBUTING.md](CONTRIBUTING.md)** – How to contribute to the project

---

## 🔧 Build & Development

### Local Development
```bash
# Restore dependencies
dotnet restore

# Build debug
dotnet build -c Debug

# Run application
dotnet run -p src/SystemPulse.App/SystemPulse.App.csproj

# Run tests
dotnet test
```

### Release Build
```bash
dotnet publish -c Release -o ./publish
```

---

## 🤝 Contributing

Contributions are welcome! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

### Development Workflow
1. Fork the repository
2. Create feature branch: `git checkout -b feature/your-feature`
3. Commit changes: `git commit -am 'Add feature: description'`
4. Push to branch: `git push origin feature/your-feature`
5. Submit Pull Request

---

## 📄 License

This project is licensed under the **MIT License** – see [LICENSE](LICENSE) for details.

---

## 👨‍💻 Author

**[George Pricop](https://github.com/Gzeu)** – Blockchain Developer & AI Automation Specialist  
Based in București, Romania

---

## 🌟 Support

- ⭐ **Star** this repository if you find it useful!
- 💬 **Issues** – Report bugs or request features
- 📧 **Contact** – Reach out on GitHub

---

## 🔗 Resources

- [WinUI 3 Documentation](https://learn.microsoft.com/en-us/windows/apps/winui/winui3/)
- [Windows App SDK Docs](https://learn.microsoft.com/en-us/windows/apps/windows-app-sdk/)
- [MVVM Toolkit](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)
- [LiveCharts2](https://livecharts.dev/)
- [Fluent Design System](https://www.microsoft.com/design/fluent)

---

**Made with ❤️ by [Gzeu](https://github.com/Gzeu)**
