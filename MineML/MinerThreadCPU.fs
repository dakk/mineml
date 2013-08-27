
namespace MineML
open System


type MinerThreadCPU (p : Parameters) = 
    inherit MinerThread (p)    
    
    member this.SubmitWorkResult (w : Work) res = ()
    
    member this.Work (w : Work) = Some ""
    
    override this.Body () =
        if this.RPC.Authenticate () then
            let rec iteration () =
                // Get next work
                let w = this.RPC.GetWork ()
                
                // Check work data
                if w.Target.IsNone || w.Data.IsNone then
                    Threading.Thread.Sleep (1000)
                    iteration ()
                
                // Execute the work
                let res = this.Work w
                
                // If result, submit it to the server
                if res.IsSome then 
                    this.SubmitWorkResult w res.Value
                    
                // Iterate
                iteration ()
            iteration ()
                
        
        
        else
            this.AddMessage "RPC: failed to authenticate"        
            ()