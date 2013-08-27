
namespace MineML
open System


type MinerThreadCPU (p : Parameters) = 
    inherit MinerThread (p)    
    
    member this.SubmitWorkResult (w : byte []) res = ()
    
    member this.Work (w : byte []) = Some ""
    
    override this.Body () =
        if this.RPC.Authenticate () then
            this.AddMessage "RPC: authenticated"
               
            let rec iteration () =
                // Get next work
                let w = this.RPC.GetWork ()
                
                // Check work data
                if w.IsNone then
                    Threading.Thread.Sleep (1000)
                    iteration ()
                
                // Execute the work
                let res = this.Work w.Value
                
                // If result, submit it to the server
                if res.IsSome then 
                    this.SubmitWorkResult w.Value res.Value
                    
                // Iterate
                iteration ()
            iteration ()
                
        
        
        else
            this.AddMessage "RPC: failed to authenticate"        
            ()