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

namespace Starter_Package {
    public class CarManager : MonoBehaviour {
        [SerializeField] GameObject _carPrefab;
        [SerializeField] ReticleBehaviour _reticle;
        [SerializeField] DrivingSurfaceManager _drivingSurfaceManager;

        CarBehaviour _car;

        void Update() {
            if (_car is not null || !WasTapped() || _reticle.CurrentPlane is null) return;

            var obj = Instantiate(_carPrefab);
            _car = obj.GetComponent<CarBehaviour>();
            _car.Reticle = _reticle;
            _car.transform.position = _reticle.transform.position;
            _drivingSurfaceManager.LockPlane(_reticle.CurrentPlane);
        }

        bool WasTapped() {
            if (Input.GetMouseButtonDown(0))
                return true;

            if (Input.touchCount == 0)
                return false;

            var touch = Input.GetTouch(0);

            return touch.phase is TouchPhase.Began;
        }
    }
}