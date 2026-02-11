using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Diagnostics;
using System.Security.Principal;
using Microsoft.Win32;
using System.Collections.Generic;

namespace ContextMenuInstaller
{
    /// <summary>
    /// Helper tool that reads JSON config from the web app and applies it to Windows Registry
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔═══════════════════════════════════════════════════════╗");
            Console.WriteLine("║   Windows 11 Context Menu Customizer - Installer     ║");
            Console.WriteLine("║   Generated from Context Menu Studio Web App          ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();

            // Check for admin rights
            if (!IsAdministrator())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ ERROR: This program requires Administrator privileges!");
                Console.WriteLine("\nPlease:");
                Console.WriteLine("1. Right-click this program");
                Console.WriteLine("2. Select 'Run as administrator'");
                Console.WriteLine("3. Try again");
                Console.ResetColor();
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✓ Running with Administrator privileges");
            Console.ResetColor();
            Console.WriteLine();

            // Main menu
            while (true)
            {
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("1. Install context menu from JSON file");
                Console.WriteLine("2. Remove all custom context menu items");
                Console.WriteLine("3. View installed custom items");
                Console.WriteLine("4. Exit");
                Console.Write("\nEnter your choice (1-4): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        InstallFromJson();
                        break;
                    case "2":
                        RemoveAllCustomItems();
                        break;
                    case "3":
                        ViewInstalledItems();
                        break;
                    case "4":
                        Console.WriteLine("\nGoodbye!");
                        return;
                    default:
                        Console.WriteLine("\n❌ Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void InstallFromJson()
        {
            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("INSTALL FROM JSON");
            Console.WriteLine(new string('═', 60));

            // Get file path
            Console.WriteLine("\nEnter the full path to your context-menu-config.json file:");
            Console.WriteLine("(You can drag and drop the file here)");
            Console.Write("> ");
            string configPath = Console.ReadLine()?.Trim('"') ?? "";

            if (!File.Exists(configPath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ File not found: {configPath}");
                Console.ResetColor();
                return;
            }

            try
            {
                // Read and parse JSON
                Console.WriteLine("\n📄 Reading configuration...");
                string jsonContent = File.ReadAllText(configPath);
                var config = JsonSerializer.Deserialize<MenuConfiguration>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (config?.Configuration?.Items == null || config.Configuration.Items.Length == 0)
                {
                    Console.WriteLine("❌ No menu items found in configuration file.");
                    return;
                }

                Console.WriteLine($"\n✓ Found {config.Configuration.Items.Length} menu items");
                Console.WriteLine($"✓ Generated: {config.Timestamp}");
                Console.WriteLine($"✓ Version: {config.Version}");

                // Show what will be installed
                Console.WriteLine("\n📋 Menu items to be installed:");
                for (int i = 0; i < config.Configuration.Items.Length; i++)
                {
                    var item = config.Configuration.Items[i];
                    if (item.Type != "separator")
                    {
                        Console.WriteLine($"   {i + 1}. {item.Label}");
                        if (!string.IsNullOrEmpty(item.Command))
                        {
                            Console.WriteLine($"      Command: {item.Command}");
                        }
                        if (item.Submenu != null && item.Submenu.Length > 0)
                        {
                            Console.WriteLine($"      Submenu: {item.Submenu.Length} items");
                        }
                    }
                }

                // Confirm installation
                Console.WriteLine("\n⚠️  This will modify your Windows Registry.");
                Console.Write("Continue with installation? (Y/N): ");
                string confirm = Console.ReadLine()?.ToUpper() ?? "";

                if (confirm != "Y")
                {
                    Console.WriteLine("Installation cancelled.");
                    return;
                }

                // Create restore point suggestion
                Console.WriteLine("\n💡 TIP: It's recommended to create a System Restore Point first.");
                Console.Write("Have you created a restore point? (Y/N): ");
                string restorePoint = Console.ReadLine()?.ToUpper() ?? "";

                if (restorePoint != "Y")
                {
                    Console.WriteLine("\nTo create a restore point:");
                    Console.WriteLine("1. Search 'Create a restore point' in Windows");
                    Console.WriteLine("2. Click 'Create' and give it a name");
                    Console.WriteLine("3. Come back and run this installer again");
                    Console.Write("\nPress any key to continue anyway, or Ctrl+C to exit...");
                    Console.ReadKey();
                }

                // Install items
                Console.WriteLine("\n⚙️  Installing context menu items...");
                int successCount = 0;
                int failCount = 0;

                foreach (var item in config.Configuration.Items)
                {
                    if (item.Type == "separator")
                    {
                        continue;
                    }

                    try
                    {
                        InstallMenuItem(item);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"  ✓ Installed: {item.Label}");
                        Console.ResetColor();
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"  ❌ Failed: {item.Label} - {ex.Message}");
                        Console.ResetColor();
                        failCount++;
                    }
                }

                Console.WriteLine($"\n📊 Installation Summary:");
                Console.WriteLine($"   ✓ Successful: {successCount}");
                if (failCount > 0)
                {
                    Console.WriteLine($"   ❌ Failed: {failCount}");
                }

                // Restart Explorer
                Console.WriteLine("\n🔄 Restarting Windows Explorer to apply changes...");
                RestartExplorer();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✓ Installation complete!");
                Console.WriteLine("✓ Right-click on any file to see your custom context menu!");
                Console.ResetColor();
            }
            catch (JsonException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ Error parsing JSON file: {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ Unexpected error: {ex.Message}");
                Console.ResetColor();
            }
        }

        static void InstallMenuItem(MenuItem item)
        {
            // Clean label for registry key name
            string sanitizedLabel = new string(item.Label.Where(c => char.IsLetterOrDigit(c)).ToArray());
            if (string.IsNullOrEmpty(sanitizedLabel))
            {
                sanitizedLabel = "CustomItem";
            }

            string keyName = $"CustomMenu_{sanitizedLabel}_{item.Id}";

            // Install for all files context
            string[] contextPaths = new[]
            {
                $@"*\shell\{keyName}",  // All files
            };

            foreach (string contextPath in contextPaths)
            {
                // Create main menu item key
                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(contextPath))
                {
                    if (key == null)
                    {
                        throw new Exception($"Failed to create registry key: {contextPath}");
                    }

                    key.SetValue("MUIVerb", item.Label);

                    // Set icon if provided
                    if (!string.IsNullOrEmpty(item.Icon))
                    {
                        key.SetValue("Icon", item.Icon);
                    }
                }

                // Add command if provided
                if (!string.IsNullOrEmpty(item.Command) && (item.Submenu == null || item.Submenu.Length == 0))
                {
                    using (RegistryKey cmdKey = Registry.ClassesRoot.CreateSubKey($@"{contextPath}\command"))
                    {
                        if (cmdKey != null)
                        {
                            string command = ConvertCommand(item.Command);
                            cmdKey.SetValue("", command);
                        }
                    }
                }

                // Add submenu items if provided
                if (item.Submenu != null && item.Submenu.Length > 0)
                {
                    using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(contextPath, true))
                    {
                        if (key != null)
                        {
                            key.SetValue("SubCommands", "");
                        }
                    }

                    foreach (var subItem in item.Submenu)
                    {
                        if (subItem.Type != "separator")
                        {
                            InstallSubMenuItem(contextPath, subItem);
                        }
                    }
                }
            }
        }

        static void InstallSubMenuItem(string parentPath, MenuItem subItem)
        {
            string sanitizedLabel = new string(subItem.Label.Where(c => char.IsLetterOrDigit(c)).ToArray());
            string subKeyPath = $@"{parentPath}\shell\{sanitizedLabel}";

            using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(subKeyPath))
            {
                if (key != null)
                {
                    key.SetValue("", subItem.Label);

                    if (!string.IsNullOrEmpty(subItem.Icon))
                    {
                        key.SetValue("Icon", subItem.Icon);
                    }
                }
            }

            if (!string.IsNullOrEmpty(subItem.Command))
            {
                using (RegistryKey cmdKey = Registry.ClassesRoot.CreateSubKey($@"{subKeyPath}\command"))
                {
                    if (cmdKey != null)
                    {
                        string command = ConvertCommand(subItem.Command);
                        cmdKey.SetValue("", command);
                    }
                }
            }
        }

        static string ConvertCommand(string command)
        {
            // Convert common commands to Windows commands
            var commandMap = new Dictionary<string, string>
            {
                ["open"] = "explorer.exe \"%1\"",
                ["notepad"] = "notepad.exe \"%1\"",
                ["vscode"] = "code \"%1\"",
                ["powershell"] = "powershell.exe -NoExit -Command \"Set-Location -Path (Split-Path -Parent '%1')\"",
                ["cmd"] = "cmd.exe /k cd /d \"%V\"",
            };

            if (commandMap.ContainsKey(command.ToLower()))
            {
                return commandMap[command.ToLower()];
            }

            // If not a known command, return as-is (assumed to be a full command)
            return command;
        }

        static void RemoveAllCustomItems()
        {
            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("REMOVE ALL CUSTOM ITEMS");
            Console.WriteLine(new string('═', 60));

            Console.WriteLine("\n⚠️  This will remove ALL custom context menu items.");
            Console.Write("Are you sure? (Y/N): ");
            string confirm = Console.ReadLine()?.ToUpper() ?? "";

            if (confirm != "Y")
            {
                Console.WriteLine("Operation cancelled.");
                return;
            }

            try
            {
                int removedCount = 0;
                string[] contextPaths = new[]
                {
                    @"*\shell",
                    @"Directory\shell",
                    @"DesktopBackground\shell",
                };

                foreach (string contextPath in contextPaths)
                {
                    using (RegistryKey shellKey = Registry.ClassesRoot.OpenSubKey(contextPath, true))
                    {
                        if (shellKey != null)
                        {
                            string[] subKeyNames = shellKey.GetSubKeyNames();
                            foreach (string subKeyName in subKeyNames)
                            {
                                if (subKeyName.StartsWith("CustomMenu_"))
                                {
                                    shellKey.DeleteSubKeyTree(subKeyName);
                                    Console.WriteLine($"  ✓ Removed: {subKeyName}");
                                    removedCount++;
                                }
                            }
                        }
                    }
                }

                Console.WriteLine($"\n✓ Removed {removedCount} custom menu items");

                if (removedCount > 0)
                {
                    Console.WriteLine("\n🔄 Restarting Windows Explorer...");
                    RestartExplorer();
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✓ Cleanup complete!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ Error: {ex.Message}");
                Console.ResetColor();
            }
        }

        static void ViewInstalledItems()
        {
            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("INSTALLED CUSTOM ITEMS");
            Console.WriteLine(new string('═', 60));

            try
            {
                string[] contextPaths = new[]
                {
                    @"*\shell",
                    @"Directory\shell",
                    @"DesktopBackground\shell",
                };

                bool foundAny = false;

                foreach (string contextPath in contextPaths)
                {
                    using (RegistryKey shellKey = Registry.ClassesRoot.OpenSubKey(contextPath))
                    {
                        if (shellKey != null)
                        {
                            string[] subKeyNames = shellKey.GetSubKeyNames();
                            var customKeys = subKeyNames.Where(k => k.StartsWith("CustomMenu_")).ToArray();

                            if (customKeys.Length > 0)
                            {
                                Console.WriteLine($"\n📁 {contextPath}:");
                                foreach (string keyName in customKeys)
                                {
                                    using (RegistryKey itemKey = shellKey.OpenSubKey(keyName))
                                    {
                                        if (itemKey != null)
                                        {
                                            string label = itemKey.GetValue("MUIVerb") as string ?? keyName;
                                            Console.WriteLine($"   • {label}");
                                            foundAny = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (!foundAny)
                {
                    Console.WriteLine("\nNo custom context menu items found.");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ Error: {ex.Message}");
                Console.ResetColor();
            }
        }

        static void RestartExplorer()
        {
            try
            {
                // Kill all explorer processes
                Process[] explorerProcesses = Process.GetProcessesByName("explorer");
                foreach (Process explorer in explorerProcesses)
                {
                    explorer.Kill();
                }

                // Wait for processes to terminate
                System.Threading.Thread.Sleep(1000);

                // Start explorer again
                Process.Start("explorer.exe");

                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("✓ Windows Explorer restarted successfully");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"⚠️  Could not restart Explorer automatically: {ex.Message}");
                Console.WriteLine("Please restart Explorer manually or reboot your computer.");
                Console.ResetColor();
            }
        }

        static bool IsAdministrator()
        {
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }
    }

    #region Data Models matching the JSON from web app

    public class MenuConfiguration
    {
        public string Version { get; set; }
        public string GeneratedBy { get; set; }
        public string Timestamp { get; set; }
        public ConfigurationData Configuration { get; set; }
    }

    public class ConfigurationData
    {
        public MenuItem[] Items { get; set; }
        public MenuStyle Style { get; set; }
    }

    public class MenuItem
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public string Command { get; set; }
        public string Icon { get; set; }
        public string Shortcut { get; set; }
        public MenuItem[] Submenu { get; set; }
    }

    public class MenuStyle
    {
        public string BackgroundColor { get; set; }
        public string TextColor { get; set; }
        public string HoverBackgroundColor { get; set; }
        public int BorderRadius { get; set; }
        public int FontSize { get; set; }
    }

    #endregion
}
