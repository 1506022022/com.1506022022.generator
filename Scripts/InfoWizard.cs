using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class InfoWizard : ScriptableWizard
{
    int index = 0;
    string connection = ",\n";
    List<CharacterGenerateData> characterSheet = new();
    public List<JsonFormat> JsonFormats = new();
    public List<Generator> Generators = new();

    [MenuItem(itemName: "Tools/MonsterGenerator")]
    static void CreateWizard()
    {
        var wizard = GetWindow<InfoWizard>("InfoWizard");
    }
    private void OnWizardOtherButton() { }
    private void OnWizardCreate()
    {
        for (int i = 0; i < Generators.Count; i++)
        {
            var generator = Generators[i];
            if (generator is ICharacterGenerateData cgd) cgd.Initialize(characterSheet);
            Generators[i].Generate();
        }
    }
    private void OnWizardUpdate()
    {
        for (int i = 0; i < characterSheet.Count; i++)
        {
            characterSheet[i] ??= new();
        }
    }
    protected override bool DrawWizardGUI()
    {
        var result = base.DrawWizardGUI();

        DrawJsonPopup();
        DrawConnectionField();
        DrawCharacterSheetEditor();
        DrawJsonViewr();

        return result;
    }

    private void DrawJsonViewr()
    {
        if (!OutOfRange())
        {
            var json = "";
            for (int i = 0; i < characterSheet.Count; i++)
            {
                if (i > 0) json += connection;
                json += JsonFormatUtility.ReplaceFormat(characterSheet[i], JsonFormats[index]);
            }
            EditorGUILayout.TextArea(json);
        }
    }

    private void DrawCharacterSheetEditor()
    {
        if (GUILayout.Button("AddCharacter"))
        {
            characterSheet.Add(new());
        }
        if (GUILayout.Button("RemoveCharacter"))
        {
            characterSheet.Remove(characterSheet.Last());
        }

        for (int i = 0; i < characterSheet.Count; i++)
        {
            var characterData = characterSheet[i];
            EditorGUILayout.BeginHorizontal();
            characterData.Id = EditorGUILayout.TextField(string.IsNullOrEmpty(characterData.Id) ? nameof(characterData.Id) : characterData.Id);
            characterData.VideoId = EditorGUILayout.TextField(string.IsNullOrEmpty(characterData.VideoId) ? nameof(characterData.VideoId) : characterData.VideoId);
            characterData.ModelPath = EditorGUILayout.TextField(string.IsNullOrEmpty(characterData.ModelPath) ? nameof(characterData.ModelPath) : characterData.ModelPath);
            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawConnectionField()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Connection", GUILayout.Width(80));
        connection = EditorGUILayout.TextArea(connection);
        EditorGUILayout.EndHorizontal();
    }

    private void DrawJsonPopup()
    {
        index = EditorGUILayout.Popup(index, JsonFormats.Where(x => x != null).Select(x => x.name).ToArray());
        if (JsonFormats.Count == 0 || JsonFormats[index] == null)
        {
            index = 0;
        }
    }

    bool OutOfRange()
    {
        return index < 0 || index >= JsonFormats.Count || JsonFormats.Count == 0 || JsonFormats[index] == null;
    }

    #region Cache
    const string path = "Packages/com.1506022022.Generator/Resources/InfoWizardCache.asset";
    private void OnDestroy()
    {
        InfoWizardCache infoWizardCache;
        infoWizardCache = AssetDatabase.LoadAssetAtPath<InfoWizardCache>(path);
        if (infoWizardCache == null)
        {
            infoWizardCache = ScriptableObject.CreateInstance<InfoWizardCache>();
            AssetDatabase.CreateAsset(infoWizardCache, path);
        }
        infoWizardCache.JsonFormats = JsonFormats;
        infoWizardCache.characterSheet = characterSheet;
        infoWizardCache.index = index;
        infoWizardCache.connection = connection;
        infoWizardCache.Generators = Generators;
        EditorUtility.SetDirty(infoWizardCache);
        AssetDatabase.SaveAssets();
    }

    private void OnEnable()
    {
        var infoWizardCache = AssetDatabase.LoadAssetAtPath<InfoWizardCache>(path);
        if (infoWizardCache == null) return;

        if (infoWizardCache.Generators != null) Generators = infoWizardCache.Generators;
        if (infoWizardCache.characterSheet != null) characterSheet = infoWizardCache.characterSheet;
        if (infoWizardCache.JsonFormats != null) JsonFormats = infoWizardCache.JsonFormats;
        if (!string.IsNullOrEmpty(infoWizardCache.connection)) connection = infoWizardCache.connection;
        index = infoWizardCache.index;
    }
    #endregion
}
