
namespace MineML
open System

type Work =
    {
        Target : string option
        Data : string option
    }
        

(* RPC comunication class *)
type BitcoinRPC (host : string, port : UInt32, user : string, password : string) = 
    //member val public Client : System.Net.Http = new System.Net.WebClient () with get, set
    
    (* Authenticate the client *)
    member this.Authenticate () =
        let authp = sprintf "%s:%s" user password
        let authh = sprintf "Basic %s" 
                        ( let bb = System.Text.Encoding.Unicode.GetBytes(authp)
                          System.Convert.ToBase64String(bb) )
                          
        // Create the client connection        
        
        false
        
    
    (* Send an rpcjson request *)    
    member this.Request (cmd : string) (pars : string list) = ""
    
    
    (* GetWork request *)
    member this.GetWork () = { Target = Some ""; Data = Some "" }
