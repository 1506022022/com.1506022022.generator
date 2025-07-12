using System.Collections.Generic;
using UnityEngine;

public class InfoWizardCache : ScriptableObject
{
    public List<JsonFormat> JsonFormats;
    public List<Generator> Generators;
    public List<CharacterGenerateData> characterSheet;
    public int index;
    public string connection;
}