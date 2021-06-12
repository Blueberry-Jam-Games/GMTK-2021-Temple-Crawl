using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColourCurveControl : MonoBehaviour
{
    private Volume camVolume;
    private ColorCurves colourControl;

    private TextureCurve localCurve;

    private Keyframe[] keyframes = null;

    [SerializeField] Pair[] keyframeInput;

    // Start is called before the first frame update
    void Start()
    {
        keyframes = new Keyframe[keyframeInput.Length];
        for(int i = 0, count = keyframeInput.Length; i < count; i++)
        {
            keyframes[i] = new Keyframe(keyframeInput[i].time, keyframeInput[i].value);
        }
        camVolume = GetComponent<Volume>();
        VolumeProfile profile = camVolume.sharedProfile;
        if(profile.TryGet<ColorCurves>(out ColorCurves cols))
        {
            Debug.Log("colour control found");
            colourControl = cols;
            colourControl.hueVsSat.overrideState = true;
        }

        localCurve = new TextureCurve(new AnimationCurve(keyframes), 0f, true, new Vector2(0, 1));
        colourControl.hueVsSat.Override(localCurve);
    }

    public void UpdateKeyframe(int index, float newValue)
    {
        keyframes[index].value = newValue;
        localCurve.MoveKey(index, keyframes[index]);
        colourControl.hueVsSat.Override(localCurve);
    }

    [System.Serializable]
    private struct Pair
    {
        public float time;
        public float value;
    }
}
