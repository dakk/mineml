
namespace MineML
open System
open System.Security.Cryptography


type MinerThreadCPU (p : Parameters) = 
    inherit MinerThread (p)    
    
    member val NOnce : uint32 = uint32 0 with get, set
    member val Done : uint32 = uint32 0 with get, set
    member val Hasher : SHA256Managed = new SHA256Managed () with get, set
    
    member this.SubmitWorkResult (share : byte []) =
        let data = Utils.addPadding(Utils.endianFlip32BitChunks(Utils.toStringFromByte(share)))
        let reply = this.RPC.Request "getwork" (Some data)
        if reply.IsSome then
            (Text.RegularExpressions.Regex.Match(reply.Value, "\"result\": true")).Success
        else
            false
    
    member this.Sha256 (input : byte []) =
        this.Hasher.ComputeHash(input, 0, input.Length)

        
    (* Find share, return None if not found or Some p *)
    member this.Work (data : byte []) batchsize = 
        let rec loop bs =
            match bs with
                | 0 -> None
                | n ->
                    let mutable data_w : byte [] = Array.create (data.Length) (new Byte())
                    BitConverter.GetBytes(this.NOnce).CopyTo(data_w, data.Length - 4)
                    let doublehash : byte [] = this.Sha256(this.Sha256(data_w))
                    
                    let mutable zerob = 0
                    let mutable i = 31
                    while i >= 28 do
                        let dh : byte = Array.get doublehash i
                        if dh > byte 0 then i <- 0
                        else
                            i <- i - 1
                            zerob <- zerob + 1
                    
                    if zerob = 4 then Some data_w
                    else
                        this.NOnce <- this.NOnce + uint32 1
                        
                        if this.NOnce = UInt32.MaxValue then this.NOnce <- uint32 0
                        loop (bs-1)
                
        loop batchsize
                
    override this.Body () =
        if this.RPC.Authenticate () then
            this.AddMessage "RPC: authenticated"
               
            let rec iteration () =
                // Get next work
                let w = this.RPC.GetWork ()
                
                // Check work data
                if w.IsNone then
                    //this.AddMessage "Invalid work"
                    Threading.Thread.Sleep (1000)
                    iteration ()
                
                //this.AddMessage (sprintf "Good work -> %s" (Utils.toStringFromByte w.Value))
                    
                // Execute the work
                let res = this.Work w.Value 100000
                
                // If result, submit it to the server
                if res.IsSome then 
                    if this.SubmitWorkResult (res.Value) then
                        this.AddMessage "Ok!!!"
                        this.Done <- this.Done + uint32 1
                        this.AddMessage (sprintf "Done: %d" this.Done)  
                    else
                        () //this.AddMessage "Sending Error"
                else
                    () //this.AddMessage "Bad result"
                
                //this.AddMessage (sprintf "Done: %d" this.Done)    
                
                // Iterate
                iteration ()
            iteration ()
                
        
        
        else
            this.AddMessage "RPC: failed to authenticate"        
            ()