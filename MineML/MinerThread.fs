
namespace MineML
open System
open System.Threading

[<AbstractClass>]
type MinerThread (p : Parameters) =       
    abstract member Body : unit -> unit
    member val public RPC : BitcoinRPC = new BitcoinRPC (p.Host, p.Port, p.User, p.Password) with get, set
