#include <stdio.h>
//#include <fstream>
#include <string.h>
#include <dirent.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <switch.h>

#include "TcpCommand.h"
#include "result.h"

#define PORT 0xCAFE

#define TITLE_ID 0x420000000000001E


extern "C" {
    extern u32 __start__;

    u32 __nx_applet_type = AppletType_None;

    #define INNER_HEAP_SIZE 0x260000
    size_t nx_inner_heap_size = INNER_HEAP_SIZE;
    char   nx_inner_heap[INNER_HEAP_SIZE];
    
    void __libnx_initheap(void);
    void __appInit(void);
    void __appExit(void);
}

void fatalLater(Result err) {
    Handle srv;
#ifdef DEBUG
    while (R_FAILED(smGetServiceOriginal(&srv, smEncodeName("fatal:u")))) {
        // wait one sec and retry
        svcSleepThread(1000000000L);
    }
    IpcCommand c;
    ipcInitialize(&c);
    ipcSendPid(&c);
    struct {
        u64 magic;
        u64 cmd_id;
        u64 result;
        u64 unknown;
    } *raw;

    (void*)raw = ipcPrepareHeader(&c, sizeof(*raw));

    raw->magic = SFCI_MAGIC;
    raw->cmd_id = 1;
    raw->result = err;
    raw->unknown = 0;

    ipcDispatch(srv);
    svcCloseHandle(srv);
#endif
    (void)err;
    svcExitProcess();
    __builtin_unreachable();
}


// we override libnx internals to do a minimal init
void __libnx_initheap(void) {
	void*  addr = nx_inner_heap;
	size_t size = nx_inner_heap_size;

	/* Newlib */
	extern char* fake_heap_start;
	extern char* fake_heap_end;

	fake_heap_start = (char*)addr;
	fake_heap_end   = (char*)addr + size;
}

void registerFspLr()
{
    if (kernelAbove400())
        return;

    Result rc = fsprInitialize();
    if (R_FAILED(rc))
        fatalLater(rc);

    u64 pid;
    svcGetProcessId(&pid, CUR_PROCESS_HANDLE);

    rc = fsprRegisterProgram(pid, TITLE_ID, FsStorageId_NandSystem, NULL, 0, NULL, 0);
    if (R_FAILED(rc))
        fatalLater(rc);
    fsprExit();
}

void __appInit(void)
{
    Result rc;
    svcSleepThread(10000000000L);
    rc = smInitialize();
    if (R_FAILED(rc))
        fatalLater(rc);
    
    rc = fsInitialize();
    if (R_FAILED(rc))
        fatalLater(rc);
    
    registerFspLr();
    
    rc = fsdevMountSdmc();
    if (R_FAILED(rc))
        fatalLater(rc);
}

void __appExit(void)
{
    fsdevUnmountAll();
    fsExit();
    smExit();
    audoutExit();
}

Result socket_init(int* server_fd, sockaddr_in* address)
{
    int yes = 1;
    Result rc;
    
    rc = socketInitializeDefault();
    if(rc != 0)
    {
        return rc;
    }
    // create socket
    if ((*server_fd = socket(AF_INET, SOCK_STREAM, 0)) == 0)
    {
        return -1;
    }
    // Forcefully attaching socket to port
    rc = setsockopt(*server_fd, SOL_SOCKET, SO_REUSEADDR, &yes, sizeof(yes));
    if (rc != 0)
    {
        return rc;
    }
    address->sin_family = AF_INET;
    address->sin_addr.s_addr = INADDR_ANY;
    address->sin_port = htons( PORT );
      
    //attach socket to port
    if (bind(*server_fd, (struct sockaddr *)address, sizeof(*address))<0)
    {
        return -1;
    }
    
    return 0;
}


int main(int argc, char **argv)
{
    (void)argc;
    (void)argv;
    
    int server_fd, new_socket;
    struct sockaddr_in address;
    int addrlen = sizeof(address);
    
    //init socket
    if (R_FAILED(socket_init(&server_fd, &address)))
    {
        fatalSimple(MAKERESULT(Module_Discord, Error_InitSocket));
    }
    
	//waiting for connection
        if (listen(server_fd, 3) < 0)
        {
            fatalSimple(MAKERESULT(Module_Discord, Error_Listen));
        }
    while(true)
    {

        //Accepting;
        if ((new_socket = accept(server_fd, (struct sockaddr *)&address, (socklen_t*)&addrlen))<0)
        {
            if (listen(server_fd, 3) < 0)
        {
            fatalSimple(MAKERESULT(Module_Discord, Error_Listen));
        }
        }
        
        StartReceiving(new_socket);
        close(new_socket);
    }
    
    
    socketExit();
    return 0;
}
