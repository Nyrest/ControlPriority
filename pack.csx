#r "System.IO.Compression"
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

const string ModReleasePath = ".\\ModRelease";
const string ModDllName = "ControlPriority.dll";
const string ModTomlName = "ControlPriority.toml";

ModReleaseInfo[] outputs = new ModReleaseInfo[]
{
    new ("Control Priority"), // The Elder Scroll Skyrim SE/VR
};

byte[] dll64 = File.ReadAllBytes($".\\x64\\Release\\{ModDllName}");
byte[] toml = File.ReadAllBytes($".\\x64\\Release\\{ModTomlName}");

if (!Directory.Exists(ModReleasePath)) Directory.CreateDirectory(ModReleasePath);

Parallel.ForEach(outputs, releaseInfo =>
{
    using FileStream fs = new($"{ModReleasePath}\\{releaseInfo.modName}.zip", FileMode.Create, FileAccess.Write);
    using ZipArchive zipArchive = new ZipArchive(fs, ZipArchiveMode.Create, false, Encoding.UTF8);

    using (Stream dllStream = zipArchive.CreateEntry(GetEntryFullName("ControlPriority.dll"), CompressionLevel.Optimal).Open())
    {
        dllStream.Write(dll64, 0, dll64.Length);
    }

    using (Stream tomlStream = zipArchive.CreateEntry(GetEntryFullName("ControlPriority.toml"), CompressionLevel.Optimal).Open())
    {
        tomlStream.Write(toml, 0, toml.Length);
    }

    Console.WriteLine($"{releaseInfo.modName}.zip");

    string GetEntryFullName(string name)
    {
        return $"plugins\\{name}";
    }
});

record ModReleaseInfo(string modName);
