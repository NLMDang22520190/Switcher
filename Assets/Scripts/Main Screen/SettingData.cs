using System;

[Serializable]
public class SettingData
{
    public int inputDelay;
    public float musicVolume;
    public float sfxVolume;
    public int graphicsQuality;

    public SettingData(float musicVolume, float sfxVolume, int graphicsQuality, int inputDelay)
    {
        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;
        this.graphicsQuality = graphicsQuality;
        this.inputDelay = inputDelay;
    }
}
