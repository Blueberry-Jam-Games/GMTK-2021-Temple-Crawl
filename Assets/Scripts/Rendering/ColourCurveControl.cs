using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColourCurveControl : MonoBehaviour
{
    Volume camVolume;
    // Start is called before the first frame update
    void Start()
    {
        camVolume = GetComponent<Volume>();
        VolumeProfile profile = camVolume.sharedProfile;
        if(profile.TryGet<ColorCurves>(out ColorCurves colourcontrol))
        {
            Debug.Log("colour control found");
            colourcontrol.hueVsSat = new TextureCurveParameter(new TextureCurve(new[] { new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f) }, 0f, false, new Vector2(0f, 1f)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
