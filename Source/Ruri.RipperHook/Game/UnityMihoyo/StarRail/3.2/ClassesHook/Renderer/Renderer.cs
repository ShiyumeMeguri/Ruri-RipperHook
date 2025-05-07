﻿using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated;
using AssetRipper.SourceGenerated.Classes.ClassID_25;
using AssetRipper.SourceGenerated.Subclasses.PPtr_Material;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace Ruri.RipperHook.StarRail_3_2;

public partial class StarRail_3_2_Hook
{
    [RetargetMethodFunc(ClassIDType.TrailRenderer, ClassHookVersion)]
    [RetargetMethodFunc(ClassIDType.VFXRenderer, ClassHookVersion)]
    [RetargetMethodFunc(ClassIDType.TilemapRenderer, ClassHookVersion)]
    [RetargetMethodFunc(ClassIDType.SpriteMask, ClassHookVersion)]
    [RetargetMethodFunc(ClassIDType.MeshRenderer, ClassHookVersion)]
    [RetargetMethodFunc(ClassIDType.BillboardRenderer, ClassHookVersion)]
    [RetargetMethodFunc(ClassIDType.SpriteRenderer, ClassHookVersion)]
    [RetargetMethodFunc(ClassIDType.ParticleSystemRenderer, ClassHookVersion)]
    [RetargetMethodFunc(ClassIDType.ParticleSystemRenderer, ClassHookVersion)]
    [RetargetMethodFunc(ClassIDType.SpriteShapeRenderer, ClassHookVersion)]
    [RetargetMethodFunc(ClassIDType.LineRenderer, ClassHookVersion)]
    private static bool RendererReadRelease(ILContext il)
    {
        var ilCursor = new ILCursor(il);
        var startIndex = ilCursor.Index;
        if (ilCursor.TryGotoNext(MoveType.After, instr => instr.OpCode == OpCodes.Stfld && ((FieldReference)instr.Operand).Name == "m_SortingOrder"))
        {
            var targetLabel = ilCursor.MarkLabel();
            ilCursor.Goto(startIndex);
            ilCursor.Emit(OpCodes.Br, targetLabel);
            ilCursor.GotoLabel(targetLabel);
            var destMethod = typeof(StarRail_3_2_Hook).GetMethod(nameof(Renderer_2019_3_0_a6_ReadRelease));
            ilCursor.Emit(OpCodes.Ldarg_0);
            ilCursor.Emit(OpCodes.Ldarg_1);
            ilCursor.Emit(OpCodes.Call, destMethod);
            return true;
        }

        return false;
    }

    public void Renderer_2019_3_0_a6_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as Renderer_2019_3_0_a6;
        var type = typeof(Renderer_2019_3_0_a6);

        _this.GameObject_C25.ReadRelease(ref reader);
        _this.Enabled_C25 = reader.ReadBoolean();
        _this.CastShadows_C25_Byte = reader.ReadByte();
        _this.ReceiveShadows_C25_Byte = reader.ReadByte();
        _this.DynamicOccludee_C25 = reader.ReadByte();
        _this.MotionVectors_C25 = reader.ReadByte();
        _this.LightProbeUsage_C25 = reader.ReadByte();
        _this.ReflectionProbeUsage_C25_Byte = reader.ReadByte();
        _this.RayTracingMode_C25 = reader.ReadRelease_ByteAlign();
        _this.RenderingLayerMask_C25 = reader.ReadUInt32();
        _this.RendererPriority_C25 = reader.ReadInt32();
        _this.LightmapIndex_C25_UInt16 = reader.ReadUInt16();
        _this.LightmapIndexDynamic_C25 = reader.ReadUInt16();
        _this.LightmapTilingOffset_C25.ReadRelease(ref reader);
        _this.LightmapTilingOffsetDynamic_C25.ReadRelease(ref reader);
        SetAssetListField<PPtr_Material_5>(type, "m_Materials", ref reader);
        _this.StaticBatchInfo_C25.ReadRelease(ref reader);
        _this.StaticBatchRoot_C25.ReadRelease(ref reader);
        _this.ProbeAnchor_C25.ReadRelease(ref reader);
        _this.LightProbeVolumeOverride_C25.ReadRelease_AssetAlign(ref reader);
        _this.SortingLayerID_C25_Int32 = reader.ReadInt32();
        _this.SortingLayer_C25 = reader.ReadInt16();
        _this.SortingOrder_C25 = reader.ReadRelease_Int16Align();
        var m_RenderFlag = reader.ReadUInt32();
        var m_StreamingMipmapBias = reader.ReadSingle();
        reader.Align();
    }
}