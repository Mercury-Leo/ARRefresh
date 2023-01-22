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
using UnityEngine.XR.ARFoundation;

namespace Starter_Package {
    public class PackageSpawner : MonoBehaviour {
        [SerializeField] DrivingSurfaceManager _drivingSurfaceManager;
        [SerializeField] GameObject _packagePrefab;
        
        PackageBehaviour _package;

        void Update() {
            var lockedPlane = _drivingSurfaceManager.LockedPlane;
            if (lockedPlane is null) return;

            if (_package is null)
                SpawnPackage(lockedPlane);

            if (_package is null)
                return;

            var packagePosition = _package.gameObject.transform.position;
            packagePosition.Set(packagePosition.x, lockedPlane.center.y, packagePosition.z);
        }

        public static Vector3 RandomInTriangle(Vector3 v1, Vector3 v2) {
            var u = Random.Range(0.0f, 1.0f);
            var v = Random.Range(0.0f, 1.0f);

            if (v + u > 1) {
                v = 1 - v;
                u = 1 - u;
            }

            return (v1 * u) + (v2 * v);
        }

        public static Vector3 FindRandomLocation(ARPlane plane) {
            // Select random triangle in Mesh
            var mesh = plane.GetComponent<ARPlaneMeshVisualizer>().mesh;
            var triangles = mesh.triangles;
            var triangle = triangles[(int)Random.Range(0, triangles.Length - 1)] / 3 * 3;
            var vertices = mesh.vertices;
            var randomInTriangle = RandomInTriangle(vertices[triangle], vertices[triangle + 1]);
            var randomPoint = plane.transform.TransformPoint(randomInTriangle);

            return randomPoint;
        }

        public void SpawnPackage(ARPlane plane) {
            var packageClone = Instantiate(_packagePrefab);
            packageClone.transform.position = FindRandomLocation(plane);

            _package = packageClone.GetComponent<PackageBehaviour>();
        }
    }
}