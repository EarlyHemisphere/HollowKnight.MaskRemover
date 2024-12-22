using Modding;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MaskRemover {
    public class MaskRemover: Mod, ITogglableMod {
        internal static MaskRemover instance;
        private GameObject[] masks;


        public MaskRemover(): base("Mask Remover") {
            instance = this;
        }

        public override void Initialize() {
            Log("Initializing");

            instance = this;
            masks = new GameObject[9];
            ModHooks.HeroUpdateHook += PollForMasks;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneChanged;

            Log("Initialized");
        }

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        private void SceneChanged(Scene _, Scene to) {
            if (to.name == Constants.MENU_SCENE) {
                masks = new GameObject[9];
                ModHooks.HeroUpdateHook += PollForMasks;
            }
        }

        private void PollForMasks() {
            foreach (int num in Enumerable.Range(0, 9)) {
                masks[num] = GameObject.Find("Health " + (num + 1));
            }
            if (masks.All(obj => obj != null)) {
                foreach (GameObject mask in masks) {
                    mask.transform.localScale = new Vector3(0, 0, 0);
                }
                ModHooks.HeroUpdateHook -= PollForMasks;
            }
        }

        public void Unload() {
            if (masks.All(obj => obj != null)) {
                foreach (GameObject mask in masks) {
                    mask.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                }
            }
            ModHooks.HeroUpdateHook -= PollForMasks;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= SceneChanged;
        }
    }
}
