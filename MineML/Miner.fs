
namespace MineML
open System
open System.Threading

(* The Miner. It starts threads and handles statistics gathering *)
type Miner (p : Parameters) = 
    // Thread list
    member val public MinerThreads : MinerThread list = [] with get, set
    
    
    member this.Prepare () =
        // Create all cpu threads
        for x in 1 .. p.CpuThreads do
            let nmt = new MinerThreadCPU (p) :> MinerThread
            nmt.Thread <- new Thread (nmt.Body)
            nmt.Thread.Name <- sprintf "T%d" x
            this.MinerThreads <- nmt :: this.MinerThreads
            
    
    member this.Loop () =
        // Start all threads
        for x in this.MinerThreads do
            printf "Starting thread %s\n" x.Thread.Name
            x.Thread.Start ()
            
        while true do
            for x in this.MinerThreads do
                // Print all threads messages
                let rec printMessages l =
                    match l with
                        | [] -> ()
                        | m::ml -> printf "%s -> %s\n" x.Thread.Name m; printMessages ml
                        
                printMessages (x.ClearMessages ())
            
