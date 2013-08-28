
namespace MineML
open System
open System.IO

        

(* RPC comunication class *)
type BitcoinRPC (host : string, port : UInt32, user : string, password : string) = 
    member val public Uri : Uri = new Uri (
                                            if host.StartsWith "http" then sprintf "%s:%d" host port 
                                            else sprintf "http://%s:%d" host port
                                          ) with get, set
    
    
    (* Check the client authentication *)
    member this.Authenticate () = true
        
    
    (* Send an rpcjson request *)    
    member this.Request (cmd : string) (pars : string option) = 
        //printf "%s\n\n" (this.Uri.ToString ())
        let webreq = Net.WebRequest.Create (this.Uri)
        webreq.Credentials <- new Net.NetworkCredential (user, password)
        webreq.ContentType <- "application/json-rpc"
        webreq.Method <- "POST"
        

        let jsonparam = if pars.IsNone then "" else "\"" + pars.Value + "\""
        let request = "{\"id\": 0, \"method\": \"" + cmd + "\", \"params\": [" + jsonparam + "]}"
        let bytereq = Text.Encoding.UTF8.GetBytes(request)
        webreq.ContentLength <- int64 bytereq.Length
        
        try
            let stream = webreq.GetRequestStream ()
            stream.Write (bytereq, 0, bytereq.Length)
            
            let webresp = webreq.GetResponse ()
            let rstream = webresp.GetResponseStream ()
            let rsreader = new StreamReader (rstream)
            Some (rsreader.ReadToEnd ())
        with
            | e -> printf "%s\n" (e.Message); None
        
        
    (* Send a share *)
    member this.SendShare (share : byte []) =
        let data = Utils.addPadding (Utils.endianFlip32BitChunks (Utils.toStringFromByte share))
        let reply = this.Request "getwork" (Some data)
        
        if reply.IsNone then false
        else
            let mat : Text.RegularExpressions.Match = 
                Text.RegularExpressions.Regex.Match (reply.Value, "\"result\": true")
            mat.Success
        
 
    (* Parse json data *)
    member this.ParseData (json : string) =
        let mat = Text.RegularExpressions.Regex.Match(json, "\"data\": \"([A-Fa-f0-9]+)")
        if mat.Success then
            Some (Utils.toBytes (Utils.endianFlip32BitChunks (Utils.removePadding((mat.Groups.Item 1).Value))))
        else
            None
                                     
                                    
    (* GetWork request *)
    member this.GetWork () = 
        let req = (this.Request "getwork" None)
        if req.IsSome then this.ParseData req.Value else None
    
    
    
