using System.Collections.Generic;

[System.Serializable]
public class NoteData
{
    public string noteRestriction;
    public List<double> timeStamps;
}

[System.Serializable]
public class SongData
{
    public List<NoteData> notes;
}
