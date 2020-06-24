
//listing of all techniques and passes with embedded asm listings 

technique RenderWithPRTColorLights
{
    pass P0
    {
        vertexshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float4 MaterialDiffuseColor;
            //   float4 aPRTConstants[70];
            //   float4x4 g_mWorldViewProjection;
            //
            //
            // Registers:
            //
            //   Name                   Reg   Size
            //   ---------------------- ----- ----
            //   aPRTConstants          c0      70
            //   g_mWorldViewProjection c70      4
            //   MaterialDiffuseColor   c74      1
            //
            
                vs_2_0
                def c75, 1, 0, 0, 0
                dcl_blendweight1 v0
                dcl_blendweight2 v1
                dcl_position v2
                dcl_texcoord v3
                dcl_blendweight v4
                dp4 oPos.x, v2, c70
                dp4 oPos.y, v2, c71
                dp4 oPos.z, v2, c72
                dp4 oPos.w, v2, c73
                mova a0.x, v4.x
                mul r0, v1, c2[a0.x]
                mad r0, v0, c1[a0.x], r0
                dp4 r0.x, r0, c75.x
                add r0.x, r0.x, c0[a0.x].x
                mul oD0.x, r0.x, c74.x
                mul r0, v1, c4[a0.x]
                mad r0, v0, c3[a0.x], r0
                dp4 r0.x, r0, c75.x
                add r0.x, r0.x, c0[a0.x].y
                mul oD0.y, r0.x, c74.y
                mul r0, v1, c6[a0.x]
                mad r0, v0, c5[a0.x], r0
                dp4 r0.x, r0, c75.x
                add r0.x, r0.x, c0[a0.x].z
                mov r0.w, c0[a0.x].w
                mul oD0.zw, r0.xyxw, c74
                mov oT0.xy, v3
            
            // approximately 22 instruction slots used
            };

        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   sampler2D AlbedoSampler;
            //
            //
            // Registers:
            //
            //   Name          Reg   Size
            //   ------------- ----- ----
            //   AlbedoSampler s0       1
            //
            
                ps_2_0
                dcl v0
                dcl t0.xy
                dcl_2d s0
                texld r0, t0, s0
                mul r0, r0, v0
                mov oC0, r0
            
            // approximately 3 instruction slots used (1 texture, 2 arithmetic)
            };
    }
}

technique RenderWithPRTColorLightsNoAlbedo
{
    pass P0
    {
        vertexshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float4 MaterialDiffuseColor;
            //   float4 aPRTConstants[70];
            //   float4x4 g_mWorldViewProjection;
            //
            //
            // Registers:
            //
            //   Name                   Reg   Size
            //   ---------------------- ----- ----
            //   aPRTConstants          c0      70
            //   g_mWorldViewProjection c70      4
            //   MaterialDiffuseColor   c74      1
            //
            
                vs_2_0
                def c75, 1, 0, 0, 0
                dcl_blendweight1 v0
                dcl_blendweight2 v1
                dcl_position v2
                dcl_blendweight v3
                dp4 oPos.x, v2, c70
                dp4 oPos.y, v2, c71
                dp4 oPos.z, v2, c72
                dp4 oPos.w, v2, c73
                mova a0.x, v3.x
                mul r0, v1, c2[a0.x]
                mad r0, v0, c1[a0.x], r0
                dp4 r0.x, r0, c75.x
                add r0.x, r0.x, c0[a0.x].x
                mul oD0.x, r0.x, c74.x
                mul r0, v1, c4[a0.x]
                mad r0, v0, c3[a0.x], r0
                dp4 r0.x, r0, c75.x
                add r0.x, r0.x, c0[a0.x].y
                mul oD0.y, r0.x, c74.y
                mul r0, v1, c6[a0.x]
                mad r0, v0, c5[a0.x], r0
                dp4 r0.x, r0, c75.x
                add r0.x, r0.x, c0[a0.x].z
                mov r0.w, c0[a0.x].w
                mul oD0.zw, r0.xyxw, c74
                mov oT0.xy, c75.y
            
            // approximately 22 instruction slots used
            };

        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
                ps_2_0
                dcl v0
                mov oC0, v0
            
            // approximately 1 instruction slot used
            };
    }
}
