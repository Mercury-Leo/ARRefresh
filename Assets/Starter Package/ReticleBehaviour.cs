﻿/*
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

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Starter_Package {
    public class ReticleBehaviour : MonoBehaviour {
        [SerializeField] DrivingSurfaceManager _drivingSurfaceManager;
        [SerializeField] ARRaycastManager _raycastManager;
        [SerializeField] ARPlaneManager _planeManager;

        public ARPlane CurrentPlane { get; private set; }

        Camera MainCamera => _mainCamera ??= Camera.main;
        Camera _mainCamera;
        GameObject _child;

        void Start() {
            _child = transform.GetChild(0).gameObject;
        }

        void Update() {
            var screenCenter = MainCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

            var hits = new List<ARRaycastHit>();
            _raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinBounds);

            CurrentPlane = null;
            ARRaycastHit? hit = null;

            if (hits.Count > 0) {
                var lockedPlane = _drivingSurfaceManager.LockedPlane;
                hit = lockedPlane == null
                    ? hits[0]
                    : hits.SingleOrDefault(x => x.trackableId == lockedPlane.trackableId);
            }

            if (hit.HasValue) {
                CurrentPlane = _planeManager.GetPlane(hit.Value.trackableId);
                transform.position = hit.Value.pose.position;
            }

            _child.SetActive(CurrentPlane is not null);
        }
    }
}