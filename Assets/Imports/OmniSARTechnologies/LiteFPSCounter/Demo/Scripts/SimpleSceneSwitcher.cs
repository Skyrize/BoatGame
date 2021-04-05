///
// Simple Scene Switcher
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Manager = UnityEngine.SceneManagement.SceneManager;

namespace OmniSARTechnologies.LiteFPSCounter.Examples {
    public class SimpleSceneSwitcher : MonoBehaviour {
        public Text sceneNameText;

        private void Start() {
            UpdateSceneNameText();
        }

        public void ChangeActiveScene(int buildIndexOffset) {
            if (Manager.sceneCountInBuildSettings < 1) {
                return;
            }

            int newSceneBuildIndex = Manager.GetActiveScene().buildIndex;
            newSceneBuildIndex += buildIndexOffset;
            newSceneBuildIndex += Manager.sceneCountInBuildSettings << 10;
            newSceneBuildIndex %= Manager.sceneCountInBuildSettings;
            newSceneBuildIndex = Mathf.Clamp(newSceneBuildIndex, 0, Manager.sceneCountInBuildSettings - 1);

            Manager.LoadScene(newSceneBuildIndex);
            UpdateSceneNameText();
        }

        private void UpdateSceneNameText() {
            if (!sceneNameText) {
                return;
            }

            sceneNameText.text = Manager.GetActiveScene().name;
        }
    }
}
