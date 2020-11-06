//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
//
// Note: shader requires additional functionality:
//       Minimum-precision data types
//
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_Position              0   xyzw        0     NONE   float   xyzw
// SV_VertexID              0   x           1   VERTID    uint   x   
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_Position              0   xyzw        0      POS   float   xyzw
// SV_ClipDistance          0   x           1  CLIPDST   float   x   
// SV_CullDistance          0    y          1  CULLDST   float    y  
// TEXCOORD                 0   x           2     NONE  min16f       
// TEXCOORD                 1    y          2     NONE min2_8f       
// TEXCOORD                 2   x           3     NONE  min16i       
// TEXCOORD                 3    y          3     NONE  min16i       
// TEXCOORD                 4     z         3     NONE  min16u       
//
vs_5_0
dcl_globalFlags refactoringAllowed | enableMinimumPrecision
dcl_input v0.xyzw
dcl_input_sgv v1.x, vertex_id
dcl_output_siv o0.xyzw, position
dcl_output_siv o1.x, clip_distance
dcl_output_siv o1.y, cull_distance
dcl_temps 1
utof r0.x, v1.x
add o0.x, r0.x, v0.x
mov o0.yzw, v0.yyzw
mov o1.x, l(0)
mov o1.y, l(0)
ret 
// Approximately 6 instruction slots used