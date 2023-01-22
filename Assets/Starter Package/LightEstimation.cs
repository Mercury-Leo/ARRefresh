/*
 * Copyright 2021 Google LLC
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(Light))]
public class LightEstimation : MonoBehaviour {
    [SerializeField] ARCameraManager _arCameraManager;
    [SerializeField] Light _light;


    void Awake() {
        TryGetComponent(out _light);
    }

    void OnEnable() {
        _arCameraManager.frameReceived += FrameReceived;
    }

    void OnDisable() {
        _arCameraManager.frameReceived -= FrameReceived;
    }

    void FrameReceived(ARCameraFrameEventArgs args) {
        var lightEstimation = args.lightEstimation;

        if (lightEstimation.averageBrightness.HasValue)
            _light.intensity = lightEstimation.averageBrightness.Value;

        if (lightEstimation.averageColorTemperature.HasValue)
            _light.colorTemperature = lightEstimation.averageColorTemperature.Value;

        if (lightEstimation.colorCorrection.HasValue)
            _light.color = lightEstimation.colorCorrection.Value;

        if (lightEstimation.mainLightDirection.HasValue)
            _light.transform.rotation = Quaternion.LookRotation(lightEstimation.mainLightDirection.Value);

        if (lightEstimation.mainLightColor.HasValue)
            _light.color = lightEstimation.mainLightColor.Value;

        if (lightEstimation.mainLightIntensityLumens.HasValue)
            _light.intensity = lightEstimation.averageMainLightBrightness.Value;

        if (lightEstimation.ambientSphericalHarmonics.HasValue) {
            RenderSettings.ambientMode = AmbientMode.Skybox;
            RenderSettings.ambientProbe = lightEstimation.ambientSphericalHarmonics.Value;
        }
    }
}