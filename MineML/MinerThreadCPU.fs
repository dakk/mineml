
namespace MineML
open System

type MinerThreadCPU (p : Parameters) = 
    inherit MinerThread (p)
    
    override this.Body () =
        if this.RPC.Authenticate () then
            ()
        
        
        else
            printf "RPC: failed to authenticate\n"        
            ()