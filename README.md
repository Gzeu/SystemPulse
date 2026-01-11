# SystemPulse

[![Build and Test](https://github.com/Gzeu/SystemPulse/actions/workflows/build.yml/badge.svg)](https://github.com/Gzeu/SystemPulse/actions/workflows/build.yml)

Advanced system monitoring application for Windows 10/11 built with WinUI 3 and .NET 8.

## Features

### Real-time Monitoring
- **CPU Usage**: Live percentage with historical graphs
- **Memory (RAM)**: Usage tracking and peak analysis
- **GPU Monitoring**: Graphics card performance (DirectX 12+)
- **Network**: Bandwidth utilization
- **Disk I/O**: Read/write performance

### Process Management
- List all running processes
- Sort and filter by CPU, memory, name
- Kill or suspend processes
- View detailed process information
- Process tree visualization

### System Services
- View Windows services status
- Start/stop/restart services
- Configure startup mode
- Service dependency tracking

### Performance Analysis
- 5-minute performance history
- Peak/average statistics
- Exportable performance reports
- System snapshot generation

### Advanced Features
- Auto-start application management
- Active user sessions monitoring
- System resource alerts
- Dark/Light theme support
- Customizable refresh intervals

## Requirements

- Windows 10 (19041+) or Windows 11
- .NET 8.0 Runtime
- Administrator privileges for process/service management

## Installation

### From Release
1. Download latest release from [GitHub Releases](https://github.com/Gzeu/SystemPulse/releases)
2. Extract to desired location
3. Run `SystemPulse.exe`

### Build from Source
```bash
git clone https://github.com/Gzeu/SystemPulse.git
cd SystemPulse
dotnet build src/SystemPulse.App
dotnet run --project src/SystemPulse.App
```

## Development

### Prerequisites
- Visual Studio 2022 or JetBrains Rider
- .NET 8 SDK
- Windows App SDK 1.6+

### Architecture
See [ARCHITECTURE.md](docs/ARCHITECTURE.md) for detailed design documentation.

### Running Tests
```bash
dotnet test
```

### Building Release
```bash
dotnet publish src/SystemPulse.App -c Release
```

## Project Structure

- `src/SystemPulse.App/` - WinUI application
- `src/SystemPulse.Core/` - Core business logic
- `src/SystemPulse.Tests/` - Unit tests
- `docs/` - Documentation

## Technologies

- **UI Framework**: WinUI 3
- **Language**: C# 12
- **Framework**: .NET 8.0
- **Pattern**: MVVM with CommunityToolkit.MVVM
- **Logging**: Serilog
- **DI**: Microsoft.Extensions.DependencyInjection

## Roadmap

- [x] Core process monitoring
- [x] Service management
- [ ] GPU monitoring (NVIDIA/AMD specific)
- [ ] Custom alerts and thresholds
- [ ] Performance export (CSV/JSON)
- [ ] Plugin system
- [ ] Portable version

## License

MIT License - See LICENSE file for details

## Contributing

Contributions welcome! Please:
1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Open a Pull Request

## Support

For issues and feature requests, please use the [GitHub Issues](https://github.com/Gzeu/SystemPulse/issues) tracker.

---

**Author**: Gzeu  
**Version**: 0.1.0  
**Last Updated**: January 2026
