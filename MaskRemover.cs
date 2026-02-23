using Modding;
using System.Collections.Generic;
using UnityEngine;

namespace MaskRemover {
    public class MaskRemover : Mod, ITogglableMod {
        internal static MaskRemover instance;
        private const string HealthDisplayFsm = "health_display";
        private readonly List<GameObject> masks = new List<GameObject>();

        public MaskRemover() : base("Mask Remover") {
            instance = this;
        }

        public override void Initialize() {
            Log("Initializing");

            instance = this;
            On.PlayMakerFSM.OnEnable += OnFsmEnable;
            HideExistingMasks();

            Log("Initialized");
        }

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        private void HideExistingMasks() {
            if (GameCameras.instance == null) return;
            GameObject hudCanvas = GameCameras.instance.hudCanvas;
            if (hudCanvas == null) return;

            foreach (PlayMakerFSM fsm in hudCanvas.GetComponentsInChildren<PlayMakerFSM>(true)) {
                if (fsm.FsmName == HealthDisplayFsm) {
                    HideMask(fsm.gameObject);
                }
            }
        }

        private void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
            orig(self);

            if (self.FsmName == HealthDisplayFsm) {
                HideMask(self.gameObject);
            }
        }

        private void HideMask(GameObject mask) {
            if (!masks.Contains(mask)) {
                masks.Add(mask);
            }
            mask.transform.localScale = Vector3.zero;
        }

        public void Unload() {
            On.PlayMakerFSM.OnEnable -= OnFsmEnable;
            foreach (GameObject mask in masks) {
                if (mask != null) {
                    mask.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                }
            }
            masks.Clear();
        }
    }
}
