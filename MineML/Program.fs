module MineML.Main

open System
open System.IO


(* Parse parameters list *)
let rec parseParameters (ss : string list) (ps : Parameters) =
    let getvalue (keyvalue : string) = keyvalue.Substring ((keyvalue.IndexOf "=") + 1, 
                                        keyvalue.Length - (keyvalue.IndexOf "=") - 1)
    match ss with
        | [] -> ps
        | x::xl ->
            if x.StartsWith "host" then parseParameters xl { ps with Host = getvalue x }
            elif x.StartsWith "port" then parseParameters xl { ps with Port = UInt32.Parse (getvalue x) }
            elif x.StartsWith "user" then parseParameters xl { ps with User = getvalue x }
            elif x.StartsWith "password" then parseParameters xl { ps with Password = getvalue x }
            elif x.StartsWith "cpu_threads" then parseParameters xl { ps with CpuThreads = Int32.Parse (getvalue x) }
            else parseParameters xl ps
                
            
(* Program entry point *)
[<EntryPoint>]
let main (args : string []) = 
    Console.WriteLine("MineML")
    
    // Check if parameter contains a conf path
    let pars =
        if args.Length < 1 then Parameters.Empty
        else 
            // Read text file, parse arguments
            let strings = File.ReadAllLines (let a = (Array.toList args) in a.Head)
            parseParameters (Array.toList strings) Parameters.Empty
     
    // Create the miner object and start all threads 
    let miner = new Miner (pars)
    do  miner.Prepare ()
        miner.Loop ()    
    0

