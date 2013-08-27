
namespace MineML
open System
open System.Threading

(* The Miner. It starts threads and handles statistics gathering *)
type Miner (p : Parameters) = 
    // Thread list
    member val public Threads : Thread list = [] with get, set
    
    
    member this.Prepare () =
        // Create all cpu threads
        for x in 1 .. p.CpuThreads do
            let nmt = new MinerThreadCPU (p)
            let nthread = new Thread (nmt.Body)
            nthread.Name <- sprintf "T%d" x
            this.Threads <- nthread :: this.Threads
            
    
    member this.Loop () =
        // Start all threads
        for x in this.Threads do
            printf "Starting thread %s\n" x.Name
            x.Start ()
            
            
        for x in this.Threads do
            //x.Join ()
            //printf "Thread %s joined main\n" x.Name
