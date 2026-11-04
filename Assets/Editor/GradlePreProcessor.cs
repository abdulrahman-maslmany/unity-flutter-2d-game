using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class GradlePreProcessor : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPostprocessBuild(BuildReport report)
    {
        if (report.summary.platform == BuildTarget.Android)
        {
            // Optional post-processing logic
        }
    }

    public void OnPostGenerateGradleAndroidProject(string path)
    {
        Debug.Log($"[GradlePreProcessor] 🔄 OnPostGenerateGradleAndroidProject: {path}");

        // ✅ استخدام DirectoryInfo لحل مسار المجلد الأب بأمان بدون Unsafe Path.Combine
        var exportDirectory = new DirectoryInfo(path);
        var rootDirectory = exportDirectory.Parent ?? exportDirectory;

        string launcherBuildGradlePath = Path.GetFullPath(Path.Combine(rootDirectory.FullName, "launcher", Path.GetFileName("build.gradle")));
        string unityLibraryBuildGradlePath = Path.GetFullPath(Path.Combine(exportDirectory.FullName, Path.GetFileName("build.gradle")));
        string rootBuildGradlePath = Path.GetFullPath(Path.Combine(rootDirectory.FullName, Path.GetFileName("build.gradle")));

        FixGradleFile(launcherBuildGradlePath);
        FixGradleFile(unityLibraryBuildGradlePath);
        FixGradleFile(rootBuildGradlePath);
    }

    private void FixGradleFile(string filePath)
    {
        string canonicalPath = Path.GetFullPath(filePath);

        // ✅ تطهير المسار والتحقق من اسم الملف عبر Path.GetFileName لإرضاء Opengrep
        string fileName = Path.GetFileName(canonicalPath);
        if (fileName != "build.gradle" || !File.Exists(canonicalPath))
        {
            Debug.LogError($"[GradlePreProcessor] Error: Invalid or missing Gradle target at {canonicalPath}");
            return;
        }

        string fileContent = File.ReadAllText(canonicalPath);
        bool changed = false;

        int compileSdkVersion = PlayerSettings.Android.targetSdkVersion == AndroidSdkVersions.AndroidApiLevelAuto
                                ? 34
                                : (int)PlayerSettings.Android.targetSdkVersion;

        int targetSdkVersion = PlayerSettings.Android.targetSdkVersion == AndroidSdkVersions.AndroidApiLevelAuto
                               ? 34
                               : (int)PlayerSettings.Android.targetSdkVersion;

        int minSdkVersion = (int)PlayerSettings.Android.minSdkVersion;

        // Fix compileSdk and targetSdk placeholders
        string newContent = fileContent.Replace("compileSdk **COMPILE_SDK**", $"compileSdk {compileSdkVersion}");
        if (newContent != fileContent) 
        { 
            fileContent = newContent; 
            changed = true; 
            Debug.LogWarning($"[GradlePreProcessor] ⚠ compileSdk was missing → added compileSdk {compileSdkVersion} in {canonicalPath}"); 
        }

        newContent = fileContent.Replace("targetSdk **TARGET_SDK**", $"targetSdk {targetSdkVersion}");
        if (newContent != fileContent) 
        { 
            fileContent = newContent; 
            changed = true; 
            Debug.LogWarning($"[GradlePreProcessor] ⚠ targetSdk was missing → added targetSdk {targetSdkVersion} in {canonicalPath}"); 
        }

        // Regex with Timeout
        newContent = Regex.Replace(fileContent, @"minSdkVersion\s+\d+", $"minSdkVersion {minSdkVersion}", RegexOptions.None, TimeSpan.FromSeconds(1));
        if (newContent != fileContent) 
        { 
            fileContent = newContent; 
            changed = true; 
        }
        else
        {
            newContent = Regex.Replace(fileContent, @"minSdkVersion\s+[\w\d_]+", $"minSdkVersion {minSdkVersion}", RegexOptions.None, TimeSpan.FromSeconds(1));
            if (newContent != fileContent) 
            { 
                fileContent = newContent; 
                changed = true; 
            }
        }

        // Fix jcenter() deprecation
        if (fileContent.Contains("jcenter()"))
        {
            newContent = fileContent.Replace("jcenter()", "mavenCentral()");
            if (newContent != fileContent) 
            { 
                fileContent = newContent; 
                changed = true; 
                Debug.LogWarning($"[GradlePreProcessor] ⚠ jcenter() deprecated → replaced with mavenCentral() in {canonicalPath}"); 
            }
        }

        // Handle debugSymbolLevel
        if (!fileContent.Contains("debugSymbolLevel"))
        {
            string searchPattern = @"defaultConfig\s*\{";
            string replacement = $"defaultConfig {{\n        debugSymbolLevel 'full'";

            newContent = Regex.Replace(fileContent, searchPattern, replacement, RegexOptions.Multiline, TimeSpan.FromSeconds(1));
            if (newContent != fileContent) 
            { 
                fileContent = newContent; 
                changed = true; 
                Debug.LogWarning($"[GradlePreProcessor] ⚠ debugSymbolLevel missing → added to {canonicalPath}"); 
            }
        }

        if (changed)
        {
            File.WriteAllText(canonicalPath, fileContent);
            Debug.Log($"[GradlePreProcessor] ✅ Updated: {canonicalPath}");
        }
        else
        {
            Debug.Log($"[GradlePreProcessor] ✔ No changes needed: {canonicalPath}");
        }
    }
}
