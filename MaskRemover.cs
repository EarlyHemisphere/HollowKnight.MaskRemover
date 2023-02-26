using Modding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaskRemover : Mod, ITogglableMod {
    internal static MaskRemover Instance;
    private GameObject[] masks;

    public MaskRemover() : base("Mask Remover") {
       Instance = this;
    }

    public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects) {
        Log("Initializing");

        Instance = this;
        ModHooks.SavegameLoadHook += InitiateMaskPoll;

        Log("Initialized");
    }

    public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

    private void InitiateMaskPoll(int _) {
        masks = new GameObject[9];
        ModHooks.HeroUpdateHook += PollForMasks;
    }

    private void PollForMasks() {
        foreach (int num in Enumerable.Range(1, 9)) {
            masks[num-1] = GameObject.Find("Health " + num);
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
        ModHooks.SavegameLoadHook -= InitiateMaskPoll;
    }
}