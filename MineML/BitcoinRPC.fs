
namespace MineML
open System

(* RPC comunication class *)
type BitcoinRPC (host : string, port : UInt32, user : string, password : string) = 
    //member val public Client : System.Net.Http = new System.Net.WebClient () with get, set
    
    member this.Authenticate () =
        let authp = sprintf "%s:%s" user password
        let authh = sprintf "Basic %s" 
                        ( let bb = System.Text.Encoding.Unicode.GetBytes(authp)
                          System.Convert.ToBase64String(bb) )
        
        //self.conn = httplib.HTTPConnection(host, port, False, 30)
        
        true
