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

        // ✅ استخدام Path.GetFullPath لتفادي تحذيرات أمان Path Traversal
        string launcherBuildGradlePath = Path.GetFullPath(Path.Combine(path, "..", "launcher", "build.gradle"));
        string unityLibraryBuildGradlePath = Path.GetFullPath(Path.Combine(path, "build.gradle"));
        string rootBuildGradlePath = Path.GetFullPath(Path.Combine(path, "..", "build.gradle"));

        FixGradleFile(launcherBuildGradlePath);
        FixGradleFile(unityLibraryBuildGradlePath);
        FixGradleFile(rootBuildGradlePath);
    }

    private void FixGradleFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"[GradlePreProcessor] Error: Gradle file not found at {filePath}");
            return;
        }

        string fileContent = File.ReadAllText(filePath);
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
            Debug.LogWarning($"[GradlePreProcessor] ⚠ compileSdk was missing → added compileSdk {compileSdkVersion} in {filePath}"); 
        }

        newContent = fileContent.Replace("targetSdk **TARGET_SDK**", $"targetSdk {targetSdkVersion}");
        if (newContent != fileContent) 
        { 
            fileContent = newContent; 
            changed = true; 
            Debug.LogWarning($"[GradlePreProcessor] ⚠ targetSdk was missing → added targetSdk {targetSdkVersion} in {filePath}"); 
        }

        // ✅ إصلاح الأمان الأول: إضافة Timeout لمنع ReDoS
        newContent = Regex.Replace(fileContent, @"minSdkVersion\s+\d+", $"minSdkVersion {minSdkVersion}", RegexOptions.None, TimeSpan.FromSeconds(1));
        if (newContent != fileContent) 
        { 
            fileContent = newContent; 
            changed = true; 
        }
        else
        {
            // ✅ إصلاح الأمان الثاني: إضافة Timeout لمنع ReDoS
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
                Debug.LogWarning($"[GradlePreProcessor] ⚠ jcenter() deprecated → replaced with mavenCentral() in {filePath}"); 
            }
        }

        // Handle debugSymbolLevel
        if (!fileContent.Contains("debugSymbolLevel"))
        {
            string searchPattern = @"defaultConfig\s*\{";
            string replacement = $"defaultConfig {{\n        debugSymbolLevel 'full'";

            // ✅ إصلاح الأمان الثالث: إضافة Timeout لمنع ReDoS
            newContent = Regex.Replace(fileContent, searchPattern, replacement, RegexOptions.Multiline, TimeSpan.FromSeconds(1));
            if (newContent != fileContent) 
            { 
                fileContent = newContent; 
                changed = true; 
                Debug.LogWarning($"[GradlePreProcessor] ⚠ debugSymbolLevel missing → added to {filePath}"); 
            }
        }

        if (changed)
        {
            File.WriteAllText(filePath, fileContent);
            Debug.Log($"[GradlePreProcessor] ✅ Updated: {filePath}");
        }
        else
        {
            Debug.Log($"[GradlePreProcessor] ✔ No changes needed: {filePath}");
        }
    }
}
