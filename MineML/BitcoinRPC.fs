
namespace MineML
open System
open System.IO

        

(* RPC comunication class *)
type BitcoinRPC (host : string, port : UInt32, user : string, password : string) = 
    member val public Uri : Uri = new Uri (
                                            if host.StartsWith "http" then host else "http://"+host
                                          ) with get, set
    
    
    (* Check the client authentication *)
    member this.Authenticate () = true
        
    
    (* Send an rpcjson request *)    
    member this.Request (cmd : string) (pars : string option) = 
        let webreq = Net.WebRequest.Create (this.Uri)
        webreq.Credentials <- new Net.NetworkCredential (user, password)
        webreq.ContentType <- "application/json-rpc"
        webreq.Method <- "POST";

        let jsonparam = if pars.IsNone then "" else "\"" + pars.Value + "\""
        let request = "{\"id\": 0, \"method\": \"" + cmd + "\", \"params\": [" + jsonparam + "]}"
        let bytereq = Text.Encoding.UTF8.GetBytes(request)
        webreq.ContentLength <- int64 bytereq.Length
        
        let stream = webreq.GetRequestStream ()
        stream.Write (bytereq, 0, bytereq.Length)
        
        let webresp = webreq.GetResponse ()
        let rstream = webresp.GetResponseStream ()
        let rsreader = new StreamReader (rstream)
        rsreader.ReadToEnd ()
        
        
    (* Send a share *)
    member this.SendShare (share : byte []) =
        let data = Utils.addPadding (Utils.endianFlip32BitChunks (Utils.toStringFromByte share))
        let reply = this.Request "getwork" (Some data)
        let mat : Text.RegularExpressions.Match = Text.RegularExpressions.Regex.Match (reply, "\"result\": true")
        mat.Success
        
 
    (* Parse json data *)
    member this.ParseData (json : string) =
        let mat = Text.RegularExpressions.Regex.Match(json, "\"data\": \"([A-Fa-f0-9]+)")
        if mat.Success then
            Some (Utils.toBytes (Utils.endianFlip32BitChunks (Utils.removePadding((mat.Groups.Item 1).Value))))
        else
            None
                                     
                                    
    (* GetWork request *)
    member this.GetWork () = this.ParseData (this.Request "getwork" None)
    
    
    
