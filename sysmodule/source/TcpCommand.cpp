#include "TcpCommand.h"

const int SRV_MAGIC = 0x11223300;
const int CLT_MAGIC = 0x33221100;
const int CONTROL_NACP_SIZE = 0x4000;
const int CONTROL_FULL_SIZE = 0x24000;

const int SERVER_VERSION = 1 << 16 | 0 << 8 | 3;


void SendRaw(int socket, void* buff, size_t size)
{
    size_t total = 0;
    while (total < size)
    {
        size_t count = send(socket, (char*)buff + total, size - total, 0);
        if (count <= 0)
            fatalSimple(MAKERESULT(Module_Discord, Error_SendData));
        total += count;
    }
}
void ReceiveRaw(int socket, void* buff, size_t size)
{
    size_t total = 0;
    while (total < size)
    {
        size_t count = recv(socket, (char*)buff + total, size - total, 0);
        if (count <= 0)
            fatalSimple(MAKERESULT(Module_Discord, Error_RecData));
        total += count;
    }
}

ClientCommand ReceiveCommand(int socket)
{
    int ret;
    ReceiveRaw(socket, &ret, 4);
    if ((ret & 0xFFFFFF00) != CLT_MAGIC)
    {
        return ClientCommand::Disconnect;
    }
    return (ClientCommand)(ret & 0xFF);
}

void SendBuffer(int socket, void* data, size_t size)
{
    int header = SRV_MAGIC | (u8)ServerCommand::Normal;
    
    SendRaw(socket, &header, 4);
    if (data != NULL)
        SendRaw(socket, data, size);
}

void SendConfirm(int socket)
{
    int header = SRV_MAGIC | (u8)ServerCommand::Confirm;
    SendRaw(socket, &header, 4);
}

void ReceiveBuffer(int socket, void* out_buff, size_t size)
{
    int ret;
    ReceiveRaw(socket, &ret, 4);
    
    if ((ret & 0xFFFFFF00) != CLT_MAGIC)
    {
        fatalSimple(MAKERESULT(Module_Discord, Error_InvalidMagic));
    }
    
    if ((ret & 0xFF) != (int)ClientCommand::SendBuffer)
    {
        fatalSimple(MAKERESULT(Module_Discord, Error_CmdIdNotSendBuff));
    }
    
    ReceiveRaw(socket, out_buff, size);
}

void ReceiveConfirm(int socket)
{  
    int ret;
    ReceiveRaw(socket, &ret, 4);
    
    if ((ret & 0xFFFFFF00) != CLT_MAGIC)
    {
        fatalSimple(MAKERESULT(Module_Discord, Error_InvalidMagic));
    }
    
    if ((ret & 0xFF) != (int)ClientCommand::Confirm)
    {
        fatalSimple(MAKERESULT(Module_Discord, Error_CmdIdNotConfirm));
    }
}




void SendAppList(int socket)
{
    Result rc;
    NsApplicationRecord* list = new NsApplicationRecord[255];
    int count = 0;
    
    rc = nsListApplicationRecord(list, sizeof(NsApplicationRecord)*255, &count);
    if (R_FAILED(rc))
    {
        fatalSimple(MAKERESULT(Module_Discord, Error_ListAppFailed));
    }
    
    SendBuffer(socket, &count, 4);
    ReceiveConfirm(socket);
    SendBuffer(socket, list, sizeof(NsApplicationRecord) * count);
    
    delete[] list;
}

void SendCurrentApp(int socket)
{
    Result rc;
    u64 pid;
    u64 tid = 0;
    int count;
    
    u64* pids = new u64[256];
    u32 pid_count;
    Handle debug_handle;
    DebugEventInfo *d = new DebugEventInfo;
    
    /*
    rc = pmdmntGetApplicationPid(&pid);
    if (R_SUCCEEDED(rc))
    {
        rc = pminfoGetTitleId(&tid, pid);
        if (R_FAILED(rc))
            fatalSimple(MAKERESULT(Module_Discord, Error_GetProcessTid));
    }*/
    
    /*
    ------------------------------------------------------------------------------------------------------------------------------
    for some reasons, the code above doesn't work on fws >= 5.x.x (pmdmntGetApplicationPid gives wrong infos) so I have to use a hackjob
    also if you know what the issue is, PLEASE TELL ME, I'd really like to not do it like this
    ------------------------------------------------------------------------------------------------------------------------------
    */
    
    rc = svcGetProcessList(&pid_count, pids, 256);
    if (R_FAILED(rc))
    {
        fatalSimple(MAKERESULT(Module_Discord, Error_GetPidList));
    }
    
    for(int i = 0; i < pid_count; i++)
    {
        //applications processes always have PIDs > 0x80
        //but atmosphere's pm don't recalculates the pids when a process is removed from the boot list
        //so I put 70 to be safe (it now only might bne a problem is more than 10 processes are not booted or were killed)
        if (pids[i] >= 0x70)
        {
            //try debugging each application process
            rc = svcDebugActiveProcess(&debug_handle, pids[i]);
            if (R_SUCCEEDED(rc))
            {
                pid = pids[i];
                
                //attach process
                while (R_SUCCEEDED(svcGetDebugEvent((u8 *)d, debug_handle)))
                {
                    if(d->type == DebugEventType::AttachProcess)
                    {
                        break;
                    }
                }
                
                tid = d->info.attach_process.title_id;
                
                rc = svcCloseHandle(debug_handle);
                if(R_FAILED(rc))
                {
                    fatalSimple(MAKERESULT(Module_Discord, Error_CloseHandle));
                }
                
                //avoid non-game titles like applets
                if (tid < 0x0100000000010000)
                {
                    tid = 0;
                    continue;
                }
                else break;
            }
        }
    }
    
exit_send_current:
    SendBuffer(socket, &tid, 8);
    
    delete[] pids;
}

void SendVersion(int socket)
{
    int ver = SERVER_VERSION;
    SendBuffer(socket, &ver, 4);
}

void SendActiveUser(int socket)
{
    Result rc;
    u128 userID=0;
    bool account_selected=0;
    AccountProfile profile;
    AccountProfileBase profilebase;
    
    rc = accountGetActiveUser(&userID, &account_selected);
    if(R_FAILED(rc))
        fatalSimple(MAKERESULT(Module_Discord, Error_GetAciveUser));
    
    SendBuffer(socket, &account_selected, 1);
    
    if(account_selected)
    {
        ReceiveConfirm(socket);
        
        rc = accountGetProfile(&profile, userID);
        if(R_FAILED(rc))
            fatalSimple(MAKERESULT(Module_Discord, Error_GetProfile));
        
        rc = accountProfileGet(&profile, NULL, &profilebase);
        if(R_FAILED(rc))
            fatalSimple(MAKERESULT(Module_Discord, Error_ProfileGet));
        
        accountProfileClose(&profile);
        
        SendBuffer(socket, profilebase.username, 0x20);
    }
}

void SendControlData(int socket)
{
    u64 tid;
    size_t outsize;
    Result rc;
    NsApplicationControlData* control = new NsApplicationControlData;
    
    SendConfirm(socket);
    ReceiveBuffer(socket, &tid, 8);

    rc = nsGetApplicationControlData(1, tid, control, sizeof(NsApplicationControlData), &outsize);
    if (R_FAILED(rc))
    {
        fatalSimple(MAKERESULT(Module_Discord, Error_GetControlData));
    }
    
    if(outsize < CONTROL_NACP_SIZE)
    {
        fatalSimple(MAKERESULT(Module_Discord, Error_InvalidControlSize));
    }
    
    SendBuffer(socket, control, sizeof(NsApplicationControlData));
    
    delete control;
}

void StartReceiving(int client)
{
    //initialise ns
    Result rc = nsInitialize();
    if (R_FAILED(rc))
        fatalSimple(MAKERESULT(789, Error_InitNS));
    
    //initialise acc
    rc = accountInitialize();
    if (R_FAILED(rc))
        fatalSimple(MAKERESULT(789, Error_InitACC));
    
    //initialise pm:dmnt
    rc = pmdmntInitialize();
    if (R_FAILED(rc))
        fatalSimple(MAKERESULT(789, Error_InitPMDMNT));
    
    //initialise pm:info
    rc = pminfoInitialize();
    if (R_FAILED(rc))
        fatalSimple(MAKERESULT(789, Error_InitPMINFO));
    
    ClientCommand cmd_id;
    while(true)
    {
        cmd_id = ReceiveCommand(client);
        
        switch (cmd_id)
        {
            case ClientCommand::GetControlData:
                SendControlData(client);
                break;
            case ClientCommand::ListApps:
                SendAppList(client);
                break;
            case ClientCommand::GetActiveUser:
                SendActiveUser(client);
                break;
            case ClientCommand::GetCurrentApp:
                SendCurrentApp(client);
                break;
            case ClientCommand::GetVersion:
                SendVersion(client);
                break;
            case ClientCommand::Disconnect:
                goto exit_receiving;
            default:
                break;
        }
    }
    
exit_receiving:
    nsExit();
    accountExit();
    pmdmntExit();
    pminfoExit();
}