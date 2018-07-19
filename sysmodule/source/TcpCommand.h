#pragma once

#include <stdio.h>
//#include <fstream>
#include <string.h>
#include <dirent.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <switch.h>
#include "utils.h"
#include "creport_debug_types.h"


enum class ClientCommand : u8 {
    SendBuffer      = 0,
    Confirm         = 1,
    GetControlData  = 2,
    ListApps        = 3,
    GetActiveUser   = 4,
    GetCurrentApp   = 5,
    GetVersion      = 6,
    Disconnect      = 0xFF,
};

enum class ServerCommand : u8 {
    Exception       = 0,
    Normal          = 1,
    Confirm         = 2,
};


void StartReceiving(int client);