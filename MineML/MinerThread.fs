
namespace MineML
open System
open System.Threading

[<AbstractClass>]
type MinerThread (p : Parameters) =       
    abstract member Body : unit -> unit
    
    member val public RPC : BitcoinRPC = new BitcoinRPC (p.Host, p.Port, p.User, p.Password) with get, set
    member val public Messages : string list = [] with get, set
    member val public Thread : Thread = null with get, set
    
    member this.AddMessage s = this.Messages <- this.Messages @ [ s ]
    
    member this.ClearMessages () = 
        let ms = this.Messages
        this.Messages <- []
        ms
    
    
