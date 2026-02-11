# Context Menu Installer

A Windows desktop helper tool for [Context Menu Studio](https://your-website-url-here.com) - automatically applies custom context menu configurations to Windows 11.

## 📥 Download

**[Download Latest Release](https://github.com/YOUR-USERNAME/context-menu-installer/releases/latest)**

Get the latest `ContextMenuInstaller.exe` from the Releases page.

## ✨ Features

- ✅ Install custom context menus from JSON files
- ✅ Remove all custom menu items
- ✅ View installed items
- ✅ Automatic Windows Explorer restart
- ✅ Safe registry modifications
- ✅ No installation required

## 🚀 How to Use

1. **Design your menu** in [Context Menu Studio](https://your-website-url-here.com)
2. **Export** your configuration as JSON
3. **Download** this installer tool
4. **Run as administrator** (right-click → Run as administrator)
5. **Choose option 1** to install
6. **Point it** to your JSON file
7. **Done!** Right-click any file to see your menu

## 📋 Requirements

- Windows 10 (version 1809 or later) or Windows 11
- Administrator privileges
- No additional software needed

## 🛡️ Safety

- All changes are reversible (use option 2 to remove)
- Recommended: Create a System Restore Point before installing
- Open source - view the code yourself
- No data collection or internet connection required

## 🐛 Troubleshooting

### Menu items don't appear
- Restart Windows Explorer (the tool does this automatically)
- Or restart your computer
- Verify with option 3 (View installed items)

### "Access Denied" error
- Must run as Administrator
- Right-click the .exe → "Run as administrator"

### Windows Defender warning
- This is normal for registry tools
- The program is safe and open source
- Scan report: [Add your VirusTotal link]

## 🔧 Building from Source

### Using Visual Studio
1. Clone this repository
2. Open `ContextMenuInstaller.csproj` in Visual Studio 2022
3. Build → Publish
4. Choose "Folder" and configure for single-file

### Using .NET CLI
```bash
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

### Using Build Script
```bash
# Windows PowerShell
.\Build-SingleFile.ps1

# Or batch file
Build-SingleFile.bat
```

## 📝 License

[Choose your license - MIT recommended]

## 🤝 Contributing

Issues and pull requests are welcome!

## 🔗 Links

- **Web App:** [Context Menu Studio](https://your-website-url-here.com)
- **Documentation:** [User Guide](https://your-website-url-here.com/docs)
- **Support:** [GitHub Issues](https://github.com/YOUR-USERNAME/context-menu-installer/issues)

## 📊 Version History

### v1.0.0 (2026-02-11)
- Initial release
- Install context menus from JSON
- Remove custom items
- View installed items
- Auto-restart Explorer

---

**Made with ❤️ for Context Menu Studio users**
