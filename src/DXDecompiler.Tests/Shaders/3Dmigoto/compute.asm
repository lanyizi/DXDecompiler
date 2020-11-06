//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
//
// Note: shader requires additional functionality:
//       64 UAV slots
//       Shader extensions for 11.1
//
//
// Buffer Definitions: 
//
// Resource bind info for rw_structured_glc
// {
//
//   struct buf
//   {
//       
//       float foo;                     // Offset:    0
//       float bar;                     // Offset:    4
//
//   } $Element;                        // Offset:    0 Size:     8
//
// }
//
//
// Resource Bindings:
//
// Name                                 Type  Format         Dim      HLSL Bind  Count
// ------------------------------ ---------- ------- ----------- -------------- ------
// rw_byte_buf                           UAV    byte         r/w             u0      1 
// rw_byte_buf_glc                       UAV    byte         r/w             u1      1 
// rw_structured_glc                     UAV  struct         r/w             u2      1 
// rw_buf_glc                            UAV   float         buf             u3      1 
// rw_tex1d_glc                          UAV   float          1d             u4      1 
// rw_tex1d_array_glc                    UAV   float     1darray             u5      1 
// rw_tex2d_glc                          UAV   float          2d             u6      1 
// rw_tex2d_array_glc                    UAV   float     2darray             u7      1 
// rw_tex3d_glc                          UAV   float          3d             u8      1 
//
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// no Input
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// no Output
cs_5_0
dcl_globalFlags refactoringAllowed | skipOptimization | enable11_1ShaderExtensions
dcl_uav_raw u0
dcl_uav_raw_glc u1
dcl_uav_structured_glc u2, 8
dcl_uav_typed_buffer_glc (float,float,float,float) u3
dcl_uav_typed_texture1d_glc (float,float,float,float) u4
dcl_uav_typed_texture1darray_glc (float,float,float,float) u5
dcl_uav_typed_texture2d_glc (float,float,float,float) u6
dcl_uav_typed_texture2darray_glc (float,float,float,float) u7
dcl_uav_typed_texture3d_glc (float,float,float,float) u8
dcl_temps 2
dcl_tgsm_raw g0, 4
dcl_tgsm_structured g1, 8, 3
dcl_thread_group 4, 2, 1
mov r0.x, l(0)
store_raw g0.x, l(0), r0.x
mov r0.x, l(0)
bufinfo_indexable(raw_buffer)(mixed,mixed,mixed,mixed) r0.y, u0.yxzw
mov r0.y, r0.y
iadd r0.x, r0.y, r0.x
imm_atomic_iadd r1.x, u0, l(0), l(1)
mov r1.x, r1.x
iadd r0.x, r0.x, r1.x
atomic_iadd u0, l(0), l(1)
and r0.y, l(-1), l(1)
imm_atomic_and r1.x, u0, l(0), r0.y
mov r1.x, r1.x
iadd r0.x, r0.x, r1.x
and r0.y, l(0), l(1)
atomic_and u0, l(0), r0.y
imm_atomic_cmp_exch r1.x, u0, l(0), l(1), l(6)
mov r1.x, r1.x
iadd r0.x, r0.x, r1.x
atomic_cmp_store u0, l(0), l(2), l(3)
imm_atomic_exch r1.x, u0, l(0), l(5)
mov r1.x, r1.x
iadd r0.x, r0.x, r1.x
imm_atomic_imax r1.x, u0, l(0), l(6)
mov r1.x, r1.x
iadd r0.x, r0.x, r1.x
atomic_imax u0, l(0), l(7)
mov r0.y, l(6)
imm_atomic_umax r1.x, u0, l(0), r0.y
mov r1.x, r1.x
iadd r0.x, r0.x, r1.x
mov r0.y, l(7)
atomic_umax u0, l(0), r0.y
imm_atomic_imin r1.x, u0, l(0), l(4)
mov r1.x, r1.x
iadd r0.x, r0.x, r1.x
atomic_imin u0, l(0), l(2)
mov r0.y, l(4)
imm_atomic_umin r1.x, u0, l(0), r0.y
mov r1.x, r1.x
iadd r0.x, r0.x, r1.x
mov r0.y, l(2)
atomic_umin u0, l(0), r0.y
imm_atomic_or r1.x, u0, l(0), l(7)
mov r1.x, r1.x
iadd r0.x, r0.x, r1.x
atomic_or u0, l(0), l(9)
imm_atomic_xor r1.x, u0, l(0), l(8)
mov r1.x, r1.x
iadd r0.x, r0.x, r1.x
atomic_xor u0, l(0), l(9)
ld_raw_indexable(raw_buffer)(mixed,mixed,mixed,mixed) r0.y, l(1), u0.xxxx
iadd r0.x, r0.y, r0.x
store_raw u0.x, l(5), l(8)
ld_raw_indexable(raw_buffer)(mixed,mixed,mixed,mixed) r0.y, l(0), u1.xxxx
iadd r0.x, r0.y, r0.x
mov r0.y, l(0)
ld_structured_indexable(structured_buffer, stride=8)(mixed,mixed,mixed,mixed) r0.y, r0.y, l(0), u2.xxxx
utof r0.x, r0.x
add r0.x, r0.y, r0.x
ftou r0.x, r0.x
mov r0.y, l(0)
ld_uav_typed_indexable(buffer)(float,float,float,float) r0.y, r0.yyyy, u3.yxzw
utof r0.x, r0.x
add r0.x, r0.y, r0.x
ftou r0.x, r0.x
mov r0.y, l(0)
ld_uav_typed_indexable(texture1d)(float,float,float,float) r0.y, r0.yyyy, u4.yxzw
utof r0.x, r0.x
add r0.x, r0.y, r0.x
ftou r0.x, r0.x
mov r1.xyzw, l(0,0,0,0)
ld_uav_typed_indexable(texture1darray)(float,float,float,float) r0.y, r1.xyzw, u5.yxzw
utof r0.x, r0.x
add r0.x, r0.y, r0.x
ftou r0.x, r0.x
mov r1.xyzw, l(0,0,0,0)
ld_uav_typed_indexable(texture2d)(float,float,float,float) r0.y, r1.xyzw, u6.yxzw
utof r0.x, r0.x
add r0.x, r0.y, r0.x
ftou r0.x, r0.x
mov r1.xyzw, l(0,0,0,0)
ld_uav_typed_indexable(texture2darray)(float,float,float,float) r0.y, r1.xyzw, u7.yxzw
utof r0.x, r0.x
add r0.x, r0.y, r0.x
ftou r0.x, r0.x
mov r1.xyzw, l(0,0,0,0)
ld_uav_typed_indexable(texture3d)(float,float,float,float) r0.y, r1.xyzw, u8.yxzw
utof r0.x, r0.x
add r0.x, r0.y, r0.x
ftou r0.x, r0.x
firstbit_hi r0.y, r0.x
ieq r0.z, r0.y, l(-1)
ineg r0.y, r0.y
iadd r0.y, r0.y, l(31)
movc r0.y, r0.z, l(-1), r0.y
iadd r0.x, r0.y, r0.x
firstbit_lo r0.y, r0.x
iadd r0.x, r0.y, r0.x
firstbit_shi r0.y, r0.x
ieq r0.z, r0.y, l(-1)
ineg r0.y, r0.y
iadd r0.y, r0.y, l(31)
movc r0.y, r0.z, l(-1), r0.y
iadd r0.x, r0.y, r0.x
mov r0.y, l(1)
mov r0.z, l(3)
msad r0.y, r0.x, r0.y, r0.z
iadd r0.x, r0.y, r0.x
sync_uglobal_g
imm_atomic_iadd r1.x, g0, l(0), l(1)
mov r1.x, r1.x
iadd r0.x, r0.x, r1.x
sync_uglobal_g_t
imm_atomic_iadd r1.x, g0, l(0), l(1)
mov r1.x, r1.x
iadd r0.x, r0.x, r1.x
sync_uglobal
imm_atomic_iadd r1.x, g0, l(0), l(1)
mov r1.x, r1.x
iadd r0.x, r0.x, r1.x
sync_uglobal_t
imm_atomic_iadd r1.x, g0, l(0), l(1)
mov r1.x, r1.x
iadd r0.x, r0.x, r1.x
sync_g
imm_atomic_iadd r1.x, g0, l(0), l(1)
mov r1.x, r1.x
iadd r0.x, r0.x, r1.x
sync_g_t
imm_atomic_iadd r1.x, g0, l(0), l(1)
mov r1.x, r1.x
iadd r0.x, r0.x, r1.x
ld_structured r0.y, l(2), l(4), g1.xxxx
iadd r0.x, r0.y, r0.x
atomic_iadd u0, l(1), r0.x
ret 
// Approximately 137 instruction slots used