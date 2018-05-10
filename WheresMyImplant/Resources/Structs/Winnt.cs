﻿using System.Runtime.InteropServices;

using WORD = System.UInt16;
using DWORD = System.UInt32;
using QWORD = System.UInt64;
using ULONGLONG = System.UInt64;

using PVOID = System.IntPtr;
using LPVOID = System.IntPtr;
using DWORD_PTR = System.IntPtr;
using SIZE_T = System.IntPtr;

namespace WheresMyImplant
{
    public class Winnt
    {
        private const DWORD EXCEPTION_MAXIMUM_PARAMETERS = 15;

        ////////////////////////////////////////////////////////////////////////////////
        // https://msdn.microsoft.com/en-us/library/windows/desktop/aa366786(v=vs.85).aspx
        ////////////////////////////////////////////////////////////////////////////////
        public const DWORD PAGE_NOACCESS = 0x01;
        public const DWORD PAGE_READONLY = 0x02;
        public const DWORD PAGE_READWRITE = 0x04;
        public const DWORD PAGE_WRITECOPY = 0x08;
        public const DWORD PAGE_EXECUTE = 0x10;
        public const DWORD PAGE_EXECUTE_READ = 0x20;
        public const DWORD PAGE_EXECUTE_READWRITE = 0x40;
        public const DWORD PAGE_EXECUTE_WRITECOPY = 0x80;
        public const DWORD PAGE_GUARD = 0x100;
        public const DWORD PAGE_NOCACHE = 0x200;
        public const DWORD PAGE_WRITECOMBINE = 0x400;
        public const DWORD PAGE_TARGETS_INVALID = 0x40000000;
        public const DWORD PAGE_TARGETS_NO_UPDATE = 0x40000000;

        public enum CONTEXT_FLAGS : uint
        {
            CONTEXT_i386 = 0x10000,
            CONTEXT_i486 = 0x10000,   //  same as i386
            CONTEXT_CONTROL = CONTEXT_i386 | 0x0001, // SS:SP, CS:IP, FLAGS, BP
            CONTEXT_INTEGER = CONTEXT_i386 | 0x0002, // AX, BX, CX, DX, SI, DI
            CONTEXT_SEGMENTS = CONTEXT_i386 | 0x0004, // DS, ES, FS, GS
            CONTEXT_FLOATING_POINT = CONTEXT_i386 | 0x0008, // 387 state
            CONTEXT_DEBUG_REGISTERS = CONTEXT_i386 | 0x0010, // DB 0-3,6,7
            CONTEXT_EXTENDED_REGISTERS = CONTEXT_i386 | 0x0020, // cpu specific extensions
            CONTEXT_FULL = CONTEXT_CONTROL | CONTEXT_INTEGER | CONTEXT_SEGMENTS,
            CONTEXT_ALL = CONTEXT_CONTROL | CONTEXT_INTEGER | CONTEXT_SEGMENTS | CONTEXT_FLOATING_POINT | CONTEXT_DEBUG_REGISTERS | CONTEXT_EXTENDED_REGISTERS
        }

        public enum CONTEXT_FLAGS64 : uint
        {
            CONTEXT_AMD64 = 0x100000,
            CONTEXT_CONTROL = CONTEXT_AMD64 | 0x01, // SS:SP, CS:IP, FLAGS, BP
            CONTEXT_INTEGER = CONTEXT_AMD64 | 0x02, // AX, BX, CX, DX, SI, DI
            CONTEXT_SEGMENTS = CONTEXT_AMD64 | 0x04, // DS, ES, FS, GS
            CONTEXT_FLOATING_POINT = CONTEXT_AMD64 | 0x08, // 387 state
            CONTEXT_DEBUG_REGISTERS = CONTEXT_AMD64 | 0x10, // DB 0-3,6,7
            CONTEXT_EXTENDED_REGISTERS = CONTEXT_AMD64 | 0x20, // cpu specific extensions
            CONTEXT_FULL = 1048587,//CONTEXT_CONTROL | CONTEXT_INTEGER | CONTEXT_SEGMENTS,
            CONTEXT_ALL = CONTEXT_CONTROL | CONTEXT_INTEGER | CONTEXT_SEGMENTS | CONTEXT_FLOATING_POINT | CONTEXT_DEBUG_REGISTERS | CONTEXT_EXTENDED_REGISTERS
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct _FLOATING_SAVE_AREA
        {
            public DWORD ControlWord;
            public DWORD StatusWord;
            public DWORD TagWord;
            public DWORD ErrorOffset;
            public DWORD ErrorSelector;
            public DWORD DataOffset;
            public DWORD DataSelector;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
            public byte[] RegisterArea;
            public DWORD Cr0NpxState;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CONTEXT
        {
            public CONTEXT_FLAGS ContextFlags; //set this to an appropriate value 
            // Retrieved by CONTEXT_DEBUG_REGISTERS 
            public uint Dr0;
            public uint Dr1;
            public uint Dr2;
            public uint Dr3;
            public uint Dr6;
            public uint Dr7;
            // Retrieved by CONTEXT_FLOATING_POINT 
            public _FLOATING_SAVE_AREA FloatSave;
            // Retrieved by CONTEXT_SEGMENTS 
            public uint SegGs;
            public uint SegFs;
            public uint SegEs;
            public uint SegDs;
            // Retrieved by CONTEXT_INTEGER 
            public uint Edi;
            public uint Esi;
            public uint Ebx;
            public uint Edx;
            public uint Ecx;
            public uint Eax;
            // Retrieved by CONTEXT_CONTROL 
            public uint Ebp;
            public uint Eip;
            public uint SegCs;
            public uint EFlags;
            public uint Esp;
            public uint SegSs;
            // Retrieved by CONTEXT_EXTENDED_REGISTERS 
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            public byte[] ExtendedRegisters;
        } 

        [StructLayout(LayoutKind.Sequential)]
        public struct _M128A
        {
            public ulong High;
            public long Low;
        }

        
        [StructLayout(LayoutKind.Sequential)]
        public struct _XMM_SAVE_AREA32
        {
            public WORD ControlWord;
            public WORD StatusWord;
            public byte TagWord;
            public byte Reserved1;
            public WORD ErrorOpcode;
            public DWORD ErrorOffset;
            public WORD ErrorSelector;
            public WORD Reserved2;
            public DWORD DataOffset;
            public WORD DataSelector;
            public WORD Reserved3;
            public WORD MxCsr;
            public WORD MxCsr_Mask;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public _M128A[] FloatRegisters;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public _M128A[] XmmRegisters;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 96)]
            public byte[] Reserved4;
        }
        


        [StructLayout(LayoutKind.Sequential)]
        public struct CONTEXT64
        {
            public ulong P1Home;
            public ulong P2Home;
            public ulong P3Home;
            public ulong P4Home;
            public ulong P5Home;
            public ulong P6Home;

            public CONTEXT_FLAGS64 ContextFlags;
            public uint MxCsr;

            public ushort SegCs;
            public ushort SegDs;
            public ushort SegEs;
            public ushort SegFs;
            public ushort SegGs;
            public ushort SegSs;
            public uint EFlags;

            public ulong Dr0;
            public ulong Dr1;
            public ulong Dr2;
            public ulong Dr3;
            public ulong Dr6;
            public ulong Dr7;

            public ulong Rax;
            public ulong Rcx;
            public ulong Rdx;
            public ulong Rbx;
            public ulong Rsp;
            public ulong Rbp;
            public ulong Rsi;
            public ulong Rdi;
            public ulong R8;
            public ulong R9;
            public ulong R10;
            public ulong R11;
            public ulong R12;
            public ulong R13;
            public ulong R14;
            public ulong R15;

            public ulong Rip;

            public _XMM_SAVE_AREA32 FltSave;

            /*
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public _M128A[] Header;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public _M128A[] Legacy;
            public _M128A Xmm0;
            public _M128A Xmm1;
            public _M128A Xmm2;
            public _M128A Xmm3;
            public _M128A Xmm4;
            public _M128A Xmm5;
            public _M128A Xmm6;
            public _M128A Xmm7;
            public _M128A Xmm8;
            public _M128A Xmm9;
            public _M128A Xmm10;
            public _M128A Xmm11;
            public _M128A Xmm12;
            public _M128A Xmm13;
            public _M128A Xmm14;
            public _M128A Xmm15;
             */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            public _M128A[] VectorRegister;
            public ulong VectorControl;

            public ulong DebugControl;
            public ulong LastBranchToRip;
            public ulong LastBranchFromRip;
            public ulong LastExceptionToRip;
            public ulong LastExceptionFromRip;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct _EXCEPTION_POINTERS
        {
            public System.IntPtr ExceptionRecord;
            public System.IntPtr ContextRecord;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        struct _EXCEPTION_RECORD 
        {
            public DWORD ExceptionCode;
            public DWORD ExceptionFlags;
            public System.IntPtr hExceptionRecord;
            public PVOID ExceptionAddress;
            public DWORD NumberParameters;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public DWORD[] ExceptionInformation;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct _IMAGE_DATA_DIRECTORY
        {
            public DWORD VirtualAddress;
            public DWORD Size;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct _IMAGE_DOS_HEADER
        {
            public WORD e_magic;
            public WORD e_cblp;
            public WORD e_cp;
            public WORD e_crlc;
            public WORD e_cparhdr;
            public WORD e_minalloc;
            public WORD e_maxalloc;
            public WORD e_ss;
            public WORD e_sp;
            public WORD e_csum;
            public WORD e_ip;
            public WORD e_cs;
            public WORD e_lfarlc;
            public WORD e_ovno;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] //Matt has this set to 8
            public WORD[] e_res;
            public WORD e_oemid;
            public WORD e_oeminfo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public WORD[] e_res2;
            public DWORD e_lfanew; //Maybe Int64?
        };

        //http://www.exploit-monday.com/2012/04/64-bit-process-replacement-in.html
        public enum IMAGE_FILE_MACHINE : ushort
        {
            I386 = 0x014c,
            IA64 = 0x0200,
            AMD64 = 0x8664,
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct _IMAGE_NT_HEADERS
        {
            public DWORD Signature;                         
            public _IMAGE_FILE_HEADER FileHeader;           
            public _IMAGE_OPTIONAL_HEADER OptionalHeader;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct _IMAGE_NT_HEADERS64
        {
            public DWORD Signature;
            public _IMAGE_FILE_HEADER FileHeader;
            public _IMAGE_OPTIONAL_HEADER64 OptionalHeader;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct _IMAGE_FILE_HEADER
        {
            public IMAGE_FILE_MACHINE Machine;
            public WORD NumberOfSections;
            public DWORD TimeDateStamp;
            public DWORD PointerToSymbolTable;
            public DWORD NumberOfSymbols;
            public WORD SizeOfOptionalHeader;
            public WORD Characteristics;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct _IMAGE_OPTIONAL_HEADER
        {
            public WORD Magic;
            public byte MajorLinkerVersion;
            public byte MinorLinkerVersion;
            public DWORD SizeOfCode;
            public DWORD SizeOfInitializedData;
            public DWORD SizeOfUninitializedData;
            public DWORD AddressOfEntryPoint;
            public DWORD BaseOfCode;
            public DWORD BaseOfData;
            public DWORD ImageBase;
            public DWORD SectionAlignment;
            public DWORD FileAlignment;
            public WORD MajorOperatingSystemVersion;
            public WORD MinorOperatingSystemVersion;
            public WORD MajorImageVersion;
            public WORD MinorImageVersion;
            public WORD MajorSubsystemVersion;
            public WORD MinorSubsystemVersion;
            public DWORD Win32VersionValue;
            public DWORD SizeOfImage;
            public DWORD SizeOfHeaders;
            public DWORD CheckSum;
            public WORD Subsystem;
            public WORD DllCharacteristics;
            public DWORD SizeOfStackReserve;
            public DWORD SizeOfStackCommit;
            public DWORD SizeOfHeapReserve;
            public DWORD SizeOfHeapCommit;
            public DWORD LoaderFlags;
            public DWORD NumberOfRvaAndSizes;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public Winnt._IMAGE_DATA_DIRECTORY[] ImageDataDirectory;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct _IMAGE_OPTIONAL_HEADER64
        {
            public WORD Magic;
            public byte MajorLinkerVersion;
            public byte MinorLinkerVersion;
            public DWORD SizeOfCode;
            public DWORD SizeOfInitializedData;
            public DWORD SizeOfUninitializedData;
            public DWORD AddressOfEntryPoint;
            public DWORD BaseOfCode;
            public QWORD ImageBase;
            public DWORD SectionAlignment;
            public DWORD FileAlignment;
            public WORD MajorOperatingSystemVersion;
            public WORD MinorOperatingSystemVersion;
            public WORD MajorImageVersion;
            public WORD MinorImageVersion;
            public WORD MajorSubsystemVersion;
            public WORD MinorSubsystemVersion;
            public DWORD Win32VersionValue;
            public DWORD SizeOfImage;
            public DWORD SizeOfHeaders;
            public DWORD CheckSum;
            public WORD Subsystem;
            public WORD DllCharacteristics;
            public QWORD SizeOfStackReserve;
            public QWORD SizeOfStackCommit;
            public QWORD SizeOfHeapReserve;
            public QWORD SizeOfHeapCommit;
            public DWORD LoaderFlags;
            public DWORD NumberOfRvaAndSizes; //106
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public Winnt._IMAGE_DATA_DIRECTORY[] ImageDataDirectory; //234
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct _IMAGE_SECTION_HEADER
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] Name;
            public DWORD VirtualSize;
            public DWORD VirtualAddress;
            public DWORD SizeOfRawData;
            public DWORD PointerToRawData;
            public DWORD PointerToRelocations;
            public DWORD PointerToLinenumbers;
            public WORD NumberOfRelocations;
            public WORD NumberOfLinenumbers;
            public DWORD Characteristics;
        };

        [StructLayout(LayoutKind.Sequential)]
        internal struct _MEMORY_BASIC_INFORMATION32
        {
            public DWORD BaseAddress;
            public DWORD AllocationBase;
            public DWORD AllocationProtect;
            public DWORD RegionSize;
            public DWORD State;
            public DWORD Protect;
            public DWORD Type;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct _MEMORY_BASIC_INFORMATION64 
        {
            public ULONGLONG BaseAddress;
            public ULONGLONG AllocationBase;
            public DWORD AllocationProtect;
            public DWORD __alignment1;
            public ULONGLONG RegionSize;
            public DWORD State;
            public DWORD Protect;
            public DWORD Type;
            public DWORD __alignment2;
        }
    }
}