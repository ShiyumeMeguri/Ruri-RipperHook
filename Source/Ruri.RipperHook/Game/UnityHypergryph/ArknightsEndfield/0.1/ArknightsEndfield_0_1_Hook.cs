﻿using Ruri.RipperHook.ArknightsEndfieldCommon;
using Ruri.RipperHook.Crypto;
using Ruri.RipperHook.HookUtils.BundleFileBlockReaderHook;

namespace Ruri.RipperHook.ArknightsEndfield_0_1;

public partial class ArknightsEndfield_0_1_Hook : RipperHook
{
    protected ArknightsEndfield_0_1_Hook()
    {
        ArknightsEndfieldCommon_Hook.CustomLZ4 = new LZ4_ArknightsEndfield();
        RuriRuntimeHook.commonDecryptor = new FairGuardDecryptor();
    }

    protected override void InitAttributeHook()
    {
        additionalNamespaces.Add(typeof(ArknightsEndfieldCommon_Hook).Namespace);
        AddExtraHook(typeof(BundleFileBlockReaderHook).Namespace, () => { BundleFileBlockReaderHook.CustomBlockCompression = ArknightsEndfieldCommon_Hook.CustomBlockCompression; });
        base.InitAttributeHook();
    }
}