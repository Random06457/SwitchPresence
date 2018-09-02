#pragma once

enum
{
    Module_Discord = 789,
};

enum
{
    Breakpoint                  = 0, //can be used to localise an error
    Error_InitSocket            = 1,
    Error_Listen                = 2,
    Error_Accepting             = 3,
    Error_ListAppFailed         = 4,
    Error_InvalidMagic          = 5,
    Error_CmdIdNotConfirm       = 6,
    Error_CmdIdNotSendBuff      = 7,
    Error_RecData               = 8,
    Error_SendData              = 9,
    Error_InitNS                = 10,
    Error_InitACC               = 11,
    Error_GetControlData        = 12,
    Error_InvalidControlSize    = 13,
    Error_GetAciveUser          = 14,
    Error_GetProfile            = 15,
    Error_ProfileGet            = 16,
    Error_InitPMDMNT            = 17,
    Error_GetAppPid             = 18,
    Error_GetProcessTid         = 19,
    Error_InitPMINFO            = 20,
    Error_GetPidList            = 21,
    Error_GetDebugProc          = 22,
    Error_CloseHandle           = 23,
};