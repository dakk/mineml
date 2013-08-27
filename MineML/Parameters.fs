
namespace MineML
open System

type Parameters =
    {
        Host: string
        Port: uint32
        User: string
        Password: string
        
        CpuThreads: int
    }
    static member Empty = 
        { 
            Host = "127.0.0.1"
            Port = uint32 80
            User = "user"
            Password = "pass"
            CpuThreads = 4 
        }
