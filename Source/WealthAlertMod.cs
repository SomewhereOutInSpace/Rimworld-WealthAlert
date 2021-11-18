using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace WealthAlert
{
    public class WealthThreshold : IExposable
    {
        public int threshold = 0;
        public string EditBuffer = "";

        public void ExposeData()
        {
            Scribe_Values.Look(ref threshold, "threshold", 0);
            Scribe_Values.Look(ref EditBuffer, "EditBuffer", "");
        }
    }

    public class WealthAlertSettings : ModSettings
    {
        public List<WealthThreshold> thresholds = new List<WealthThreshold>();
        public override void ExposeData()
        {
            Scribe_Collections.Look(ref thresholds, "thresholds", LookMode.Deep);
            base.ExposeData();
        }
    }

    public class WealthAlertMod : Mod
    {
        WealthAlertSettings settings;
        public WealthAlertMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<WealthAlertSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            if(settings.thresholds == null) { settings.thresholds = new List<WealthThreshold>(); }

            if (listingStandard.ButtonText("WealthAlertAddWT".Translate()))
            {
                if (settings.thresholds.Count >= 19)
                {
                    Messages.Message("WealthAlertFull".Translate(), MessageTypeDefOf.RejectInput);
                }
                else 
                {
                    settings.thresholds.Add(new WealthThreshold());
                }
            }
            if (listingStandard.ButtonText("WealthAlertRemWT".Translate()))
            {
                settings.thresholds.RemoveLast();
            }

            listingStandard.GapLine();

            foreach(WealthThreshold wt in settings.thresholds)
            {
                listingStandard.IntEntry(ref wt.threshold, ref wt.EditBuffer, multiplier: 1000);
            }
            listingStandard.End();
        }

        public override string SettingsCategory()
        {
            return "WealthAlertName".Translate();
        }

    }
}
