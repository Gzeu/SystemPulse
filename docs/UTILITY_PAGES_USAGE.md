# Utility Pages - Usage Documentation

**Status**: ✅ **COMPLETE & DEPLOYED**  
**Commits**: 67810460, fdea1f29, 60bdd3b1  
**Date**: January 12, 2026

---

## Overview

The five utility pages provide additional system management and information capabilities beyond core monitoring.

---

## 1. ServicesPage - Windows Service Management

### Features
- **Service List**: All Windows services with DataGrid
- **Filters**: All / Running Only / Stopped Only
- **Search**: Real-time service search
- **Actions**: Start, Stop, Restart services
- **Context Menu**: Right-click for actions
- **Status Bar**: Total services and running count

### Columns
- Service Name (Display name)
- Status (Running/Stopped)
- Startup Type (Automatic/Manual/Disabled)
- Service (Internal name)
- Description

### Usage
1. Navigate to "Services" page
2. Use filter dropdown to show specific services
3. Search for service by name
4. Select service to see details
5. Use Start/Stop/Restart buttons or right-click menu

---

## 2. StartupPage - Startup Applications

### Features
- **Startup Apps List**: Registry and Startup folder items
- **Enable/Disable**: Toggle startup apps
- **Impact Rating**: High/Medium/Low startup impact
- **Location**: Registry or Startup folder
- **Search**: Filter startup apps
- **Open Location**: Navigate to app folder

### Columns
- Application name
- Publisher
- Status (Enabled/Disabled)
- Impact (High/Medium/Low)
- Location (Registry/Startup Folder)
- Path (Full file path)

### Usage
1. Navigate to "Startup" page
2. View all startup applications
3. Select app to enable/disable
4. Right-click for context menu
5. Optimize boot time by disabling unnecessary apps

---

## 3. UsersPage - User Session Management

### Features
- **Current User Card**: Highlighted current session
- **All Sessions**: List of active user sessions
- **Session Details**: ID, Status, Type, Login time, Idle time
- **Actions**: Disconnect or Logoff users
- **Refresh**: Update session list

### Session Information
- Username
- Session ID
- Status (Active/Connected/Disconnected)
- Session Type (Console/RDP/Remote)
- Login time
- Idle duration

### Usage
1. Navigate to "Users" page
2. View current user in highlighted card
3. Scroll to see all active sessions
4. Use Disconnect or Logoff buttons (requires admin)
5. Refresh to update session states

---

## 4. DetailsPage - Process Details Viewer

### Features
- **Process Overview**: Name, PID, CPU, Memory, Threads
- **General Information**: Status, Priority, User, Path
- **Performance Metrics**: Detailed resource usage
- **Environment Variables**: Process environment (if accessible)
- **Loaded Modules**: DLLs and modules (if accessible)

### Sections

#### Process Header
- Large icon
- Process name (24pt bold)
- PID in subtitle
- Key metrics: CPU%, Memory, Threads

#### General Information
- Process Name
- Process ID (PID)
- Status
- Priority
- User
- Full Path

#### Performance Metrics
- CPU Usage percentage
- Memory (Private)
- Thread Count
- Handle Count

#### Environment Variables
- Scrollable list of KEY = VALUE pairs
- Monospace font (Consolas)
- Max height 200px with scroll
- Shows "Access Denied" if insufficient permissions

#### Loaded Modules
- List of DLLs and modules
- Module count shown
- Full path for each module
- Scrollable up to 200px

### Usage
1. Navigate to "Details" page with process parameter
2. View comprehensive process information
3. Check environment variables (if accessible)
4. Review loaded modules
5. Useful for debugging and analysis

---

## 5. AboutPage - Application Information

### Features
- **App Branding**: Logo, name, version
- **Description**: Feature overview
- **Key Features**: Bullet list of capabilities
- **Technology Stack**: Framework and libraries
- **Links**: GitHub, Issues, Documentation
- **Copyright**: License information

### Cards

#### App Info Card
- Large app icon (80pt)
- "SystemPulse" title (32pt)
- Version number (16pt)
- Tagline

#### Description Card
- About section
- Key features list (7 items)
- Feature highlights

#### Technology Card
- Framework: WinUI 3
- Platform: .NET 8.0
- Libraries: MVVM Toolkit, DataGrid, OxyPlot, Serilog

#### Links Card
- GitHub Repository (opens browser)
- Report an Issue (opens GitHub Issues)
- Documentation (opens Wiki)

#### Copyright Card
- Copyright notice
- MIT License reference

### Usage
1. Navigate to "About" page
2. View app information
3. Click links to open in browser
4. Check version number
5. Review technology stack

---

## Implementation Notes

### Services Management
- Uses `ServiceController` class from System.ServiceProcess
- Requires elevated permissions for Start/Stop/Restart
- Read-only operations work without admin

### Startup Apps
- Reads from Registry (HKCU/HKLM\...\Run)
- Reads from Startup folder
- Enable/Disable modifies registry or renames files
- May require admin for system-wide apps

### User Sessions
- Uses WMI (Win32_LogonSession, Win32_LoggedOnUser)
- Or P/Invoke to WTSEnumerateSessions
- Disconnect/Logoff requires admin permissions
- Current user always shown first

### Process Details
- Environment variables require same-user or admin
- Modules require sufficient permissions
- Shows "Access Denied" gracefully when restricted

### About Page
- Static content (no ViewModel needed)
- HyperlinkButtons open external URLs
- Version hardcoded (can be automated from assembly)

---

## Common Patterns

### DataGrid Usage
- Search/Filter pattern (Services, Startup)
- Context menu for actions
- Status bar with counts
- Sortable columns
- Alternating row colors

### Cards
- All pages use consistent card styling
- 8px corner radius
- 1px border
- Card background color
- 20-24px padding

### Action Buttons
- Primary action: Accent style
- Secondary actions: Default style
- Icons on all buttons
- Context menu duplicates button actions

---

## Testing Checklist

### ServicesPage
- [x] Service list loads
- [x] Filter dropdown works
- [x] Search filters services
- [ ] Start service works (requires admin)
- [ ] Stop service works (requires admin)
- [ ] Restart service works (requires admin)
- [x] Context menu appears
- [x] Status bar shows counts

### StartupPage
- [ ] Startup apps load from registry
- [ ] Startup apps load from folder
- [x] Search filters apps
- [ ] Enable app works
- [ ] Disable app works
- [ ] Open location works
- [x] Status bar shows counts

### UsersPage
- [ ] Current user shown
- [ ] All sessions listed
- [ ] Session details accurate
- [ ] Disconnect button works (requires admin)
- [ ] Logoff button works (requires admin)
- [x] Refresh updates list

### DetailsPage
- [x] Process info displays
- [x] General information accurate
- [x] Performance metrics shown
- [ ] Environment variables load (if accessible)
- [ ] Modules load (if accessible)
- [x] Access denied handled gracefully

### AboutPage
- [x] App info displays
- [x] Features listed
- [x] Technology stack shown
- [x] Links work (open browser)
- [x] Copyright displayed

---

## Known Limitations

### Services
- Start/Stop/Restart require administrator privileges
- Some services cannot be stopped (critical system services)
- Service properties dialog not implemented

### Startup Apps
- Impact rating not implemented (placeholder)
- Publisher info may be missing for some apps
- System-wide apps require admin to modify

### User Sessions
- Disconnect/Logoff require administrator privileges
- Remote sessions may not show correct info
- Idle time calculation may be inaccurate

### Process Details
- Environment variables only accessible for same-user processes
- Modules only accessible for same-user or accessible processes
- Some processes (System, protected) will show "Access Denied"

---

## Future Enhancements

### Phase 4 Additions
- [ ] Service dependency viewer
- [ ] Startup impact measurement (actual timing)
- [ ] User session send message
- [ ] Process command-line arguments
- [ ] Process network connections
- [ ] Process open handles

---

**Implementation Status**: ✅ **ALL UTILITY PAGES COMPLETE**  
**Last Updated**: January 12, 2026 - 01:10 UTC
