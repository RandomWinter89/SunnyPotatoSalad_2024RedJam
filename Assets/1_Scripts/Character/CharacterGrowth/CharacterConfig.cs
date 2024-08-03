using AYellowpaper.SerializedCollections;

public class CharacterConfig
{
    public SerializedDictionary<Directions, AnimationClipData> animationClipDatas = new();
    public SerializedDictionary<string, AnimationClipData> specialAnimationClipDatas = new();
    public float speedMultiplier = 1f;
    public float manueverability = 1f;
    public float scale = 1f;
    public float cameraZoom = 10f;

    public CharacterConfig(GrowthStageData growthStageData)
    {
        this.animationClipDatas = growthStageData.animationClipDatas;
        this.specialAnimationClipDatas = growthStageData.specialAnimationClipDatas;
        this.speedMultiplier = growthStageData.speedMultiplier;
        this.manueverability = growthStageData.manueverability;
        this.scale = growthStageData.scale;
        this.cameraZoom = growthStageData.cameraZoom;
    }
}