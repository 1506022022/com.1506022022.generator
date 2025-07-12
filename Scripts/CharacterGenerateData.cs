using System.Collections.Generic;

public interface ICharacterGenerateData
{
    void Initialize(List<CharacterGenerateData> CharacterGenerateDatas);
}

[System.Serializable]
public class CharacterGenerateData
{
    public string Id;
    public string VideoId;
    public string ModelPath;
}