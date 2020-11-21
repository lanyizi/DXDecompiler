//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
//
// Buffer Definitions: 
//
// cbuffer $Globals
// {
//
//   float4 gFloat1;                    // Offset:    0 Size:    16
//   float4 gFloat2;                    // Offset:   16 Size:    16
//   float4 gFloatArr1[4];              // Offset:   32 Size:    64 [unused]
//      = 0xbf800000 0x3f800000 0x00000000 0x40000000 
//        0x3f800000 0x3f800000 0x00000000 0x40400000 
//        0xbf800000 0xbf800000 0x00000000 0x40800000 
//        0x3f800000 0xbf800000 0x00000000 0x40a00000 
//   int4 gInt1;                        // Offset:   96 Size:    16 [unused]
//   int4 gInt2;                        // Offset:  112 Size:    16 [unused]
//   bool gBool1;                       // Offset:  128 Size:     4 [unused]
//   bool gBool2;                       // Offset:  132 Size:     4 [unused]
//   float4x4 gMatrix1;                 // Offset:  144 Size:    64 [unused]
//   
//   struct cClass1
//   {
//       
//       float4 foo;                    // Offset:  208
//       float4 bar;                    // Offset:  224
//
//   } gAbstractInterface2;             // Offset:  208 Size:    32 [unused]
//   
//   struct cClass2
//   {
//       
//       float4 foo;                    // Offset:  240
//       float4 bar;                    // Offset:  256
//
//   } gAbstractInterface3;             // Offset:  240 Size:    32 [unused]
//   float g_fTessellationFactor;       // Offset:  272 Size:     4 [unused]
//
// }
//
//
// Resource Bindings:
//
// Name                                 Type  Format         Dim      HLSL Bind  Count
// ------------------------------ ---------- ------- ----------- -------------- ------
// $Globals                          cbuffer      NA          NA            cb0      1 
//
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// TEXCOORD                 1   x           0     NONE    uint       
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_Position              0   xyzw        0      POS   float   xyzw
//
gs_5_0
dcl_globalFlags refactoringAllowed
dcl_constantbuffer CB0[2], immediateIndexed
dcl_input v[4][0].x
dcl_temps 3
dcl_inputprimitive lineadj 
dcl_stream m0
dcl_outputtopology pointlist 
dcl_output_siv o0.xyzw, position
dcl_maxout 144
mul r0.xyzw, cb0[0].xyzw, cb0[1].xyzw
mov r1.xyzw, l(0,0,0,0)
mov r2.x, l(0)
loop 
  ige r2.y, r2.x, l(4)
  breakc_nz r2.y
  mov o0.xyzw, r0.xyzw
  emit_stream m0
  iadd r2.x, r2.x, l(1)
  mov r1.xyzw, r0.xyzw
endloop 
cut_stream m0
mov o0.xyzw, r1.xyzw
emit_stream m0
cut_stream m0
mov o0.xyzw, r1.xyzw
emit_stream m0
ret 
// Approximately 18 instruction slots used