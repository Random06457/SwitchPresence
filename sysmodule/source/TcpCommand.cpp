#include "TcpCommand.h"

const int SRV_MAGIC = 0x11223300;
const int CLT_MAGIC = 0x33221100;
const int CONTROL_NACP_SIZE = 0x4000;
const int CONTROL_FULL_SIZE = 0x24000;

const int SERVER_VERSION = 1 << 16 | 0 << 8 | 0;


//returns cmd id
ClientCommand ReceiveCommand(int socket)
{
    int ret;
    char* buff = new char[256];
    
    int len = recv(socket , buff, 256, 0);
    
    if(len < 0)
    {   
        return ClientCommand::Disconnect;
    }
    
    ret = *((int*)buff);
    
    if ((ret & 0xFFFFFF00) != CLT_MAGIC)
    {
        return ClientCommand::Disconnect;
    }
    
    delete[] buff;
    
    return (ClientCommand)(ret & 0xFF);
}

void SendBuffer(int socket, ServerCommand cmd, void* data, size_t size)
{
    
    u8* buff = new u8[size + 4];
    
    *((int*)buff) = SRV_MAGIC | (u8)cmd;
    
    if (data != nullptr)
        memcpy(buff + 4, data, size);
    
    
    size_t total = 0;
    while (total < size + 4) {
        size_t count = send(socket, buff + total, (size + 4) - total, 0);
        if (count <= 0)
            fatalSimple(MAKERESULT(Module_Discord, Error_SendData));
        total += count;
    }
    
    delete[] buff;
}

void SendConfirm(int socket)
{
    SendBuffer(socket, ServerCommand::Confirm, nullptr, 0);
}

void ReceiveBuffer(int socket, void* out_buff, size_t size)
{
    int ret = 0;
    u8* buff = new u8[size+4];
    
    size_t total = 0;
    
    while (total < size+4)
    {
        size_t count = recv(socket, buff + total, (size + 4) - total, 0);
        if (count <= 0)
            fatalSimple(MAKERESULT(Module_Discord, Error_RecData));
        total += count;
    }
    
    ret = *((int*)buff);
    
    if ((ret & 0xFFFFFF00) != CLT_MAGIC)
    {
        fatalSimple(MAKERESULT(Module_Discord, Error_InvalidMagic));
    }
    
    if ((ret & 0xFF) != (int)ClientCommand::SendBuffer)
    {
        fatalSimple(MAKERESULT(Module_Discord, Error_CmdIdNotSendBuff));
    }
    
    memcpy(out_buff, buff + 4, size);
    
    delete[] buff;
}

void ReceiveConfirm(int socket)
{  
    int ret = 0;
    int len = recv(socket , &ret, 4, 0);
    
    if(len < 0)
    {
        fatalSimple(MAKERESULT(Module_Discord, Error_RecData));
    }
    
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
    
    SendBuffer(socket, ServerCommand::Normal, &count, 4);
    ReceiveConfirm(socket);
    SendBuffer(socket, ServerCommand::Normal, list, sizeof(NsApplicationRecord) * count);
    
    delete[] list;
}

void SendCurrentApp(int socket)
{
    Result rc;
    u64 pid;
    u64 tid = 0;
    
    rc = pmdmntGetApplicationPid(&pid);
    
    if (R_SUCCEEDED(rc))
    {
        rc = pminfoGetTitleId(&tid, pid);
        if (R_FAILED(rc))
            fatalSimple(MAKERESULT(Module_Discord, Error_GetProcessTid));
    }
    
    SendBuffer(socket, ServerCommand::Normal, &tid, 8);
}

void SendVersion(int socket)
{
    int ver = SERVER_VERSION;
    SendBuffer(socket, ServerCommand::Normal, &ver, 4);
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
    
    SendBuffer(socket, ServerCommand::Normal, &account_selected, 1);
    
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
        
        SendBuffer(socket, ServerCommand::Normal, profilebase.username, 0x20);
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
    
    SendBuffer(socket, ServerCommand::Normal, control, sizeof(NsApplicationControlData));
    
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