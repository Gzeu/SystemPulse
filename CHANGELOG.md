# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Initial project scaffolding and structure
- GitHub repository with CI/CD workflows
- Visual Studio solution and project files
- MVVM Toolkit integration setup
- WinUI 3 main window template with navigation
- Documentation framework

### Planned Features
- Overview/Dashboard with real-time performance charts
- Advanced process management and filtering
- Performance monitoring with historical data
- Startup application manager
- System services management
- Active user sessions management
- GPU usage monitoring
- Process control (kill, suspend, resume)
- System logging and snapshot export
- Theme customization (Light/Dark/System)
- Settings and preferences UI

## [0.1.0] - TBD (Project Initialization)

### Initial Release
- First public beta release
- Core system monitoring functionality
- Basic process management
- Performance visualization
- User-friendly interface

---

## Guidelines

### Versioning
- **MAJOR**: Breaking changes (e.g., API redesign)
- **MINOR**: New features (backward compatible)
- **PATCH**: Bug fixes and improvements

### Categories
- **Added**: New features
- **Changed**: Modified functionality
- **Deprecated**: Soon-to-be removed features
- **Removed**: Removed features
- **Fixed**: Bug fixes
- **Security**: Security fixes

### Commit Messages
Use conventional commits:
- `feat:` - Feature addition
- `fix:` - Bug fix
- `docs:` - Documentation
- `style:` - Code style (formatting, etc.)
- `refactor:` - Code refactoring
- `perf:` - Performance improvement
- `test:` - Test addition/modification
- `chore:` - Build, dependencies, config
- `ci:` - CI/CD changes

### Release Process
1. Update CHANGELOG.md with all changes
2. Update version in SystemPulse.App.csproj
3. Create git tag: `git tag v0.1.0`
4. Push tag: `git push origin v0.1.0`
5. GitHub Actions creates release automatically

---

For more details, see [CONTRIBUTING.md](CONTRIBUTING.md)
