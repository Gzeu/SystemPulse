# SystemPulse Architecture

## Overview
SystemPulse is a modular Windows system monitoring application built with WinUI 3 and .NET 8.

## Project Structure

```
src/
├── SystemPulse.App/          # WinUI application
│   ├── Services/            # Service implementations
│   ├── ViewModels/          # MVVM ViewModels
│   ├── Views/               # XAML Pages
│   └── Helpers/             # Utility helpers
├── SystemPulse.Core/        # Core business logic
│   ├── Models/
│   ├── Extensions/
│   └── Abstractions/
└── SystemPulse.Tests/       # Unit tests
```

## Service Layer

### ISystemMonitorService
Core performance monitoring using WMI and PerformanceCounters.

### IProcessService
Process enumeration, filtering, and management.

### IWMIService
WMI queries for services, GPU, and user management.

### ILoggingService
Serilog-based logging with file rotation.

### ISettingsService
User settings persistence using JSON.

## MVVM Pattern

Using CommunityToolkit.MVVM for:
- ObservableObject
- RelayCommand
- ObservableProperty with automatic property change notification

## Dependencies

- **WinUI 3**: Modern Windows UI framework
- **MVVM Toolkit**: Simplifies ViewModel implementation
- **Serilog**: Structured logging
- **System.Management**: WMI access
- **System.Diagnostics.PerformanceCounter**: Performance metrics

## Data Flow

```
Services (WMI, PerformanceCounter)
    ↓
ViewModels (ObservableProperty)
    ↓
Views (XAML Binding)
    ↓
UI (Real-time Updates)
```
