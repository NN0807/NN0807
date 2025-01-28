#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

/// <summary>
/// オーディオファイルのパスをスクリプトで保存する
/// </summary>
public class AudioPathCreate : AssetPostprocessor
{
    // BGMアセットの格納場所
    const string BGM_ASSET_PATH = "Assets/Resources/BGM";
    // BGMスクリプトのパス
    const string BGM_SCRIPT_PATH = "Assets/Scripts/Audio/BGMPath.cs";

    // SEアセットの格納場所
    const string SE_ASSET_PATH = "Assets/Resources/SE";
    // SEスクリプトのパス
    const string SE_SCRIPT_PATH = "Assets/Scripts/Audio/SEPath.cs";

    //[MenuItem("Tools/Update AudioPath")]
    //private static void　UpdateAudioPath()
    //{
    //    // スクリプト更新
    //    CreateScript(BGM_SCRIPT_PATH);
    //    CreateScript(SE_SCRIPT_PATH);
    //}

    /// <summary>
    /// アセットが追加・削除・移動した時に呼ばれる
    /// </summary>
    /// <param name="importedAssets">追加されたアセットパス</param>
    /// <param name="deletedAssets">削除されたアセットパス</param>
    /// <param name="movedAssets">移動したアセットパス</param>
    /// <param name="movedFromAssetPaths">移動前のアセットパス</param>
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        // インポートされたアセットを確認
        foreach (var asset in importedAssets)
        {
            // ディレクトリパス取得
            string directryPath = Path.GetFullPath(Path.GetDirectoryName(asset)).TrimEnd(Path.DirectorySeparatorChar);
            string BGMPath = Path.GetFullPath(BGM_ASSET_PATH).TrimEnd(Path.DirectorySeparatorChar);
            string SEPath = Path.GetFullPath(SE_ASSET_PATH).TrimEnd(Path.DirectorySeparatorChar);

            // 管理対象のフォルダのアセットか確認
            if (string.Equals(directryPath, BGMPath, System.StringComparison.OrdinalIgnoreCase))
            {
                // スクリプト更新
                CreateScript(BGM_SCRIPT_PATH);
            }
            if (string.Equals(directryPath, SEPath, System.StringComparison.OrdinalIgnoreCase))
            {
                // スクリプト更新
                CreateScript(SE_SCRIPT_PATH);
            }
        }

        // 削除されたアセットを確認
        foreach (var asset in deletedAssets)
        {
            // ディレクトリパス取得
            string directryPath = Path.GetFullPath(Path.GetDirectoryName(asset)).TrimEnd(Path.DirectorySeparatorChar);
            string targetpath = Path.GetFullPath(BGM_ASSET_PATH).TrimEnd(Path.DirectorySeparatorChar);
            string SEPath = Path.GetFullPath(SE_ASSET_PATH).TrimEnd(Path.DirectorySeparatorChar);

            // 管理対象のフォルダのアセットか確認
            if (string.Equals(directryPath, targetpath, System.StringComparison.OrdinalIgnoreCase))
            {
                // スクリプト更新
                CreateScript(BGM_SCRIPT_PATH);
            }
            if (string.Equals(directryPath, SEPath, System.StringComparison.OrdinalIgnoreCase))
            {
                // スクリプト更新
                CreateScript(SE_SCRIPT_PATH);
            }
        }
    }

    /// <summary>
    /// スクリプト作成・更新
    /// </summary>
    /// <param name="scriptPath">対象のスクリプトパス</param>
    private static void CreateScript(string scriptPath)
    {
        // スクリプトに書く内容(固定)
        string newScriptContent =
            "using UnityEngine;\n\n" +
            "public static class " + Path.GetFileNameWithoutExtension(scriptPath) + "\n" +
            "{\n";

        // 既存のスクリプト内容
        Dictionary<string, string> currentVariables = new Dictionary<string, string>();

        // すでにスクリプトがあるか
        if (File.Exists(scriptPath))
        {
            // スクリプトに記載されているコードを取得
            string currentScriptContent = File.ReadAllText(scriptPath);

            // 既存の変数名とパスを取得して辞書に保存
            string pattern = @"public const string (\w+) = ""(.+?)"";";
            MatchCollection matches = Regex.Matches(currentScriptContent, pattern);

            foreach (Match match in matches)
            {
                string variableName = match.Groups[1].Value;
                string variablePath = match.Groups[2].Value;
                currentVariables[variableName] = variablePath;
            }
        }

        string assetFilePath = default;
        if (scriptPath == BGM_SCRIPT_PATH) assetFilePath = BGM_ASSET_PATH;
        else if (scriptPath == SE_SCRIPT_PATH) assetFilePath = SE_ASSET_PATH;

        // フォルダ内のアセット取得
        string[] assetPaths = AssetDatabase.FindAssets("", new[] { assetFilePath });
        List<string> newAssets = new List<string>();

        foreach (string guid in assetPaths)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            string assetName = Path.GetFileNameWithoutExtension(assetPath);

            // 新しいアセットならリストに追加
            if (!currentVariables.ContainsKey(assetName))
            {
                newAssets.Add(assetPath);
            }

            // 既存のアセット内容も保持
            if (currentVariables.ContainsValue(assetPath))
            {
                newScriptContent += $"    public const string {assetName} = \"{assetPath}\";\n";
                currentVariables.Remove(assetName);
            }
        }

        // 新規アセットを追記
        foreach (string newAsset in newAssets)
        {
            string assetName = Path.GetFileNameWithoutExtension(newAsset);
            newScriptContent += $"    public const string {assetName} = \"{newAsset}\";\n";
        }

        newScriptContent += "}";

        // スクリプトファイルを作成して内容を書き込む
        File.WriteAllText(scriptPath, newScriptContent);

        // リフレッシュ
        UnityEditor.AssetDatabase.Refresh();
    }
}

#endif