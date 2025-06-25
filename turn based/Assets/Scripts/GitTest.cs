using UnityEditor;
using UnityEngine;
using System.Diagnostics;

public static class GitTest
{
    [MenuItem("Tools/Test Git")]
    public static void RunGit()
    {
        var process = new Process();
        process.StartInfo.FileName = "git";
        process.StartInfo.Arguments = "--version";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

       UnityEngine. Debug.Log("Git output: " + output);
    }
}
